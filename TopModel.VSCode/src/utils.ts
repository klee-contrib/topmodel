const exec = require("child_process").exec;

export async function execute(command: string) {
    return new Promise((resolve, reject) => {
        exec(command, function (error: string, stdout: string, stderr: string) {
            if (error || stderr !== "") {
                reject(error || stderr);
            } else {
                resolve(stdout);
            }
        });
    });
}
