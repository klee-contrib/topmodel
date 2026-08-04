////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE, DO_QUANTITE, DO_TELEPHONE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {TableReadEntity} from "./table-read";

export type RestaurantAvecStatistiques = EntityToType<RestaurantAvecStatistiquesEntityType>;
export type RestaurantAvecStatistiquesEntityType = typeof RestaurantAvecStatistiquesEntity;

export const RestaurantAvecStatistiquesEntity = entity({
    id: e.field(DO_ID, f => f
        .label("restaurant.lieu.id")
        .comment("comments.restaurant.lieu.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.lieu.nom")
        .comment("comments.restaurant.lieu.nom")
    ),
    adresse: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.lieu.adresse")
        .comment("comments.restaurant.lieu.adresse")
    ),
    telephone: e.field(DO_TELEPHONE, f => f.optional()
        .label("restaurant.restaurant.telephone")
        .comment("comments.restaurant.restaurant.telephone")
    ),
    menus: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.menus")
        .comment("comments.restaurant.restaurant.menus")
    ),
    plats: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.plats")
        .comment("comments.restaurant.restaurant.plats")
    ),
    promotions: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.restaurant.promotions")
        .comment("comments.restaurant.restaurant.promotions")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.avisClients")
        .comment("comments.restaurant.restaurant.avisClients")
    ),
    tableIds: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.restaurant.tableIds")
        .comment("comments.restaurant.restaurant.tableIds")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
        .comment("comments.common.dateCreation.dateCreation")
    ),
    tables: e.list(TableReadEntity, f => f
        .label("restaurant.restaurantAvecStatistiques.tables")
        .comment("comments.restaurant.restaurantAvecStatistiques.tables")
    ),
    nombrePlats: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.restaurantAvecStatistiques.nombrePlats")
        .comment("comments.restaurant.restaurantAvecStatistiques.nombrePlats")
    ),
    nombreTables: e.field(DO_QUANTITE, f => f.defaultValue(0)
        .label("restaurant.restaurantAvecStatistiques.nombreTables")
        .comment("comments.restaurant.restaurantAvecStatistiques.nombreTables")
    )
});
