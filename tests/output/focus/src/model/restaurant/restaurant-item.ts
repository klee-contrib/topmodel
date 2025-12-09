////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_LIBELLE, DO_TELEPHONE} from "../../domains";

export type RestaurantItem = EntityToType<RestaurantItemEntityType>;
export type RestaurantItemEntityType = typeof RestaurantItemEntity;

export const RestaurantItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.restaurant.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.restaurant.nom")
    ),
    adresse: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.restaurant.adresse")
    ),
    telephone: e.field(DO_TELEPHONE, f => f.optional()
        .label("restaurant.restaurant.telephone")
    )
});
