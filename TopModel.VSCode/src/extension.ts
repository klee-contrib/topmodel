import { groupBy } from "es-toolkit";
import * as fs from "fs";
import { configure } from "mobx";
import { ExtensionContext, Uri, window, workspace } from "vscode";

import { Application } from "./application";
import { t } from "./i18n";
import { State } from "./state";
import { TopModelConfig, TopModelException } from "./types";
import { execute } from "./utils";

const open = require("open");
const yaml = require("js-yaml");

let state: State;

configure({ enforceActions: "never" });

export async function activate(ctx: ExtensionContext) {
    try {
        const installed = await checkDotnetInstall();
        if (installed) {
            const confs = await findConfFiles();

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
                    new Application(workspace.workspaceFolders!.find((w) => w.name === workspaceName)!, confs, ctx),
            );

            state = new State(ctx);
            state.applications.push(...applications);
        }
    } catch (error: any) {
        handleError(error);
    }
}

export function deactivate() {
    state.preview?.panel.dispose();
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
