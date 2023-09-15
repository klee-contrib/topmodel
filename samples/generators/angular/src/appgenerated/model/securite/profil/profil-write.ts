////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE_LISTE, DO_LIBELLE} from "@domains";

import {DroitCode} from "./references";

export interface ProfilWrite {
    libelle?: string,
    droits?: DroitCode[]
}

export const ProfilWriteEntity = {
    libelle: {
        type: "field",
        name: "libelle",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "securite.profil.profil.libelle"
    },
    droits: {
        type: "field",
        name: "droits",
        domain: DO_CODE_LISTE,
        isRequired: false,
        label: "securite.profil.profil.droits"
    }
} as const
