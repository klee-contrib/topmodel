////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_LIBELLE} from "../../domains";

import {PersonneItemEntity, PersonneItem} from "./personne-item";

export interface ClientItem extends PersonneItem {
    nomComplet?: string;
}

export const ClientItemEntity = {
    ...PersonneItemEntity,
    nomComplet: {
        type: "field",
        name: "nomComplet",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "NomComplet"
    }
} as const;
