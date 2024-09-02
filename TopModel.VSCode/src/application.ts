import { makeAutoObservable } from "mobx";
import { commands, ExtensionContext, Terminal, window, workspace } from "vscode";
import { LanguageClient, ServerOptions } from "vscode-languageclient/node";

import { COMMANDS, COMMANDS_OPTIONS, SERVER_EXE } from "./const";
import { TopModelConfig } from "./types";
import { execute } from "./utils";

export class Application {
    private _terminal?: Terminal;
    public client?: LanguageClient;
    public modelRoot?: string;
    public get terminal(): Terminal {
        if (!this._terminal) {
            this._terminal = window.createTerminal({
                name: `modgen - ${this.config.app}`,
                message: "TopModel",
            });
        }
        this._terminal.show();
        return this._terminal;
    }

    public status: "LOADING" | "STARTED" | "ERROR" = "LOADING";
    constructor(
        public readonly _configPath: string,
        public readonly config: TopModelConfig,
        public readonly extensionContext: ExtensionContext
    ) {
        makeAutoObservable(this);
        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });
        this.start();
    }

    public get configPath() {
        return this._configPath;
    }

    public async start() {
        this.startLanguageServer();
        this.registerCommands();
    }

    public startModgen(watch: boolean) {
        let path = this._configPath;
        if (path.startsWith("/")) {
            path = path.substring(1);
        }

        this.terminal.sendText(`modgen -f ${path}` + (watch ? " --watch" : ""));
        this.terminal.show();
    }

    private async startLanguageServer() {
        const stdout = await execute("dotnet --list-runtimes");
        const runtimeVersion = stdout.includes("Microsoft.NETCore.App 8.0")
            ? "net8.0"
            : stdout.includes("Microsoft.NETCore.App 6.0")
            ? "net6.0"
            : undefined;

        if (!runtimeVersion) {
            window.showErrorMessage("Aucun runtime .NET trouvé pour lancer l'extension TopModel (net6.0 ou net8.0)");
            return;
        }

        const args = [
            this.extensionContext.asAbsolutePath(`./language-server-${runtimeVersion}/TopModel.LanguageServer.dll`),
        ];
        let configRelativePath = workspace.asRelativePath(this._configPath);
        args.push(this._configPath.substring(1));
        let serverOptions: ServerOptions = {
            run: { command: SERVER_EXE, args },
            debug: { command: SERVER_EXE, args },
        };
        let configFolderA = configRelativePath.split("/");
        configFolderA.pop();
        const configFolder = configFolderA.join("/");
        this.modelRoot = this.config.modelRoot ?? configFolder;
        this.client = new LanguageClient(
            `TopModel - ${this.config.app}`,
            `TopModel - ${this.config.app}`,
            serverOptions,
            {
                workspaceFolder: workspace.workspaceFolders?.find((w) => {
                    return this._configPath.toLowerCase().includes(w.uri.path.toLowerCase());
                }),
            }
        );
        await this.client.start();
        this.status = "STARTED";
    }

    private registerCommands() {
        this.registerModgen(false);
        this.registerModgen(true);
    }

    private registerModgen(watch: boolean) {
        const modgenCommand = (watch ? COMMANDS.modgenWatch : COMMANDS.modgen) + " - " + this.config.app;
        const modgen = commands.registerCommand(modgenCommand, () => this.startModgen(watch));
        COMMANDS_OPTIONS[modgenCommand] = {
            title: `${this.config.app} - modgen - Lancer la génération ${watch ? "en continu" : ""}`,
            description: `Lancer la génération${watch ? " continue" : ""} de ${this.config.app}`,
            command: modgenCommand,
        };
        this.extensionContext.subscriptions.push(modgen);
    }
}
