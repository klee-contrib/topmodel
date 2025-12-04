////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_PRIX, DO_QUANTITE} from "../../domains";

export type StatistiquesRestaurant = EntityToType<StatistiquesRestaurantEntityType>;
export type StatistiquesRestaurantEntityType = typeof StatistiquesRestaurantEntity;

export const StatistiquesRestaurantEntity = entity({
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.statistiquesRestaurant.restaurantId")
    ),
    nombreCommandes: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.statistiquesRestaurant.nombreCommandes")
    ),
    chiffreAffaires: e.field(DO_PRIX, f => f.defaultValue(0)
        .label("restaurant.statistiquesRestaurant.chiffreAffaires")
    ),
    nombreClients: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.statistiquesRestaurant.nombreClients")
    ),
    noteMoyenne: e.field(DO_PRIX, f => f.optional()
        .label("restaurant.statistiquesRestaurant.noteMoyenne")
    )
});
