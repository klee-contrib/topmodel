////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type CategoriePlatCode = "AUTRE" | "BOISSON" | "DESSERT" | "ENTREE" | "PRINCIPAL";
export type CategoriePlatOrdre = 1 | 2 | 3 | 4 | 5;
export interface CategoriePlat {
    code: CategoriePlatCode;
    libelle: string;
    ordre: CategoriePlatOrdre;
    prixMoyen?: number;
}
export const categoriePlatList: CategoriePlat[] = [
    {
        code: "BOISSON",
        libelle: "Boisson",
        ordre: 1,
        prixMoyen: 2
    },
    {
        code: "ENTREE",
        libelle: "Entrée",
        ordre: 2
    },
    {
        code: "PRINCIPAL",
        libelle: "Plat principal",
        ordre: 3,
        prixMoyen: 10
    },
    {
        code: "DESSERT",
        libelle: "Dessert",
        ordre: 4
    },
    {
        code: "AUTRE",
        libelle: "Autre",
        ordre: 5
    },
];
export const categoriePlat = {list: categoriePlatList, valueKey: "code", labelKey: "libelle"} as const;

export interface CategoriePlatRegion {
    regionCode: RegionCode;
    categoriePlat: CategoriePlatCode;
}
export const categoriePlatRegionList: CategoriePlatRegion[] = [
    {
        regionCode: "IDF",
        categoriePlat: "ENTREE"
    },
    {
        regionCode: "IDF",
        categoriePlat: "DESSERT"
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
        code: "92",
        libelle: "Hauts de Seine",
        regionCode: "IDF"
    },
    {
        code: "75",
        libelle: "Paris",
        regionCode: "IDF"
    },
    {
        code: "94",
        libelle: "Seine et Marne",
        regionCode: "IDF"
    },
    {
        code: "93",
        libelle: "Seine Saint Denis",
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
        code: "ANNULE",
        libelle: "Annulée"
    },
    {
        code: "EN_ATT",
        libelle: "En attente"
    },
    {
        code: "EN_PREP",
        libelle: "En préparation"
    },
    {
        code: "PRETE",
        libelle: "Prête"
    },
    {
        code: "SERVIE",
        libelle: "Servie"
    },
];
export const statutCommande = {list: statutCommandeList, valueKey: "code", labelKey: "libelle"} as const;

export type TypeTerrasse = "EXT" | "INT";
export interface TypeTerrasseObject {
    code: TypeTerrasse;
}
export const typeTerrasseList: TypeTerrasseObject[] = [
    {
        code: "INT"
    },
    {
        code: "EXT"
    },
];
