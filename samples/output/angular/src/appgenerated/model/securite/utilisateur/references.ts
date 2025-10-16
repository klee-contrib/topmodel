////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type TypeUtilisateurCode = "ADMIN" | "CLIENT" | "GEST";
export interface TypeUtilisateur {
    code: TypeUtilisateurCode;
    libelle: string;
}
export const typeUtilisateurList: TypeUtilisateur[] = [
    {
        code: "ADMIN",
        libelle: "securite.utilisateur.typeUtilisateur.values.Admin"
    },
    {
        code: "GEST",
        libelle: "securite.utilisateur.typeUtilisateur.values.Gestionnaire"
    },
    {
        code: "CLIENT",
        libelle: "securite.utilisateur.typeUtilisateur.values.Client"
    },
];

