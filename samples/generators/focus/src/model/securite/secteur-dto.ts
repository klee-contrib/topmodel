////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2} from "@focus4/stores";
import {DO_ID} from "../../domains";

export type SecteurDto = EntityToType<SecteurDtoEntityType>
export interface SecteurDtoEntityType {
    id: FieldEntry2<typeof DO_ID, number>
}

export const SecteurDtoEntity: SecteurDtoEntityType = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "securite.secteur.id"
    }
}
