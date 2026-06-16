import { autorun, makeAutoObservable } from "mobx";
import { commands, ExtensionContext, Position, StatusBarAlignment, StatusBarItem, window } from "vscode";
import { Application } from "./application";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";
import { t } from "./i18n";
import { TopModelPreviewPanel } from "./preview";
import { TmdTool } from "./tool";
import { Status } from "./types";
import { killAllLanguageServers } from "./utils";

const open = require("open").default;

export class State {
    tools = {
        modgen: new TmdTool("TopModel.Generator", "modgen"),
        tmdgen: new TmdTool("TopModel.ModelGenerator", "tmdgen"),
        ls: new TmdTool("TopModel.LanguageServer", "modls"),
    };
    topModelStatusBar: StatusBarItem;
    applications: Application[] = [];
    error?: string;
    preview?: TopModelPreviewPanel;
    private _versionMismatchNotified = false;
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
        if (this.appStatus === "ERROR" || this.error) {
            return "ERROR";
        }

        if (this.toolsStatus === "INSTALLING") {
            return "INSTALLING";
        }

        // Tant qu'un client LSP ou un outil n'est pas prêt, on reste en chargement (spinner)
        // sans repasser par WARNING/READY : cela évite que la double-coche clignote au démarrage.
        if (this.appStatus !== "READY" || this.toolsStatus === "LOADING") {
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

    /** Agrège le statut des clients LSP de tous les workspace folders. */
    get appStatus(): Status {
        if (this.applications.some((a) => a.clientStatus === "ERROR")) {
            return "ERROR";
        }

        if (this.applications.some((a) => a.clientStatus === "LOADING")) {
            return "LOADING";
        }

        return "READY";
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
     * Initialise (et installe si nécessaire) le tool modls, partagé par tous les workspace folders.
     * Les clients LSP eux-mêmes (un par workspace folder) sont démarrés par les {@link Application}.
     */
    public async installLanguageServerTool(): Promise<boolean> {
        // La mise à jour de modls doit arrêter les serveurs (qui verrouillent leurs fichiers) avant
        // `dotnet tool update`, puis les redémarrer. On câble ces hooks avant l'init du tool.
        // On arrête proprement nos propres clients, puis on tue tous les modls de la machine (ceux
        // des autres fenêtres VSCode comprises) afin de libérer l'ensemble des verrous fichiers.
        this.tools.ls.onBeforeUpdate = async () => {
            await this.stopLanguageServer();
            await killAllLanguageServers();
        };
        this.tools.ls.onAfterUpdate = () => this.startLanguageServer();

        await this.tools.ls.init(this.context);
        return this.tools.ls.installed === true;
    }

    /** Démarre le language server de chaque workspace folder (idempotent). */
    public async startLanguageServer() {
        await Promise.all(this.applications.map((app) => app.startLanguageServer()));
    }

    /** Arrête le language server de chaque workspace folder (libération des verrous fichiers). */
    public async stopLanguageServer() {
        await Promise.all(this.applications.map((app) => app.stopLanguageServer()));
    }

    /** Arrête puis redémarre le language server de chaque workspace folder (commande utilisateur). */
    public async restartLanguageServer() {
        await Promise.all(this.applications.map((app) => app.restartLanguageServer()));
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
                this.preview = new TopModelPreviewPanel(this.context, this.applications);
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
