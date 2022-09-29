import { LanguageClient, ServerOptions } from "vscode-languageclient/node";
import { ExtensionContext, workspace, commands, window, Terminal } from "vscode";
import { TopModelConfig } from "./types";
import { addPreviewApplication } from "./preview";
import { COMMANDS, COMMANDS_OPTIONS, SERVER_EXE } from "./const";

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

    public status: "LOADING" | "STARTED" = "LOADING";
    constructor(
        private readonly configPath: string,
        public readonly config: TopModelConfig,
        public readonly extensionContext: ExtensionContext
    ) {
        window.onDidCloseTerminal((terminal) => {
            if (terminal === this._terminal) {
                this._terminal = undefined;
            }
        });
    }

    public async start() {
        await this.startLanguageServer();
        this.registerCommands();
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
        // Create the language client and start the client.
        this.client = new LanguageClient(
            `TopModel - ${this.config.app}`,
            `TopModel - ${this.config.app}`,
            serverOptions,
            {}
        );
        await this.client.start();
        addPreviewApplication(this);
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
            title: `${this.config.app} - Lancer la génération ${watch ? "en continu" : ""}`,
            description: `Lancer la génération ${watch ? "continue " : ""} de ${this.config.app}`,
            command: modgenCommand,
        };
        this.extensionContext.subscriptions.push(modgen);
    }
}
