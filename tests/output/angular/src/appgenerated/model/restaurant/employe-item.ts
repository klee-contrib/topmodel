////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {PersonneItemEntity} from "./personne-item";

export type EmployeItem = EntityToType<EmployeItemEntityType>;
export type EmployeItemEntityType = typeof EmployeItemEntity;

export const EmployeItemEntity = entity({
    ...PersonneItemEntity,
    matricule: e.field(DO_CODE, f => f
        .label("restaurant.employe.matricule")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.employe.restaurantId")
    ),
    autresEmployes: e.recursiveList(f => f
        .label("restaurant.employeItem.autresEmployes")
    )
});
