////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_LISTE, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type TableClientRead = EntityToType<TableClientReadEntityType>;
export type TableClientReadEntityType = typeof TableClientReadEntity;

export const TableClientReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.tableClient.id")
    ),
    numero: e.field(DO_CODE, f => f
        .label("restaurant.tableClient.numero")
    ),
    capacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.tableClient.capacite")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.tableClient.disponible")
    ),
    restaurantIdRestaurant: e.field(DO_ID, f => f
        .label("restaurant.tableClient.restaurantIdRestaurant")
    ),
    commandes: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.tableClient.commandes")
    ),
    reservationsTable: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.tableClient.reservationsTable")
    )
});
