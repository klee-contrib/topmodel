////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "../../domains";

import {TableItemEntity, TableItem} from "./table-item";

export interface RestaurantWrite {
    nom: string;
    adresse?: string;
    telephone?: string;
    menus: number[];
    plats: number[];
    promotions?: number[];
    avisClients: number[];
    tables: TableItem[];
}

export const RestaurantWriteEntity = {
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
    tables: {
        type: "list",
        entity: TableItemEntity
    }
} as const;
