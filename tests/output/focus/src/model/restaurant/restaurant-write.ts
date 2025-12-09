////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "../../domains";

export type RestaurantWrite = EntityToType<RestaurantWriteEntityType>;
export type RestaurantWriteEntityType = typeof RestaurantWriteEntity;

export const RestaurantWriteEntity = entity({
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
