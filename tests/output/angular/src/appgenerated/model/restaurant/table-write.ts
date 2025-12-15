////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type TableWrite = EntityToType<TableWriteEntityType>;
export type TableWriteEntityType = typeof TableWriteEntity;

export const TableWriteEntity = entity({
    numero: e.field(DO_CODE, f => f
        .label("restaurant.tableRestaurant.numero")
    ),
    capacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.tableRestaurant.capacite")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.tableRestaurant.disponible")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.tableRestaurant.restaurantId")
    )
});
