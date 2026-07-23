////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE, DO_SEQ_ID} from "../../domains";

export interface ReservationRead {
    id: number;
    dateReservation: string;
    nombrePersonnes: number;
    commentaire?: string;
    confirmee: boolean;
    clientId: number;
    tableId?: number;
    restaurantId: number;
    dateCreation: string;
    clientNom: string;
    clientPrenom: string;
    clientEmail?: string;
    tableNumero: string;
    tableCapacite: number;
}

export const ReservationReadEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_SEQ_ID,
        isRequired: true,
        label: "Id"
    },
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
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    },
    clientNom: {
        type: "field",
        name: "clientNom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Nom"
    },
    clientPrenom: {
        type: "field",
        name: "clientPrenom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Prénom"
    },
    clientEmail: {
        type: "field",
        name: "clientEmail",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Courriel"
    },
    tableNumero: {
        type: "field",
        name: "tableNumero",
        domain: DO_CODE,
        isRequired: true,
        label: "TableNumero"
    },
    tableCapacite: {
        type: "field",
        name: "tableCapacite",
        domain: DO_QUANTITE,
        isRequired: true,
        label: "TableCapacite"
    }
} as const;
