////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_EMAIL, DO_ID, DO_NUMBER} from "@domains";

import {TypeUtilisateurCode} from "./references";

export interface UtilisateurDto {
    id?: number,
    age?: number,
    profilId?: number,
    email?: string,
    typeUtilisateurCode?: TypeUtilisateurCode,
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
    age: {
        type: "field",
        name: "age",
        domain: DO_NUMBER,
        isRequired: false,
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
    typeUtilisateurCode: {
        type: "field",
        name: "typeUtilisateurCode",
        domain: DO_CODE,
        isRequired: false,
        label: "utilisateur.utilisateur.typeUtilisateurCode"
    },
    utilisateurParent: {
        type: "object",
    }
} as const
