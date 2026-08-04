import { workspace, WorkspaceFolder } from "vscode";

import { SERVER_EXE, SETTINGS } from "./const";
import path = require("path");

const cp = require("child_process");
const exec = cp.exec;

export async function execute(command: string) {
    return new Promise<string>((resolve, reject) => {
        exec(command, function (error: string, stdout: string, stderr: string) {
            if (error || stderr !== "") {
                reject(error || stderr);
            } else {
                resolve(stdout);
            }
        });
    });
}

export const isWindows = process.platform === "win32";

/**
 * Chemin de lancement du language server configuré par l'utilisateur, s'il y en a un.
 *
 * Vide (valeur par défaut) signifie qu'on utilise le tool global `modls`, installé et mis à jour
 * par l'extension.
 */
export function getLanguageServerPath(): string | undefined {
    return workspace.getConfiguration("topmodel").get<string>(SETTINGS.languageServerPath)?.trim() || undefined;
}

/**
 * Commande à lancer pour démarrer le language server d'un workspace folder.
 *
 * Par défaut le tool global `modls`. Si {@link getLanguageServerPath} est renseigné, on lance
 * l'exécutable indiqué à la place, ce qui permet de debugger un build local du language server ou
 * d'utiliser une autre version que celle installée globalement. Le chemin accepte la variable
 * `${workspaceFolder}`, peut être relatif (résolu depuis le workspace folder), et un `.dll` est
 * lancé via `dotnet`.
 */
export function getServerCommand(workspaceFolder: WorkspaceFolder): { command: string; args: string[] } {
    const configuredPath = getLanguageServerPath();
    if (!configuredPath) {
        return { command: SERVER_EXE, args: [] };
    }

    const folderPath = workspaceFolder.uri.fsPath;
    const substitutedPath = configuredPath.replace(/\$\{workspaceFolder\}/g, folderPath);
    const resolvedPath = path.resolve(folderPath, substitutedPath);

    return path.extname(resolvedPath).toLowerCase() === ".dll"
        ? { command: "dotnet", args: [resolvedPath] }
        : { command: resolvedPath, args: [] };
}

/**
 * Tue un processus et toute sa descendance (et attend confirmation de la fin).
 *
 * Indispensable pour le language server (modls) : tant que le processus tourne, il garde
 * verrouillés les fichiers de son répertoire d'installation, ce qui fait échouer
 * `dotnet tool update` avec une erreur d'accès refusé.
 */
export function killProcessTree(pid?: number): Promise<void> {
    return new Promise<void>((resolve) => {
        if (pid === undefined) {
            resolve();
            return;
        }

        try {
            if (isWindows) {
                // /T : arbre de processus, /F : force. Le callback garantit que le kill est terminé.
                cp.execFile("taskkill", ["/T", "/F", "/PID", String(pid)], () => resolve());
            } else {
                try {
                    process.kill(pid, "SIGKILL");
                } catch {
                    // déjà mort
                }
                resolve();
            }
        } catch {
            resolve();
        }
    });
}

/**
 * Tue tous les processus du language server (modls) de la machine, quelle que soit la fenêtre
 * VSCode qui les a lancés.
 *
 * Indispensable avant `dotnet tool update` du language server : tant qu'un process modls tourne
 * (y compris celui d'une autre fenêtre), il garde verrouillés les fichiers de l'outil et fait
 * échouer la mise à jour. Les autres fenêtres détectent la mort de leur process et le redémarrent.
 */
export function killAllLanguageServers(): Promise<void> {
    return new Promise<void>((resolve) => {
        try {
            if (isWindows) {
                // /T : arbre de processus, /F : force, /IM : par nom d'image.
                cp.execFile("taskkill", ["/T", "/F", "/IM", `${SERVER_EXE}.exe`], () => resolve());
            } else {
                // -x : correspondance exacte du nom de processus.
                cp.execFile("pkill", ["-x", SERVER_EXE], () => resolve());
            }
        } catch {
            resolve();
        }
    });
}