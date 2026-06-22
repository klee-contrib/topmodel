////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE, DO_SEQ_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type PromotionRead = EntityToType<PromotionReadEntityType>;
export type PromotionReadEntityType = typeof PromotionReadEntity;

export const PromotionReadEntity = entity({
    platId: e.field(DO_SEQ_ID, f => f
        .label("restaurant.promotion.platId")
    ),
    libelle: e.field(DO_LIBELLE, f => f
        .label("restaurant.promotion.libelle")
    ),
    pourcentageReduction: e.field(DO_QUANTITE, f => f
        .label("restaurant.promotion.pourcentageReduction")
    ),
    dateDebut: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.promotion.dateDebut")
    ),
    dateFin: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.promotion.dateFin")
    ),
    active: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.promotion.active")
    ),
    restaurantId: e.field(DO_ID, f => f.optional()
        .label("restaurant.promotion.restaurantId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    )
});
