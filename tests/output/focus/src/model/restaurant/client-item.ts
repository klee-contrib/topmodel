////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_LIBELLE} from "../../domains";

import {PersonneItemEntity} from "./personne-item";

export type ClientItem = EntityToType<ClientItemEntityType>;
export type ClientItemEntityType = typeof ClientItemEntity;

export const ClientItemEntity = entity({
    ...PersonneItemEntity,
    nomComplet: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.clientItem.nomComplet")
    )
});
