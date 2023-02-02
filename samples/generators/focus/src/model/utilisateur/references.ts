////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type TypeUtilisateurCode = "ADM" | "CLI" | "GES";
export interface TypeUtilisateur {
    code: TypeUtilisateurCode;
    libelle: string;
}
export const typeUtilisateur = {type: {} as TypeUtilisateur, valueKey: "code", labelKey: "libelle"} as const;
