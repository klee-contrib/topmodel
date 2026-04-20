////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2} from "@focus4/stores";
import {DO_CODE, DO_DATE, DO_EMAIL, DO_LIBELLE} from "../../domains";

import {TypeUtilisateurCode} from "../refs/enums";

export type UtilisateurCreateDto = EntityToType<UtilisateurCreateDtoEntityType>;
export interface UtilisateurCreateDtoEntityType {
    utilisateurEmail: FieldEntry2<typeof DO_EMAIL, string>;
    utilisateurNom: FieldEntry2<typeof DO_LIBELLE, string>;
    utilisateurDateInscription: FieldEntry2<typeof DO_DATE, string>;
    utilisateurTypeUtilisateurCode: FieldEntry2<typeof DO_CODE, TypeUtilisateurCode>;
}

export const UtilisateurCreateDtoEntity: UtilisateurCreateDtoEntityType = {
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
};
