////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type DroitsCode = "CRE" | "MOD" | "SUP";
export interface Droits {
    code: DroitsCode;
    libelle: string;
    typeProfilCode?: TypeProfilCode;
}
export const droitsList: Droits[] = [
    {
        code: "CRE",
        libelle: "securite.droits.values.CRE",
        typeProfilCode: "ADM"
    },
    {
        code: "MOD",
        libelle: "securite.droits.values.MOD"
    },
    {
        code: "SUP",
        libelle: "securite.droits.values.SUP"
    },
];


export type TypeProfilCode = "ADM" | "GES";
export interface TypeProfil {
    code: TypeProfilCode;
    libelle: string;
}
export const typeProfilList: TypeProfil[] = [
    {
        code: "ADM",
        libelle: "securite.typeProfil.values.ADM"
    },
    {
        code: "GES",
        libelle: "securite.typeProfil.values.GES"
    },
];

