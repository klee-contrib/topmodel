////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE_HEURE, DO_ID, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type TableRead = EntityToType<TableReadEntityType>;
export type TableReadEntityType = typeof TableReadEntity;

export const TableReadEntity = entity({
    id: e.field(DO_ID, f => f
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
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    )
});
