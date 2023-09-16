////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_LIBELLE} from "@domains";

export interface ProfilItem {
    id?: number,
    libelle?: string
}

export const ProfilItemEntity = {
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
} as const
