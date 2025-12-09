////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type EmployeWrite = EntityToType<EmployeWriteEntityType>;
export type EmployeWriteEntityType = typeof EmployeWriteEntity;

export const EmployeWriteEntity = entity({
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
