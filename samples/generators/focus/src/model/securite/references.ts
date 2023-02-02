////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type DroitCode = "CRE" | "MOD" | "SUP";
export interface Droit {
    code: DroitCode;
    libelle: string;
    typeProfilCode?: TypeProfilCode;
}
export const droit = {type: {} as Droit, valueKey: "code", labelKey: "libelle"} as const;

export type TypeProfilCode = "ADM" | "GES";
export interface TypeProfil {
    code: TypeProfilCode;
    libelle: string;
}
export const typeProfil = {type: {} as TypeProfil, valueKey: "code", labelKey: "libelle"} as const;
