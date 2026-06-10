import { makeAutoObservable } from "mobx";
import { ExtensionContext, Terminal, Uri, window, WorkspaceFolder } from "vscode";
import { LanguageClient, ServerOptions } from "vscode-languageclient/node";

import { SERVER_EXE } from "./const";
import { TopModelConfig } from "./types";
import path = require("path");

export class Application {
    private _terminal?: Terminal;
    public client?: LanguageClient;
    public get terminal(): Terminal {
        if (!this._terminal) {
            this._terminal = window.createTerminal({
                name: `modgen - ${this.workspaceFolder.name}`,
                message: "TopModel",
            });
        }
        this._terminal.show();
        return this._terminal;
    }

    public status: "LOADING" | "STARTED" | "ERROR" = "LOADING";
    constructor(
        public readonly workspaceFolder: WorkspaceFolder,
        private readonly configs: { config: TopModelConfig; file: Uri }[],
        private readonly extensionContext: ExtensionContext,
    ) {
        makeAutoObservable(this);
        this.status = "LOADING";
        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });
        this.startLanguageServer();
    }

    public get modelRootFolders() {
        return this.configs.map((c) => path.dirname(path.resolve(c.file.fsPath, c.config.modelRoot ?? "./")));
    }

    public get configPaths() {
        return this.configs.map((c) => c.file.fsPath);
    }

    public startModgen(watch: boolean) {
        this.terminal.sendText(
            `modgen ${this.configPaths.map((f) => `-f ${f}`).join(" ")}` + (watch ? " --watch" : ""),
        );
        this.terminal.show();
    }

    private async startLanguageServer() {
        const args = [
            this.extensionContext.asAbsolutePath(path.join(`./language-server`, `TopModel.LanguageServer.dll`)),
        ];
        args.push(...this.configPaths.flatMap((f) => ["-f", f]));
        let serverOptions: ServerOptions = {
            run: { command: SERVER_EXE, args },
            debug: { command: SERVER_EXE, args },
        };
        this.client = new LanguageClient(
            `TopModel - ${this.workspaceFolder.name}`,
            `TopModel - ${this.workspaceFolder.name}`,
            serverOptions,
            { workspaceFolder: this.workspaceFolder },
        );
        await this.client.start();

        this.status = "STARTED";
    }
}
