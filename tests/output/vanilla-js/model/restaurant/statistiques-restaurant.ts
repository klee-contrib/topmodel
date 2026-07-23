////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_PRIX, DO_QUANTITE} from "../../domains";

export interface StatistiquesRestaurant {
    restaurantId: number;
    nombreCommandes: number;
    chiffreAffaires: number;
    nombreClients: number;
    noteMoyenne?: number;
}

export const StatistiquesRestaurantEntity = {
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: true,
        label: "RestaurantId"
    },
    nombreCommandes: {
        type: "field",
        name: "nombreCommandes",
        domain: DO_QUANTITE,
        defaultValue: 0,
        isRequired: true,
        label: "NombreCommandes"
    },
    chiffreAffaires: {
        type: "field",
        name: "chiffreAffaires",
        domain: DO_PRIX,
        defaultValue: 0,
        isRequired: true,
        label: "ChiffreAffaires"
    },
    nombreClients: {
        type: "field",
        name: "nombreClients",
        domain: DO_QUANTITE,
        defaultValue: 0,
        isRequired: true,
        label: "NombreClients"
    },
    noteMoyenne: {
        type: "field",
        name: "noteMoyenne",
        domain: DO_PRIX,
        isRequired: false,
        label: "NoteMoyenne"
    }
} as const;
