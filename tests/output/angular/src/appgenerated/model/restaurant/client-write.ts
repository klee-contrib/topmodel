////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_ID, DO_LIBELLE, DO_LISTE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {DepartementCode} from "./enums";

export type ClientWrite = EntityToType<ClientWriteEntityType>;
export type ClientWriteEntityType = typeof ClientWriteEntity;

export const ClientWriteEntity = entity({
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.nom")
        .comment("comments.restaurant.personneBase.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.prenom")
        .comment("comments.restaurant.personneBase.prenom")
    ),
    departementCode: e.field(DO_CODE, f => f.type<DepartementCode>().defaultValue("75").optional()
        .label("restaurant.personne.departementCode")
        .comment("comments.restaurant.personne.departementCode")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
        .comment("comments.restaurant.client.email")
    ),
    swileCardId: e.field(DO_ID, f => f.optional()
        .label("restaurant.client.swileCardId")
        .comment("comments.restaurant.client.swileCardId")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClients")
        .comment("comments.restaurant.client.avisClients")
    )
});
