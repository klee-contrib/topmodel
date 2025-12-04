////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type CategoriePlatCode = "BOISSON" | "DESSERT" | "ENTREE" | "PLAT";
export interface CategoriePlat {
    code: CategoriePlatCode;
    libelle: string;
}
export const categoriePlatList: CategoriePlat[] = [
    {
        code: "ENTREE",
        libelle: "restaurant.categoriePlat.values.Entree"
    },
    {
        code: "PLAT",
        libelle: "restaurant.categoriePlat.values.Plat"
    },
    {
        code: "DESSERT",
        libelle: "restaurant.categoriePlat.values.Dessert"
    },
    {
        code: "BOISSON",
        libelle: "restaurant.categoriePlat.values.Boisson"
    },
];


export type StatutCommandeCode = "ANNULE" | "EN_ATT" | "EN_PREP" | "PRETE" | "SERVIE";
export interface StatutCommande {
    code: StatutCommandeCode;
    libelle: string;
}
export const statutCommandeList: StatutCommande[] = [
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

