////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

export interface ReservationWrite {
    dateReservation: string;
    nombrePersonnes: number;
    commentaire?: string;
    confirmee: boolean;
    clientId: number;
    tableId?: number;
    restaurantId: number;
}

export const ReservationWriteEntity = {
    dateReservation: {
        type: "field",
        name: "dateReservation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "DateReservation"
    },
    nombrePersonnes: {
        type: "field",
        name: "nombrePersonnes",
        domain: DO_QUANTITE,
        isRequired: true,
        label: "NombrePersonnes"
    },
    commentaire: {
        type: "field",
        name: "commentaire",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Commentaire"
    },
    confirmee: {
        type: "field",
        name: "confirmee",
        domain: DO_BOOLEEN,
        defaultValue: false,
        isRequired: true,
        label: "Confirmee"
    },
    clientId: {
        type: "field",
        name: "clientId",
        domain: DO_ID,
        isRequired: true,
        label: "ClientId"
    },
    tableId: {
        type: "field",
        name: "tableId",
        domain: DO_ID,
        isRequired: false,
        label: "TableId"
    },
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: true,
        label: "RestaurantId"
    }
} as const;
