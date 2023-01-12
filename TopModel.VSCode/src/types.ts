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

export type Status = "LOADING" | "ERROR" | "WARNING" | "READY" | "INSTALLING";