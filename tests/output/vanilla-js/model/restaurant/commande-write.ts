////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "../../domains";

import {ClientWriteEntity, ClientWrite} from "./client-write";
import {StatutCommande} from "./enums";
import {LigneCommandeWriteEntity, LigneCommandeWrite} from "./ligne-commande-write";
import {ReservationWriteEntity, ReservationWrite} from "./reservation-write";

export interface CommandeWrite {
    dateCommande: string;
    dateLivraison?: string;
    montantTotal: number;
    tableId?: number;
    statutCommande: StatutCommande;
    avisClientId?: number;
    client: ClientWrite;
    reservation?: ReservationWrite;
    lignes: LigneCommandeWrite[];
}

export const CommandeWriteEntity = {
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
    client: {
        type: "object",
        entity: ClientWriteEntity
    },
    reservation: {
        type: "object",
        entity: ReservationWriteEntity
    },
    lignes: {
        type: "list",
        entity: LigneCommandeWriteEntity
    }
} as const;
