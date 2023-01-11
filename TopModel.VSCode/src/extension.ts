import {
    ExtensionContext,
    workspace,
    commands,
    window,
    StatusBarItem,
    StatusBarAlignment,
    Uri,
    Position,
} from "vscode";
import * as fs from "fs";
import { TopModelConfig, TopModelException } from "./types";
import { registerPreview } from "./preview";
import { Application } from "./application";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";
import { execute } from "./utils";
import { Global } from "./global";
import { State } from "./state";
import { autorun, configure } from "mobx";

const open = require("open");
const yaml = require("js-yaml");

let topModelStatusBar: StatusBarItem;
let state: State;

configure({ enforceActions: "never" });

export async function activate(ctx: ExtensionContext) {
    state = new State(ctx);
    autorun(() => updateStatusBar());
    createStatusBar();

    checkInstall();
    if (state.applications.length === 0) {
        const confs = await findConfFiles();
        try {
            state.applications.push(...confs.map((conf) => new Application(conf.file.path, conf.config, ctx)));
            registerGlobalCommands();
        } catch (error: any) {
            handleError(error);
        }
    }
}

function createStatusBar() {
    topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
    state.context.subscriptions.push(topModelStatusBar);
    updateStatusBar();
    topModelStatusBar.show();
}

/********************************************************* */
/*********************** CHECKS ************************** */
/********************************************************* */
async function checkInstall() {
    const dotnetIsInstalled = await execute('echo ;%PATH%; | find /C /I "dotnet"');
    if (dotnetIsInstalled !== "1\r\n") {
        const selection = await window.showInformationMessage("Dotnet is not installed", "Show download page");
        if (selection === "Show download page") {
            open("https://dotnet.microsoft.com/download/dotnet/6.0");
        }
    }
}

/***********************************************************/
/********************* COMMANDS ****************************/
/***********************************************************/
function registerGlobalCommands() {
    const modgenUpdate = commands.registerCommand(COMMANDS.updateModgen, () => state.tools.modgen.update());
    state.context.subscriptions.push(modgenUpdate);
    COMMANDS_OPTIONS.update = {
        title: "Mettre à jour le générateur",
        description: "Mise à jour manuelle de l'outil de génération",
        command: COMMANDS.updateModgen,
        detail: "L'extension et le générateur sont versionnés séparément. Vous pouvez activer la mise à jour automatique dans les paramètres de l'extension.",
    };
    const tmdgenUpdate = commands.registerCommand(COMMANDS.updateTmdgen, () => state.tools.tmdgen.update());
    state.context.subscriptions.push(tmdgenUpdate);
    COMMANDS_OPTIONS.update = {
        title: "Mettre à jour le générateur",
        description: "Mise à jour manuelle de l'outil de génération",
        command: COMMANDS.updateTmdgen,
        detail: "L'extension et le générateur sont versionnés séparément. Vous pouvez activer la mise à jour automatique dans les paramètres de l'extension.",
    };
    commands.registerCommand(COMMANDS.findRef, async (line: number) => {
        await commands.executeCommand(
            "editor.action.goToLocations",
            window.activeTextEditor!.document.uri,
            new Position(line, 0),
            []
        );
        await commands.executeCommand("editor.action.goToReferences");
    });
    registerPreview(state.context);
    registerChooseCommand();
    new Global(state.context).registerCommands();
}

function registerChooseCommand() {
    state.context.subscriptions.push(
        commands.registerCommand(COMMANDS.chooseCommand, async () => {
            const options = COMMANDS_OPTIONS;
            const quickPick = window.createQuickPick();
            quickPick.items = Object.keys(options).map((key) => ({
                key,
                label: options[key].title,
                description: options[key].description,
                detail: options[key].detail,
            }));
            quickPick.onDidChangeSelection((selection) => {
                if (selection[0]) {
                    commands.executeCommand(options[(selection[0] as any).key].command);
                    quickPick.hide();
                }
            });
            quickPick.onDidHide(() => quickPick.dispose());
            quickPick.show();
        })
    );
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
            .filter((e) => e.app)[0];
        return { config: c, file };
    });

    return configs;
}

export function updateStatusBar() {
    topModelStatusBar.text = state.statusText;
    topModelStatusBar.tooltip = state.statusTooltip;
    topModelStatusBar.show();
}

function handleError(exception: TopModelException) {
    window.showErrorMessage(exception.message);
    state.error = exception.message;
}
