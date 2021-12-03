import { LanguageClient, LanguageClientOptions, ServerOptions } from 'vscode-languageclient/node';
import { Trace } from 'vscode-jsonrpc';
import { ExtensionContext, workspace, commands, window } from 'vscode';
import * as fs from "fs";
import { TopModelConfig } from './types';

const yaml = require("js-yaml");

let NEXT_TERM_ID = 1;

export function activate(context: ExtensionContext) {


    findConfFile().then((conf) => {
        const config = ((conf as any).config) as TopModelConfig;
        const configPath = (conf as any).configPath;
        if (config) {
            startLanguageServer(context, configPath, config);
            // If the extension is launched in debug mode then the debug server options are used
            // Otherwise the run options are used
            registerCommands(context, configPath);
        } else {
            window.showWarningMessage("Topmodel : No configuration file found");
        }
    });
}

function registerCommands(context: ExtensionContext, configPath: any) {
    context.subscriptions.push(commands.registerCommand(
        "extension.topmodel",
        () => {
            startModgen(false, configPath);
        }
    ));
    context.subscriptions.push(commands.registerCommand(
        "extension.topmodel.watch",
        () => {
            startModgen(true, configPath);

        }
    ));
    return NEXT_TERM_ID;
}

async function findConfFile() {
    return workspace.findFiles("**/*.yaml").then((files) => {
        let config;
        let configPath;
        files.forEach((file) => {
            const doc = fs.readFileSync(file.path.replace("/c", "c"), "utf8");
            doc
                .split("---")
                .filter((e) => e)
                .map(yaml.load)
                .map(e => e as TopModelConfig)
                .forEach((e: TopModelConfig) => {
                    if (e.app) {
                        config = e;
                        configPath = file.path;
                    }
                });
        });
        return { config, configPath };
    });
}

function startLanguageServer(context: ExtensionContext, configPath: any, config: TopModelConfig) {
    window.showInformationMessage("Starting TopModel Language Server");
    // The server is implemented in node
    let serverExe = 'dotnet';

    const args = [context.asAbsolutePath("./net6.0/TopModel.LanguageServer.dll")];
    args.push(configPath.split("/").pop());
    let serverOptions: ServerOptions = {
        run: { command: serverExe, args },
        debug: { command: serverExe, args }
    };
    let configFolderA = configPath.split("/");
    configFolderA.pop();
    const configFolder = configFolderA.join('/');
    const modelRoot = config.modelRoot || configFolder;
    // Options to control the language client
    let clientOptions: LanguageClientOptions = {
        // Register the server for plain text documents
        documentSelector: [{ language: 'yaml' }, { pattern: `${modelRoot}/*.yml` }],
        synchronize: {
            configurationSection: 'topmodel',
            fileEvents: workspace.createFileSystemWatcher(`${modelRoot}/*.yml`)
        },
    };

    // Create the language client and start the client.
    const client = new LanguageClient('topmodel', 'TopModel', serverOptions, clientOptions);
    client.trace = Trace.Verbose;
    let disposable = client.start();

    // Push the disposable to the context's subscriptions so that the
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);
}
function startModgen(watch: boolean, configPath: string) {
    const terminal = window.createTerminal(`Topmodel : #${NEXT_TERM_ID++}`);
    // Display a message box to the user
    terminal.sendText(
        `modgen ${configPath}` + (watch ? "--watch" : "")
    );
}