////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE} from "../../domains";

import {DepartementCode, StatutCommande} from "./enums";

export interface ClientAvecCommandes {
    id: number;
    nom: string;
    prenom: string;
    departementCode?: DepartementCode;
    dateCreation: string;
    email?: string;
    avisClients: number[];
    commandeId: number[];
    commandeDateCommande: string[];
    commandeDateLivraison?: string[];
    commandeMontantTotal: number[];
    commandeClientId: number[];
    commandeTableId?: number[];
    commandeReservationId?: number[];
    commandeStatutCommande: StatutCommande[];
    commandeAvisClientId?: number[];
    commandeLignes: number[][];
    commandeDateCreation: string[];
}

export const ClientAvecCommandesEntity = {
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
    avisClients: {
        type: "field",
        name: "avisClients",
        domain: DO_LISTE,
        isRequired: true,
        label: "AvisClients"
    },
    commandeId: {
        type: "field",
        name: "commandeId",
        domain: DO_LISTE,
        isRequired: true,
        label: "CommandeId"
    },
    commandeDateCommande: {
        type: "field",
        name: "commandeDateCommande",
        domain: DO_LISTE,
        isRequired: true,
        label: "CommandeDateCommande"
    },
    commandeDateLivraison: {
        type: "field",
        name: "commandeDateLivraison",
        domain: DO_LISTE,
        isRequired: false,
        label: "CommandeDateLivraison"
    },
    commandeMontantTotal: {
        type: "field",
        name: "commandeMontantTotal",
        domain: DO_LISTE,
        isRequired: true,
        label: "Montant total"
    },
    commandeClientId: {
        type: "field",
        name: "commandeClientId",
        domain: DO_LISTE,
        isRequired: true,
        label: "CommandeClientId"
    },
    commandeTableId: {
        type: "field",
        name: "commandeTableId",
        domain: DO_LISTE,
        isRequired: false,
        label: "CommandeTableId"
    },
    commandeReservationId: {
        type: "field",
        name: "commandeReservationId",
        domain: DO_LISTE,
        isRequired: false,
        label: "CommandeReservationId"
    },
    commandeStatutCommande: {
        type: "field",
        name: "commandeStatutCommande",
        domain: DO_LISTE,
        isRequired: true,
        label: "CommandeStatutCommande"
    },
    commandeAvisClientId: {
        type: "field",
        name: "commandeAvisClientId",
        domain: DO_LISTE,
        isRequired: false,
        label: "CommandeAvisClientId"
    },
    commandeLignes: {
        type: "field",
        name: "commandeLignes",
        domain: DO_LISTE,
        isRequired: true,
        label: "CommandeLignes"
    },
    commandeDateCreation: {
        type: "field",
        name: "commandeDateCreation",
        domain: DO_LISTE,
        isRequired: true,
        label: "Date de création"
    }
} as const;
