////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type MenuRead = EntityToType<MenuReadEntityType>;
export type MenuReadEntityType = typeof MenuReadEntity;

export const MenuReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.menu.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.menu.nom")
    ),
    description: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.menu.description")
    ),
    prix: e.field(DO_PRIX, f => f
        .label("restaurant.menu.prix")
    ),
    disponible: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("restaurant.menu.disponible")
    ),
    dateDebut: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.menu.dateDebut")
    ),
    dateFin: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.menu.dateFin")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.menu.restaurantId")
    )
});
