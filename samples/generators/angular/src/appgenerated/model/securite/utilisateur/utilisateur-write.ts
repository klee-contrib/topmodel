////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE, DO_EMAIL, DO_ID, DO_LIBELLE} from "@domains";

import {TypeUtilisateurCode} from "./references";

export interface UtilisateurWrite {
    nom?: string,
    prenom?: string,
    email?: string,
    dateNaissance?: string,
    actif?: boolean,
    profilId?: number,
    typeUtilisateurCode?: TypeUtilisateurCode
}

export const UtilisateurWriteEntity = {
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
    dateNaissance: {
        type: "field",
        name: "dateNaissance",
        domain: DO_DATE,
        isRequired: false,
        label: "securite.utilisateur.utilisateur.dateNaissance"
    },
    actif: {
        type: "field",
        name: "actif",
        domain: DO_BOOLEEN,
        isRequired: true,
        defaultValue: true,
        label: "securite.utilisateur.utilisateur.actif"
    },
    profilId: {
        type: "field",
        name: "profilId",
        domain: DO_ID,
        isRequired: true,
        label: "securite.utilisateur.utilisateur.profilId"
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
