import Ajv from "ajv";
import { readFile } from "fs/promises";
import { load } from "js-yaml";
import { makeAutoObservable } from "mobx";
import { ExtensionContext, Terminal, window, workspace } from "vscode";

import { TopModelConfig } from "./types";
import path = require("path");

export class Application {
    private _terminal?: Terminal;
    public status: "LOADING" | "STARTED" | "ERROR" = "LOADING";

    constructor(
        public readonly _configPath: string,
        public readonly config: TopModelConfig,
        public readonly extensionContext: ExtensionContext,
    ) {
        makeAutoObservable(this);
        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });
        this.start();
    }

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

    public get modelRoot(): string {
        return this.config.modelRoot ?? this.configFolder;
    }

    public get modelRootFolder() {
        return path.dirname(path.resolve(this._configPath, this.modelRoot ?? "./"));
    }

    public get configPath() {
        return this._configPath;
    }

    public get configFolder() {
        return workspace.asRelativePath(path.dirname(this._configPath));
    }

    public get workspaceFolder() {
        return workspace.workspaceFolders?.find((w) => {
            return this._configPath.toLowerCase().includes(w.uri.fsPath.toLowerCase());
        });
    }

    public async start() {
        this.status = (await this.validateConfigFile()) ? "STARTED" : "ERROR";
    }

    public startModgen(watch: boolean) {
        let p = this._configPath;
        this.terminal.sendText(`modgen -f ${p}` + (watch ? " --watch" : ""));
        this.terminal.show();
    }

    public async validateConfigFile() {
        const configFile = await readFile(this._configPath, "utf8");
        const schemaLinePrefix = "# yaml-language-server: $schema=";

        const schemaLine = configFile.split("\n").find((line) => line.startsWith(schemaLinePrefix));

        if (schemaLine) {
            const schemaUrl = path.resolve(
                path.dirname(this._configPath),
                schemaLine.substring(schemaLinePrefix.length).trim().replace("\r", ""),
            );

            try {
                const schemaFile = await readFile(schemaUrl, "utf8");
                const ajv = new Ajv({ allErrors: true, strict: true });
                const validate = ajv.compile(JSON.parse(schemaFile));
                try {
                    const config = load(configFile);
                    return validate(config);
                } catch {
                    return false;
                }
            } catch {
                return true;
            }
        }

        return true;
    }
}
