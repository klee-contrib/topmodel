export const SERVER_EXE = 'dotnet';

export const COMMANDS = {
    update: "topmodel.modgen.update",
    modgen: "topmodel.modgen",
    modgenWatch: "topmodel.modgen.watch",
    preview: "topmodel.preview",
    findRef: "topmodel.findRef",
    chooseCommand: "topmodel.chooseCommand"
};

export const COMMANDS_OPTIONS: { [key: string]: { title: string, description: string, detail?: string, command: typeof COMMANDS[keyof typeof COMMANDS] } } = {};