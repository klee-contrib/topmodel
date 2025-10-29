////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ENTIER, DO_ID, DO_LIBELLE} from "../../../domains";

export type ProfilItem = EntityToType<ProfilItemEntityType>;
export type ProfilItemEntityType = typeof ProfilItemEntity;

export const ProfilItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("securite.profil.profil.id")
    ),
    libelle: e.field(DO_LIBELLE, f => f
        .label("securite.profil.profil.libelle")
    ),
    nombreUtilisateurs: e.field(DO_ENTIER, f => f
        .label("securite.profil.profilItem.nombreUtilisateurs")
    )
});
