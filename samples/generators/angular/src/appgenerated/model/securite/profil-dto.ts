////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_CODE_LIST, DO_ID} from "@domains";

import {UtilisateurDtoEntity, UtilisateurDto} from "../utilisateur/utilisateur-dto";
import {DroitCode, TypeProfilCode} from "./references";
import {SecteurDtoEntity, SecteurDto} from "./secteur-dto";

export interface ProfilDto {
    id?: number,
    typeProfilCode?: TypeProfilCode,
    droits?: DroitCode[],
    utilisateurs?: UtilisateurDto[],
    secteurs?: SecteurDto[]
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
        domain: DO_CODE_LIST,
        isRequired: false,
        label: "securite.profil.droits"
    },
    utilisateurs: {
        type: "list",
        entity: UtilisateurDtoEntity
    },
    secteurs: {
        type: "list",
        entity: SecteurDtoEntity
    }
} as const
