////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type CategoriePlatCode = "BOISSON" | "DESSERT" | "ENTREE" | "PLAT";
export type CategoriePlatOrdre = 1 | 2 | 3 | 4;
export interface CategoriePlat {
    code: CategoriePlatCode;
    libelle: string;
    ordre: CategoriePlatOrdre;
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
        ordre: 3
    },
    {
        code: "DESSERT",
        libelle: "restaurant.categoriePlat.values.Dessert",
        ordre: 4
    },
    {
        code: "BOISSON",
        libelle: "restaurant.categoriePlat.values.Boisson",
        ordre: 1
    },
];


export type DepartementCode = "75" | "92" | "93" | "94";
export interface Departement {
    code: DepartementCode;
    libelle: string;
}
export const departementList: Departement[] = [
    {
        code: "75",
        libelle: "restaurant.departement.values.Paris"
    },
    {
        code: "92",
        libelle: "restaurant.departement.values.HautsDeSeine"
    },
    {
        code: "93",
        libelle: "restaurant.departement.values.SeineSaintDenis"
    },
    {
        code: "94",
        libelle: "restaurant.departement.values.SeineEtMarne"
    },
];


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

