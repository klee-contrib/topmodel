export const SERVER_EXE = "dotnet";

export const COMMANDS = {
    modgen: "topmodel.modgen",
    modgenWatch: "topmodel.modgen.watch",
    preview: "topmodel.preview",
    findRef: "topmodel.findRef",
    releaseNote: "topmodel.releaseNote",
    chooseCommand: "topmodel.chooseCommand",
};

// Stockage de l'ensemble des commandes disponibles.
// Pour affichage dans une liste déroulante au clic sur la barre de status
export const COMMANDS_OPTIONS: {
    [key: string]: {
        title: string;
        description: string;
        detail?: string;
        command: (typeof COMMANDS)[keyof typeof COMMANDS];
    };
} = {};
