////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE, DO_TELEPHONE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ReservationAvecDetails = EntityToType<ReservationAvecDetailsEntityType>;
export type ReservationAvecDetailsEntityType = typeof ReservationAvecDetailsEntity;

export const ReservationAvecDetailsEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.reservation.id")
    ),
    dateReservation: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.reservation.dateReservation")
    ),
    nombrePersonnes: e.field(DO_QUANTITE, f => f
        .label("restaurant.reservation.nombrePersonnes")
    ),
    commentaire: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.reservation.commentaire")
    ),
    confirmee: e.field(DO_BOOLEEN, f => f.defaultValue(false)
        .label("restaurant.reservation.confirmee")
    ),
    clientIdClient: e.field(DO_ID, f => f
        .label("restaurant.reservation.clientIdClient")
    ),
    tableClientIdTable: e.field(DO_ID, f => f.optional()
        .label("restaurant.reservation.tableClientIdTable")
    ),
    restaurantIdRestaurant: e.field(DO_ID, f => f
        .label("restaurant.reservation.restaurantIdRestaurant")
    ),
    clientNom: e.field(DO_LIBELLE, f => f
        .label("restaurant.client.nom")
    ),
    clientPrenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.client.prenom")
    ),
    clientTelephone: e.field(DO_TELEPHONE, f => f.optional()
        .label("restaurant.client.telephone")
    ),
    tableClientNumero: e.field(DO_CODE, f => f
        .label("restaurant.tableClient.numero")
    ),
    tableClientCapacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.tableClient.capacite")
    )
});
