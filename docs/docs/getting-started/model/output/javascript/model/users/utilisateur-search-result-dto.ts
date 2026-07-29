////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE, DO_EMAIL, DO_LIBELLE} from "../../domains";

import {TypeUtilisateurCode} from "../refs/enums";

export interface UtilisateurSearchResultDto {
    email: string;
    nom?: string;
    dateInscription?: string;
    typeUtilisateurCode?: TypeUtilisateurCode;
    libelleTypeUtilisateur: string;
}

export const UtilisateurSearchResultDtoEntity = {
    email: {
        type: "field",
        name: "email",
        domain: DO_EMAIL,
        isRequired: true,
        label: "users.utilisateur.email"
    },
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "users.utilisateur.nom"
    },
    dateInscription: {
        type: "field",
        name: "dateInscription",
        domain: DO_DATE,
        isRequired: false,
        label: "users.utilisateur.dateInscription"
    },
    typeUtilisateurCode: {
        type: "field",
        name: "typeUtilisateurCode",
        domain: DO_CODE,
        isRequired: false,
        label: "users.utilisateur.typeUtilisateurCode"
    },
    libelleTypeUtilisateur: {
        type: "field",
        name: "libelleTypeUtilisateur",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "refs.typeUtilisateur.libelle"
    }
} as const;
