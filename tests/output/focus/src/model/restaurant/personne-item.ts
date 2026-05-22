////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_LIBELLE} from "../../domains";

export type PersonneItem = EntityToType<PersonneItemEntityType>;
export type PersonneItemEntityType = typeof PersonneItemEntity;

export const PersonneItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.personneItem.id")
    ),
    nom: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.personneItem.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.personneItem.prenom")
    )
});
