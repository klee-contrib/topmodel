////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "../../domains";

export type RestaurantRead = EntityToType<RestaurantReadEntityType>;
export type RestaurantReadEntityType = typeof RestaurantReadEntity;

export const RestaurantReadEntity = entity({
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
    ),
    tableClientsRestaurant: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.tableClientsRestaurant")
    ),
    platsRestaurant: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.platsRestaurant")
    ),
    avisClientsRestaurant: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.avisClientsRestaurant")
    ),
    menusRestaurant: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.menusRestaurant")
    ),
    reservationsRestaurant: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.reservationsRestaurant")
    ),
    promotionsRestaurant: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.restaurant.promotionsRestaurant")
    )
});
