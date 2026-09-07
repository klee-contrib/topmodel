////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

import {DepartementCode, StatutCommande} from "./enums";

export type ClientAvecCommandes = EntityToType<ClientAvecCommandesEntityType>;
export type ClientAvecCommandesEntityType = typeof ClientAvecCommandesEntity;

export const ClientAvecCommandesEntity = entity({
    id: e.field(DO_ID, f => f
        .label("restaurant.personneBase.id")
        .comment("comments.restaurant.personneBase.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.nom")
        .comment("comments.restaurant.personneBase.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.prenom")
        .comment("comments.restaurant.personneBase.prenom")
    ),
    departementCode: e.field(DO_CODE, f => f.type<DepartementCode>().optional()
        .label("restaurant.personne.departementCode")
        .comment("comments.restaurant.personne.departementCode")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
        .comment("comments.common.dateCreation.dateCreation")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
        .comment("comments.restaurant.client.email")
    ),
    swileCardId: e.field(DO_ID, f => f.optional()
        .label("restaurant.client.swileCardId")
        .comment("comments.restaurant.client.swileCardId")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClients")
        .comment("comments.restaurant.client.avisClients")
    ),
    commandeId: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.id")
        .comment("comments.restaurant.clientAvecCommandes.commandeId")
    ),
    commandeDateCommande: e.field(DO_LISTE, f => f.type<string[]>()
        .label("restaurant.commande.dateCommande")
        .comment("comments.restaurant.clientAvecCommandes.commandeDateCommande")
    ),
    commandeDateLivraison: e.field(DO_LISTE, f => f.type<string[]>().optional()
        .label("restaurant.commande.dateLivraison")
        .comment("comments.restaurant.clientAvecCommandes.commandeDateLivraison")
    ),
    commandeMontantTotal: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.montantTotal")
        .comment("comments.restaurant.clientAvecCommandes.commandeMontantTotal")
    ),
    commandeClientId: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.clientId")
        .comment("comments.restaurant.clientAvecCommandes.commandeClientId")
    ),
    commandeTableId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.tableId")
        .comment("comments.restaurant.clientAvecCommandes.commandeTableId")
    ),
    commandeReservationId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.reservationId")
        .comment("comments.restaurant.clientAvecCommandes.commandeReservationId")
    ),
    commandeStatutCommande: e.field(DO_LISTE, f => f.type<StatutCommande[]>()
        .label("restaurant.commande.statutCommande")
        .comment("comments.restaurant.clientAvecCommandes.commandeStatutCommande")
    ),
    commandeAvisClientId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.avisClientId")
        .comment("comments.restaurant.clientAvecCommandes.commandeAvisClientId")
    ),
    commandeLignes: e.field(DO_LISTE, f => f.type<number[][]>()
        .label("restaurant.commande.lignes")
        .comment("comments.restaurant.clientAvecCommandes.commandeLignes")
    ),
    commandeDateCreation: e.field(DO_LISTE, f => f.type<string[]>()
        .label("common.dateCreation.dateCreation")
        .comment("comments.restaurant.clientAvecCommandes.commandeDateCreation")
    )
});
