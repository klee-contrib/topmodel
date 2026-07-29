////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_LIBELLE} from "../../domains";

export interface UtilisateurUpdateDto {
    nom?: string;
}

export const UtilisateurUpdateDtoEntity = {
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "users.utilisateur.nom"
    }
} as const;
