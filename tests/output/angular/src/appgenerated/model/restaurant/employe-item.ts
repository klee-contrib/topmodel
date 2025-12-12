////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID, DO_LIBELLE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type EmployeItem = EntityToType<EmployeItemEntityType>;
export type EmployeItemEntityType = typeof EmployeItemEntity;

export const EmployeItemEntity = entity({
    matricule: e.field(DO_CODE, f => f
        .label("restaurant.employe.matricule")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.employe.restaurantId")
    ),
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.employeItem.id")
    ),
    nom: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.employeItem.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.employeItem.prenom")
    )
});
