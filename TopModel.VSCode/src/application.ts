import { ChildProcess, spawn } from "child_process";
import { makeAutoObservable } from "mobx";
import { Terminal, Uri, window, WorkspaceFolder } from "vscode";
import { CloseAction, ErrorAction, LanguageClient, ServerOptions } from "vscode-languageclient/node";

import { t } from "./i18n";
import { Status, TopModelConfig } from "./types";
import { getServerCommand, killProcessTree } from "./utils";
import path = require("path");

export class Application {
    /** Délai avant redémarrage auto : laisse à une mise à jour en cours (autre fenêtre) le temps
     * de terminer avant de relancer modls, qui re-verrouillerait sinon les fichiers de l'outil. */
    private static readonly AUTO_RESTART_DELAY_MS = 3000;
    /** Plafond de redémarrages automatiques consécutifs, pour éviter une boucle si modls est cassé. */
    private static readonly MAX_AUTO_RESTARTS = 5;

    private _terminal?: Terminal;
    /** Client LSP propre à ce workspace folder. */
    public client?: LanguageClient;
    public clientStatus: Status = "LOADING";
    /** Référence au processus modls pour pouvoir le tuer de façon fiable (libération du verrou fichiers). */
    private serverProcess?: ChildProcess;
    /** Nombre de redémarrages automatiques consécutifs (remis à zéro dès que le serveur est prêt). */
    private autoRestartAttempts = 0;
    /** Timer du redémarrage auto en attente, annulé dès qu'une action de cycle de vie délibérée a lieu. */
    private autoRestartTimer?: ReturnType<typeof setTimeout>;

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

    /** Racines de modèle de ce workspace folder, résolues depuis le dossier de chaque fichier de config. */
    public get modelRootFolders() {
        return this.configs.map((c) => path.resolve(path.dirname(c.file.fsPath), c.config.modelRoot ?? "./"));
    }

    private get configPaths() {
        return this.configs.map((c) => c.file.fsPath);
    }

    /**
     * Démarre le client LSP de ce workspace folder s'il ne tourne pas déjà (idempotent).
     * Le processus modls est lancé par nos soins afin d'en conserver le PID et de pouvoir
     * le tuer de façon fiable lors de l'arrêt ou d'une mise à jour.
     */
    public async startLanguageServer() {
        // Un démarrage délibéré rend caduc tout redémarrage auto en attente.
        this.cancelPendingAutoRestart();

        if (this.client) {
            return;
        }

        if (this.configs.length === 0) {
            this.clientStatus = "READY";
            return;
        }

        try {
            this.clientStatus = "LOADING";
            // La commande dépend du paramètre `topmodel.languageServerPath` : tool global `modls`
            // par défaut, ou l'exécutable indiqué par l'utilisateur (debug, version alternative).
            const { command, args: commandArgs } = getServerCommand(this.workspaceFolder);
            const args = [...commandArgs, ...this.configPaths.flatMap((f) => ["-f", f])];
            const cwd = this.workspaceFolder.uri.fsPath;
            const serverOptions: ServerOptions = () => {
                const proc = spawn(command, args, { cwd });
                this.serverProcess = proc;
                proc.on("error", (error) => this.onServerProcessError(proc, command, error));
                proc.on("exit", () => this.onServerProcessExit(proc));
                return Promise.resolve(proc);
            };
            // Glob absolu scopant les providers aux fichiers de CE workspace folder.
            const folderPattern = `${this.workspaceFolder.uri.fsPath.replace(/\\/g, "/")}/**/*.tmd`;
            this.client = new LanguageClient(
                `TopModel - ${this.workspaceFolder.name}`,
                `TopModel - ${this.workspaceFolder.name}`,
                serverOptions,
                {
                    workspaceFolder: this.workspaceFolder,
                    documentSelector: [{ pattern: folderPattern }],
                    errorHandler: {
                        error: () => ({ action: ErrorAction.Continue }),
                        closed: () => ({ action: CloseAction.DoNotRestart }),
                    },
                },
            );
            await this.client.start();
            this.clientStatus = "READY";
            this.autoRestartAttempts = 0;
        } catch (error) {
            this.clientStatus = "ERROR";
            console.error(error);
        }
    }

    /**
     * Arrête le client LSP et garantit la mort du processus modls (et de sa descendance),
     * sans quoi les fichiers de l'outil resteraient verrouillés.
     */
    public async stopLanguageServer() {
        // Un arrêt délibéré rend caduc tout redémarrage auto en attente.
        this.cancelPendingAutoRestart();

        const proc = this.serverProcess;
        this.serverProcess = undefined;

        if (this.client) {
            try {
                await this.client.dispose();
            } catch (error) {
                console.error(error);
            }
            this.client = undefined;
        }

        if (proc && proc.exitCode === null) {
            await killProcessTree(proc.pid);
        }
    }

    /** Arrête puis redémarre le language server de ce workspace folder. */
    public async restartLanguageServer() {
        await this.stopLanguageServer();
        await this.startLanguageServer();
    }

    /**
     * Appelé quand le processus n'a pas pu être lancé (typiquement un `languageServerPath` erroné).
     * On remonte l'erreur à l'utilisateur et on n'enchaîne pas sur le redémarrage automatique, qui
     * ne ferait que répéter le même échec.
     */
    private onServerProcessError(proc: ChildProcess, command: string, error: Error) {
        if (this.serverProcess !== proc) {
            return;
        }

        this.serverProcess = undefined;
        this.clientStatus = "ERROR";
        console.error(error);
        window.showErrorMessage(t("languageServerStartFailed", [command, error.message]));
    }

    /**
     * Appelé quand le processus modls s'arrête. S'il a été tué par un tiers (typiquement la mise à
     * jour de l'outil déclenchée depuis une autre fenêtre VSCode, qui doit arrêter tous les modls
     * pour libérer les verrous), on le redémarre automatiquement pour rétablir le service.
     *
     * Les arrêts volontaires (stop/restart de notre fait) sont ignorés : `serverProcess` a alors
     * déjà été vidé ou remplacé, si bien que `this.serverProcess !== proc`.
     */
    private onServerProcessExit(proc: ChildProcess) {
        if (this.serverProcess !== proc) {
            return;
        }

        this.serverProcess = undefined;

        if (this.autoRestartAttempts >= Application.MAX_AUTO_RESTARTS) {
            this.clientStatus = "ERROR";
            return;
        }

        this.autoRestartAttempts++;
        this.clientStatus = "LOADING";
        this.autoRestartTimer = setTimeout(() => {
            this.autoRestartTimer = undefined;
            this.restartLanguageServer();
        }, Application.AUTO_RESTART_DELAY_MS);
    }

    /** Annule un redémarrage automatique programmé mais pas encore déclenché. */
    private cancelPendingAutoRestart() {
        if (this.autoRestartTimer) {
            clearTimeout(this.autoRestartTimer);
            this.autoRestartTimer = undefined;
        }
    }
}
