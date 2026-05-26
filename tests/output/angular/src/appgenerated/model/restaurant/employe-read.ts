////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_PRIX, DO_TELEPHONE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {DepartementCode} from "./enums";

export type EmployeRead = EntityToType<EmployeReadEntityType>;
export type EmployeReadEntityType = typeof EmployeReadEntity;

export const EmployeReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.personne.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.prenom")
    ),
    departementCode: e.field(DO_CODE, f => f.type<DepartementCode>().optional()
        .label("restaurant.personne.departementCode")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    ),
    telephone: e.field(DO_TELEPHONE, f => f.optional()
        .label("restaurant.employe.telephone")
    ),
    dateNaissance: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.employe.dateNaissance")
    ),
    matricule: e.field(DO_CODE, f => f
        .label("restaurant.employe.matricule")
    ),
    dateEmbauche: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.employe.dateEmbauche")
    ),
    salaire: e.field(DO_PRIX, f => f.optional()
        .label("restaurant.employe.salaire")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.employe.restaurantId")
    )
});
