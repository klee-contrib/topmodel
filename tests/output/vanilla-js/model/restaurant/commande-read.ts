////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_ID_2, DO_PRIX} from "../../domains";

import {ClientReadEntity, ClientRead} from "./client-read";
import {StatutCommande} from "./enums";
import {LigneCommandeReadEntity, LigneCommandeRead} from "./ligne-commande-read";
import {ReservationReadEntity, ReservationRead} from "./reservation-read";

export interface CommandeRead {
    commandeId: number;
    commandeDateCommande: string;
    commandeDateLivraison?: string;
    commandeMontantTotal: number;
    commandeTableId?: number;
    commandeStatutCommande: StatutCommande;
    commandeAvisClientId?: number;
    commandeDateCreation: string;
    client: ClientRead;
    reservation?: ReservationRead;
    lignes: LigneCommandeRead[];
}

export const CommandeReadEntity = {
    commandeId: {
        type: "field",
        name: "commandeId",
        domain: DO_ID_2,
        isRequired: true,
        label: "CommandeId"
    },
    commandeDateCommande: {
        type: "field",
        name: "commandeDateCommande",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "CommandeDateCommande"
    },
    commandeDateLivraison: {
        type: "field",
        name: "commandeDateLivraison",
        domain: DO_DATE_HEURE,
        isRequired: false,
        label: "CommandeDateLivraison"
    },
    commandeMontantTotal: {
        type: "field",
        name: "commandeMontantTotal",
        domain: DO_PRIX,
        isRequired: true,
        label: "Montant total"
    },
    commandeTableId: {
        type: "field",
        name: "commandeTableId",
        domain: DO_ID,
        isRequired: false,
        label: "CommandeTableId"
    },
    commandeStatutCommande: {
        type: "field",
        name: "commandeStatutCommande",
        domain: DO_CODE,
        defaultValue: "EN_ATT",
        isRequired: true,
        label: "CommandeStatutCommande"
    },
    commandeAvisClientId: {
        type: "field",
        name: "commandeAvisClientId",
        domain: DO_ID,
        isRequired: false,
        label: "CommandeAvisClientId"
    },
    commandeDateCreation: {
        type: "field",
        name: "commandeDateCreation",
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
