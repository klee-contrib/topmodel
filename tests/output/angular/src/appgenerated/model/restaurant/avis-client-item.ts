////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type AvisClientItem = EntityToType<AvisClientItemEntityType>;
export type AvisClientItemEntityType = typeof AvisClientItemEntity;

export const AvisClientItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.avisClient.id")
    ),
    note: e.field(DO_QUANTITE, f => f
        .label("restaurant.avisClient.note")
    ),
    dateAvis: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.avisClient.dateAvis")
    ),
    approuve: e.field(DO_BOOLEEN, f => f.defaultValue(false)
        .label("restaurant.avisClient.approuve")
    ),
    clientIdClient: e.field(DO_ID, f => f
        .label("restaurant.avisClient.clientIdClient")
    ),
    restaurantIdRestaurant: e.field(DO_ID, f => f
        .label("restaurant.avisClient.restaurantIdRestaurant")
    )
});
