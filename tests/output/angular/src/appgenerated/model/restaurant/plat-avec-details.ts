////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_ID, DO_LIBELLE, DO_LISTE, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {CategoriePlatCode} from "./references";

export type PlatAvecDetails = EntityToType<PlatAvecDetailsEntityType>;
export type PlatAvecDetailsEntityType = typeof PlatAvecDetailsEntity;

export const PlatAvecDetailsEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.plat.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.plat.nom")
    ),
    description: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.plat.description")
    ),
    prix: e.field(DO_PRIX, f => f
        .label("restaurant.plat.prix")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.plat.disponible")
    ),
    categoriePlatCode: e.field(DO_CODE, f => f.type<CategoriePlatCode>()
        .label("restaurant.plat.categoriePlatCode")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.plat.restaurantId")
    ),
    ligneCommandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.plat.ligneCommandes")
    )
});
