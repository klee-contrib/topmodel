////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_CODE, DO_ID, DO_LIBELLE, DO_LISTE, DO_PRIX} from "../../domains";

import {CategoriePlatCode} from "./references";

export type PlatWrite = EntityToType<PlatWriteEntityType>;
export type PlatWriteEntityType = typeof PlatWriteEntity;

export const PlatWriteEntity = entity({
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
    categoriePlatCodeCategoriePlat: e.field(DO_CODE, f => f.type<CategoriePlatCode>()
        .label("restaurant.plat.categoriePlatCodeCategoriePlat")
    ),
    restaurantIdRestaurant: e.field(DO_ID, f => f
        .label("restaurant.plat.restaurantIdRestaurant")
    ),
    ligneCommandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.plat.ligneCommandes")
    ),
    menuPlatsPlat: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.plat.menuPlatsPlat")
    ),
    promotionPlatsPlat: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.plat.promotionPlatsPlat")
    )
});
