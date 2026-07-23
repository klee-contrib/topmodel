////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID_2, DO_PRIX, DO_QUANTITE, DO_SEQ_ID} from "../../domains";

export interface LigneCommandeWrite {
    quantite: number;
    prixUnitaire: number;
    prixTotal: number;
    commandeId: number;
    platId: number;
}

export const LigneCommandeWriteEntity = {
    quantite: {
        type: "field",
        name: "quantite",
        domain: DO_QUANTITE,
        isRequired: true,
        label: "Quantite"
    },
    prixUnitaire: {
        type: "field",
        name: "prixUnitaire",
        domain: DO_PRIX,
        isRequired: true,
        label: "PrixUnitaire"
    },
    prixTotal: {
        type: "field",
        name: "prixTotal",
        domain: DO_PRIX,
        isRequired: true,
        label: "PrixTotal"
    },
    commandeId: {
        type: "field",
        name: "commandeId",
        domain: DO_ID_2,
        isRequired: true,
        label: "CommandeId"
    },
    platId: {
        type: "field",
        name: "platId",
        domain: DO_SEQ_ID,
        isRequired: true,
        label: "PlatId"
    }
} as const;
