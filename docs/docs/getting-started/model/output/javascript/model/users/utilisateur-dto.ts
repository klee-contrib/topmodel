////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_LIBELLE, DO_PAGE} from "../../domains";

import {api-types} from "@/services";
import {AdresseDto} from "./adresse-dto";

export interface UtilisateurDto {
    nomUtilisateur?: string;
    adresse: Page<AdresseDto>;
}

export const UtilisateurDtoEntity = {
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
} as const;
