import { groupBy } from "es-toolkit";
import * as fs from "fs";
import { configure } from "mobx";
import { ExtensionContext, Uri, window, workspace } from "vscode";

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
        if (installed) {
            state = new State(ctx);

            const confs = await findConfFiles();

            await state.initLanguageServer(confs);

            const applications = Object.entries(
                groupBy(
                    confs,
                    (conf) =>
                        workspace.workspaceFolders?.find((w) =>
                            conf.file.fsPath.toLowerCase().includes(w.uri.fsPath.toLowerCase()),
                        )!.name!,
                ),
            ).map(
                ([workspaceName, confs]) =>
                    new Application(workspace.workspaceFolders!.find((w) => w.name === workspaceName)!, confs),
            );

            state.applications.push(...applications);
        }
    } catch (error: any) {
        handleError(error);
    }
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

async function findConfFiles(): Promise<{ config: TopModelConfig; file: Uri }[]> {
    const files = await workspace.findFiles("**/topmodel*.config");
    let configs: { config: TopModelConfig; file: Uri }[] = files.map((file) => {
        const doc = fs.readFileSync(file.fsPath, "utf8");
        const c = doc
            .split("---")
            .filter((e) => e)
            .map(yaml.load)
            .map((e) => e as TopModelConfig)
            .filter((e) => e?.app)
            .filter((e) => e.app)[0];
        return { config: c, file };
    });

    return configs;
}

function handleError(exception: TopModelException) {
    window.showErrorMessage(exception.message);
    state.error = exception.message;
}
