import { ChildProcess, spawn } from "child_process";
import { autorun, makeAutoObservable } from "mobx";
import { commands, ExtensionContext, Position, StatusBarAlignment, StatusBarItem, Uri, window, workspace } from "vscode";
import { LanguageClient, ServerOptions } from "vscode-languageclient/node";
import { Application } from "./application";
import { COMMANDS, COMMANDS_OPTIONS, SERVER_EXE } from "./const";
import { t } from "./i18n";
import { TopModelPreviewPanel } from "./preview";
import { TmdTool } from "./tool";
import { Status, TopModelConfig } from "./types";
import { killProcessTree } from "./utils";

const open = require("open").default;

export class State {
    tools = {
        modgen: new TmdTool("TopModel.Generator", "modgen"),
        tmdgen: new TmdTool("TopModel.ModelGenerator", "tmdgen"),
        ls: new TmdTool("TopModel.LanguageServer", "modls"),
    };
    topModelStatusBar: StatusBarItem;
    applications: Application[] = [];
    client?: LanguageClient;
    clientStatus: Status = "LOADING";
    error?: string;
    preview?: TopModelPreviewPanel;
    private _versionMismatchNotified = false;
    /** Référence au processus modls pour pouvoir le tuer de façon fiable (libération du verrou fichiers). */
    private serverProcess?: ChildProcess;
    /** Fichiers de configuration alimentant le language server, conservés pour les redémarrages. */
    private confs: { config: TopModelConfig; file: Uri }[] = [];
    constructor(public readonly context: ExtensionContext) {
        makeAutoObservable(this);
        this.topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
        this.context.subscriptions.push(this.topModelStatusBar);
        autorun(() => this.updateStatusBar());
        autorun(() => this.notifyVersionMismatch());
        this.initTools();
        this.registerCommands();
    }

    get status(): Status {
        if (this.clientStatus === "ERROR" || this.error) {
            return "ERROR";
        }

        if (this.toolsStatus === "INSTALLING") {
            return "INSTALLING";
        }

        // Tant que le client LSP ou un outil n'est pas prêt, on reste en chargement (spinner)
        // sans repasser par WARNING/READY : cela évite que la double-coche clignote au démarrage.
        if (this.clientStatus !== "READY" || this.toolsStatus === "LOADING") {
            return "LOADING";
        }

        if (this.toolsStatus === "WARNING") {
            return "WARNING";
        }

        return "READY";
    }

    get statusTooltip(): string {
        switch (this.status) {
            case "ERROR":
                return t("extensionStartFailed");
            case "INSTALLING":
                return t("installing");
            case "LOADING":
                return t("loading");
            case "WARNING":
            case "READY":
                let tooltip = t("started", [this.applications.map((app) => app.workspaceFolder.name).join(", ")]);

                if (this.tools.modgen.updateAvailable) {
                    tooltip += ` | ${t("toolCouldBeUpdated", [this.tools.modgen.name])}`;
                }

                if (this.tools.tmdgen.updateAvailable) {
                    tooltip += ` | ${t("toolCouldBeUpdated", [this.tools.tmdgen.name])}`;
                }

                if (this.tools.ls.updateAvailable) {
                    tooltip += ` | ${t("toolCouldBeUpdated", [this.tools.ls.name])}`;
                }

                if (!this.versionsAligned) {
                    tooltip += ` | ${t("toolsVersionMismatch", [this.mismatchedTools])}`;
                }

                return tooltip;
            default:
                return "";
        }
    }

    get statusText() {
        let text = "";

        const { statusText: stModgen } = this.tools.modgen;
        const { installed: stInstalled, statusText: stTmdgen } = this.tools.tmdgen;

        // L'icône est dérivée du statut global (this.status), même source de vérité que le
        // tooltip, pour éviter que l'icône et le tooltip divergent et fassent clignoter la coche.
        switch (this.status) {
            case "LOADING":
            case "INSTALLING":
                text += "$(loading~spin) ";
                break;
            case "WARNING":
                text += "$(warning) ";
                break;
            default:
                text += "$(check-all) ";
        }

        text += stModgen;
        if (stInstalled) {
            text += " | " + stTmdgen;
        }

        return text;
    }

