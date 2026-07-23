////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_LIBELLE, DO_LISTE} from "../../domains";

import {DepartementCode} from "./enums";

export interface ClientWrite {
    nom: string;
    prenom: string;
    departementCode?: DepartementCode;
    email?: string;
    avisClients: number[];
}

export const ClientWriteEntity = {
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Nom"
    },
    prenom: {
        type: "field",
        name: "prenom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Prénom"
    },
    departementCode: {
        type: "field",
        name: "departementCode",
        domain: DO_CODE,
        defaultValue: "75",
        isRequired: false,
        label: "DepartementCode"
    },
    email: {
        type: "field",
        name: "email",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Courriel"
    },
    avisClients: {
        type: "field",
        name: "avisClients",
        domain: DO_LISTE,
        isRequired: true,
        label: "AvisClients"
    }
} as const;
