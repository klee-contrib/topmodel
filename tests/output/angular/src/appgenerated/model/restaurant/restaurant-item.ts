////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_LIBELLE, DO_TELEPHONE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type RestaurantItem = EntityToType<RestaurantItemEntityType>;
export type RestaurantItemEntityType = typeof RestaurantItemEntity;

export const RestaurantItemEntity = entity({
    id: e.field(DO_ID, f => f
        .label("restaurant.lieu.id")
        .comment("comments.restaurant.lieu.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.lieu.nom")
        .comment("comments.restaurant.lieu.nom")
    ),
    adresse: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.lieu.adresse")
        .comment("comments.restaurant.lieu.adresse")
    ),
    telephone: e.field(DO_TELEPHONE, f => f.defaultValue("XX.XX.XX.XX.XX").optional()
        .label("restaurant.restaurant.telephone")
        .comment("comments.restaurant.restaurant.telephone")
    )
});
