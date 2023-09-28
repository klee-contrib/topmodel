import { ExtensionContext, workspace, window, Uri } from "vscode";
import * as fs from "fs";
import { TopModelConfig, TopModelException } from "./types";
import { Application } from "./application";
import { execute } from "./utils";
import { State } from "./state";
import { configure } from "mobx";

const open = require("open");
const yaml = require("js-yaml");

let state: State;

configure({ enforceActions: "never" });

export async function activate(ctx: ExtensionContext) {
    state = new State(ctx);
    checkInstall();
    if (state.applications.length === 0) {
        const confs = await findConfFiles();
        try {
            state.applications.push(...confs.map((conf) => new Application(conf.file.path, conf.config, ctx)));
        } catch (error: any) {
            handleError(error);
        }
    }
}

/********************************************************* */
/*********************** CHECKS ************************** */
/********************************************************* */
async function checkInstall() {
    const dotnetIsInstalled = await execute('echo ;%PATH%; | find /C /I "dotnet"');
    if (dotnetIsInstalled !== "1\r\n") {
        const selection = await window.showInformationMessage(
            "Dotnet n'est pas installé",
            "Ouvrir la page de téléchargement"
        );
        if (selection === "Ouvrir la page de téléchargement") {
            open("https://dotnet.microsoft.com/download/dotnet/6.0");
        }
    }
}

async function findConfFiles(): Promise<{ config: TopModelConfig; file: Uri }[]> {
    const files = await workspace.findFiles("**/topmodel*.config");
    let configs: { config: TopModelConfig; file: Uri }[] = files.map((file) => {
        const doc = fs.readFileSync(file.path.substring(1), "utf8");
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
