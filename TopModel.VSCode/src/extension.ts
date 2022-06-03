import { LanguageClient, LanguageClientOptions, ServerOptions } from 'vscode-languageclient/node';
import { Trace } from 'vscode-jsonrpc';
import { ExtensionContext, workspace, commands, window, StatusBarItem, StatusBarAlignment, Terminal, Uri, Position } from 'vscode';
import * as fs from "fs";
import { TopModelConfig, TopModelException } from './types';
import { registerPreview } from './preview';

const open = require('open');
const exec = require('child_process').exec;
const yaml = require("js-yaml");

const SERVER_EXE = 'dotnet';
export const COMMANDS = {
    update: "topmodel.modgen.update",
    install: "topmodel.modgen.install",
    modgen: "topmodel.modgen",
    modgenWatch: "topmodel.modgen.watch",
    preview: "topmodel.preview",
    findRef: "topmodel.findRef"
};

let NEXT_TERM_ID = 1;
let currentTerminal: Terminal;
let lsStarted = false;
let topModelStatusBar: StatusBarItem;

export function activate(context: ExtensionContext) {
    if (!lsStarted) {
        createStatusBar();
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
function createStatusBar() {
    topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
    topModelStatusBar.text = '$(loading~spin) Topmodel';
    topModelStatusBar.tooltip = 'Topmodel is loading configuration';
    topModelStatusBar.show();
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
            checkTopModelInsall();
        }
    });
}

function checkTopModelInsall() {
    execute('dotnet tool list -g | find /C /I "topmodel"', async (result: string) => {
        if (result !== '1\r\n') {
            const option = "Install TopModel";
            const selection = await window.showInformationMessage('TopModel n\'est pas installé', option);
            if (selection === option) {
                commands.executeCommand(COMMANDS.install);
            }
        } else {
            checkTopModelUpdate();
        }
    });
}
async function checkTopModelUpdate() {
    const https = require('https');
    const options = {
        hostname: 'api.nuget.org',
        port: 443,
        path: '/v3-flatcontainer/TopModel.Generator/index.json',
        method: 'GET'
    };

    const req = https.request(options, (res: any) => {
        res.on('data', (reponse: string) => {
            const { versions }: { versions: string[] } = JSON.parse(reponse);
            const latest = versions[versions.length - 1];
            execute(`modgen --version`, async (result: string) => {
                const currentVersion = result.replace('\r\n', '');
                if (currentVersion !== latest) {
                    const option = "Update TopModel";
                    const selection = await window.showInformationMessage(`TopModel peut être mis à jour (${currentVersion} > ${latest})`, option);
                    if (selection === option) {
                        commands.executeCommand(COMMANDS.update);
                    }
                }
            });
        });
    });

    req.on('error', (error: any) => {
        console.error(error);
    });

    req.end();
}


/********************************************************* */
/********************* COMMANDS ************************** */
/********************************************************* */

function installModgen() {
    const terminal = getTerminal();
    terminal.sendText("dotnet tool install --global TopModel.Generator");
    terminal.show();
}

function updateModgen() {
    const terminal = getTerminal();
    terminal.sendText("dotnet tool update --global TopModel.Generator");
    terminal.show();
    open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
}
function startModgen(watch: boolean, configPath: string) {
    const terminal = getTerminal();
    terminal.sendText(
        `modgen ${configPath}` + (watch ? " --watch" : "")
    );
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
    const modgenInstall = commands.registerCommand(COMMANDS.install, () => installModgen());
    const modgenUpdate = commands.registerCommand(COMMANDS.update, () => updateModgen());
    context.subscriptions.push(modgenInstall, modgenUpdate, modgen, modgenWatch);
    commands.registerCommand(COMMANDS.findRef, async (line: number) => {
        await commands.executeCommand("editor.action.goToLocations", window.activeTextEditor!.document.uri, new Position(line, 0), []);
        await commands.executeCommand("editor.action.goToReferences");

    });

    return NEXT_TERM_ID;
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
        throw new TopModelException("Topmodel a démarré car un fichier de configuration se trouvait dans votre workspace, mais il est désormais introuvable.");
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
    client.trace = Trace.Verbose;

    let disposable = client.start();
    client.onReady().then(() => {
        handleLsReady(config, context);
        registerPreview(context, client);
    });


    // Push the disposable to the context's subscriptions so that the
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);
}


function handleLsReady(config: TopModelConfig, context: ExtensionContext): void {
    topModelStatusBar.text = "$(check-all) TopModel";
    topModelStatusBar.tooltip = "TopModel is running for app " + config.app;
    topModelStatusBar.command = "extension.topmodel";
    context.subscriptions.push(topModelStatusBar);
    lsStarted = true;

}

function handleError(exception: TopModelException) {
    window.showErrorMessage(exception.message);
    topModelStatusBar.text = "$(diff-review-close) TopModel";
    topModelStatusBar.tooltip = "TopModel is not running";
}