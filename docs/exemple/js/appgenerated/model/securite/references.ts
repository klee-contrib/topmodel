////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type DroitsCode = "CRE" | "MOD" | "SUP";
export interface Droits {
    code: DroitsCode;
    libelle: string;
}
export const droits = {type: {} as Droits, valueKey: "code", labelKey: "libelle"} as const;

export type TypeProfilCode = "ADM" | "GES";
export interface TypeProfil {
    code: TypeProfilCode;
    libelle: string;
}
export const typeProfil = {type: {} as TypeProfil, valueKey: "code", labelKey: "libelle"} as const;
