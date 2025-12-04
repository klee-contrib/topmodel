////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_LIBELLE} from "../../domains";

export type ClientMinimal = EntityToType<ClientMinimalEntityType>;
export type ClientMinimalEntityType = typeof ClientMinimalEntity;

export const ClientMinimalEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.clientMinimal.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.clientMinimal.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.clientMinimal.prenom")
    ),
    nomComplet: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.clientMinimal.nomComplet")
    )
});
