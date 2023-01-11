import { autorun, makeAutoObservable, makeObservable, observable } from "mobx";
import { ExtensionContext } from "vscode";
import { Application } from "./application";
import { updateStatusBar } from "./extension";
import { ExtensionStatus, TmdTool } from "./types";

export class ExtensionState {
    status: ExtensionStatus = "LOADING";
    tools = {
        topmodel: {
            name: "TopModel.Generator",
            command: "modgen"
        } as TmdTool,
        tmdgen: {
            name: "TopModel.ModelGenerator",
            command: "tmdgen"
        } as TmdTool
    };

    context: ExtensionContext;
    applications: Application[] = [];
    constructor(ctx: ExtensionContext) {
        this.context = ctx;
        makeAutoObservable(this);
    }
}
