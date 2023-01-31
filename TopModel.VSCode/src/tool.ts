import { autorun, makeAutoObservable } from "mobx";
import { request } from "https";
import { execute } from "./utils";
import { commands, ExtensionContext, window, workspace } from "vscode";
import { Status } from "./types";
import { Disposable } from "vscode-jsonrpc";
const open = require("open");

export class TmdTool {
    currentVersion?: string;
    latestVersion?: string;
    error?: string;
    installed?: boolean;
    status?: Status;
    updateCommandDisposable?: Disposable;
    constructor(
        public readonly name: "TopModel.Generator" | "TopModel.ModelGenerator",
        public readonly command: "modgen" | "tmdgen"
    ) {
        makeAutoObservable(this);
        if (this.name === "TopModel.Generator") {
            autorun(() => this.onInstalledChanged());
        }

        autorun(() => this.checkUpdate());
    }

    get statusText(): string {
        let text: string = `${this.command}${this.currentVersion ? " v" + this.currentVersion : ""} `;
        let icon: string | undefined;
        switch (this.status) {
            case "ERROR":
                icon = "diff-review-close";
                if (this.installed) {
                    text += "en erreur";
                } else {
                    text += "n'est pas installé";
                }
                break;
            case "INSTALLING":
                text += `-> v${this.latestVersion}`;
                break;
            case "LOADING":
                icon = "loading~spin";
                text += `chargement`;
                break;
            case "READY":
                return text;
        }
        if (icon) {
            return `$(${icon}) ${text}`;
        } else {
            return text;
        }
    }

    public async init() {
        await Promise.all([this.loadLatestVersion(), this.loadCurrentVersion(), this.checkInstall()]);
        if (this.installed) {
            this.status = "READY";
        } else {
            this.status = "ERROR";
        }
    }

    private async loadLatestVersion() {
        const options = {
            hostname: "api.nuget.org",
            port: 443,
            path: `/v3-flatcontainer/${this.name.toLowerCase()}/index.json`,
            method: "GET",
        };

        const req = request(options, res => {
            res.on("data", async response => {
                const { versions }: { versions: string[] } = JSON.parse(response);
                this.latestVersion = versions[versions.length - 1];
            });
        });

        req.on("error", (error: any) => {
            this.error = error;
            this.status = "ERROR";
            console.error(error);
        });

        req.end();
    }

    private async showReleaseNote(text: string) {
        const buttonText = "Voir la release note";
        const selection = await window.showInformationMessage(
            text,
            buttonText
        );
        if (selection === buttonText) {
            open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
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
            result = await execute(`dotnet tool list -g | find /C /I "${this.name.toLowerCase()}"`);
        } catch (error: any) {
            result = "Not Installed";
        }

        this.installed = result === "1\r\n";
    }

    public get updateAvailable(): boolean {
        return (
            !!this.installed &&
            !!this.currentVersion &&
            !!this.latestVersion &&
            this.currentVersion !== this.latestVersion
        );
    }

    public async checkUpdate() {
        if (this.updateAvailable && this.installed) {
            const extensionConfiguration = workspace.getConfiguration("topmodel");
            if (extensionConfiguration.autoUpdate) {
                this.update();
            } else {
                const option = `Mettre à jour ${this.name}`;
                const selection = await window.showInformationMessage(
                    `L'outil ${this.name} peut être mis à jour (${this.currentVersion} > ${this.latestVersion})`,
                    option
                );
                if (selection === option) {
                    this.update();
                }
            }
        }
    }

    public async update() {
        this.status = "INSTALLING";
        const oldVersion = this.currentVersion;
        await execute(`dotnet nuget locals http-cache --clear`);
        await execute(`dotnet tool update --global ${this.name}`);
        await this.loadCurrentVersion();
        this.status = "READY";
        if (this.latestVersion) {
            this.showReleaseNote(`TopModel a été mis à jour ${oldVersion} --> ${this.latestVersion}`);
        }
    }

    private async onInstalledChanged() {
        if (this.installed === false) {
            const option = `Installer ${this.name}`;
            const selection = await window.showInformationMessage(`${this.name} n'est pas installé`, option);
            if (selection === option) {
                this.install();
            }
        }
    }

    public registerUpdateCommand(context: ExtensionContext) {
        this.updateCommandDisposable = commands.registerCommand(`topmodel.${this.command}.update`, () => this.update());
        context.subscriptions.push(this.updateCommandDisposable);
    }
}
