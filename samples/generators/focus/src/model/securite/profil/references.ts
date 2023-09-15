////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type DroitCode = "CREATE" | "DELETE" | "READ" | "UPDATE";
export interface Droit {
    code: DroitCode;
    libelle: string;
    typeDroitCode: TypeDroitCode;
}
export const droit = {type: {} as Droit, valueKey: "code", labelKey: "libelle"} as const;

export type TypeDroitCode = "ADMIN" | "READ" | "WRITE";
export interface TypeDroit {
    code: TypeDroitCode;
    libelle: string;
}
export const typeDroit = {type: {} as TypeDroit, valueKey: "code", labelKey: "libelle"} as const;
