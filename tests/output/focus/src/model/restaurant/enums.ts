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
export const categoriePlatList: CategoriePlat[] = [
    {
        code: "ENTREE",
        libelle: "restaurant.categoriePlat.values.Entree",
        ordre: 2
    },
    {
        code: "PLAT",
        libelle: "restaurant.categoriePlat.values.Plat",
        ordre: 3,
        prixMoyen: 10
    },
    {
        code: "DESSERT",
        libelle: "restaurant.categoriePlat.values.Dessert",
        ordre: 4
    },
    {
        code: "BOISSON",
        libelle: "restaurant.categoriePlat.values.Boisson",
        ordre: 1,
        prixMoyen: 2
    },
];
export const categoriePlat = {list: categoriePlatList, valueKey: "code", labelKey: "libelle"} as const;

export interface CategoriePlatRegion {
    regionCode: RegionCode;
    categoriePlatCode: CategoriePlatCode;
}
export const categoriePlatRegionList: CategoriePlatRegion[] = [
    {
        regionCode: "IDF",
        categoriePlatCode: "ENTREE"
    },
    {
        regionCode: "IDF",
        categoriePlatCode: "DESSERT"
    },
];

export type DepartementCode = "75" | "92" | "93" | "94";
export interface Departement {
    code: DepartementCode;
    libelle: string;
    regionCode: RegionCode;
}
export const departementList: Departement[] = [
    {
        code: "75",
        libelle: "restaurant.departement.values.Paris",
        regionCode: "IDF"
    },
    {
        code: "92",
        libelle: "restaurant.departement.values.HautsDeSeine",
        regionCode: "IDF"
    },
    {
        code: "93",
        libelle: "restaurant.departement.values.SeineSaintDenis",
        regionCode: "IDF"
    },
    {
        code: "94",
        libelle: "restaurant.departement.values.SeineEtMarne",
        regionCode: "IDF"
    },
];
export const departement = {list: departementList, valueKey: "code", labelKey: "libelle"} as const;

export type RegionCode = "IDF";
export interface Region {
    code: RegionCode;
    libelle: string;
    nomResponsable?: string;
}
export const region = {type: {} as Region, valueKey: "code", labelKey: "libelle"} as const;

export type StatutCommande = "ANNULE" | "EN_ATT" | "EN_PREP" | "PRETE" | "SERVIE";
export interface StatutCommandeObject {
    code: StatutCommande;
    libelle: string;
}
export const statutCommandeList: StatutCommandeObject[] = [
    {
        code: "EN_ATT",
        libelle: "restaurant.statutCommande.values.EnAttente"
    },
    {
        code: "EN_PREP",
        libelle: "restaurant.statutCommande.values.EnPreparation"
    },
    {
        code: "PRETE",
        libelle: "restaurant.statutCommande.values.Prete"
    },
    {
        code: "SERVIE",
        libelle: "restaurant.statutCommande.values.Servie"
    },
    {
        code: "ANNULE",
        libelle: "restaurant.statutCommande.values.Annulee"
    },
];
export const statutCommande = {list: statutCommandeList, valueKey: "code", labelKey: "libelle"} as const;
