////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_ID_2} from "../../domains";

export type FactureItem = EntityToType<FactureItemEntityType>;
export type FactureItemEntityType = typeof FactureItemEntity;

export const FactureItemEntity = entity({
    id: e.field(DO_ID, f => f
        .label("restaurant.facture.id")
    ),
    commandeId: e.field(DO_ID_2, f => f
        .label("restaurant.facture.commandeId")
    )
});
