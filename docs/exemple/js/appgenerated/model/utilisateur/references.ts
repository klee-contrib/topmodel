////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type TypeUtilisateurCode = "ADM" | "CLI" | "GES";
export interface TypeUtilisateur {
    code: TypeUtilisateurCode;
    libelle: string;
}
export const typeUtilisateurList: TypeUtilisateur[] = [
    {
        code: "ADM",
        libelle: "utilisateur.typeUtilisateur.values.ADM"
    },
    {
        code: "GES",
        libelle: "utilisateur.typeUtilisateur.values.GES"
    },
    {
        code: "CLI",
        libelle: "utilisateur.typeUtilisateur.values.CLI"
    },
];

