////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID} from "@domains";

export interface SecteurDto {
    id?: number
}

export const SecteurDtoEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "securite.secteur.id"
    }
} as const
