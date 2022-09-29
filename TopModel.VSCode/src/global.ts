import { commands, ExtensionContext, Terminal, window } from "vscode";
import { COMMANDS, COMMANDS_OPTIONS } from "./const";

export class Global {
    constructor(public readonly extensionContext: ExtensionContext) {
        window.onDidCloseTerminal((terminal) => {
            if (terminal === this._terminal) {
                this._terminal = undefined;
            }
        });
    }

    private _terminal?: Terminal;
    public get terminal(): Terminal {
        if (!this._terminal) {
            this._terminal = window.createTerminal({
                name: "modgen",
                message: "TopModel",
            });
        }
        this._terminal.show();
        return this._terminal;
    }

    public registerCommands() {
        this.registerModgen(false);
        this.registerModgen(true);
    }

    private startModgen(watch: boolean) {
        this.terminal.sendText(`modgen ${watch ? " --watch" : ""}`);
        this.terminal.show();
    }

    private registerModgen(watch: boolean) {
        const modgenCommand = watch ? COMMANDS.modgenWatch : COMMANDS.modgen;
        const modgen = commands.registerCommand(modgenCommand, () => this.startModgen(watch));
        COMMANDS_OPTIONS[modgenCommand] = {
            title: `Lancer la génération ${watch ? "en continu" : ""}`,
            description: `Lancer la génération ${watch ? "continue " : ""}`,
            command: modgenCommand,
        };
        this.extensionContext.subscriptions.push(modgen);
    }
}
