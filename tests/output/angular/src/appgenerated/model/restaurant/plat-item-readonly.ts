////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_LIBELLE, DO_PRIX, DO_SEQ_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {CategoriePlatCode} from "./enums";

export type PlatItemReadonly = EntityToType<PlatItemReadonlyEntityType>;
export type PlatItemReadonlyEntityType = typeof PlatItemReadonlyEntity;

export const PlatItemReadonlyEntity = entity({
    id: e.field(DO_SEQ_ID, f => f
        .label("restaurant.plat.id")
        .comment("comments.restaurant.plat.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.plat.nom")
        .comment("comments.restaurant.plat.nom")
    ),
    categoriePlatCode: e.field(DO_CODE, f => f.type<CategoriePlatCode>()
        .label("restaurant.plat.categoriePlatCode")
        .comment("comments.restaurant.plat.categoriePlatCode")
    ),
    prix: e.field(DO_PRIX, f => f
        .label("restaurant.plat.prix")
        .comment("comments.restaurant.plat.prix")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.plat.disponible")
        .comment("comments.restaurant.plat.disponible")
    )
});
