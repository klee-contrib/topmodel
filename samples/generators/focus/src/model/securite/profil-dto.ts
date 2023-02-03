////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {EntityToType, FieldEntry2, ListEntry, StoreNode} from "@focus4/stores";
import {DO_CODE, DO_CODE_LIST, DO_ID, DO_ID_LIST} from "../../domains";

import {UtilisateurDtoEntity, UtilisateurDtoEntityType} from "../utilisateur/utilisateur-dto";
import {DroitCode, TypeProfilCode} from "./references";

export type ProfilDto = EntityToType<ProfilDtoEntityType>;
export type ProfilDtoNode = StoreNode<ProfilDtoEntityType>;
export interface ProfilDtoEntityType {
    id: FieldEntry2<typeof DO_ID, number>,
    typeProfilCode: FieldEntry2<typeof DO_CODE, TypeProfilCode>,
    droits: FieldEntry2<typeof DO_CODE_LIST, DroitCode[]>,
    secteurs: FieldEntry2<typeof DO_ID_LIST, number[]>,
    utilisateurs: ListEntry<UtilisateurDtoEntityType>
}

export const ProfilDtoEntity: ProfilDtoEntityType = {
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
    secteurs: {
        type: "field",
        name: "secteurs",
        domain: DO_ID_LIST,
        isRequired: false,
        label: "securite.profil.secteurs"
    },
    utilisateurs: {
        type: "list",
        entity: UtilisateurDtoEntity
    }
}
