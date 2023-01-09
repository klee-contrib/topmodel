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
import { ExtensionState, TmdTool, TopModelConfig, TopModelException } from "./types";
import { registerPreview } from "./preview";
import { Application } from "./application";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";
import { execute } from "./utils";
import { Global } from "./global";

const open = require("open");
const yaml = require("js-yaml");

let topModelStatusBar: StatusBarItem;
let extentionState: ExtensionState = "LOADING";
let context: ExtensionContext;
const applications: Application[] = [];
const tools = {
    topmodel: {
        name: "TopModel.Generator",
        command: "modgen"
    } as TmdTool,
    tmdgen: {
        name: "TopModel.ModelGenerator",
        command: "tmdgen"
    } as TmdTool
} as const;

export async function activate(ctx: ExtensionContext) {
    context = ctx;
    createStatusBar();
    checkInstall();
    await loadlatestVersion(tools.topmodel);
    await loadlatestVersion(tools.tmdgen);
    if (applications.length === 0) {
        const confs = await findConfFiles();
        try {
            applications.push(...confs.map((conf) => new Application(conf.file.path, conf.config, ctx)));
            applications.forEach(async (app) => {
                await app.start();
                refreshState();
            });
            refreshState();
            registerGlobalCommands();
        } catch (error: any) {
            handleError(error);
            refreshState();
        }
    }
}

function refreshState() {
    let status: ExtensionState = "RUNNING";
    applications.forEach((a) => {
        if (a.status === "LOADING") {
            status = "LOADING";
        }
    });
    extentionState = status;
    updateStatusBar();
}

function createStatusBar() {
    topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
    context.subscriptions.push(topModelStatusBar);
    updateStatusBar();
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
    } else {
        await checkToollInsall(tools.topmodel);
        await checkToollInsall(tools.tmdgen);
    }
}

async function loadlatestVersion(tool: TmdTool) {
    const https = require("https");
    const options = {
        hostname: "api.nuget.org",
        port: 443,
        path: `/v3-flatcontainer/${tool.name}/index.json`,
        method: "GET",
    };

    const req = await https.request(options, (res: any) => {
        res.on("data", async (reponse: string) => {
            const { versions }: { versions: string[] } = JSON.parse(reponse);
            tool.latestVersion = versions[versions.length - 1];
        });
    });

    req.on("error", (error: any) => {
        console.error(error);
    });

    req.end();
}

async function checkToollInsall(tool: TmdTool) {
    let result;
    try {
        result = await execute(`dotnet tool list -g | find /C /I "${tool.name.toLowerCase()}"`);
    } catch (error: any) {
        result = "Not Installed";
    }
    if (result !== "1\r\n") {
        if (tool.name === "TopModel.Generator") {
            const option = "Installer TopModel";
            const selection = await window.showInformationMessage("TopModel n'est pas installé", option);
            if (selection === option) {
                installTool(tools.topmodel);
            }
        }
    } else {
        // L'outil est installé, recherche de mise à jour
        await checkToolUpdate(tool);
    }
}

async function checkToolUpdate(tool: TmdTool) {
    // Chargement de la version installée de l'outil
    await loadCurrentVersion(tool);
    if (tool.currentVersion !== tool.latestVersion && tool.latestVersion) {
        const extensionConfiguration = workspace.getConfiguration("topmodel");
        if (extensionConfiguration.autoUpdate) {
            updateTool(tool);
        } else {
            const option = `Mettre à jour ${tool.name}`;
            const selection = await window.showInformationMessage(
                `Le générateur TopModel peut être mis à jour (${tool.currentVersion} > ${tool.latestVersion})`,
                option
            );
            if (selection === option) {
                updateTool(tool);
            }
        }
    }
}

async function loadCurrentVersion(tool: TmdTool) {
    try {
        const result = (await execute(`${tool.command} --version`)) as string;
        tool.currentVersion = result.replace("\r\n", "");
    } catch (error) {
        console.error("Erreur pendant le chargement de la version courante de l'outil", tool, error);
    }

    updateStatusBar();
}

/***********************************************************/
/********************* COMMANDS ****************************/
/***********************************************************/

