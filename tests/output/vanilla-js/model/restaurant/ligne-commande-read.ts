////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID_2, DO_PRIX, DO_QUANTITE, DO_SEQ_ID} from "../../domains";

export interface LigneCommandeRead {
    id: number;
    quantite: number;
    prixUnitaire: number;
    prixTotal: number;
    commandeId: number;
    platId: number;
    dateCreation: string;
}

export const LigneCommandeReadEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID_2,
        isRequired: true,
        label: "Id"
    },
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
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    }
} as const;
