////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_LIBELLE, DO_LISTE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ClientWrite = EntityToType<ClientWriteEntityType>;
export type ClientWriteEntityType = typeof ClientWriteEntity;

export const ClientWriteEntity = entity({
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.prenom")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    ),
    commandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.commandes")
    ),
    reservations: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.reservations")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClients")
    )
});
