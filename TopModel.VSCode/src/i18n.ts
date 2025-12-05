import { env } from "vscode";

type LangTranslations = {
    startGeneration: string;
    startGenerationWatch: string;
    startGenerationDescription: string;
    startGenerationWatchDescription: string;
    dotnetIsNotInstalled: string;
    openDotnetDownloadPage: string;
    noPersistenceClassInThisFile: string;
    displayHideCode: string;
    extensionStartFailed: string;
    loading: string;
    installing: string;
    started: string;
    toolCouldBeUpdated: string;
    updateTool: string;
};
type AvailableLanguages = "fr" | "en";

const translations: Record<AvailableLanguages, LangTranslations> = {
    fr: {
        startGeneration: "Lancer la génération",
        startGenerationWatch: "Lancer la génération en continue",
        startGenerationDescription: "Lancer la génération",
        startGenerationWatchDescription: "Lancer la génération",
        dotnetIsNotInstalled: "Dotnet n'est pas installé",
        openDotnetDownloadPage: "Ouvrir la page de téléchargement",
        noPersistenceClassInThisFile: "Pas de classe persistée dans ce fichier",
        displayHideCode: "Afficher/masquer le code",
        extensionStartFailed: "L'extension TopModel n'a pas démarré correctement",
        loading: "Chargement en cours...",
        installing: "Installation en cours...",
        started: "L'extension TopModel est démarrée",
        toolCouldBeUpdated: "L'outil {0} pourrait être mis à jour",
        updateTool: "Mettre à jour {0}",
    },
    en: {
        startGeneration: "Start Generation",
        startGenerationWatch: "Start Continuous Generation",
        startGenerationDescription: "Start generation",
        startGenerationWatchDescription: "Start Continuous Generation",
        dotnetIsNotInstalled: ".NET is not installed",
        openDotnetDownloadPage: "Open .NET download page",
        noPersistenceClassInThisFile: "No persisted class in this file",
        displayHideCode: "Show/hide code",
        extensionStartFailed: "TopModel extension failed to start",
        loading: "Loading...",
        installing: "Installing...",
        started: "TopModel extension is started",
        toolCouldBeUpdated: "The tool {0} could be updated",
        updateTool: "Update {0}",
    },
};

function getLangKey(lang?: string): AvailableLanguages {
    if (!lang) {
        lang = env.language;
    }

    const keys: keyof typeof translations = Object.keys(translations) as unknown as keyof typeof translations;
    if (!keys.includes(lang)) {
        if (lang.includes("_")) {
            return getLangKey(lang.split("_")[0]);
        } else {
            return "fr";
        }
    }

    return lang as AvailableLanguages;
}

export function t(key: keyof LangTranslations, args: string[] = []): string {
    const langKey = getLangKey();
    const translation = translations[langKey][key] || key;
    return translation.replace(/\{(\d+)\}/g, (match, index) => args[index] || match);
}
