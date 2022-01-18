////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type TypeProfilCode = "ADM" | "GES";
export interface TypeProfil {
    code: TypeProfilCode;
    libelle: string;
}
export const typeProfil = {type: {} as TypeProfil, valueKey: "code", labelKey: "libelle"} as const;
