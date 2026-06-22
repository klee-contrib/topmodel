import { makeAutoObservable } from "mobx";
import { Terminal, Uri, window, WorkspaceFolder } from "vscode";

import { TopModelConfig } from "./types";
import path = require("path");

export class Application {
    private _terminal?: Terminal;
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

    constructor(
        public readonly workspaceFolder: WorkspaceFolder,
        private readonly configs: { config: TopModelConfig; file: Uri }[],
    ) {
        makeAutoObservable(this);
        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });
    }

    public get modelRootFolders() {
        return this.configs.map((c) => path.dirname(path.resolve(c.file.fsPath, c.config.modelRoot ?? "./")));
    }
}
