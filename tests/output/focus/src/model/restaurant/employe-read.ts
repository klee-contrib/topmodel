////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "../../domains";

export type EmployeRead = EntityToType<EmployeReadEntityType>;
export type EmployeReadEntityType = typeof EmployeReadEntity;

export const EmployeReadEntity = entity({
    matricule: e.field(DO_CODE, f => f
        .label("restaurant.employe.matricule")
    ),
    dateEmbauche: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.employe.dateEmbauche")
    ),
    salaire: e.field(DO_PRIX, f => f.optional()
        .label("restaurant.employe.salaire")
    ),
    restaurantIdRestaurant: e.field(DO_ID, f => f
        .label("restaurant.employe.restaurantIdRestaurant")
    )
});
