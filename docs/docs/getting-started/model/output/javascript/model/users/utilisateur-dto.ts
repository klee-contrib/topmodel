////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2} from "@focus4/stores";
import {DO_LIBELLE, DO_PAGE} from "../../domains";

import {api-types} from "@/services";
import {AdresseDto} from "./adresse-dto";

export type UtilisateurDto = EntityToType<UtilisateurDtoEntityType>;
export interface UtilisateurDtoEntityType {
    nomUtilisateur: FieldEntry2<typeof DO_LIBELLE, string>;
    adresse: FieldEntry2<typeof DO_PAGE, Page<AdresseDto>>;
}

export const UtilisateurDtoEntity: UtilisateurDtoEntityType = {
    nomUtilisateur: {
        type: "field",
        name: "nomUtilisateur",
        domain: DO_LIBELLE,
        isRequired: false,
        label: "users.utilisateur.nom"
    },
    adresse: {
        type: "field",
        name: "adresse",
        domain: DO_PAGE,
        isRequired: true,
        label: "users.utilisateurDto.adresse"
    }
};
