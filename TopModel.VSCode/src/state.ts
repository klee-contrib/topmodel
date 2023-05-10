import { autorun, makeAutoObservable } from "mobx";
import {
    commands,
    ExtensionContext,
    Position,
    StatusBarAlignment,
    StatusBarItem,
    Uri,
    window,
    workspace,
} from "vscode";
import { Application } from "./application";
import { TopModelPreviewPanel } from "./preview";
import { TmdTool } from "./tool";
import { Status } from "./types";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";
import { execute } from "./utils";

const open = require("open");

export class State {
    tools = {
        modgen: new TmdTool("TopModel.Generator", "modgen"),
        tmdgen: new TmdTool("TopModel.ModelGenerator", "tmdgen"),
    };
    topModelStatusBar: StatusBarItem;
    applications: Application[] = [];
    error?: string;
    preview?: TopModelPreviewPanel;
    constructor(public readonly context: ExtensionContext) {
        makeAutoObservable(this);
        this.initTools();
        this.registerCommands();
        this.topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
        this.context.subscriptions.push(this.topModelStatusBar);
        autorun(() => this.updateStatusBar());
    }

    get status(): Status {
        let status: Status = "LOADING";
        if (this.appStatus === "READY" && this.toolsStatus === "READY") {
            status = "READY";
        }

        if (this.toolsStatus === "INSTALLING") {
            status = "INSTALLING";
        } else if (this.toolsStatus === "WARNING") {
            status = "WARNING";
        }

        if (this.error) {
            status = "ERROR";
        }

        return status;
    }

    get statusTooltip(): string {
        switch (this.status) {
            case "ERROR":
                return "L'extension TopModel n'a pas démarré correctement";
            case "INSTALLING":
                return "Installation en cours...";
            case "LOADING":
                return "Chargement en cours...";
            case "WARNING":
            case "READY":
                let tooltip = `L'extension TopModel est démarrée (${this.applications
                    .map((app) => app.config.app)
                    .join(", ")})`;

                if (this.tools.modgen.updateAvailable) {
                    tooltip += ` | L'outil ${this.tools.modgen.name} pourrait être mis à jour`;
                }

                if (this.tools.tmdgen.updateAvailable) {
                    tooltip += ` | L'outil ${this.tools.tmdgen.name} pourrait être mis à jour`;
                }

                return tooltip;
        }
    }

    get statusText(): string {
        let text = "";
        if (this.appStatus === "LOADING" || this.toolsStatus === "INSTALLING" || this.toolsStatus === "LOADING") {
            text += "$(loading~spin) ";
        } else if (this.toolsStatus === "WARNING") {
            text += "$(warning) ";
        } else {
            text += "$(check-all) ";
        }

        text += this.tools.modgen.statusText;
        if (this.tools.tmdgen.installed) {
            text += " | " + this.tools.tmdgen.statusText;
        }

        return text;
    }

    get appStatus(): Status {
        let status: Status = this.applications.length === 0 ? "LOADING" : "READY";
        this.applications.forEach((a) => {
            if (status !== "ERROR" && a.status === "LOADING") {
                status = "LOADING";
            } else if (a.status === "ERROR") {
                status = "ERROR";
            }
        });
        return status;
    }

    get toolsStatus(): Status {
        if (this.tools.tmdgen.status === "INSTALLING" || this.tools.modgen.status === "INSTALLING") {
            return "INSTALLING";
        } else if (
            (this.tools.tmdgen.installed && this.tools.tmdgen.status === "ERROR") ||
            this.tools.modgen.status === "ERROR"
        ) {
            return "ERROR";
        } else if (
            (this.tools.tmdgen.installed && this.tools.tmdgen.status === "LOADING") ||
            this.tools.modgen.status === "LOADING"
        ) {
            return "LOADING";
        } else if (
            (this.tools.tmdgen.installed && this.tools.tmdgen.updateAvailable) ||
            this.tools.modgen.updateAvailable
        ) {
            return "WARNING";
        }

        return "READY";
    }

    private async initTools() {
        await this.tools.modgen.init(this.context);
        await this.tools.tmdgen.init(this.context);
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
        this.registerSchema();
        this.registerUdpateSettings();
        this.registerChooseCommand();
        this.registerReleaseNote();
    }

    private registerPreviewCommand() {
        commands.registerCommand(COMMANDS.preview, () => {
            if (!this.preview) {
                this.preview = new TopModelPreviewPanel(this.context, this.applications);
                this.preview.panel.onDidDispose(
                    () => (this.preview = undefined),
                    undefined,
                    this.context.subscriptions
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
                    []
                );
                await commands.executeCommand("editor.action.goToReferences");
            })
        );
    }

    private registerSchema() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.schema, async () => {
                await execute(`modgen -s`);
            })
        );
    }

    private registerUdpateSettings() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.updateSettings, async () => {
                const textDecoder = new TextDecoder();
                const textEncoder = new TextEncoder();
                const settingFiles = await workspace.findFiles(".vscode/settings.json");
                let settings: any;
                let uri: Uri;
                if (settingFiles.length === 1) {
                    uri = settingFiles[0];
                    const file = await workspace.fs.readFile(uri);
                    const settingsFile = textDecoder.decode(file);
                    settings = JSON.parse(settingsFile);
                } else {
                    settings = {};
                    uri = Uri.joinPath(workspace.workspaceFolders![0].uri, ".vscode", "settings.json");
                }
                settings["yaml.schemas"] = {};
                for (let appKey in this.applications) {
                    const application = this.applications[appKey];
                    const relativePath = workspace.asRelativePath(application.configPath);
                    settings["yaml.schemas"][`${relativePath}.schema.json`] = relativePath;
                }

                workspace.fs.writeFile(uri, textEncoder.encode(JSON.stringify(settings, null, 2)));
            })
        );
    }

    private registerReleaseNote() {
        this.context.subscriptions.push(
            commands.registerCommand(COMMANDS.releaseNote, async () => {
                open("https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md");
            })
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
            })
        );
    }
}
