////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type PromotionWrite = EntityToType<PromotionWriteEntityType>;
export type PromotionWriteEntityType = typeof PromotionWriteEntity;

export const PromotionWriteEntity = entity({
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
