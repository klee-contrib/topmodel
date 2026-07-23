////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID} from "../../domains";

import {PersonneItemEntity, PersonneItem} from "./personne-item";

export interface EmployeItem extends PersonneItem {
    matricule: string;
    restaurantId: number;
    autresEmployes: EmployeItem[];
}

export const EmployeItemEntity = {
    ...PersonneItemEntity,
    matricule: {
        type: "field",
        name: "matricule",
        domain: DO_CODE,
        isRequired: true,
        label: "Matricule"
    },
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: true,
        label: "RestaurantId"
    },
    autresEmployes: {
        type: "recursive-list"
    }
} as const;
