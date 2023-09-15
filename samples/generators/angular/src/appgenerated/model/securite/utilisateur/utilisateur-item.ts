////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_EMAIL, DO_ID, DO_LIBELLE} from "@domains";

import {TypeUtilisateurCode} from "./references";

export interface UtilisateurItem {
    id?: number,
    nom?: string,
    prenom?: string,
    email?: string,
    typeUtilisateurCode?: TypeUtilisateurCode
}

export const UtilisateurItemEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "securite.utilisateur.utilisateur.id"
    },
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "securite.utilisateur.utilisateur.nom"
    },
    prenom: {
        type: "field",
        name: "prenom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "securite.utilisateur.utilisateur.prenom"
    },
    email: {
        type: "field",
        name: "email",
        domain: DO_EMAIL,
        isRequired: true,
        label: "securite.utilisateur.utilisateur.email"
    },
    typeUtilisateurCode: {
        type: "field",
        name: "typeUtilisateurCode",
        domain: DO_CODE,
        isRequired: true,
        defaultValue: "GEST",
        label: "securite.utilisateur.utilisateur.typeUtilisateurCode"
    }
} as const
