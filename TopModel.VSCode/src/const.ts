export const SERVER_EXE = "modls";

/** Clés des paramètres de l'extension, relatives à la section `topmodel`. */
export const SETTINGS = {
    languageServerPath: "languageServerPath",
};

export const COMMANDS = {
    modgen: "topmodel.modgen",
    modgenWatch: "topmodel.modgen.watch",
    preview: "topmodel.preview",
    findRef: "topmodel.findRef",
    releaseNote: "topmodel.releaseNote",
    chooseCommand: "topmodel.chooseCommand",
    restartLanguageServer: "topmodel.modls.restart",
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
