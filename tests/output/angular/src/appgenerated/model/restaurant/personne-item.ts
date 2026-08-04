////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_LIBELLE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type PersonneItem = EntityToType<PersonneItemEntityType>;
export type PersonneItemEntityType = typeof PersonneItemEntity;

export const PersonneItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.personneItem.id")
        .comment("comments.restaurant.personneItem.id")
    ),
    nom: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.personneItem.nom")
        .comment("comments.restaurant.personneItem.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.personneItem.prenom")
        .comment("comments.restaurant.personneItem.prenom")
    )
});
