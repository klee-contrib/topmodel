////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_ID_2, DO_PRIX} from "../../domains";

import {ClientReadEntity, ClientRead} from "./client-read";
import {StatutCommande} from "./enums";
import {LigneCommandeReadEntity, LigneCommandeRead} from "./ligne-commande-read";
import {ReservationReadEntity, ReservationRead} from "./reservation-read";

export interface CommandeRead {
    id: number;
    dateCommande: string;
    dateLivraison?: string;
    montantTotal: number;
    tableId?: number;
    statutCommande: StatutCommande;
    avisClientId?: number;
    dateCreation: string;
    client: ClientRead;
    reservation?: ReservationRead;
    lignes: LigneCommandeRead[];
}

export const CommandeReadEntity = {
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
    dateLivraison: {
        type: "field",
        name: "dateLivraison",
        domain: DO_DATE_HEURE,
        isRequired: false,
        label: "DateLivraison"
    },
    montantTotal: {
        type: "field",
        name: "montantTotal",
        domain: DO_PRIX,
        isRequired: true,
        label: "Montant total"
    },
    tableId: {
        type: "field",
        name: "tableId",
        domain: DO_ID,
        isRequired: false,
        label: "TableId"
    },
    statutCommande: {
        type: "field",
        name: "statutCommande",
        domain: DO_CODE,
        defaultValue: "EN_ATT",
        isRequired: true,
        label: "StatutCommande"
    },
    avisClientId: {
        type: "field",
        name: "avisClientId",
        domain: DO_ID,
        isRequired: false,
        label: "AvisClientId"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    },
    client: {
        type: "object",
        entity: ClientReadEntity
    },
    reservation: {
        type: "object",
        entity: ReservationReadEntity
    },
    lignes: {
        type: "list",
        entity: LigneCommandeReadEntity
    }
} as const;
