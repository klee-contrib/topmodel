////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_EMAIL, DO_ID, DO_NUMBER} from "@domains";

import {TypeUtilisateurCode} from "./references";

export interface UtilisateurDto {
    utilisateurId?: number,
    utilisateurAge?: number,
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
    utilisateurAge: {
        type: "field",
        name: "utilisateurAge",
        domain: DO_NUMBER,
        isRequired: false,
        label: "utilisateur.utilisateur.age"
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
