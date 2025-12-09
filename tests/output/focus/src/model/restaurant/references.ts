////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type CategoriePlatCode = "BOISSON" | "DESSERT" | "ENTREE" | "PLAT";
export interface CategoriePlat {
    code: CategoriePlatCode;
    libelle: string;
}
export const categoriePlat = {type: {} as CategoriePlat, valueKey: "code", labelKey: "libelle"} as const;

export type StatutCommandeCode = "ANNULE" | "EN_ATT" | "EN_PREP" | "PRETE" | "SERVIE";
export interface StatutCommande {
    code: StatutCommandeCode;
    libelle: string;
}
export const statutCommande = {type: {} as StatutCommande, valueKey: "code", labelKey: "libelle"} as const;
