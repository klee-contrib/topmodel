////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

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
    clientId: e.field(DO_ID, f => f
        .label("restaurant.avisClient.clientId")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.avisClient.restaurantId")
    )
});
