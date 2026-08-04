import { autorun, makeAutoObservable } from "mobx";
import { commands, ExtensionContext, Terminal, window, workspace } from "vscode";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";
import { t } from "./i18n";
import { Status } from "./types";
import { execute, isWindows } from "./utils";

export class TmdTool {
    currentVersion?: string;
    versions: string[] = [];
    error?: unknown;
    installed?: boolean;
    status: Status = "LOADING";
    /**
     * Hooks optionnels exécutés autour de la mise à jour de l'outil.
     * Utilisés pour le language server : il faut arrêter le processus modls (qui verrouille
     * les fichiers de l'outil) avant `dotnet tool update`, puis le redémarrer ensuite.
     */
    public onBeforeUpdate?: () => Promise<void>;
    public onAfterUpdate?: () => Promise<void>;
    private _terminal?: Terminal;
    constructor(
        public readonly name: "TopModel.Generator" | "TopModel.ModelGenerator" | "TopModel.LanguageServer",
        public readonly command: "modgen" | "tmdgen" | "modls",
    ) {
        makeAutoObservable(this);
        if (this.name === "TopModel.Generator") {
            autorun(() => this.onInstalledChanged());
        }

        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });
    }

    get isGenerator() {
        return this.command !== "modls";
    }

    get latestVersion() {
        if (this.versions.length === 0) {
            return undefined;
        }

        return this.versions[this.versions.length - 1];
    }

    get statusText() {
        const { currentVersion, latestVersion } = this;

        let text = `${this.command}${currentVersion ? " v" + currentVersion : ""}`;

        switch (this.status) {
            case "ERROR":
                text += ` ${this.installed ? t("statusInError") : t("statusNotInstalled")}`;
                break;
            case "INSTALLING":
                text += ` -> v${latestVersion}`;
                break;
            case "LOADING":
                break;
        }

        return text;
    }

    public async init(context: ExtensionContext) {
        await this.checkInstall();
        this.registerInstallCommand(context);
        // Le language server est requis au démarrage de l'extension : on l'installe automatiquement.
        if (!this.installed && this.name === "TopModel.LanguageServer") {
            await this.install();
        }

        if (this.installed) {
            await this.loadCurrentVersion();
            await this.loadVersions();
            this.registerCommands(context);
            await this.tryUpdate();
        } else {
            await this.loadVersions();
            this.status = "ERROR";
        }
    }

    private async loadVersions() {
        try {
            const res = await fetch(`https://api.nuget.org/v3-flatcontainer/${this.name.toLowerCase()}/index.json`);
            const { versions } = (await res.json()) as { versions: string[] };
            this.versions = versions.filter((v) => !v.includes("-") || this.currentVersion?.includes("-"));
        } catch (error) {
            this.error = error;
            this.status = "ERROR";
            console.error(error);
        }
    }

    private async showReleaseNote(text: string) {
        const buttonText = t("releaseNoteButton");
        const selection = await window.showInformationMessage(text, buttonText);
        if (selection === buttonText) {
            commands.executeCommand(COMMANDS.releaseNote);
        }
    }

    public async install() {
        this.status = "INSTALLING";
        if (this.name === "TopModel.LanguageServer") {
            window.showInformationMessage(t("languageServerInstalling"));
        }

        await execute(`dotnet tool install --global ${this.name}`);
        this.installed = true;
        await this.loadCurrentVersion();
        this.showReleaseNote(t("toolInstalled", [this.name, this.currentVersion ?? ""]));
        this.status = "READY";
    }

    public async loadCurrentVersion(overrideCommand?: string) {
        try {
            this.currentVersion = ((await execute(overrideCommand ?? `${this.command} --version`)) as string).trim();
            if (!this.currentVersion) {
                throw new Error(t("versionNotFound", [this.name]));
            }
        } catch (error) {
            console.error(t("versionLoadError", [this.name]), this, error);
            this.status = "ERROR";
            this.error = t("versionLoadError", [this.name]);
        }
    }

    private async checkInstall() {
        let result;
        try {
            if (isWindows) {
                result = await execute(`dotnet tool list -g | find /C /I "${this.name.toLowerCase()}"`);
                this.installed = result === "1\r\n";
            } else {
                result = await execute(`dotnet tool list -g | grep -i ${this.name.toLowerCase()} | wc -l`);
                this.installed = result.trim() === "1";
            }
            // oxlint-disable-next-line no-unused-vars
        } catch (_error: any) {
            result = "Not Installed";
            this.installed = false;
        }
    }

    public get updateAvailable(): boolean {
        return (
            !!this.installed &&
            !!this.currentVersion &&
            !!this.latestVersion &&
            this.currentVersion !== this.latestVersion
        );
    }

    public async tryUpdate() {
        if (this.updateAvailable) {
            const extensionConfiguration = workspace.getConfiguration("topmodel");
            if (extensionConfiguration.autoUpdate) {
                await this.update();
            } else {
                this.status = "READY";
                const shouldUpdate = t("updateTool", [this.name]);
                const showChangelog = t("releaseNoteButton");
                const selection = await window.showInformationMessage(
                    t("toolCanBeUpdated", [this.name, this.currentVersion ?? "", this.latestVersion ?? ""]),
                    shouldUpdate,
                    showChangelog,
                );
                if (selection === shouldUpdate) {
                    await this.update();
                } else if (selection === showChangelog) {
                    await commands.executeCommand(COMMANDS.releaseNote);
                }
            }
        } else {
            this.status = "READY";
        }
    }

    public async update(reloadVersions = false) {
        if (reloadVersions) {
            await this.loadVersions();
        }

        this.status = "INSTALLING";
        const oldVersion = this.currentVersion;
        try {
            // Arrête le processus qui verrouille les fichiers de l'outil (cf. language server).
            await this.onBeforeUpdate?.();
            await execute(`dotnet nuget locals http-cache --clear`);
            await execute(
                `dotnet tool update --global ${this.name}${this.latestVersion ? ` --version ${this.latestVersion}` : ""}`,
            );
            await this.loadCurrentVersion();
            this.status = "READY";
            this.showReleaseNote(t("toolUpdated", [this.name, oldVersion ?? "", this.currentVersion ?? ""]));
        } catch (error) {
            this.status = "ERROR";
            this.error = t("toolUpdateErrorState", [this.name]);
            await window.showInformationMessage(t("toolUpdateError", [this.name, String(error)]));
        } finally {
            // Redémarre le processus arrêté, que la mise à jour ait réussi ou échoué.
            await this.onAfterUpdate?.();
        }
    }

    private async onInstalledChanged() {
        if (this.installed === false) {
            const option = t("installToolButton", [this.name]);
            const selection = await window.showInformationMessage(t("toolNotInstalled", [this.name]), option);
            if (selection === option) {
                await this.install();
            }
        }
    }
    public registerCommands(context: ExtensionContext) {
        this.registerUpdateCommand(context);
        // Le language server n'a pas de commande de génération à lancer.
        if (this.isGenerator) {
            this.registerStartCommand(false, context);
            this.registerStartCommand(true, context);
        }
    }

    private registerInstallCommand(context: ExtensionContext) {
        const installCommandDisposable = commands.registerCommand(`topmodel.${this.command}.install`, () =>
            this.install(),
        );
        COMMANDS_OPTIONS[`topmodel.${this.command}.install`] = {
            title: `${this.command} - ${t("installToolButton", [this.command])}`,
            description: `${t("installToolButton", [this.command])}`,
            command: `topmodel.${this.command}.install`,
        };
        context.subscriptions.push(installCommandDisposable);
    }

    private registerUpdateCommand(context: ExtensionContext) {
        const updateCommandDisposable = commands.registerCommand(`topmodel.${this.command}.update`, () =>
            this.update(true),
        );
        COMMANDS_OPTIONS[`topmodel.${this.command}.update`] = {
            title: `${this.command} - ${t("updateTool", [this.command])}`,
            description: `${t("updateTool", [this.command])}`,
            command: `topmodel.${this.command}.update`,
        };
        context.subscriptions.push(updateCommandDisposable);
    }

    private registerStartCommand(watch: boolean, context: ExtensionContext) {
        const startCommand = `topmodel.${this.command}${watch ? ".watch" : ""}`;
        const modgen = commands.registerCommand(startCommand, () => this.start(watch));
        COMMANDS_OPTIONS[startCommand] = {
            title: `${this.command} - ${t(watch ? "startGenerationWatch" : "startGeneration")}`,
            description: `${t(watch ? "startGenerationWatch" : "startGeneration")}`,
            command: startCommand,
        };
        context.subscriptions.push(modgen);
    }

    private start(watch: boolean) {
        this.terminal.sendText(`\n${this.command} ${watch ? " --watch" : ""}`);
        this.terminal.show();
    }

    private get terminal(): Terminal {
        if (!this._terminal) {
            this._terminal = window.createTerminal({
                name: this.name,
                hideFromUser: true,
            });
        }
        return this._terminal;
    }
}
