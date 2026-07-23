////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID_2} from "../../domains";

export type CommandeResume = EntityToType<CommandeResumeEntityType>;
export type CommandeResumeEntityType = typeof CommandeResumeEntity;

export const CommandeResumeEntity = entity({
    id: e.field(DO_ID_2, f => f
        .label("restaurant.commande.id")
    )
});
