////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE_LISTE, DO_LIBELLE} from "../../../domains";

import {DroitCode} from "./references";

export type ProfilWrite = EntityToType<ProfilWriteEntityType>;
export type ProfilWriteEntityType = typeof ProfilWriteEntity;

export const ProfilWriteEntity = entity({
    libelle: e.field(DO_LIBELLE, f => f
        .label("securite.profil.profil.libelle")
    ),
    droits: e.field(DO_CODE_LISTE, f => f.type<DroitCode[]>().optional()
        .label("securite.profil.profilRead.droits")
    )
});
