////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2} from "@focus4/stores";
import {DO_ID, DO_LIBELLE} from "../../../domains";

export type ProfilItem = EntityToType<ProfilItemEntityType>
export interface ProfilItemEntityType {
    id: FieldEntry2<typeof DO_ID, number>,
    libelle: FieldEntry2<typeof DO_LIBELLE, string>
}

export const ProfilItemEntity: ProfilItemEntityType = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "securite.profil.profil.id"
    },
    libelle: {
        type: "field",
        name: "libelle",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "securite.profil.profil.libelle"
    }
}
