import { autorun, makeAutoObservable } from "mobx";
import {
    commands,
    DocumentSelector,
    ExtensionContext,
    extensions,
    languages,
    LanguageStatusItem,
    LanguageStatusSeverity,
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
import { getLanguageServerPath, getServerCommand, killAllLanguageServers } from "./utils";
import { SchemaContentProvider, YamlExtensionApi } from "./schemas";

const SCHEME = "topmodel";
const MODEL_SCHEMA_URI = `${SCHEME}:/schema.json`;

/** Fichiers TopModel : c'est sur ceux-ci que le cartouche des versions des outils est affiché. */
const TOPMODEL_SELECTOR: DocumentSelector = [
    { pattern: "**/*.tmd" },
    { pattern: "**/topmodel*.config" },
    { pattern: "**/tmdgen*.config" },
];

const open = require("open").default;

export class State {
    tools = {
        modgen: new TmdTool("TopModel.Generator", "modgen"),
        tmdgen: new TmdTool("TopModel.ModelGenerator", "tmdgen"),
        ls: new TmdTool("TopModel.LanguageServer", "modls"),
    };
    topModelStatusBar: StatusBarItem;
    private toolLanguageStatusItems: Record<string, LanguageStatusItem>;
    applications: Application[] = [];
    error?: string;
    preview?: TopModelPreviewPanel;
    /** Schéma des fichiers de modèle, servi par le language server. @see loadSchemas */
    private modelSchema?: string;
    private _versionMismatchNotified = false;
    /** Vrai une fois le tool modls initialisé, pour ne pas le réinitialiser (ni réenregistrer ses commandes). */
    private _lsToolInitialized = false;
    constructor(public readonly context: ExtensionContext) {
        makeAutoObservable(this);
        this.topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
        this.context.subscriptions.push(this.topModelStatusBar);
        this.toolLanguageStatusItems = this.createToolLanguageStatusItems();
        autorun(() => this.updateStatusBar());
        autorun(() => this.updateToolLanguageStatusItems());
        autorun(() => this.notifyVersionMismatch());
        this.initTools();
        this.registerCommands();
        this.watchLanguageServerPath();
    }

    /** Un {@link LanguageStatusItem} par outil, affiché dans le cartouche natif au survol des fichiers TopModel. */
    private createToolLanguageStatusItems(): Record<string, LanguageStatusItem> {
        const items: Record<string, LanguageStatusItem> = {};
        for (const tool of Object.values(this.tools)) {
            const item = languages.createLanguageStatusItem(`topmodel.status.${tool.command}`, TOPMODEL_SELECTOR);
            item.name = tool.name;
            this.context.subscriptions.push(item);
            items[tool.command] = item;
        }

        return items;
    }

    private updateToolLanguageStatusItems() {
        for (const tool of Object.values(this.tools)) {
            const item = this.toolLanguageStatusItems[tool.command];
            item.text = tool.statusText;
            item.detail = tool.name;
            item.busy = tool.status === "LOADING" || tool.status === "INSTALLING";
            item.severity =
                tool.status === "ERROR"
                    ? LanguageStatusSeverity.Error
                    : tool.updateAvailable
                      ? LanguageStatusSeverity.Warning
                      : LanguageStatusSeverity.Information;
            if (tool.installed === false && tool.status !== "INSTALLING") {
                item.command = {
                    title: t("installToolButton", [tool.command]),
                    command: `topmodel.${tool.command}.install`,
                };
            } else if (tool.updateAvailable && tool.status !== "INSTALLING") {
                item.command = { title: t("updateTool", [tool.command]), command: `topmodel.${tool.command}.update` };
            } else {
                item.command = undefined;
            }
        }
    }

    get status(): Status {
        if (this.appStatus === "ERROR" || this.error || this.tools.ls.status === "ERROR") {
            return "ERROR";
        }

        if (this.tools.ls.status === "INSTALLING") {
            return "INSTALLING";
        }

        // Tant que le client LSP ou modls n'est pas prêt, on reste en chargement (spinner)
        // sans repasser par WARNING/READY : cela évite que la coche clignote au démarrage.
        if (this.appStatus !== "READY" || this.tools.ls.status === "LOADING") {
            return "LOADING";
        }

        if (this.tools.ls.updateAvailable || !this.versionsAligned) {
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

    get statusIcon() {
        switch (this.status) {
            case "LOADING":
            case "WARNING":
                return "$(warning)";
            default:
                return "$(check-all)";
        }
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
            await this.loadCustomLanguageServerVersion();
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
     * Récupère la version du language server lancé depuis un chemin custom, via `--version` sur
     * l'exécutable réellement configuré (et non le tool global `modls`), pour l'afficher dans la
     * barre de statut même hors du tool global.
     */
    private async loadCustomLanguageServerVersion() {
        const folder = workspace.workspaceFolders?.[0];
        if (!folder) {
            return;
        }

        const { command, args } = getServerCommand(folder);
        const quoted = [command, ...args, "--version"].map((part) => (part.includes(" ") ? `"${part}"` : part));
        await this.tools.ls.loadCurrentVersion(quoted.join(" "));
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
        await this.loadSchemas();
    }

    /**
     * Charge les schémas JSON servis par le language server, pour ne pas avoir à les lui redemander
     * à chaque fichier ouvert.
     *
     * Un seul chargement suffit : les schémas ne dépendent pas du modèle chargé, seulement de la
     * version de modls, si bien que n'importe lequel des serveurs peut les fournir.
     */
    private async loadSchemas() {
        const client = this.applications[0]?.client;
        if (this.modelSchema || !client) {
            return;
        }

        try {
            const response = await client.sendRequest<{ content: string } | null>("schema");
            this.modelSchema = response?.content;
            if (this.modelSchema) {
                this.context.subscriptions.push(
                    workspace.registerTextDocumentContentProvider(SCHEME, new SchemaContentProvider(this.modelSchema!)),
                );

                const api = await extensions.getExtension<YamlExtensionApi>("redhat.vscode-yaml")?.activate();
                api?.registerContributor(
                    SCHEME,
                    (resource) => (resource.endsWith(".tmd") ? MODEL_SCHEMA_URI : undefined),
                    () => this.modelSchema!,
                    "TopModel",
                );
            }
        } catch (error) {
            console.error(error);
        }
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
        this.topModelStatusBar.text = `${this.statusIcon} TopModel`;
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
