////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_QUANTITE, DO_TELEPHONE} from "../../domains";

import {TableReadEntity, TableRead} from "./table-read";

export interface RestaurantAvecStatistiques {
    id: number;
    nom: string;
    adresse?: string;
    telephone?: string;
    menus: number[];
    plats: number[];
    promotions?: number[];
    avisClients: number[];
    tableIds: number[];
    dateCreation: string;
    tables: TableRead[];
    nombrePlats: number;
    nombreTables: number;
}

export const RestaurantAvecStatistiquesEntity = {
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
        defaultValue: "XX.XX.XX.XX.XX",
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
    tableIds: {
        type: "field",
        name: "tableIds",
        domain: DO_LISTE,
        isRequired: true,
        label: "TableIds"
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
        entity: TableReadEntity
    },
    nombrePlats: {
        type: "field",
        name: "nombrePlats",
        domain: DO_QUANTITE,
        defaultValue: 0,
        isRequired: true,
        label: "NombrePlats"
    },
    nombreTables: {
        type: "field",
        name: "nombreTables",
        domain: DO_QUANTITE,
        defaultValue: 0,
        isRequired: true,
        label: "NombreTables"
    }
} as const;
