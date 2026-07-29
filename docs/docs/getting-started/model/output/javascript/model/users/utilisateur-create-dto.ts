////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE, DO_EMAIL, DO_LIBELLE} from "../../domains";

import {TypeUtilisateurCode} from "../refs/enums";

export interface UtilisateurCreateDto {
    utilisateurEmail: string;
    utilisateurNom?: string;
    utilisateurDateInscription?: string;
    utilisateurTypeUtilisateurCode?: TypeUtilisateurCode;
}

export const UtilisateurCreateDtoEntity = {
    utilisateurEmail: {
        type: "field",
        name: "utilisateurEmail",
        domain: DO_EMAIL,
        isRequired: true,
        label: "users.utilisateur.email"
    },
    utilisateurNom: {
        type: "field",
        name: "utilisateurNom",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "users.utilisateur.nom"
    },
    utilisateurDateInscription: {
        type: "field",
        name: "utilisateurDateInscription",
        domain: DO_DATE,
        isRequired: false,
        label: "users.utilisateur.dateInscription"
    },
    utilisateurTypeUtilisateurCode: {
        type: "field",
        name: "utilisateurTypeUtilisateurCode",
        domain: DO_CODE,
        isRequired: false,
        label: "users.utilisateur.typeUtilisateurCode"
    }
} as const;
