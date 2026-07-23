////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_ID_2, DO_PRIX} from "../../domains";

import {StatutCommande} from "./enums";

export interface CommandeItem {
    id: number;
    dateCommande: string;
    montantTotal: number;
    statutCommande: StatutCommande;
    clientId: number;
}

export const CommandeItemEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID_2,
        isRequired: true,
        label: "Id"
    },
    dateCommande: {
        type: "field",
        name: "dateCommande",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "DateCommande"
    },
    montantTotal: {
        type: "field",
        name: "montantTotal",
        domain: DO_PRIX,
        isRequired: true,
        label: "Montant total"
    },
    statutCommande: {
        type: "field",
        name: "statutCommande",
        domain: DO_CODE,
        defaultValue: "EN_ATT",
        isRequired: true,
        label: "StatutCommande"
    },
    clientId: {
        type: "field",
        name: "clientId",
        domain: DO_ID,
        isRequired: true,
        label: "ClientId"
    }
} as const;
