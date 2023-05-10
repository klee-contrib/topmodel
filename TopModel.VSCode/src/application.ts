import { LanguageClient, ServerOptions } from "vscode-languageclient/node";
import { ExtensionContext, workspace, commands, window, Terminal } from "vscode";
import { TopModelConfig } from "./types";
import { COMMANDS, COMMANDS_OPTIONS, SERVER_EXE } from "./const";
import { makeAutoObservable } from "mobx";

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
        public readonly configPath: string,
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

    public async start() {
        await this.startLanguageServer();
        this.registerCommands();
        this.status = "STARTED";
    }

    public startModgen(watch: boolean) {
        let path = this.configPath;
        if (path.startsWith("/")) {
            path = path.substring(1);
        }

        this.terminal.sendText(`modgen -f ${path}` + (watch ? " --watch" : ""));
        this.terminal.show();
    }

    private async startLanguageServer() {
        const args = [this.extensionContext.asAbsolutePath("./language-server/TopModel.LanguageServer.dll")];
        let configRelativePath = workspace.asRelativePath(this.configPath);
        if ((workspace.workspaceFolders?.length || 0) > 1) {
            configRelativePath = configRelativePath.split("/").splice(1).join("/");
        }
        args.push(this.configPath.substring(1));
        let serverOptions: ServerOptions = {
            run: { command: SERVER_EXE, args },
            debug: { command: SERVER_EXE, args },
        };
        let configFolderA = configRelativePath.split("/");
        configFolderA.pop();
        const configFolder = configFolderA.join("/");
        this.modelRoot = this.config.modelRoot || configFolder;
        this.client = new LanguageClient(
            `TopModel - ${this.config.app}`,
            `TopModel - ${this.config.app}`,
            serverOptions,
            {}
        );
        await this.client.start();
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
            command: modgenCommand
        };
        this.extensionContext.subscriptions.push(modgen);
    }
}
