import { LanguageClient, LanguageClientOptions, ServerOptions } from 'vscode-languageclient/node';
import { ExtensionContext, workspace, commands, window, StatusBarItem, StatusBarAlignment, Terminal, Uri, Position, extensions } from 'vscode';
import * as fs from "fs";
import { TopModelConfig, TopModelException } from './types';
import { registerPreview } from './preview';

const open = require('open');
const exec = require('child_process').exec;
const yaml = require("js-yaml");

const SERVER_EXE = 'dotnet';
export const COMMANDS = {
    update: "topmodel.modgen.update",
    modgen: "topmodel.modgen",
    modgenWatch: "topmodel.modgen.watch",
    preview: "topmodel.preview",
    findRef: "topmodel.findRef",
    chooseCommand: "topmodel.chooseCommand"
};

let NEXT_TERM_ID = 1;
let currentTerminal: Terminal;
let lsStarted = false;
let topModelStatusBar: StatusBarItem;
let currentVersion: string;
let lastVersion: string;
let extentionState: "LOADING" | "ERROR" | "RUNNING" | "UPDATING" | "INSTALLING" = "LOADING";

export function activate(context: ExtensionContext) {
    if (!lsStarted) {
        createStatusBar(context);
        checkInstall();
        findConfFile().then((conf) => {
            const config = conf.config;
            const configPath = conf.file.path;
            startLanguageServer(context, configPath, config);
            registerCommands(context, configPath);
        }, error => {
            handleError(error);
        });
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

async function loadLastVersion() {
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
            lastVersion = versions[versions.length - 1];
        });
    });

    req.on('error', (error: any) => {
        console.error(error);
    });

    req.end();
}

async function checkTopModelInsall() {
    await loadLastVersion();
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
        if (currentVersion !== lastVersion) {
            const extensionConfiguration = workspace.getConfiguration('topmodel');
            if (extensionConfiguration.autoUpdate) {
                updateModgen();
            } else {
                const option = "Mettre à jour le générateur";
                const selection = await window.showInformationMessage(`Le générateur TopModel peut être mis à jour (${currentVersion} > ${lastVersion})`, option);
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
        const selection = await window.showInformationMessage(`Le générateur TopModel v${lastVersion} a été installé`, "Voir la release note");
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
        const selection = await window.showInformationMessage(`TopModel a été mis à jour ${currentVersion} --> ${lastVersion}`, "Voir la release note");
        if (selection === "Voir la release note") {
            open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
        }
    });
}

function startModgen(watch: boolean, configPath: string) {
    const terminal = getTerminal();
    terminal.sendText(
        `modgen ${configPath}` + (watch ? " --watch" : "")
    );
    terminal.show();
}

function getTerminal() {
    if (!currentTerminal || !window.terminals.includes(currentTerminal)) {
        currentTerminal = window.createTerminal({
            name: `Topmodel : #${NEXT_TERM_ID++}`,
            message: "Starting modgen in a new terminal"
        });
    }
    return currentTerminal;
}

function registerCommands(context: ExtensionContext, configPath: string) {
    const modgen = commands.registerCommand(
        COMMANDS.modgen,
        () => {
            startModgen(false, configPath);
        }
    );
    const modgenWatch =
        commands.registerCommand(COMMANDS.modgenWatch,
            () => {
                startModgen(true, configPath);
            }
        );
    const modgenUpdate = commands.registerCommand(COMMANDS.update, () => updateModgen());
    context.subscriptions.push( modgenUpdate, modgen, modgenWatch);
    commands.registerCommand(COMMANDS.findRef, async (line: number) => {
        await commands.executeCommand("editor.action.goToLocations", window.activeTextEditor!.document.uri, new Position(line, 0), []);
        await commands.executeCommand("editor.action.goToReferences");

    });
    registerChooseCommand(context);

    return NEXT_TERM_ID;
}

function registerChooseCommand(context: ExtensionContext) {
    context.subscriptions.push(commands.registerCommand(COMMANDS.chooseCommand, async () => {
        const options: { [key: string]: { title: string, description: string, detail?: string, command: typeof COMMANDS[keyof typeof COMMANDS] } } = {
            modgen: {
                title: "Lancer la génération",
                description: "Lance la commande de génération du modèle",
                command: COMMANDS.modgen
            },
            modgenWatch: {
                title: "Lancer la génération en continu",
                description: "Lance la commande de génération du modèle en mode watch",
                command: COMMANDS.modgenWatch
            },
            update: {
                title: "Mettre à jour le générateur",
                description: "Mise à jour manuelle de l'outil de génération",
                command: COMMANDS.update,
                detail: "L'extension et le générateur sont versionnés séparément. Vous pouvez activer la mise à jour automatique dans les paramètres de l'extension."
            }
        };
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

async function findConfFile(): Promise<{ config: TopModelConfig, file: Uri }> {
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
    if (configs.length > 1) {
        throw new TopModelException("Plusieurs fichiers de configuration trouvés. L'extension n'a pas démarré (coming soon)");
    } else if (configs.length === 0) {
        throw new TopModelException("L'extension Topmodel a démarrée car un fichier de configuration se trouvait dans votre workspace, mais il est désormais introuvable.");
    }
    return configs[0];
}

function startLanguageServer(context: ExtensionContext, configPath: string, config: TopModelConfig) {
    // The server is implemented in node

    const args = [context.asAbsolutePath("./language-server/TopModel.LanguageServer.dll")];
    let configRelativePath = workspace.asRelativePath(configPath);
    if ((workspace.workspaceFolders?.length || 0) > 1) {
        configRelativePath = configRelativePath.split("/").splice(1).join('/');
    }
    args.push(configPath.substring(1));
    let serverOptions: ServerOptions = {
        run: { command: SERVER_EXE, args },
        debug: { command: SERVER_EXE, args }
    };
    let configFolderA = configRelativePath.split("/");
    configFolderA.pop();
    const configFolder = configFolderA.join('/');
    let modelRoot = config.modelRoot || configFolder;
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
        registerPreview(context, client);
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
            if (currentVersion !== lastVersion) {
                topModelStatusBar.text = `$(warning)`;
                topModelStatusBar.tooltip = `Le générateur n\'est pas à jour (dernière version v${lastVersion})`;
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