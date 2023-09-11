////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEAN, DO_CODE, DO_DATE_CREATION, DO_DATE_MODIFICATION, DO_EMAIL, DO_ID, DO_ID_LIST, DO_LIBELLE, DO_NUMBER} from "@domains";

import {today} from "../../common/utils";
import {TypeUtilisateurCode} from "./references";

export interface UtilisateurSearch {
    id?: number,
    age?: number,
    profilId?: number,
    email?: string,
    nom?: string,
    actif?: boolean,
    typeUtilisateurCode?: TypeUtilisateurCode,
    utilisateursEnfant?: number[],
    dateCreation?: string,
    dateModification?: string
}

export const UtilisateurSearchEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "utilisateur.utilisateur.id"
    },
    age: {
        type: "field",
        name: "age",
        domain: DO_NUMBER,
        isRequired: false,
        defaultValue: 6,
        label: "utilisateur.utilisateur.age"
    },
    profilId: {
        type: "field",
        name: "profilId",
        domain: DO_ID,
        isRequired: false,
        label: "utilisateur.utilisateur.profilId"
    },
    email: {
        type: "field",
        name: "email",
        domain: DO_EMAIL,
        isRequired: false,
        label: "utilisateur.utilisateur.email"
    },
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: false,
        defaultValue: "Jabx",
        label: "utilisateur.utilisateur.nom"
    },
    actif: {
        type: "field",
        name: "actif",
        domain: DO_BOOLEAN,
        isRequired: false,
        label: "utilisateur.utilisateur.actif"
    },
    typeUtilisateurCode: {
        type: "field",
        name: "typeUtilisateurCode",
        domain: DO_CODE,
        isRequired: false,
        defaultValue: "ADM",
        label: "utilisateur.utilisateur.typeUtilisateurCode"
    },
    utilisateursEnfant: {
        type: "field",
        name: "utilisateursEnfant",
        domain: DO_ID_LIST,
        isRequired: false,
        label: "utilisateur.utilisateur.utilisateursEnfant"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_CREATION,
        isRequired: false,
        defaultValue: today().toISO(),
        label: "utilisateur.entityListeners.dateCreation"
    },
    dateModification: {
        type: "field",
        name: "dateModification",
        domain: DO_DATE_MODIFICATION,
        isRequired: false,
        defaultValue: today().toISO(),
        label: "utilisateur.entityListeners.dateModification"
    }
} as const
