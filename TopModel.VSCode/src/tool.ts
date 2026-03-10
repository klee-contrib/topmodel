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
    status?: Status;
    private _terminal?: Terminal;
    constructor(
        public readonly name: "TopModel.Generator" | "TopModel.ModelGenerator",
        public readonly command: "modgen" | "tmdgen",
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
                if (this.installed) {
                    text += " en erreur";
                } else {
                    text += " n'est pas installé";
                }
                break;
            case "INSTALLING":
                text += ` -> v${latestVersion}`;
                break;
            case "LOADING":
                text += `chargement`;
                break;
        }

        return text;
    }

    public async init(context: ExtensionContext) {
        await this.checkInstall();
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
        const buttonText = "Voir la release note";
        const selection = await window.showInformationMessage(text, buttonText);
        if (selection === buttonText) {
            commands.executeCommand(COMMANDS.releaseNote);
        }
    }

    private async install() {
        this.status = "INSTALLING";
        await execute(`dotnet tool install --global ${this.name}`);
        await this.loadCurrentVersion();
        this.showReleaseNote(`L'outil ${this.name} (v${this.currentVersion}) a été installé`);
        this.status = "READY";
    }

    private async loadCurrentVersion() {
        try {
            const result = (await execute(`${this.command} --version`)) as string;
            this.currentVersion = result.replace("\r\n", "");
        } catch (error) {
            console.error("Erreur pendant le chargement de la version courante de l'outil", this, error);
            this.status = "ERROR";
            this.error = "Erreur pendant le chargement de la version courante de l'outil " + this.name;
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
                const shouldUpdate = `Mettre à jour ${this.name}`;
                const showChangelog = "Voir la release note";
                const selection = await window.showInformationMessage(
                    `L'outil ${this.name} peut être mis à jour (${this.currentVersion} > ${this.latestVersion})`,
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
            await execute(`dotnet nuget locals http-cache --clear`);
            await execute(
                `dotnet tool update --global ${this.name}${this.latestVersion ? ` --version ${this.latestVersion}` : ""}`,
            );
            await this.loadCurrentVersion();
            this.status = "READY";
            this.showReleaseNote(`${this.name} a été mis à jour ${oldVersion} --> ${this.currentVersion}`);
        } catch (error) {
            this.status = "ERROR";
            this.error = "Erreur pendant la mise à jour de l'outil " + this.name;
            await window.showInformationMessage("Erreur pendant la mise à jour de l'outil " + this.name + ": " + error);
        }
    }

    private async onInstalledChanged() {
        if (this.installed === false) {
            const option = `Installer ${this.name}`;
            const selection = await window.showInformationMessage(`${this.name} n'est pas installé`, option);
            if (selection === option) {
                await this.install();
            }
        }
    }
    public registerCommands(context: ExtensionContext) {
        this.registerUpdateCommand(context);
        this.registerStartCommand(false, context);
        this.registerStartCommand(true, context);
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
