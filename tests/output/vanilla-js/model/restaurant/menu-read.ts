////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_PRIX, DO_SEQ_ID} from "../../domains";

import {CategoriePlat} from "./enums";
import {PlatItemEntity, PlatItem} from "./plat-item";

export interface MenuRead {
    id: number;
    nom: string;
    description?: string;
    prix: number;
    disponible: boolean;
    dateDebut?: string;
    dateFin?: string;
    restaurantId: number;
    dateCreation: string;
    categoriesPlat: CategoriePlat[];
    plats: PlatItem[];
}

export const MenuReadEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_SEQ_ID,
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
    dateDebut: {
        type: "field",
        name: "dateDebut",
        domain: DO_DATE_HEURE,
        isRequired: false,
        label: "DateDebut"
    },
    dateFin: {
        type: "field",
        name: "dateFin",
        domain: DO_DATE_HEURE,
        isRequired: false,
        label: "DateFin"
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
    categoriesPlat: {
        type: "field",
        name: "categoriesPlat",
        domain: DO_LISTE,
        isRequired: true,
        label: "CategoriesPlat"
    },
    plats: {
        type: "list",
        entity: PlatItemEntity
    }
} as const;
