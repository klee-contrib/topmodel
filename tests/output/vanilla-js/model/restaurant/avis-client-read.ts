////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

export interface AvisClientRead {
    id: number;
    note: number;
    commentaire?: string;
    dateAvis: string;
    approuve: boolean;
    clientId: number;
    restaurantId: number;
    dateCreation: string;
    nombreVues: number;
}

export const AvisClientReadEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: true,
        label: "Id"
    },
    note: {
        type: "field",
        name: "note",
        domain: DO_QUANTITE,
        isRequired: true,
        label: "Note"
    },
    commentaire: {
        type: "field",
        name: "commentaire",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Commentaire"
    },
    dateAvis: {
        type: "field",
        name: "dateAvis",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "DateAvis"
    },
    approuve: {
        type: "field",
        name: "approuve",
        domain: DO_BOOLEEN,
        defaultValue: false,
        isRequired: true,
        label: "Approuve"
    },
    clientId: {
        type: "field",
        name: "clientId",
        domain: DO_ID,
        isRequired: true,
        label: "ClientId"
    },
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: true,
        label: "RestaurantId"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    },
    nombreVues: {
        type: "field",
        name: "nombreVues",
        domain: DO_QUANTITE,
        defaultValue: 0,
        isRequired: true,
        label: "NombreVues"
    }
} as const;
