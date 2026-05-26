////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

export type AvisClientRead = EntityToType<AvisClientReadEntityType>;
export type AvisClientReadEntityType = typeof AvisClientReadEntity;

export const AvisClientReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.avisClient.id")
    ),
    note: e.field(DO_QUANTITE, f => f
        .label("restaurant.avisClient.note")
    ),
    commentaire: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.avisClient.commentaire")
    ),
    dateAvis: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.avisClient.dateAvis")
    ),
    approuve: e.field(DO_BOOLEEN, f => f.defaultValue(false)
        .label("restaurant.avisClient.approuve")
    ),
    nombreVues: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.avisClient.nombreVues")
    ),
    clientId: e.field(DO_ID, f => f
        .label("restaurant.avisClient.clientId")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.avisClient.restaurantId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    )
});
