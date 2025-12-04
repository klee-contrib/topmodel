////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID} from "../../domains";

export type CommandeResume = EntityToType<CommandeResumeEntityType>;
export type CommandeResumeEntityType = typeof CommandeResumeEntity;

export const CommandeResumeEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.id")
    )
});
