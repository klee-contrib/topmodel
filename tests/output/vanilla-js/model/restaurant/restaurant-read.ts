////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "../../domains";

import {TableItemEntity, TableItem} from "./table-item";

export interface RestaurantRead {
    id: number;
    nom: string;
    adresse?: string;
    telephone?: string;
    menus: number[];
    plats: number[];
    promotions?: number[];
    avisClients: number[];
    dateCreation: string;
    tables: TableItem[];
}

export const RestaurantReadEntity = {
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
    },
    menus: {
        type: "field",
        name: "menus",
        domain: DO_LISTE,
        isRequired: true,
        label: "Menus"
    },
    plats: {
        type: "field",
        name: "plats",
        domain: DO_LISTE,
        isRequired: true,
        label: "Plats"
    },
    promotions: {
        type: "field",
        name: "promotions",
        domain: DO_LISTE,
        isRequired: false,
        label: "Promotions"
    },
    avisClients: {
        type: "field",
        name: "avisClients",
        domain: DO_LISTE,
        isRequired: true,
        label: "AvisClients"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    },
    tables: {
        type: "list",
        entity: TableItemEntity
    }
} as const;
