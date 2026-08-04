////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_PRIX, DO_SEQ_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {CategoriePlat} from "./enums";
import {PlatItemEntity} from "./plat-item";

export type MenuRead = EntityToType<MenuReadEntityType>;
export type MenuReadEntityType = typeof MenuReadEntity;

export const MenuReadEntity = entity({
    id: e.field(DO_SEQ_ID, f => f
        .label("restaurant.menu.id")
        .comment("comments.restaurant.menu.id")
    ),
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
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
        .comment("comments.common.dateCreation.dateCreation")
    ),
    categoriesPlat: e.field(DO_LISTE, f => f.type<CategoriePlat[]>()
        .label("restaurant.menuRead.categoriesPlat")
        .comment("comments.restaurant.menuRead.categoriesPlat")
    ),
    plats: e.list(PlatItemEntity, f => f
        .label("restaurant.menuRead.plats")
        .comment("comments.restaurant.menuRead.plats")
    )
});
