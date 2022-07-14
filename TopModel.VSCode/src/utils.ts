const exec = require('child_process').exec;

export function execute(command: string, callback: Function) {
    exec(command, function (error: string, stdout: string, stderr: string) { callback(stdout); });
}
