import {readFile, writeFile} from "node:fs";
import {resolve} from "node:path";

// Corrige le problème d'implémentation du language server qui envoie une notification au lieu d'une requête pour le refresh de la coloration syntaxique.
// C'est bien le serveur qui implémente mal la spécification, mais il n'y a pas de solution de son côté pour que ça fonctionne...
const file = resolve(import.meta.dirname, "./node_modules/vscode-languageclient/lib/common/semanticTokens.js");
readFile(file, "utf8", (err, data) => {
    if (!err) {
        writeFile(file, data.replace("client.onRequest", "client.onNotification"), "utf8", err2 =>
            console.info(err2 ?? "Le patch de vscode-languageclient a été appliqué avec succès.")
        );
    }
});
