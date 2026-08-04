import { env } from "vscode";

type LangTranslations = {
    startGeneration: string;
    startGenerationWatch: string;
    startGenerationDescription: string;
    startGenerationWatchDescription: string;
    dotnetIsNotInstalled: string;
    openDotnetDownloadPage: string;
    languageServerInstalling: string;
    noPersistenceClassInThisFile: string;
    displayHideCode: string;
    extensionStartFailed: string;
    loading: string;
    installing: string;
    started: string;
    toolCouldBeUpdated: string;
    updateTool: string;
    toolsVersionMismatch: string;
    updateAllTools: string;
    releaseNoteButton: string;
    toolInstalled: string;
    toolCanBeUpdated: string;
    toolUpdated: string;
    toolUpdateError: string;
    installToolButton: string;
    toolNotInstalled: string;
    statusInError: string;
    statusNotInstalled: string;
    copyButton: string;
    codeCopied: string;
    versionLoadError: string;
    versionNotFound: string;
    toolUpdateErrorState: string;
    restartLanguageServer: string;
    customLanguageServer: string;
    languageServerStartFailed: string;
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
        languageServerInstalling:
            "Le language server TopModel (modls) est en cours d'installation. L'extension démarrera automatiquement une fois l'installation terminée.",
        noPersistenceClassInThisFile: "Pas de classe persistée dans ce fichier",
        displayHideCode: "Afficher/masquer le code",
        extensionStartFailed: "L'extension TopModel n'a pas démarré correctement",
        loading: "Chargement en cours...",
        installing: "Installation en cours...",
        started: "L'extension TopModel est démarrée",
        toolCouldBeUpdated: "L'outil {0} pourrait être mis à jour",
        updateTool: "Mettre à jour {0}",
        toolsVersionMismatch: "Les versions des outils TopModel ne sont pas alignées ({0})",
        updateAllTools: "Mettre à jour tous les outils",
        releaseNoteButton: "Voir la release note",
        toolInstalled: "L'outil {0} (v{1}) a été installé",
        toolCanBeUpdated: "L'outil {0} peut être mis à jour ({1} > {2})",
        toolUpdated: "{0} a été mis à jour {1} --> {2}",
        toolUpdateError: "Erreur pendant la mise à jour de l'outil {0} : {1}",
        installToolButton: "Installer {0}",
        toolNotInstalled: "{0} n'est pas installé",
        statusInError: "en erreur",
        statusNotInstalled: "n'est pas installé",
        copyButton: "Copier",
        codeCopied: "Code copié dans le presse-papiers",
        versionLoadError: "Erreur pendant le chargement de la version courante de l'outil {0}",
        versionNotFound: "Version introuvable pour {0}",
        toolUpdateErrorState: "Erreur pendant la mise à jour de l'outil {0}",
        restartLanguageServer: "Redémarrer le language server",
        customLanguageServer: "Language server personnalisé ({0})",
        languageServerStartFailed: "Impossible de démarrer le language server « {0} » : {1}",
    },
    en: {
        startGeneration: "Start Generation",
        startGenerationWatch: "Start Continuous Generation",
        startGenerationDescription: "Start generation",
        startGenerationWatchDescription: "Start Continuous Generation",
        dotnetIsNotInstalled: ".NET is not installed",
        openDotnetDownloadPage: "Open .NET download page",
        languageServerInstalling:
            "The TopModel language server (modls) is being installed. The extension will start automatically once the installation is complete.",
        noPersistenceClassInThisFile: "No persisted class in this file",
        displayHideCode: "Show/hide code",
        extensionStartFailed: "TopModel extension failed to start",
        loading: "Loading...",
        installing: "Installing...",
        started: "TopModel extension is started",
        toolCouldBeUpdated: "The tool {0} could be updated",
        updateTool: "Update {0}",
        toolsVersionMismatch: "TopModel tools versions are not aligned ({0})",
        updateAllTools: "Update all tools",
        releaseNoteButton: "View release note",
        toolInstalled: "The tool {0} (v{1}) has been installed",
        toolCanBeUpdated: "The tool {0} can be updated ({1} > {2})",
        toolUpdated: "{0} has been updated {1} --> {2}",
        toolUpdateError: "Error while updating tool {0}: {1}",
        installToolButton: "Install {0}",
        toolNotInstalled: "{0} is not installed",
        statusInError: "in error",
        statusNotInstalled: "not installed",
        copyButton: "Copy",
        codeCopied: "Code copied to clipboard",
        versionLoadError: "Error while loading the current version of tool {0}",
        versionNotFound: "Version not found for {0}",
        toolUpdateErrorState: "Error while updating tool {0}",
        restartLanguageServer: "Restart the language server",
        customLanguageServer: "Custom language server ({0})",
        languageServerStartFailed: "Failed to start the language server « {0} »: {1}",
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
