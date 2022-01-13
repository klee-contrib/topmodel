export type TopModelConfig = {
    app: string;
    modelRoot?:string;
};

export class TopModelException {
    constructor(public readonly message: string) { }
}