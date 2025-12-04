////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {StatutCommandeCode} from "./references";

export type CommandeItem = EntityToType<CommandeItemEntityType>;
export type CommandeItemEntityType = typeof CommandeItemEntity;

export const CommandeItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.id")
    ),
    dateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
    ),
    montantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
    ),
    statutCommandeCode: e.field(DO_CODE, f => f.type<StatutCommandeCode>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommandeCode")
    ),
    clientId: e.field(DO_ID, f => f
        .label("restaurant.commande.clientId")
    )
});
