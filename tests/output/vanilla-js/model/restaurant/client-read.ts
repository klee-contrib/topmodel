////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE} from "../../domains";

import {DepartementCode} from "./enums";

export interface ClientRead {
    id: number;
    nom: string;
    prenom: string;
    departementCode?: DepartementCode;
    dateCreation: string;
    email?: string;
    swileCardId?: number;
    avisClients: number[];
}

export const ClientReadEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: true,
        label: "Id"
    },
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
        isRequired: false,
        label: "DepartementCode"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    },
    email: {
        type: "field",
        name: "email",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Courriel"
    },
    swileCardId: {
        type: "field",
        name: "swileCardId",
        domain: DO_ID,
        isRequired: false,
        label: "SwileCardId"
    },
    avisClients: {
        type: "field",
        name: "avisClients",
        domain: DO_LISTE,
        isRequired: true,
        label: "AvisClients"
    }
} as const;
