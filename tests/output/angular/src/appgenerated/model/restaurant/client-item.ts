////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_LIBELLE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ClientItem = EntityToType<ClientItemEntityType>;
export type ClientItemEntityType = typeof ClientItemEntity;

export const ClientItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.personne.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.prenom")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    )
});
