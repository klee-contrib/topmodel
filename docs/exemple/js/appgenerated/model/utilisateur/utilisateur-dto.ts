////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_EMAIL, DO_ID} from "@domains";

import {TypeUtilisateurCode} from "./references";

export interface UtilisateurDto {
    utilisateurId?: number,
    utilisateuremail?: string,
    utilisateurTypeUtilisateurCode?: TypeUtilisateurCode,
    utilisateurParent?: UtilisateurDto
}

export const UtilisateurDtoEntity = {
    utilisateurId: {
        type: "field",
        name: "utilisateurId",
        domain: DO_ID,
        isRequired: true,
        label: "utilisateur.utilisateur.id"
    },
    utilisateuremail: {
        type: "field",
        name: "utilisateuremail",
        domain: DO_EMAIL,
        isRequired: false,
        label: "utilisateur.utilisateur.email"
    },
    utilisateurTypeUtilisateurCode: {
        type: "field",
        name: "utilisateurTypeUtilisateurCode",
        domain: DO_CODE,
        isRequired: false,
        label: "utilisateur.utilisateur.typeUtilisateurCode"
    },
    utilisateurParent: {
        type: "object",
    }
} as const
