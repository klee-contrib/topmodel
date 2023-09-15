////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE_LISTE, DO_DATE_HEURE, DO_ID, DO_LIBELLE} from "@domains";

import {UtilisateurItemEntity, UtilisateurItem} from "../utilisateur/utilisateur-item";
import {DroitCode} from "./references";

export interface ProfilRead {
    id?: number,
    libelle?: string,
    droits?: DroitCode[],
    dateCreation?: string,
    dateModification?: string,
    utilisateurs?: UtilisateurItem[]
}

export const ProfilReadEntity = {
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
    droits: {
        type: "field",
        name: "droits",
        domain: DO_CODE_LISTE,
        isRequired: false,
        label: "securite.profil.profil.droits"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "common.entityListeners.dateCreation"
    },
    dateModification: {
        type: "field",
        name: "dateModification",
        domain: DO_DATE_HEURE,
        isRequired: false,
        label: "common.entityListeners.dateModification"
    },
    utilisateurs: {
        type: "list",
        entity: UtilisateurItemEntity
    }
} as const
