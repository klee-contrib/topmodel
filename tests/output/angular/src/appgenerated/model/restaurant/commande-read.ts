////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {ClientReadEntity} from "./client-read";
import {StatutCommande} from "./enums";
import {LigneCommandeReadEntity} from "./ligne-commande-read";
import {ReservationReadEntity} from "./reservation-read";

export type CommandeRead = EntityToType<CommandeReadEntityType>;
export type CommandeReadEntityType = typeof CommandeReadEntity;

export const CommandeReadEntity = entity({
    id: e.field(DO_ID, f => f.optional()
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
    tableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.tableId")
    ),
    statutCommande: e.field(DO_CODE, f => f.type<StatutCommande>().defaultValue("EN_ATT")
        .label("restaurant.commande.statutCommande")
    ),
    avisClientId: e.field(DO_ID, f => f.optional()
        .label("restaurant.commande.avisClientId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    ),
    client: e.object(ClientReadEntity, f => f
        .label("restaurant.commande.clientId")
    ),
    reservation: e.object(ReservationReadEntity, f => f.optional()
        .label("restaurant.commandeRead.reservation")
    ),
    lignes: e.list(LigneCommandeReadEntity, f => f
        .label("restaurant.commande.lignes")
    )
});
