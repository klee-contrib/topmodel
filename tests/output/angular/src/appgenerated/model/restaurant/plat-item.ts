////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_LIBELLE, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {CategoriePlatCode} from "./references";

export type PlatItem = EntityToType<PlatItemEntityType>;
export type PlatItemEntityType = typeof PlatItemEntity;

export const PlatItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.plat.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.plat.nom")
    ),
    prix: e.field(DO_PRIX, f => f
        .label("restaurant.plat.prix")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.plat.disponible")
    ),
    categoriePlatCode: e.field(DO_CODE, f => f.type<CategoriePlatCode>()
        .label("restaurant.plat.categoriePlatCode")
    )
});
