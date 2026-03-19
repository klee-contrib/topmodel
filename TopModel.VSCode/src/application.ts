import Ajv from "ajv";
import { readFile } from "fs/promises";
import { load } from "js-yaml";
import { makeAutoObservable } from "mobx";
import { ExtensionContext, Terminal, Uri, window, workspace } from "vscode";
import { LanguageClient, ServerOptions } from "vscode-languageclient/node";

import { SERVER_EXE } from "./const";
import { TopModelConfig } from "./types";
import path = require("path");
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
        public readonly extensionContext: ExtensionContext,
        configs: { config: TopModelConfig; file: Uri }[],
    ) {
        makeAutoObservable(this);
        this.status = "LOADING";
        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });
        const shouldStartLanguageServer =
            configs.find(
                (c) =>
                    path.resolve(this.extensionContext.asAbsolutePath(c.file.path), c.config.modelRoot ?? "./") ===
                    this.modelRootPath,
            )?.config === config;
        this.start(shouldStartLanguageServer);

        if (shouldStartLanguageServer) {
            workspace.onDidSaveTextDocument(async (event) => {
                if (event.uri.fsPath.toLowerCase() === this._configPath.toLowerCase()) {
                    if (await this.validateConfigFile()) {
                        await this.client?.stop();
                        this.status = "LOADING";
                        this.startLanguageServer();
                    }
                }
            });
        }
    }

    public get modelRootPath() {
        const cp = this.extensionContext.asAbsolutePath(this._configPath);
        return path.resolve(cp, this.config.modelRoot ?? "./");
    }

    public get modelRootFolder() {
        return path.dirname(path.resolve(this._configPath, this.config.modelRoot ?? "./"));
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

    public async start(shouldStartLanguageServer: boolean) {
        if (shouldStartLanguageServer && (await this.validateConfigFile())) {
            this.startLanguageServer();
        } else {
            this.status = "STARTED";
        }
    }

    public startModgen(watch: boolean) {
        let path = this._configPath;
        this.terminal.sendText(`modgen -f ${path}` + (watch ? " --watch" : ""));
        this.terminal.show();
    }

    private async startLanguageServer() {
        const args = [
            this.extensionContext.asAbsolutePath(path.join(`./language-server`, `TopModel.LanguageServer.dll`)),
        ];
        args.push(this._configPath);
        let serverOptions: ServerOptions = {
            run: { command: SERVER_EXE, args },
            debug: { command: SERVER_EXE, args },
        };
        this.modelRoot = this.config.modelRoot ?? this.configFolder;
        this.client = new LanguageClient(
            `TopModel - ${this.config.app}`,
            `TopModel - ${this.config.app}`,
            serverOptions,
            { workspaceFolder: this.workspaceFolder },
        );
        await this.client.start();
        this.status = "STARTED";
    }
}
