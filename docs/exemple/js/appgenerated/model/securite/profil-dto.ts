////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID} from "@domains";

import {UtilisateurDtoEntity, UtilisateurDto} from "../utilisateur/utilisateur-dto";
import {DroitsCode, TypeProfilCode} from "./references";

export interface ProfilDto {
    id?: number,
    typeProfilCode?: TypeProfilCode,
    droitsAppli?: DroitsCode[],
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
    droitsAppli: {
        type: "field",
        name: "droitsAppli",
        domain: DO_CODE,
        isRequired: false,
        label: "securite.profil.droitsAppli"
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
