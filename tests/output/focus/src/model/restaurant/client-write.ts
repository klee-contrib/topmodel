////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_ID, DO_LIBELLE, DO_LISTE} from "../../domains";

import {DepartementCode} from "./enums";

export type ClientWrite = EntityToType<ClientWriteEntityType>;
export type ClientWriteEntityType = typeof ClientWriteEntity;

export const ClientWriteEntity = entity({
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.prenom")
    ),
    departementCode: e.field(DO_CODE, f => f.type<DepartementCode>().defaultValue("75").optional()
        .label("restaurant.personne.departementCode")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    ),
    swileCardId: e.field(DO_ID, f => f.optional()
        .label("restaurant.client.swileCardId")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClients")
    )
});
