import { TextDocumentContentProvider } from "vscode";

/** Scheme des URI des schémas servis par le language server. */

/**
 * API d'extension exposée par `redhat.vscode-yaml`, qui permet de fournir des schémas hors de
 * `contributes.yamlValidation` (limité aux fichiers embarqués dans l'extension et aux URL http).
 *
 * @see https://github.com/redhat-developer/vscode-yaml/wiki/Extension-API
 */
export interface YamlExtensionApi {
    registerContributor(
        schema: string,
        requestSchema: (resource: string) => string | undefined,
        requestSchemaContent: (uri: string) => Promise<string> | string,
        label?: string,
    ): boolean;
}

/**
 * Expose le schéma comme document virtuel, pour pouvoir l'ouvrir et l'inspecter dans l'éditeur (le
 * contenu servi à `vscode-yaml` n'étant sinon visible nulle part).
 *
 * @see https://code.visualstudio.com/api/extension-guides/virtual-documents
 */
export class SchemaContentProvider implements TextDocumentContentProvider {
    constructor(private readonly modelSchema: string) {}

    public provideTextDocumentContent(): string {
        return this.modelSchema;
    }
}
