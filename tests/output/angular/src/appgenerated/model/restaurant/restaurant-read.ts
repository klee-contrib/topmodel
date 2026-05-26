////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {TableItemEntity} from "./table-item";

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
    menus: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.menus")
    ),
    plats: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.plats")
    ),
    promotions: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.restaurant.promotions")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.avisClients")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    ),
    tables: e.list(TableItemEntity, f => f
        .label("restaurant.restaurant.tableIds")
    )
});
