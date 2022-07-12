import { LanguageClient, LanguageClientOptions, ServerOptions } from 'vscode-languageclient/node';
import { ExtensionContext, workspace, commands, window, StatusBarItem, StatusBarAlignment, Terminal, Uri, Position } from 'vscode';
import * as fs from "fs";
import { ExtensionState, TopModelConfig, TopModelException } from './types';
import { addClient, registerPreview } from './preview';

const open = require('open');
const exec = require('child_process').exec;
const yaml = require('js-yaml');

const SERVER_EXE = 'dotnet';
export const COMMANDS = {
    update: "topmodel.modgen.update",
    modgen: "topmodel.modgen",
    modgenWatch: "topmodel.modgen.watch",
    preview: "topmodel.preview",
    findRef: "topmodel.findRef",
    chooseCommand: "topmodel.chooseCommand"
};

let currentTerminals: Record<string, Terminal> = {};
let lsStarted = false;
let topModelStatusBar: StatusBarItem;
let currentVersion: string;
let latestVersion: string;
let extentionState: ExtensionState = "LOADING";

export async function activate(context: ExtensionContext) {
    if (!lsStarted) {
        createStatusBar(context);
        checkInstall();
        const confs = await findConfFiles();
        try {
            confs.forEach(conf => {
                const config = conf.config;
                const configPath = conf.file.path;
                startLanguageServer(context, configPath, config);
                registerCommands(context, configPath, config);
            });
            registerGlobalCommands(context);
            registerChooseCommand(context);
        } catch (error: any) {
            handleError(error);
        }
    }
}

function createStatusBar(context: ExtensionContext) {
    topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
    context.subscriptions.push(topModelStatusBar);
    updateStatusBar();
}

function execute(command: string, callback: Function) {
    exec(command, function (error: string, stdout: string, stderr: string) { callback(stdout); });
}

/********************************************************* */
/*********************** CHECKS ************************** */
/********************************************************* */
function checkInstall() {
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
        const selection = await window.showInformationMessage(`TopModel a été mis à jour ${currentVersion} --> ${latestVersion}`, "Voir la release note");
        if (selection === "Voir la release note") {
            open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
        }
    });
}

function startModgen(watch: boolean, configPath: string, app: string) {
    const terminal = getTerminal(app);
    let path = configPath;
    if (path.startsWith('/')) {
        path = path.substring(1);
    }

    terminal.sendText(
        `modgen ${path}` + (watch ? " --watch" : "")
    );
    terminal.show();
}

function getTerminal(app: string) {
    if (!currentTerminals[app] || !window.terminals.includes(currentTerminals[app])) {
        currentTerminals[app] = window.createTerminal({
            name: `modgen - ${app}`,
            message: "Démarrage de modgen dans un nouveau terminal"
        });
    }
    return currentTerminals[app];
}

const COMMANDS_OPTIONS: { [key: string]: { title: string, description: string, detail?: string, command: typeof COMMANDS[keyof typeof COMMANDS] } } = {};

function registerGlobalCommands(context: ExtensionContext) {
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
}

function registerCommands(context: ExtensionContext, configPath: string, config: TopModelConfig) {
    const modgen = commands.registerCommand(
        COMMANDS.modgen + " - " + config.app,
        () => {
            startModgen(false, configPath, config.app);
        }
    );
    const modgenWatch =
        commands.registerCommand(COMMANDS.modgenWatch + " - " + config.app,
            () => {
                startModgen(true, configPath, config.app);
            }
        );
    COMMANDS_OPTIONS["modgen-" + config.app] = {
        title: "Lancer la génération de " + config.app,
        description: "Lance la commande de génération du modèle",
        command: COMMANDS.modgen + " - " + config.app,
    };
    COMMANDS_OPTIONS["modgenWatch-" + config.app] = {
        title: "Lancer la génération en continu de " + config.app,
        description: "Lance la commande de génération du modèle en mode watch",
        command: COMMANDS.modgenWatch + " - " + config.app,
    };
    context.subscriptions.push(modgen, modgenWatch);
}

function registerChooseCommand(context: ExtensionContext) {
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

function startLanguageServer(context: ExtensionContext, configPath: string, config: TopModelConfig) {
    // The server is implemented in node

    const args = [context.asAbsolutePath('./language-server/TopModel.LanguageServer.dll')];
    let configRelativePath = workspace.asRelativePath(configPath);
    if ((workspace.workspaceFolders?.length || 0) > 1) {
        configRelativePath = configRelativePath.split('/').splice(1).join('/');
    }
    args.push(configPath.substring(1));
    let serverOptions: ServerOptions = {
        run: { command: SERVER_EXE, args },
        debug: { command: SERVER_EXE, args }
    };
    let configFolderA = configRelativePath.split('/');
    configFolderA.pop();
    const configFolder = configFolderA.join('/');
    const modelRoot = config.modelRoot || configFolder;
    // Options to control the language client
    let clientOptions: LanguageClientOptions = {
        // Register the server for plain text documents
        documentSelector: [{ pattern: `${modelRoot}**/tmd` }],
        synchronize: {
            configurationSection: 'topmodel',
            fileEvents: workspace.createFileSystemWatcher(`${modelRoot}**/*.tmd`)
        },
    };

    // Create the language client and start the client.
    const client = new LanguageClient('topmodel', 'TopModel', serverOptions, clientOptions);

    let disposable = client.start();
    client.onReady().then(() => {
        handleLsReady(context);
        updateStatusBar();
        addClient(client, modelRoot, config);
    });

    // Push the disposable to the context's subscriptions so that the
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);
}

function handleLsReady(context: ExtensionContext): void {
    extentionState = "RUNNING";
    context.subscriptions.push(topModelStatusBar);
    lsStarted = true;
}

function updateStatusBar() {
    switch (extentionState) {
        case 'LOADING':
            topModelStatusBar.text = `$(loading~spin)`;
            topModelStatusBar.tooltip = 'L\'extension TopModel est en cours de chargement';
            break;
        case 'RUNNING':
            topModelStatusBar.text = `$(warning)`;
            if (currentVersion !== latestVersion) {
                topModelStatusBar.text = `$(warning)`;
                topModelStatusBar.tooltip = `Le générateur n\'est pas à jour (dernière version v${latestVersion})`;
            } else {
                topModelStatusBar.text = `$(check-all)`;
                topModelStatusBar.tooltip = 'L\'extension TopModel est démarré';
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
