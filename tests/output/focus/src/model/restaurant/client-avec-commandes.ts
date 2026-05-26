////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_LISTE} from "../../domains";

import {DepartementCode, StatutCommande} from "./enums";

export type ClientAvecCommandes = EntityToType<ClientAvecCommandesEntityType>;
export type ClientAvecCommandesEntityType = typeof ClientAvecCommandesEntity;

export const ClientAvecCommandesEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.personne.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personne.prenom")
    ),
    departementCode: e.field(DO_CODE, f => f.type<DepartementCode>().optional()
        .label("restaurant.personne.departementCode")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    ),
    avisClients: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClients")
    ),
    commandeId: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.id")
    ),
    commandeDateCommande: e.field(DO_LISTE, f => f.type<string[]>()
        .label("restaurant.commande.dateCommande")
    ),
    commandeDateLivraison: e.field(DO_LISTE, f => f.type<string[]>().optional()
        .label("restaurant.commande.dateLivraison")
    ),
    commandeMontantTotal: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.montantTotal")
    ),
    commandeClientId: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.commande.clientId")
    ),
    commandeTableId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.tableId")
    ),
    commandeReservationId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.reservationId")
    ),
    commandeStatutCommande: e.field(DO_LISTE, f => f.type<StatutCommande[]>()
        .label("restaurant.commande.statutCommande")
    ),
    commandeAvisClientId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.avisClientId")
    ),
    commandeLignes: e.field(DO_LISTE, f => f.type<number[][]>()
        .label("restaurant.commande.lignes")
    ),
    commandeDateCreation: e.field(DO_LISTE, f => f.type<string[]>()
        .label("common.dateCreation.dateCreation")
    )
});
