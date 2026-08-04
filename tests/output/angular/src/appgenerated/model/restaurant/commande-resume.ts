////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID_2} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type CommandeResume = EntityToType<CommandeResumeEntityType>;
export type CommandeResumeEntityType = typeof CommandeResumeEntity;

export const CommandeResumeEntity = entity({
    id: e.field(DO_ID_2, f => f
        .label("restaurant.commande.id")
        .comment("comments.restaurant.commande.id")
    )
});
