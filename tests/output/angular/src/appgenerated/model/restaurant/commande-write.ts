////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {ClientWriteEntity} from "./client-write";
import {StatutCommande} from "./enums";
import {LigneCommandeWriteEntity} from "./ligne-commande-write";
import {ReservationWriteEntity} from "./reservation-write";

export type CommandeWrite = EntityToType<CommandeWriteEntityType>;
export type CommandeWriteEntityType = typeof CommandeWriteEntity;

export const CommandeWriteEntity = entity({
    dateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
        .comment("comments.restaurant.commande.dateCommande")
    ),
    dateLivraison: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.commande.dateLivraison")
        .comment("comments.restaurant.commande.dateLivraison")
    ),
    montantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
        .comment("comments.restaurant.commande.montantTotal")
    ),
    tableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.tableId")
        .comment("comments.restaurant.commande.tableId")
    ),
    statutCommande: e.field(DO_CODE, f => f.type<StatutCommande>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommande")
        .comment("comments.restaurant.commande.statutCommande")
    ),
    avisClientId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.avisClientId")
        .comment("comments.restaurant.commande.avisClientId")
    ),
    client: e.object(ClientWriteEntity, f => f
        .label("restaurant.commande.clientId")
        .comment("comments.restaurant.commande.clientId")
    ),
    reservation: e.object(ReservationWriteEntity, f => f.optional()
        .label("restaurant.commandeRead.reservation")
        .comment("comments.restaurant.commandeRead.reservation")
    ),
    lignes: e.list(LigneCommandeWriteEntity, f => f
        .label("restaurant.commande.lignes")
        .comment("comments.restaurant.commande.lignes")
    )
});
