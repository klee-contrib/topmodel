////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID, DO_PRIX} from "../../domains";

export interface PaiementItem {
    factureId: number;
    swileCardId: number;
    dateCommande: string;
    montantTotal: number;
}

export const PaiementItemEntity = {
    factureId: {
        type: "field",
        name: "factureId",
        domain: DO_ID,
        isRequired: true,
        label: "FactureId"
    },
    swileCardId: {
        type: "field",
        name: "swileCardId",
        domain: DO_ID,
        isRequired: true,
        label: "SwileCardId"
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
    }
} as const;
