////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LISTE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {StatutCommandeCode} from "./references";

export type CommandeWrite = EntityToType<CommandeWriteEntityType>;
export type CommandeWriteEntityType = typeof CommandeWriteEntity;

export const CommandeWriteEntity = entity({
    dateLivraison: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.commande.dateLivraison")
    ),
    clientId: e.field(DO_ID, f => f
        .label("restaurant.commande.clientId")
    ),
    tableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.tableId")
    ),
    reservationId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.reservationId")
    ),
    statutCommandeCode: e.field(DO_CODE, f => f.type<StatutCommandeCode>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommandeCode")
    ),
    ligneCommandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.ligneCommandes")
    )
});
