////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID_2} from "../../domains";

export interface CommandeResume {
    id: number;
}

export const CommandeResumeEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID_2,
        isRequired: true,
        label: "Id"
    }
} as const;
