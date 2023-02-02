////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID} from "@domains";

import {UtilisateurDtoEntity, UtilisateurDto} from "../utilisateur/utilisateur-dto";
import {DroitCode, TypeProfilCode} from "./references";

export interface ProfilDto {
    id?: number,
    typeProfilCode?: TypeProfilCode,
    droits?: DroitCode[],
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
    typeProfilCode: {
        type: "field",
        name: "typeProfilCode",
        domain: DO_CODE,
        isRequired: false,
        label: "securite.profil.typeProfilCode"
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
