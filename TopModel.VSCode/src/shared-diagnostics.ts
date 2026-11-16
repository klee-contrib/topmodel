import { DiagnosticCollection, languages } from "vscode";
import { DiagnosticCollectionProvider, DiagnosticCollectionSource } from "vscode-languageclient";

/**
 * Fournit UNE SEULE DiagnosticCollection partagée entre le push (`publishDiagnostics`) et le pull
 * (`textDocument/diagnostic`) : modls envoie les mêmes diagnostics par les deux canaux, et avec les
 * collections séparées par défaut, VS Code afficherait chaque erreur en double.
 *
 * Une instance doit être créée par LanguageClient : son état (collection + compteur) est ainsi
 * naturellement remis à zéro à chaque (re)création du client.
 */
export class SharedDiagnosticCollectionProvider implements DiagnosticCollectionProvider {
    private collection?: DiagnosticCollection;
    /** Vue de la collection pour le pull, qui ignore `delete` (cf. `create`). */
    private pullView?: DiagnosticCollection;
    /** Nombre de sources (push, pull) qui utilisent actuellement la collection. */
    private references = 0;

    create(name: string | undefined, source: DiagnosticCollectionSource): DiagnosticCollection {
        if (!this.collection) {
            this.collection = languages.createDiagnosticCollection(name);
            // À la fermeture d'un document, le pull efface l'entrée de son URI (`delete`), ce qui
            // effacerait aussi les erreurs poussées par le serveur pour un fichier fermé. On ignore donc
            // `delete` côté pull ; `set` (remplacement de l'entrée) et le reste passent normalement.
            this.pullView = new Proxy(this.collection, {
                get: (target, prop) => {
                    if (prop === "delete") {
                        return () => undefined;
                    }
                    const value = Reflect.get(target, prop, target);
                    return typeof value === "function" ? value.bind(target) : value;
                },
            });
        }

        this.references++;
        return source === DiagnosticCollectionSource.pull ? this.pullView! : this.collection;
    }

    dispose(_collection: DiagnosticCollection, _source: DiagnosticCollectionSource): void {
        // La collection n'est détruite que quand plus aucune source ne l'utilise.
        if (this.references > 0 && --this.references === 0) {
            this.collection?.dispose();
            this.collection = undefined;
            this.pullView = undefined;
        }
    }
}
