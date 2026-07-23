////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE, DO_SEQ_ID} from "../../domains";

export interface PromotionRead {
    platId: number;
    libelle: string;
    pourcentageReduction: number;
    dateDebut: string;
    dateFin: string;
    active: boolean;
    restaurantId?: number;
    dateCreation: string;
}

export const PromotionReadEntity = {
    platId: {
        type: "field",
        name: "platId",
        domain: DO_SEQ_ID,
        isRequired: true,
        label: "PlatId"
    },
    libelle: {
        type: "field",
        name: "libelle",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Libelle"
    },
    pourcentageReduction: {
        type: "field",
        name: "pourcentageReduction",
        domain: DO_QUANTITE,
        isRequired: true,
        label: "PourcentageReduction"
    },
    dateDebut: {
        type: "field",
        name: "dateDebut",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "DateDebut"
    },
    dateFin: {
        type: "field",
        name: "dateFin",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "DateFin"
    },
    active: {
        type: "field",
        name: "active",
        domain: DO_BOOLEEN,
        defaultValue: true,
        isRequired: true,
        label: "Active"
    },
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: false,
        label: "RestaurantId"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    }
} as const;
