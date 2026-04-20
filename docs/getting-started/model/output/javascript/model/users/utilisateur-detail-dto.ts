////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2} from "@focus4/stores";
import {DO_CODE, DO_DATE, DO_EMAIL, DO_LIBELLE} from "../../domains";

import {TypeUtilisateurCode} from "../refs/enums";

export type UtilisateurDetailDto = EntityToType<UtilisateurDetailDtoEntityType>;
export interface UtilisateurDetailDtoEntityType {
    email: FieldEntry2<typeof DO_EMAIL, string>;
    nom: FieldEntry2<typeof DO_LIBELLE, string>;
    dateInscription: FieldEntry2<typeof DO_DATE, string>;
    typeUtilisateurCode: FieldEntry2<typeof DO_CODE, TypeUtilisateurCode>;
    libelleTypeUtilisateur: FieldEntry2<typeof DO_LIBELLE, string>;
}

export const UtilisateurDetailDtoEntity: UtilisateurDetailDtoEntityType = {
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
};
