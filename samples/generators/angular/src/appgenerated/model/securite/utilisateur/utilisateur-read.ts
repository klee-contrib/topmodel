////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE, DO_DATE_HEURE, DO_EMAIL, DO_ID, DO_LIBELLE} from "@domains";

import {TypeUtilisateurCode} from "./references";

export interface UtilisateurRead {
    id?: number,
    nom?: string,
    prenom?: string,
    email?: string,
    dateNaissance?: string,
    actif?: boolean,
    profilId?: number,
    typeUtilisateurCode?: TypeUtilisateurCode,
    dateCreation?: string,
    dateModification?: string
}

export const UtilisateurReadEntity = {
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
    }
} as const
