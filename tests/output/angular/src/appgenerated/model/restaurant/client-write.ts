////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ClientWrite = EntityToType<ClientWriteEntityType>;
export type ClientWriteEntityType = typeof ClientWriteEntity;

export const ClientWriteEntity = entity({
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.client.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.client.prenom")
    ),
    telephone: e.field(DO_TELEPHONE, f => f.optional()
        .label("restaurant.client.telephone")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    ),
    commandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.commandes")
    ),
    avisClientsClient: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClientsClient")
    ),
    reservationsClient: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.reservationsClient")
    )
});
