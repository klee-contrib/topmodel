////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_ID, DO_LIBELLE, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type AvisClientWrite = EntityToType<AvisClientWriteEntityType>;
export type AvisClientWriteEntityType = typeof AvisClientWriteEntity;

export const AvisClientWriteEntity = entity({
    note: e.field(DO_QUANTITE, f => f
        .label("restaurant.avisClient.note")
    ),
    commentaire: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.avisClient.commentaire")
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
