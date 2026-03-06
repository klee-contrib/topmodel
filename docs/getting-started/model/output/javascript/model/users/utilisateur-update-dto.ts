////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2} from "@focus4/stores";
import {DO_LIBELLE} from "../../domains";

export type UtilisateurUpdateDto = EntityToType<UtilisateurUpdateDtoEntityType>;
export interface UtilisateurUpdateDtoEntityType {
    nom: FieldEntry2<typeof DO_LIBELLE, string>;
}

export const UtilisateurUpdateDtoEntity: UtilisateurUpdateDtoEntityType = {
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "users.utilisateur.nom"
    }
};
