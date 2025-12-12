////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_LISTE, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type TableRead = EntityToType<TableReadEntityType>;
export type TableReadEntityType = typeof TableReadEntity;

export const TableReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.table.id")
    ),
    numero: e.field(DO_CODE, f => f
        .label("restaurant.table.numero")
    ),
    capacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.table.capacite")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.table.disponible")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.table.restaurantId")
    ),
    commandes: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.table.commandes")
    ),
    reservations: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.table.reservations")
    )
});
