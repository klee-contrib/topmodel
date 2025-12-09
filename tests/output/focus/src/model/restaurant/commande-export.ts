////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LISTE, DO_PRIX} from "../../domains";

import {StatutCommandeCode} from "./references";

export type CommandeExport = EntityToType<CommandeExportEntityType>;
export type CommandeExportEntityType = typeof CommandeExportEntity;

export const CommandeExportEntity = entity({
    id: e.field(DO_ID, f => f
        .label("restaurant.commande.id")
    ),
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
    tableClientId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.tableClientId")
    ),
    statutCommandeCode: e.field(DO_CODE, f => f.type<StatutCommandeCode>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommandeCode")
    ),
    ligneCommandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.ligneCommandes")
    )
});