    get toolsStatus(): Status {
        const { status: mStatus, updateAvailable: mUpdate } = this.tools.modgen;
        const { installed: tInstalled, status: tStatus, updateAvailable: tUpdate } = this.tools.tmdgen;
        const { status: lsStatus, updateAvailable: lsUpdate } = this.tools.ls;

        if (tStatus === "INSTALLING" || mStatus === "INSTALLING" || lsStatus === "INSTALLING") {
            return "INSTALLING";
        } else if ((tInstalled && tStatus === "ERROR") || mStatus === "ERROR" || lsStatus === "ERROR") {
            return "ERROR";
        } else if ((tInstalled && tStatus === "LOADING") || mStatus === "LOADING" || lsStatus === "LOADING") {
            return "LOADING";
        } else if ((tInstalled && tUpdate) || mUpdate || lsUpdate || !this.versionsAligned) {
            return "WARNING";
        }

        return "READY";
    }

    /**
     * Vérifie que les versions installées de modls, modgen et tmdgen sont alignées
     * (même version majeure), ces outils étant publiés ensemble dans une release TopModel.
     */
    get versionsAligned(): boolean {
        const major = (version?: string) => version?.split(".")[0];
        const versions = Object.values(this.tools)
            .filter((tool) => tool.installed && tool.currentVersion)
            .map((tool) => major(tool.currentVersion));

        return new Set(versions).size <= 1;
    }

    get mismatchedTools(): string {
        return Object.values(this.tools)
            .filter((tool) => tool.installed && tool.currentVersion)
            .map((tool) => `${tool.command} v${tool.currentVersion}`)
            .join(", ");
    }

    private async initTools() {
        this.tools.modgen.init(this.context);
        this.tools.tmdgen.init(this.context);
    }

    /**
     * Initialise (et installe si nécessaire) le tool modls, puis démarre un unique client LSP
     * couvrant l'ensemble du workspace, alimenté par tous les fichiers de configuration trouvés.
     */
    public async initLanguageServer(confs: { config: TopModelConfig; file: Uri }[]) {
        this.confs = confs;

        // La mise à jour de modls doit arrêter le serveur (qui verrouille ses fichiers) avant
        // `dotnet tool update`, puis le redémarrer. On câble ces hooks avant l'init du tool :
        // si une mise à jour est déclenchée au démarrage, le serveur sera démarré par onAfterUpdate
        // et le start explicite plus bas sera ignoré (idempotence via startLanguageServer).
        this.tools.ls.onBeforeUpdate = () => this.stopLanguageServer();
        this.tools.ls.onAfterUpdate = () => this.startLanguageServer();

        await this.tools.ls.init(this.context);
        if (!this.tools.ls.installed) {
            this.clientStatus = "ERROR";
            return false;
        }

        await this.startLanguageServer();
        return true;
    }

    /**
     * Démarre le client LSP s'il ne tourne pas déjà (idempotent).
     * Le processus modls est lancé par nos soins afin d'en conserver le PID et de pouvoir
     * le tuer de façon fiable lors de l'arrêt ou d'une mise à jour.
     */
    public async startLanguageServer() {
        if (this.client) {
            return;
        }

        if (this.confs.length === 0) {
            this.clientStatus = "READY";
            return;
        }

        try {
            this.clientStatus = "LOADING";
            const args = this.confs.flatMap((conf) => ["-f", conf.file.fsPath]);
            const cwd = workspace.workspaceFolders?.[0]?.uri.fsPath;
            const serverOptions: ServerOptions = () => {
                const proc = spawn(SERVER_EXE, args, { cwd });
                this.serverProcess = proc;
                return Promise.resolve(proc);
            };
            this.client = new LanguageClient("TopModel", "TopModel", serverOptions, {});
            await this.client.start();
            this.clientStatus = "READY";
        } catch (error) {
            this.clientStatus = "ERROR";
            this.error = String(error);
        }
    }

