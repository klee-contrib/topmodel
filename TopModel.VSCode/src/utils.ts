import { SERVER_EXE } from "./const";

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