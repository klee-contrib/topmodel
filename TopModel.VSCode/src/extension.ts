import * as fs from "fs";
import { configure } from "mobx";
import { ExtensionContext, RelativePattern, Uri, window, workspace, WorkspaceFolder } from "vscode";

import { Application } from "./application";
import { t } from "./i18n";
import { State } from "./state";
import { TopModelConfig, TopModelException } from "./types";
import { execute } from "./utils";

const open = require("open").default;
const yaml = require("js-yaml");

let state: State;

configure({ enforceActions: "never" });

export async function activate(ctx: ExtensionContext) {
    try {
        const installed = await checkDotnetInstall();
        if (!installed) {
            return;
        }

        state = new State(ctx);

        const toolInstalled = await state.installLanguageServerTool();
        if (!toolInstalled) {
            return;
        }

        // Un client LSP par workspace folder, démarré une fois le tool modls disponible.
        await addApplications(workspace.workspaceFolders ?? []);

        // Le workspace peut gagner ou perdre des dossiers en cours de session. Chaque dossier ayant
        // son propre client LSP (et son propre process modls), il faut le démarrer/arrêter à la volée :
        // sans cela un dossier ajouté après coup resterait définitivement sans service.
        ctx.subscriptions.push(
            workspace.onDidChangeWorkspaceFolders(async (event) => {
                try {
                    await state.removeApplications(event.removed);
                    await addApplications(event.added);
                } catch (error: any) {
                    handleError(error);
                }
            }),
        );
    } catch (error: any) {
        handleError(error);
    }
}

/**
 * Crée et démarre une {@link Application} pour chacun des workspace folders donnés contenant au moins
 * une config TopModel. Les dossiers sans config ne lancent pas de language server.
 */
async function addApplications(folders: readonly WorkspaceFolder[]) {
    const applications: Application[] = [];

    for (const folder of folders) {
        const configs = await findConfFiles(folder);
        if (configs.length > 0) {
            applications.push(new Application(folder, configs));
        }
    }

    await state.addApplications(applications);
}

export function deactivate() {
    state?.preview?.panel.dispose();
    // On attend l'arrêt effectif du serveur (kill du processus inclus) pour libérer le verrou
    // sur les fichiers de l'outil avant que VSCode ne détruise l'hôte d'extension.
    return state?.stopLanguageServer();
}

/********************************************************* */
/*********************** CHECKS ************************** */
/********************************************************* */
async function checkDotnetInstall(): Promise<boolean> {
    try {
        await execute("dotnet -h");
        return true;
        // oxlint-disable-next-line no-unused-vars
    } catch (_err: any) {
        const selection = await window.showInformationMessage(t("dotnetIsNotInstalled"), t("openDotnetDownloadPage"));
        if (selection === t("openDotnetDownloadPage")) {
            open("https://dotnet.microsoft.com/download/dotnet");
        }

        return false;
    }
}

/**
 * Configs TopModel d'un workspace folder donné.
 *
 * La recherche est scopée au dossier (et non menée sur tout le workspace puis répartie ensuite) :
 * chaque language server ne doit recevoir que les configs du dossier dont il a la charge.
 */
async function findConfFiles(folder: WorkspaceFolder): Promise<{ config: TopModelConfig; file: Uri }[]> {
    const files = await workspace.findFiles(new RelativePattern(folder, "**/topmodel*.config"));

    return files
        .map((file) => {
            const doc = fs.readFileSync(file.fsPath, "utf8");
            const config = doc
                .split("---")
                .filter((e) => e)
                .map(yaml.load)
                .map((e) => e as TopModelConfig)
                .filter((e) => e?.app)[0];
            return { config, file };
        })
        .filter((c) => c.config);
}

function handleError(exception: TopModelException) {
    window.showErrorMessage(exception.message);
    state.error = exception.message;
}
