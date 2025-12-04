////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_LIBELLE, DO_LISTE, DO_PRIX, DO_QUANTITE, DO_TELEPHONE} from "../../domains";

export type RestaurantAvecStatistiques = EntityToType<RestaurantAvecStatistiquesEntityType>;
export type RestaurantAvecStatistiquesEntityType = typeof RestaurantAvecStatistiquesEntity;

export const RestaurantAvecStatistiquesEntity = entity({
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
    ),
    nombrePlats: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.restaurantAvecStatistiques.nombrePlats")
    ),
    nombreTables: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.restaurantAvecStatistiques.nombreTables")
    ),
    noteMoyenne: e.field(DO_PRIX, f => f.optional()
        .label("restaurant.restaurantAvecStatistiques.noteMoyenne")
    )
});
