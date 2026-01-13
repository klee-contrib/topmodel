////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {LigneCommandeWriteEntity} from "./ligne-commande-write";
import {StatutCommandeCode} from "./references";

export type CommandeWrite = EntityToType<CommandeWriteEntityType>;
export type CommandeWriteEntityType = typeof CommandeWriteEntity;

export const CommandeWriteEntity = entity({
    dateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
    ),
    dateLivraison: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.commande.dateLivraison")
    ),
    montantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
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
    avisClientId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.avisClientId")
    ),
    lignes: e.list(LigneCommandeWriteEntity, f => f
        .label("restaurant.commande.lignes")
    )
});
