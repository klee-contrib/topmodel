////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type CategoriePlatCode = "BOISSON" | "DESSERT" | "ENTREE" | "PLAT";
export interface CategoriePlat {
    code: CategoriePlatCode;
    libelle: string;
}
export const categoriePlat = {type: {} as CategoriePlat, valueKey: "code", labelKey: "libelle"} as const;

export type StatutCommande = "ANNULE" | "EN_ATT" | "EN_PREP" | "PRETE" | "SERVIE";
export const statutCommandeLabels = {
    EN_ATT: "restaurant.statutCommande.values.EnAttente",
    EN_PREP: "restaurant.statutCommande.values.EnPreparation",
    PRETE: "restaurant.statutCommande.values.Prete",
    SERVIE: "restaurant.statutCommande.values.Servie",
    ANNULE: "restaurant.statutCommande.values.Annulee"
};

