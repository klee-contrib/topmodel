////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_LIBELLE, DO_LISTE, DO_TELEPHONE} from "../../domains";

import {StatutCommandeCode} from "./references";

export type ClientAvecCommandes = EntityToType<ClientAvecCommandesEntityType>;
export type ClientAvecCommandesEntityType = typeof ClientAvecCommandesEntity;

export const ClientAvecCommandesEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.client.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("restaurant.client.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.client.prenom")
    ),
    telephone: e.field(DO_TELEPHONE, f => f.optional()
        .label("restaurant.client.telephone")
    ),
    email: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    ),
    commandes: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.commandes")
    ),
    avisClientsClient: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.avisClientsClient")
    ),
    reservationsClient: e.field(DO_LISTE, f => f.type<number[]>()
        .label("restaurant.client.reservationsClient")
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
    commandeTableClientId: e.field(DO_LISTE, f => f.type<number[]>().optional()
        .label("restaurant.commande.tableClientId")
    ),
    commandeStatutCommandeCode: e.field(DO_LISTE, f => f.type<StatutCommandeCode[]>()
        .label("restaurant.commande.statutCommandeCode")
    ),
    commandeLigneCommandes: e.field(DO_LISTE, f => f.type<number[][]>()
        .label("restaurant.commande.ligneCommandes")
    )
});
