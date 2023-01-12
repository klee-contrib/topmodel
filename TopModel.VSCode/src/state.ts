import { autorun, makeAutoObservable } from "mobx";
import { commands, ExtensionContext, Position, StatusBarAlignment, StatusBarItem, Terminal, window } from "vscode";
import { Application } from "./application";
import { TopModelPreviewPanel } from "./preview";
import { TmdTool } from "./tool";
import { Status } from "./types";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";

export class State {
    private _terminal?: Terminal;
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
        this.registerCommands();
        this.topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
        this.context.subscriptions.push(this.topModelStatusBar);
        window.onDidCloseTerminal((terminal) => {
            if (terminal.name === this._terminal?.name) {
                this._terminal = undefined;
            }
        });

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
                let tooltip = `L\'extension TopModel est démarrée (${this.applications
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
            if (a.status === "LOADING") {
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
        }
        else if (this.tools.tmdgen.installed && this.tools.tmdgen.status === "ERROR" || this.tools.modgen.status === "ERROR") {
            return "ERROR";
        }
        else if (this.tools.tmdgen.installed && this.tools.tmdgen.status === "LOADING" || this.tools.modgen.status === "LOADING") {
            return "LOADING";
        }
        else if (this.tools.tmdgen.installed && this.tools.tmdgen.updateAvailable || this.tools.modgen.updateAvailable) {
            return "WARNING";
        }

        return "READY";
    }

    public get terminal(): Terminal {
        if (!this._terminal) {
            this._terminal = window.createTerminal({
                name: "TopModel"
            });
        }
        this._terminal.show();
        return this._terminal;
    }
    private updateStatusBar() {
        this.topModelStatusBar.text = this.statusText;
        this.topModelStatusBar.tooltip = this.statusTooltip;
        this.topModelStatusBar.command = COMMANDS.chooseCommand;
        this.topModelStatusBar.show();
    }

    private registerCommands() {
        this.registerModgen(false);
        this.registerModgen(true);
        this.registerPreviewCommand();
        this.tools.modgen.registerUpdateCommand(this.context);
        this.tools.tmdgen.registerUpdateCommand(this.context);
        this.registerModgenUpdate();
        this.registerGoToLocation();
        this.registerChooseCommand();
    }

    private registerPreviewCommand() {
        commands.registerCommand(COMMANDS.preview, () => {
            if (!this.preview) {
                this.preview = new TopModelPreviewPanel(this.context, this.applications);
                this.preview.panel.onDidDispose(() => (this.preview = undefined), undefined, this.context.subscriptions);
            }

            this.preview?.panel.reveal();
        });
    }

    private registerModgenUpdate() {
        COMMANDS_OPTIONS.update = {
            title: "Mettre à jour le générateur",
            description: "Mise à jour manuelle de TopModel.Generator (modgen)",
            command: COMMANDS.updateModgen,
            detail: "L'extension et le générateur sont versionnés séparément. Vous pouvez activer la mise à jour automatique dans les paramètres de l'extension.",
        };
    }

    private registerGoToLocation() {
        commands.registerCommand(COMMANDS.findRef, async (line: number) => {
            await commands.executeCommand(
                "editor.action.goToLocations",
                window.activeTextEditor!.document.uri,
                new Position(line, 0),
                []
            );
            await commands.executeCommand("editor.action.goToReferences");
        });
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

    private startModgen(watch: boolean) {
        this.terminal.sendText(`modgen ${watch ? " --watch" : ""}`);
        this.terminal.show();
    }

    private registerModgen(watch: boolean) {
        const modgenCommand = watch ? COMMANDS.modgenWatch : COMMANDS.modgen;
        const modgen = commands.registerCommand(modgenCommand, () => this.startModgen(watch));
        COMMANDS_OPTIONS[modgenCommand] = {
            title: `modgen - Lancer la génération ${watch ? "en continu" : ""}`,
            description: `Lancer la génération ${watch ? "continue " : ""}`,
            command: modgenCommand,
        };
        this.context.subscriptions.push(modgen);
    }
}
