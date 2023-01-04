////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID} from "@domains";

import {UtilisateurDtoEntity, UtilisateurDto} from "../utilisateur/utilisateur-dto";
import {DroitsCode, TypeProfilCode} from "./references";

export interface ProfilDto {
    id?: number,
    typeProfils?: TypeProfilCode[],
    droits?: DroitsCode[],
    secteurs?: number[],
    utilisateurs?: UtilisateurDto[]
}

export const ProfilDtoEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
        isRequired: false,
        label: "securite.profil.id"
    },
    typeProfils: {
        type: "field",
        name: "typeProfils",
        domain: DO_CODE,
        isRequired: false,
        label: "securite.profil.typeProfils"
    },
    droits: {
        type: "field",
        name: "droits",
        domain: DO_CODE,
        isRequired: false,
        label: "securite.profil.droits"
    },
    secteurs: {
        type: "field",
        name: "secteurs",
        domain: DO_ID,
        isRequired: false,
        label: "securite.profil.secteurs"
    },
    utilisateurs: {
        type: "list",
        entity: UtilisateurDtoEntity
    }
} as const
