export type TopModelConfig = {
    app: string;
    modelRoot?: string;
};

export class TopModelException {
    constructor(public readonly message: string) { }
}

export type Mermaid = {
    diagram: string;
    module: string;
};

export type ExtensionState = "LOADING" | "ERROR" | "RUNNING" | "UPDATING" | "INSTALLING";


export type TmdTool = ({
    name: "TopModel.Generator";
    command: "modgen";
} | {
    name: "TopModel.ModelGenerator";
    command: "tmdgen";
}) & {
    currentVersion?: string;
    latestVersion?: string;
};
