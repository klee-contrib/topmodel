////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type CategoriePlatCode = "BOISSON" | "DESSERT" | "ENTREE" | "PLAT";
export type CategoriePlatOrdre = 1 | 2 | 3 | 4;
export interface CategoriePlat {
    code: CategoriePlatCode;
    libelle: string;
    ordre: CategoriePlatOrdre;
    prixMoyen?: number;
}
export const categoriePlat = {type: {} as CategoriePlat, valueKey: "code", labelKey: "libelle"} as const;

export interface CategoriePlatRegion {
    regionCode: RegionCode;
    categoriePlatCode: CategoriePlatCode;
}

export type DepartementCode = "75" | "92" | "93" | "94";
export interface Departement {
    code: DepartementCode;
    libelle: string;
    regionCode: RegionCode;
}
export const departement = {type: {} as Departement, valueKey: "code", labelKey: "libelle"} as const;

export type RegionCode = "IDF";
export interface Region {
    code: RegionCode;
    libelle: string;
    nomResponsable?: string;
}
export const region = {type: {} as Region, valueKey: "code", labelKey: "libelle"} as const;

export type StatutCommande = "ANNULE" | "EN_ATT" | "EN_PREP" | "PRETE" | "SERVIE";
export const statutCommandeLabels = {
    EN_ATT: "restaurant.statutCommande.values.EnAttente",
    EN_PREP: "restaurant.statutCommande.values.EnPreparation",
    PRETE: "restaurant.statutCommande.values.Prete",
    SERVIE: "restaurant.statutCommande.values.Servie",
    ANNULE: "restaurant.statutCommande.values.Annulee"
};
