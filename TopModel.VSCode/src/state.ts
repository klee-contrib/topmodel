import { autorun, makeAutoObservable } from "mobx";
import { commands, ExtensionContext, Position, StatusBarAlignment, StatusBarItem, window, workspace } from "vscode";
import { LanguageClient, ServerOptions } from "vscode-languageclient/node";
import { Application } from "./application";
import { COMMANDS, COMMANDS_OPTIONS, SERVER_EXE } from "./const";
import { t } from "./i18n";
import { TopModelPreviewPanel } from "./preview";
import { TmdTool } from "./tool";
import { Status } from "./types";
import path = require("path");

const open = require("open");

export class State {
    tools = {
        modgen: new TmdTool("TopModel.Generator", "modgen"),
        tmdgen: new TmdTool("TopModel.ModelGenerator", "tmdgen"),
    };
    topModelStatusBar: StatusBarItem;
    applications: Application[] = [];
    _client?: LanguageClient;
    lspStatus: "LOADING" | "READY" | "ERROR" = "LOADING";
    error?: string;
    preview?: TopModelPreviewPanel;
    constructor(public readonly context: ExtensionContext) {
        makeAutoObservable(this);
        this.topModelStatusBar = window.createStatusBarItem(StatusBarAlignment.Right, 100);
        this.context.subscriptions.push(this.topModelStatusBar);
        autorun(() => this.updateStatusBar());
        this.initTools();
        this.registerCommands();
    }

    get client() {
        return this._client;
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
                return t("extensionStartFailed");
            case "INSTALLING":
                return t("installing");
            case "LOADING":
                return t("loading");
            case "WARNING":
            case "READY":
                let tooltip = t("started", [this.applications.map((app) => app.config.app).join(", ")]);

                if (this.tools.modgen.updateAvailable) {
                    tooltip += ` | ${t("toolCouldBeUpdated", [this.tools.modgen.name])}`;
                }

                if (this.tools.tmdgen.updateAvailable) {
                    tooltip += ` | ${t("toolCouldBeUpdated", [this.tools.tmdgen.name])}`;
                }

                return tooltip;
            default:
                return "";
        }
    }

    get statusText() {
        let text = "";

        const { appStatus, toolsStatus } = this;
        const { statusText: stModgen } = this.tools.modgen;
        const { installed: stInstalled, statusText: stTmdgen } = this.tools.tmdgen;

        if (appStatus === "LOADING" || toolsStatus === "INSTALLING" || toolsStatus === "LOADING") {
            text += "$(loading~spin) ";
        } else if (toolsStatus === "WARNING") {
            text += "$(warning) ";
        } else {
            text += "$(check-all) ";
        }

        text += stModgen;
        if (stInstalled) {
            text += " | " + stTmdgen;
        }

        return text;
    }

    get appStatus(): Status {
        if (this.applications.some((a) => a.status === "ERROR") || this.lspStatus === "ERROR") return "ERROR";
        if (this.applications.some((a) => a.status === "LOADING") || this.lspStatus === "LOADING") return "LOADING";
        return "READY";
    }

    async startLanguageServer(configFiles: string[]): Promise<void> {
        this.lspStatus = "LOADING";
        try {
            const args = [
                this.context.asAbsolutePath(path.join("./language-server", "TopModel.LanguageServer.dll")),
                ...configFiles.flatMap((f) => ["-f", f]),
            ];
            const serverOptions: ServerOptions = {
                run: { command: SERVER_EXE, args },
                debug: { command: SERVER_EXE, args },
            };
            this._client = new LanguageClient("TopModel", "TopModel", serverOptions, {});
            await this._client.start();
            this.lspStatus = "READY";
        } catch {
            this.lspStatus = "ERROR";
        }
    }

    get toolsStatus(): Status {
        const { status: mStatus, updateAvailable: mUpdate } = this.tools.modgen;
        const { installed: tInstalled, status: tStatus, updateAvailable: tUpdate } = this.tools.tmdgen;

        if (tStatus === "INSTALLING" || mStatus === "INSTALLING") {
            return "INSTALLING";
        } else if ((tInstalled && tStatus === "ERROR") || mStatus === "ERROR") {
            return "ERROR";
        } else if ((tInstalled && tStatus === "LOADING") || mStatus === "LOADING") {
            return "LOADING";
        } else if ((tInstalled && tUpdate) || mUpdate) {
            return "WARNING";
        }

        return "READY";
    }

    private async initTools() {
        this.tools.modgen.init(this.context);
        this.tools.tmdgen.init(this.context);
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
    }

    private registerPreviewCommand() {
        commands.registerCommand(COMMANDS.preview, () => {
            if (!this.preview) {
                this.preview = new TopModelPreviewPanel(this.context, this.client);
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
