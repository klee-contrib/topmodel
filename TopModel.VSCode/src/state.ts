import { autorun, makeAutoObservable } from "mobx";
import {
    commands,
    ExtensionContext,
    StatusBarAlignment,
    StatusBarItem,
    window,
    workspace,
    WorkspaceFolder,
} from "vscode";
import { Application } from "./application";
import { COMMANDS, COMMANDS_OPTIONS, SETTINGS } from "./const";
import { t } from "./i18n";
import { TopModelPreviewPanel } from "./preview";
import { TmdTool } from "./tool";
import { Status } from "./types";
import { getLanguageServerPath, killAllLanguageServers } from "./utils";

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
    /** Vrai une fois le tool modls initialisé, pour ne pas le réinitialiser (ni réenregistrer ses commandes). */
    private _lsToolInitialized = false;
    constructor(public readonly context: ExtensionContext) {
        makeAutoObservable(this);
        this.topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
        this.context.subscriptions.push(this.topModelStatusBar);
        autorun(() => this.updateStatusBar());
        autorun(() => this.notifyVersionMismatch());
        this.initTools();
        this.registerCommands();
        this.watchLanguageServerPath();
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

                const languageServerPath = getLanguageServerPath();
                if (languageServerPath) {
                    tooltip += ` | ${t("customLanguageServer", [languageServerPath])}`;
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
        // Un language server lancé depuis un chemin custom (build local à debugger, autre version
        // sur le poste) ne passe pas par le tool global : ni installation, ni mise à jour, ni
        // contrôle d'alignement des versions.
        if (getLanguageServerPath()) {
            this.tools.ls.status = "READY";
            return true;
        }

        if (this._lsToolInitialized) {
            return this.tools.ls.installed === true;
        }

        // La mise à jour de modls doit arrêter les serveurs (qui verrouillent leurs fichiers) avant
        // `dotnet tool update`, puis les redémarrer. On câble ces hooks avant l'init du tool.
        // On arrête proprement nos propres clients, puis on tue tous les modls de la machine (ceux
        // des autres fenêtres VSCode comprises) afin de libérer l'ensemble des verrous fichiers.
        this.tools.ls.onBeforeUpdate = async () => {
            await this.stopLanguageServer();
            await killAllLanguageServers();
        };
        this.tools.ls.onAfterUpdate = () => this.startLanguageServer();

        this._lsToolInitialized = true;
        await this.tools.ls.init(this.context);
        return this.tools.ls.installed === true;
    }

    /**
     * Applique à chaud un changement de `topmodel.languageServerPath` : le serveur en cours est
     * arrêté puis relancé depuis le nouveau chemin, sans avoir à recharger la fenêtre. Le tool
     * global modls est initialisé à ce moment-là s'il ne l'avait pas été (retour au serveur par
     * défaut alors que l'extension avait démarré sur un chemin custom).
     */
    private watchLanguageServerPath() {
        this.context.subscriptions.push(
            workspace.onDidChangeConfiguration(async (event) => {
                if (!event.affectsConfiguration(`topmodel.${SETTINGS.languageServerPath}`)) {
                    return;
                }

                await this.stopLanguageServer();
                if (await this.installLanguageServerTool()) {
                    await this.startLanguageServer();
                }
            }),
        );
    }

    /**
     * Prend en charge de nouveaux workspace folders : enregistre leurs {@link Application} et démarre
     * leurs clients LSP. Les folders déjà gérés sont ignorés, si bien que l'appel est idempotent.
     */
    public async addApplications(applications: Application[]) {
        const added = applications.filter(
            (app) => !this.applications.some((a) => a.workspaceFolder.uri.fsPath === app.workspaceFolder.uri.fsPath),
        );

        // Mutation en place : la preview conserve une référence sur ce tableau.
        this.applications.push(...added);
        await Promise.all(added.map((app) => app.startLanguageServer()));
    }

    /**
     * Abandonne les workspace folders retirés du workspace : arrête leurs clients LSP (et tue les
     * processus modls correspondants, qui verrouillent les fichiers de l'outil) puis les déréférence.
     */
    public async removeApplications(folders: readonly WorkspaceFolder[]) {
        const removed = this.applications.filter((app) =>
            folders.some((folder) => folder.uri.fsPath === app.workspaceFolder.uri.fsPath),
        );

        for (const app of removed) {
            this.applications.splice(this.applications.indexOf(app), 1);
        }

        await Promise.all(removed.map((app) => app.stopLanguageServer()));
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
