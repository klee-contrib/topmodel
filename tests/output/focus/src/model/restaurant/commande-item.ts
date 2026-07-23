////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_ID_2, DO_PRIX} from "../../domains";

import {StatutCommande} from "./enums";

export type CommandeItem = EntityToType<CommandeItemEntityType>;
export type CommandeItemEntityType = typeof CommandeItemEntity;

export const CommandeItemEntity = entity({
    id: e.field(DO_ID_2, f => f
        .label("restaurant.commande.id")
    ),
    dateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
    ),
    montantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
    ),
    statutCommande: e.field(DO_CODE, f => f.type<StatutCommande>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommande")
    ),
    clientId: e.field(DO_ID, f => f
        .label("restaurant.commande.clientId")
    )
});
