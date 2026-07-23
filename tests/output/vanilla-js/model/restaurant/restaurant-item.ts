////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_LIBELLE, DO_TELEPHONE} from "../../domains";

export interface RestaurantItem {
    id: number;
    nom: string;
    adresse?: string;
    telephone?: string;
}

export const RestaurantItemEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: true,
        label: "Id"
    },
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Nom"
    },
    adresse: {
        type: "field",
        name: "adresse",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Adresse"
    },
    telephone: {
        type: "field",
        name: "telephone",
        domain: DO_TELEPHONE,
        isRequired: false,
        label: "Telephone"
    }
} as const;
