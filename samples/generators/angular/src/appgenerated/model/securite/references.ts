////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type DroitCode = "CRE" | "MOD" | "SUP";
export interface Droit {
    code: DroitCode;
    libelle: string;
    typeProfilCode?: TypeProfilCode;
}
export const droitList: Droit[] = [
    {
        code: "CRE",
        libelle: "securite.droit.values.CRE",
        typeProfilCode: "ADM"
    },
    {
        code: "MOD",
        libelle: "securite.droit.values.MOD"
    },
    {
        code: "SUP",
        libelle: "securite.droit.values.SUP"
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

