////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_PRIX, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type StatistiquesRestaurant = EntityToType<StatistiquesRestaurantEntityType>;
export type StatistiquesRestaurantEntityType = typeof StatistiquesRestaurantEntity;

export const StatistiquesRestaurantEntity = entity({
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.statistiquesRestaurant.restaurantId")
        .comment("comments.restaurant.statistiquesRestaurant.restaurantId")
    ),
    nombreCommandes: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.statistiquesRestaurant.nombreCommandes")
        .comment("comments.restaurant.statistiquesRestaurant.nombreCommandes")
    ),
    chiffreAffaires: e.field(DO_PRIX, f => f.defaultValue(0)
        .label("restaurant.statistiquesRestaurant.chiffreAffaires")
        .comment("comments.restaurant.statistiquesRestaurant.chiffreAffaires")
    ),
    nombreClients: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.statistiquesRestaurant.nombreClients")
        .comment("comments.restaurant.statistiquesRestaurant.nombreClients")
    ),
    noteMoyenne: e.field(DO_PRIX, f => f.optional()
        .label("restaurant.statistiquesRestaurant.noteMoyenne")
        .comment("comments.restaurant.statistiquesRestaurant.noteMoyenne")
    )
});
