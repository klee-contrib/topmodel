////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_CODE, DO_ID, DO_LISTE, DO_QUANTITE} from "../../domains";

export type TableClientWrite = EntityToType<TableClientWriteEntityType>;
export type TableClientWriteEntityType = typeof TableClientWriteEntity;

export const TableClientWriteEntity = entity({
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
