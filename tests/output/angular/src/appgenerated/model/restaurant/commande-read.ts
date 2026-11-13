////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_ID_2, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {ClientReadEntity} from "./client-read";
import {StatutCommande} from "./enums";
import {LigneCommandeReadEntity} from "./ligne-commande-read";
import {ReservationReadEntity} from "./reservation-read";

export type CommandeRead = EntityToType<CommandeReadEntityType>;
export type CommandeReadEntityType = typeof CommandeReadEntity;

export const CommandeReadEntity = entity({
    commandeId: e.field(DO_ID_2, f => f
        .label("restaurant.commande.id")
        .comment("comments.restaurant.commande.id")
    ),
    commandeDateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
        .comment("comments.restaurant.commande.dateCommande")
    ),
    commandeDateLivraison: e.field(DO_DATE_HEURE, f => f.optional()
        .label("restaurant.commande.dateLivraison")
        .comment("comments.restaurant.commande.dateLivraison")
    ),
    commandeMontantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
        .comment("comments.restaurant.commande.montantTotal")
    ),
    commandeTableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.tableId")
        .comment("comments.restaurant.commande.tableId")
    ),
    commandeStatutCommande: e.field(DO_CODE, f => f.type<StatutCommande>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommande")
        .comment("comments.restaurant.commande.statutCommande")
    ),
    commandeAvisClientId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.avisClientId")
        .comment("comments.restaurant.commande.avisClientId")
    ),
    commandeDateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
        .comment("comments.common.dateCreation.dateCreation")
    ),
    client: e.object(ClientReadEntity, f => f
        .label("restaurant.commande.clientId")
        .comment("comments.restaurant.commande.clientId")
    ),
    reservation: e.object(ReservationReadEntity, f => f.optional()
        .label("restaurant.commandeRead.reservation")
        .comment("comments.restaurant.commandeRead.reservation")
    ),
    lignes: e.list(LigneCommandeReadEntity, f => f
        .label("restaurant.commande.lignes")
        .comment("comments.restaurant.commande.lignes")
    )
});
