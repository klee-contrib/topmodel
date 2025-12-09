////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_QUANTITE} from "../../domains";

export type PromotionRead = EntityToType<PromotionReadEntityType>;
export type PromotionReadEntityType = typeof PromotionReadEntity;

export const PromotionReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.promotion.id")
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
    restaurantIdRestaurant: e.field(DO_ID, f => f.optional()
        .label("restaurant.promotion.restaurantIdRestaurant")
    ),
    promotionPlatsPromotion: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.promotion.promotionPlatsPromotion")
    )
});
