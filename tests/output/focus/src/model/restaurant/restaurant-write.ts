////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "../../domains";

import {TableItemEntity} from "./table-item";

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
    tableIds: e.list(TableItemEntity, f => f
        .label("restaurant.restaurant.tableIds")
    )
});
