////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ENTIER, DO_ID, DO_LIBELLE} from "@domains";

export interface ProfilItem {
    id?: number,
    libelle?: string,
    nombreUtilisateurs?: number
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
    },
    nombreUtilisateurs: {
        type: "field",
        name: "nombreUtilisateurs",
        domain: DO_ENTIER,
        isRequired: true,
        label: "securite.profil.profilItem.nombreUtilisateurs"
    }
} as const
