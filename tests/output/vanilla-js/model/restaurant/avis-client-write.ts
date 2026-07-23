////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

export interface AvisClientWrite {
    note: number;
    commentaire?: string;
    approuve: boolean;
    clientId: number;
    restaurantId: number;
}

export const AvisClientWriteEntity = {
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
    }
} as const;