    /**
     * Arrête le client LSP et garantit la mort du processus modls (et de sa descendance),
     * sans quoi les fichiers de l'outil resteraient verrouillés.
     */
    public async stopLanguageServer() {
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

    /** Arrête puis redémarre le language server (commande utilisateur). */
    public async restartLanguageServer() {
        await this.stopLanguageServer();
        await this.startLanguageServer();
    }

    private async notifyVersionMismatch() {
        const toolsReady = Object.values(this.tools).every(
            (tool) => !tool.installed || (tool.status !== "LOADING" && tool.status !== "INSTALLING"),
        );

        if (this._versionMismatchNotified || this.versionsAligned || !toolsReady) {
            return;
        }

        this._versionMismatchNotified = true;
        const updateAll = t("updateAllTools");
        const selection = await window.showWarningMessage(t("toolsVersionMismatch", [this.mismatchedTools]), updateAll);
        if (selection === updateAll) {
            await Promise.all(
                Object.values(this.tools)
                    .filter((tool) => tool.installed && tool.updateAvailable)
                    .map((tool) => tool.update(true)),
            );
        }
    }

    private updateStatusBar() {
        this.topModelStatusBar.text = this.statusText;
        this.topModelStatusBar.tooltip = this.statusTooltip;
        this.topModelStatusBar.command = COMMANDS.chooseCommand;
        this.topModelStatusBar.show();
    }

    private registerCommands() {
        this.registerPreviewCommand();
        this.registerGoToLocation();
        this.registerChooseCommand();
        this.registerReleaseNote();
        this.registerRestartLanguageServer();
    }

    private registerRestartLanguageServer() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.restartLanguageServer, () => this.restartLanguageServer()),
        );
        COMMANDS_OPTIONS[COMMANDS.restartLanguageServer] = {
            title: `modls - ${t("restartLanguageServer")}`,
            description: t("restartLanguageServer"),
            command: COMMANDS.restartLanguageServer,
        };
    }

    private registerPreviewCommand() {
        commands.registerCommand(COMMANDS.preview, () => {
            if (!this.preview) {
                this.preview = new TopModelPreviewPanel(this.context, this.applications, this.client);
                this.preview.panel.onDidDispose(
                    () => (this.preview = undefined),
                    undefined,
                    this.context.subscriptions,
                );
            }

            this.preview?.panel.reveal();
        });
    }

    private registerGoToLocation() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.findRef, async (line: number) => {
                await commands.executeCommand(
                    "editor.action.goToLocations",
                    window.activeTextEditor!.document.uri,
                    new Position(line, 0),
                    [],
                );
                await commands.executeCommand("editor.action.goToReferences");
            }),
        );
    }

    private registerReleaseNote() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.releaseNote, async () => {
                open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
            }),
        );
    }

    private registerChooseCommand() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.chooseCommand, async () => {
                const quickPick = window.createQuickPick();
                quickPick.items = Object.keys(COMMANDS_OPTIONS).map((key) => ({
                    key,
                    label: COMMANDS_OPTIONS[key].title,
                    description: COMMANDS_OPTIONS[key].description,
                    detail: COMMANDS_OPTIONS[key].detail,
                }));
                quickPick.onDidChangeSelection((selection) => {
                    if (selection[0]) {
                        commands.executeCommand(COMMANDS_OPTIONS[(selection[0] as any).key].command);
                        quickPick.hide();
                    }
                });
                quickPick.onDidHide(() => quickPick.dispose());
                quickPick.show();
            }),
        );
    }
}
