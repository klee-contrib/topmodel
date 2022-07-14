import { ExtensionContext, workspace, commands, window, StatusBarItem, StatusBarAlignment, Uri, Position } from 'vscode';
import * as fs from "fs";
import { ExtensionState, TopModelConfig, TopModelException } from './types';
import { registerPreview } from './preview';
import { Application } from './application';
import { COMMANDS, COMMANDS_OPTIONS } from './const';
import { execute } from './utils';

const open = require('open');
const yaml = require('js-yaml');

let topModelStatusBar: StatusBarItem;
let currentVersion: string;
let latestVersion: string;
let extentionState: ExtensionState = "LOADING";
let context: ExtensionContext;
const applications: Application[] = [];

export async function activate(ctx: ExtensionContext) {
    context = ctx;
    createStatusBar();
    checkInstall();
    if (applications.length === 0) {
        const confs = await findConfFiles();
        try {
            applications.push(...confs.map(conf => new Application(conf.file.path, conf.config, ctx)));
            applications.forEach(async app => {
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
    applications.forEach(a => {
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
    execute('echo ;%PATH%; | find /C /I "dotnet"', async (dotnetIsInstalled: string) => {
        if (dotnetIsInstalled !== '1\r\n') {
            const selection = await window.showInformationMessage('Dotnet is not installed', "Show download page");
            if (selection === "Show download page") {
                open("https://dotnet.microsoft.com/download/dotnet/6.0");
            }
        } else {
            await checkTopModelInsall();
        }
    });
}

async function loadlatestVersionVersion() {
    const https = require('https');
    const options = {
        hostname: 'api.nuget.org',
        port: 443,
        path: '/v3-flatcontainer/topmodel.generator/index.json',
        method: 'GET'
    };

    const req = await https.request(options, (res: any) => {
        res.on('data', async (reponse: string) => {
            const { versions }: { versions: string[] } = JSON.parse(reponse);
            latestVersion = versions[versions.length - 1];
        });
    });

    req.on('error', (error: any) => {
        console.error(error);
    });

    req.end();
}

async function checkTopModelInsall() {
    await loadlatestVersionVersion();
    execute('dotnet tool list -g | find /C /I "topmodel"', async (result: string) => {
        if (result !== '1\r\n') {
            const option = "Install TopModel";
            const selection = await window.showInformationMessage('TopModel n\'est pas installé', option);
            if (selection === option) {
                installModgen();
            }
        } else {
            checkTopModelUpdate();
        }
    });
}

async function checkTopModelUpdate() {
    await loadCurrentVersion(async () => {
        if (currentVersion !== latestVersion && latestVersion) {
            const extensionConfiguration = workspace.getConfiguration('topmodel');
            if (extensionConfiguration.autoUpdate) {
                updateModgen();
            } else {
                const option = "Mettre à jour le générateur";
                const selection = await window.showInformationMessage(`Le générateur TopModel peut être mis à jour (${currentVersion} > ${latestVersion})`, option);
                if (selection === option) {
                    updateModgen();
                }
            }
        }
    });
}

async function loadCurrentVersion(_callBack?: Function) {
    execute(`modgen --version`, async (result: string) => {
        currentVersion = result.replace('\r\n', '');
        if (_callBack) {
            _callBack();
        }
        updateStatusBar();
    });
}

/***********************************************************/
/********************* COMMANDS ****************************/
/***********************************************************/

function installModgen() {
    extentionState = "INSTALLING";
    updateStatusBar();
    execute(`dotnet tool install --global TopModel.Generator`, async () => {
        loadCurrentVersion();
        extentionState = "RUNNING";
        updateStatusBar();
        const selection = await window.showInformationMessage(`Le générateur TopModel v${latestVersion} a été installé`, "Voir la release note");
        if (selection === "Voir la release note") {
            open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
        }
    });
}

function updateModgen() {
    extentionState = "UPDATING";
    updateStatusBar();
    execute(`dotnet tool update --global TopModel.Generator`, async () => {
        loadCurrentVersion();
        extentionState = "RUNNING";
        updateStatusBar();
        if (latestVersion) {
            const selection = await window.showInformationMessage(`TopModel a été mis à jour ${currentVersion} --> ${latestVersion}`, "Voir la release note");
            if (selection === "Voir la release note") {
                open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
            }
        }
    });
}

function registerGlobalCommands() {
    const modgenUpdate = commands.registerCommand(COMMANDS.update, () => updateModgen());
    context.subscriptions.push(modgenUpdate);
    COMMANDS_OPTIONS.update = {
        title: "Mettre à jour le générateur",
        description: "Mise à jour manuelle de l'outil de génération",
        command: COMMANDS.update,
        detail: "L'extension et le générateur sont versionnés séparément. Vous pouvez activer la mise à jour automatique dans les paramètres de l'extension."

    };
    commands.registerCommand(COMMANDS.findRef, async (line: number) => {
        await commands.executeCommand("editor.action.goToLocations", window.activeTextEditor!.document.uri, new Position(line, 0), []);
        await commands.executeCommand("editor.action.goToReferences");
    });
    registerPreview(context);
    registerChooseCommand();
    registerModgen(true);
    registerModgen(false);
    refreshState();
}

function registerModgen(watch: boolean) {
    const modgen = commands.registerCommand(
        watch ? COMMANDS.modgenWatch : COMMANDS.modgen,
        () => {
            applications.forEach(app => {
                app.startModgen(watch);
            });
        });
    context.subscriptions.push(modgen);
}

function registerChooseCommand() {
    context.subscriptions.push(commands.registerCommand(COMMANDS.chooseCommand, async () => {
        const options = COMMANDS_OPTIONS;
        const quickPick = window.createQuickPick();
        quickPick.items = Object.keys(options).map(key => ({ key, label: options[key].title, description: options[key].description, detail: options[key].detail }));
        quickPick.onDidChangeSelection(selection => {
            if (selection[0]) {
                commands.executeCommand(options[(selection[0] as any).key].command);
                quickPick.hide();
            }
        });
        quickPick.onDidHide(() => quickPick.dispose());
        quickPick.show();
    }));
}

async function findConfFiles(): Promise<{ config: TopModelConfig, file: Uri }[]> {
    const files = await workspace.findFiles("**/topmodel*.config");
    let configs: { config: TopModelConfig, file: Uri }[] = files.map((file) => {
        const doc = fs.readFileSync(file.path.substring(1), "utf8");
        const c = doc
            .split("---")
            .filter(e => e)
            .map(yaml.load)
            .map(e => e as TopModelConfig)
            .filter(e => e.app)
        [0];
        return { config: c, file };
    });

    return configs;
}

function updateStatusBar() {
    switch (extentionState) {
        case 'LOADING':
            topModelStatusBar.text = `$(loading~spin)`;
            topModelStatusBar.tooltip = 'L\'extension TopModel est en cours de chargement';
            break;
        case 'RUNNING':
            if (currentVersion !== latestVersion && latestVersion) {
                topModelStatusBar.text = `$(warning)`;
                topModelStatusBar.tooltip = `Le générateur n\'est pas à jour (dernière version v${latestVersion})`;
            } else {
                topModelStatusBar.text = `$(check-all)`;
                topModelStatusBar.tooltip = `L\'extension TopModel est démarrée (${applications.map(app => app.config.app).join(', ')})`;
            }
            topModelStatusBar.command = COMMANDS.chooseCommand;
            break;
        case 'ERROR':
            topModelStatusBar.text = `$(diff-review-close)`;
            topModelStatusBar.tooltip = "l\'extension TopModel n'a pas démarré correctement";
            break;
        case 'UPDATING':
            topModelStatusBar.text = "$(loading~spin) Mise à jour du générateur";
            break;
        case 'INSTALLING':
            topModelStatusBar.text = "$(loading~spin) Installation du générateur";
            break;
    }
    if (extentionState !== "UPDATING" && extentionState !== "INSTALLING") {
        topModelStatusBar.text += ` TopModel`;
        if (currentVersion) {
            topModelStatusBar.text += ` v${currentVersion}`;
        }
    }

    topModelStatusBar.show();
}

function handleError(exception: TopModelException) {
    window.showErrorMessage(exception.message);
    extentionState = "ERROR";
    updateStatusBar();
}
