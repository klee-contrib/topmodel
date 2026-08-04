////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type TableWrite = EntityToType<TableWriteEntityType>;
export type TableWriteEntityType = typeof TableWriteEntity;

export const TableWriteEntity = entity({
    numero: e.field(DO_CODE, f => f
        .label("restaurant.table.numero")
        .comment("comments.restaurant.table.numero")
    ),
    capacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.table.capacite")
        .comment("comments.restaurant.table.capacite")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.table.disponible")
        .comment("comments.restaurant.table.disponible")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.table.restaurantId")
        .comment("comments.restaurant.table.restaurantId")
    )
});
