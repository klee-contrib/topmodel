////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_CODE, DO_ID, DO_QUANTITE} from "../../domains";

export type TableClientItem = EntityToType<TableClientItemEntityType>;
export type TableClientItemEntityType = typeof TableClientItemEntity;

export const TableClientItemEntity = entity({
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
    )
});
