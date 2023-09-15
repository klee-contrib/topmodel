////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

export type DroitCode = "CREATE" | "DELETE" | "READ" | "UPDATE";
export interface Droit {
    code: DroitCode;
    libelle: string;
    typeDroitCode: TypeDroitCode;
}
export const droitList: Droit[] = [
    {
        code: "CREATE",
        libelle: "securite.profil.droit.values.Create",
        typeDroitCode: "WRITE"
    },
    {
        code: "READ",
        libelle: "securite.profil.droit.values.Read",
        typeDroitCode: "READ"
    },
    {
        code: "UPDATE",
        libelle: "securite.profil.droit.values.Update",
        typeDroitCode: "WRITE"
    },
    {
        code: "DELETE",
        libelle: "securite.profil.droit.values.Delete",
        typeDroitCode: "ADMIN"
    },
];


export type TypeDroitCode = "ADMIN" | "READ" | "WRITE";
export interface TypeDroit {
    code: TypeDroitCode;
    libelle: string;
}
export const typeDroitList: TypeDroit[] = [
    {
        code: "READ",
        libelle: "securite.profil.typeDroit.values.Read"
    },
    {
        code: "WRITE",
        libelle: "securite.profil.typeDroit.values.Write"
    },
    {
        code: "ADMIN",
        libelle: "securite.profil.typeDroit.values.Admin"
    },
];

