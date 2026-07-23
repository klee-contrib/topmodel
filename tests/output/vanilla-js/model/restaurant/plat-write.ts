////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_PRIX} from "../../domains";

import {CategoriePlatCode} from "./enums";

export interface PlatWrite {
    nom: string;
    description?: string;
    prix: number;
    disponible: boolean;
    categoriePlatCode: CategoriePlatCode;
    restaurantId: number;
    dateCreation: string;
}

export const PlatWriteEntity = {
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Nom"
    },
    description: {
        type: "field",
        name: "description",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "Description"
    },
    prix: {
        type: "field",
        name: "prix",
        domain: DO_PRIX,
        isRequired: true,
        label: "Prix"
    },
    disponible: {
        type: "field",
        name: "disponible",
        domain: DO_BOOLEEN,
        defaultValue: true,
        isRequired: true,
        label: "Disponible"
    },
    categoriePlatCode: {
        type: "field",
        name: "categoriePlatCode",
        domain: DO_CODE,
        isRequired: true,
        label: "CategoriePlatCode"
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
    }
} as const;
