////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_ID_2} from "../../domains";

export interface FactureItem {
    id: number;
    commandeId: number;
}

export const FactureItemEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: true,
        label: "Id"
    },
    commandeId: {
        type: "field",
        name: "commandeId",
        domain: DO_ID_2,
        isRequired: true,
        label: "CommandeId"
    }
} as const;
