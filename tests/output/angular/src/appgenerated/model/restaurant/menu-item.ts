////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_ID, DO_LIBELLE, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type MenuItem = EntityToType<MenuItemEntityType>;
export type MenuItemEntityType = typeof MenuItemEntity;

export const MenuItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.menu.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.menu.nom")
    ),
    prix: e.field(DO_PRIX, f => f
        .label("restaurant.menu.prix")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.menu.disponible")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.menu.restaurantId")
    )
});