async function installTool(tool: TmdTool) {
    // Recherche de mise à jour 
    await loadlatestVersion(tool);
    extentionState = "INSTALLING";
    updateStatusBar();
    await execute(`dotnet tool install --global TopModel.Generator`);
    loadCurrentVersion(tool);
    extentionState = "RUNNING";
    updateStatusBar();
    const selection = await window.showInformationMessage(
        `Le générateur TopModel v${tool.latestVersion} a été installé`,
        "Voir la release note"
    );
    if (selection === "Voir la release note") {
        open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
    }
}

async function updateTool(tool: TmdTool) {
    extentionState = "UPDATING";
    updateStatusBar();
    await execute(`dotnet nuget locals http-cache --clear`);
    await execute(`dotnet tool update --global ${tool.name}`);
    const oldVersion = tool.currentVersion;
    await loadCurrentVersion(tool);
    extentionState = "RUNNING";
    updateStatusBar();
    if (tool.latestVersion) {
        const selection = await window.showInformationMessage(
            `TopModel a été mis à jour ${oldVersion} --> ${tool.latestVersion}`,
            "Voir la release note"
        );
        if (selection === "Voir la release note") {
            open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
        }
    }
}

function registerGlobalCommands() {
    const modgenUpdate = commands.registerCommand(COMMANDS.updateModgen, () => updateTool(tools.topmodel));
    context.subscriptions.push(modgenUpdate);
    COMMANDS_OPTIONS.update = {
        title: "Mettre à jour le générateur",
        description: "Mise à jour manuelle de l'outil de génération",
        command: COMMANDS.updateModgen,
        detail: "L'extension et le générateur sont versionnés séparément. Vous pouvez activer la mise à jour automatique dans les paramètres de l'extension.",
    };
    const tmdgenUpdate = commands.registerCommand(COMMANDS.updateTmdgen, () => updateTool(tools.topmodel));
    context.subscriptions.push(tmdgenUpdate);
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
    registerPreview(context);
    registerChooseCommand();
    new Global(context).registerCommands();
    refreshState();
}

function registerChooseCommand() {
    context.subscriptions.push(
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

function updateStatusBar() {
    switch (extentionState) {
        case "LOADING":
            topModelStatusBar.text = `$(loading~spin)`;
            topModelStatusBar.tooltip = "L'extension TopModel est en cours de chargement";
            break;
        case "RUNNING":
            if (tools.topmodel.currentVersion !== tools.topmodel.latestVersion && tools.topmodel.latestVersion) {
                topModelStatusBar.text = `$(warning)`;
                topModelStatusBar.tooltip = `Le générateur n\'est pas à jour (dernière version v${tools.topmodel.latestVersion})`;
            } else if (tools.tmdgen.currentVersion !== tools.tmdgen.latestVersion && tools.tmdgen.latestVersion) {
                topModelStatusBar.text = `$(warning)`;
                topModelStatusBar.tooltip = `Le générateur tmd n\'est pas à jour (dernière version v${tools.tmdgen.latestVersion})`;
            } else {
                topModelStatusBar.text = `$(check-all)`;
                topModelStatusBar.tooltip = `L\'extension TopModel est démarrée (${applications
                    .map((app) => app.config.app)
                    .join(", ")})`;
            }
            topModelStatusBar.command = COMMANDS.chooseCommand;
            break;
        case "ERROR":
            topModelStatusBar.text = `$(diff-review-close)`;
            topModelStatusBar.tooltip = "l'extension TopModel n'a pas démarré correctement";
            break;
        case "UPDATING":
            topModelStatusBar.text = "$(loading~spin) Mise à jour du générateur";
            break;
        case "INSTALLING":
            topModelStatusBar.text = "$(loading~spin) Installation du générateur";
            break;
    }
    if (extentionState !== "UPDATING" && extentionState !== "INSTALLING") {
        topModelStatusBar.text += ` TopModel`;
        if (tools.topmodel.currentVersion) {
            topModelStatusBar.text += ` v${tools.topmodel.currentVersion}`;
        }
    }

    topModelStatusBar.show();
}

function handleError(exception: TopModelException) {
    window.showErrorMessage(exception.message);
    extentionState = "ERROR";
    updateStatusBar();
}
