////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "../../domains";

import {ClientWriteEntity} from "./client-write";
import {StatutCommande} from "./enums";
import {LigneCommandeWriteEntity} from "./ligne-commande-write";
import {ReservationWriteEntity} from "./reservation-write";

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
    tableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.tableId")
    ),
    statutCommande: e.field(DO_CODE, f => f.type<StatutCommande>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommande")
    ),
    avisClientId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.avisClientId")
    ),
    client: e.object(ClientWriteEntity, f => f
        .label("restaurant.commande.clientId")
    ),
    reservation: e.object(ReservationWriteEntity, f => f.optional()
        .label("restaurant.commandeRead.reservation")
    ),
    lignes: e.list(LigneCommandeWriteEntity, f => f
        .label("restaurant.commande.lignes")
    )
});
