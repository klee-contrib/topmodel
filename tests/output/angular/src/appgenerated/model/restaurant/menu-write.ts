////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {CategoriePlatCode} from "./enums";

export type MenuWrite = EntityToType<MenuWriteEntityType>;
export type MenuWriteEntityType = typeof MenuWriteEntity;

export const MenuWriteEntity = entity({
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.menu.nom")
        .comment("comments.restaurant.menu.nom")
    ),
    description: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.menu.description")
        .comment("comments.restaurant.menu.description")
    ),
    prix: e.field(DO_PRIX, f => f
        .label("restaurant.menu.prix")
        .comment("comments.restaurant.menu.prix")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.menu.disponible")
        .comment("comments.restaurant.menu.disponible")
    ),
    dateDebut: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.menu.dateDebut")
        .comment("comments.restaurant.menu.dateDebut")
    ),
    dateFin: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.menu.dateFin")
        .comment("comments.restaurant.menu.dateFin")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.menu.restaurantId")
        .comment("comments.restaurant.menu.restaurantId")
    ),
    categoriesPlat: e.field(DO_LISTE, f => f.type<CategoriePlatCode[]>()
        .label("restaurant.categoriePlat.code")
        .comment("comments.restaurant.menuWrite.categoriesPlat")
    )
});
