////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_EMAIL, DO_ID} from "@domains";

import {TypeProfilCode} from "../securite/references";
import {TypeUtilisateurCode} from "./references";

export interface UtilisateurDto {
    id?: number,
    email?: string,
    typeUtilisateurCode?: TypeUtilisateurCode,
    profilId?: number,
    profilTypeProfilCode?: TypeProfilCode,
    utilisateurParent?: UtilisateurDto
}

export const UtilisateurDtoEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "utilisateur.utilisateur.id"
    },
    email: {
        type: "field",
        name: "email",
        domain: DO_EMAIL,
        isRequired: false,
        label: "utilisateur.utilisateur.email"
    },
    typeUtilisateurCode: {
        type: "field",
        name: "typeUtilisateurCode",
        domain: DO_CODE,
        isRequired: false,
        label: "utilisateur.utilisateur.typeUtilisateurCode"
    },
    profilId: {
        type: "field",
        name: "profilId",
        domain: DO_ID,
        isRequired: true,
        label: "securite.profil.id"
    },
    profilTypeProfilCode: {
        type: "field",
        name: "profilTypeProfilCode",
        domain: DO_CODE,
        isRequired: false,
        label: "securite.profil.typeProfilCode"
    },
    utilisateurParent: {
        type: "object",
    }
} as const
