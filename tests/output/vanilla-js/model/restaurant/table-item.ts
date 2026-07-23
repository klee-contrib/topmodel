////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_QUANTITE} from "../../domains";

export interface TableItem {
    id: number;
    numero: string;
    capacite: number;
    disponible: boolean;
    restaurantId: number;
}

export const TableItemEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: true,
        label: "Id"
    },
    numero: {
        type: "field",
        name: "numero",
        domain: DO_CODE,
        isRequired: true,
        label: "Numero"
    },
    capacite: {
        type: "field",
        name: "capacite",
        domain: DO_QUANTITE,
        isRequired: true,
        label: "Capacite"
    },
    disponible: {
        type: "field",
        name: "disponible",
        domain: DO_BOOLEEN,
        defaultValue: true,
        isRequired: true,
        label: "Disponible"
    },
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: true,
        label: "RestaurantId"
    }
} as const;
