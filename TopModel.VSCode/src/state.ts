import { makeAutoObservable } from "mobx";
import { ExtensionContext } from "vscode";
import { Application } from "./application";
import { TmdTool } from "./tool";
import { Status } from "./types";

export class State {
    tools = {
        modgen: new TmdTool("TopModel.Generator", "modgen"),
        tmdgen: new TmdTool("TopModel.ModelGenerator", "tmdgen"),
    };

    applications: Application[] = [];
    error?: string;
    constructor(public readonly context: ExtensionContext) {
        makeAutoObservable(this);
    }

    get status(): Status {
        let status: Status = "LOADING";
        if (this.appStatus === "LOADING" || this.applications.length === 0 || this.toolsStatus === "LOADING") {
            status = "LOADING";
        } else if (this.appStatus === "READY" && this.toolsStatus === "READY") {
            status = "READY";
        }

        if (this.toolsStatus === "INSTALLING") {
            status = "INSTALLING";
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
        if (this.appStatus === "LOADING") {
            text += "$(loading~spin) ";
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
        let status: Status = "READY";
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

        return "READY";
    }
}
