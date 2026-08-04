////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE, DO_SEQ_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ReservationRead = EntityToType<ReservationReadEntityType>;
export type ReservationReadEntityType = typeof ReservationReadEntity;

export const ReservationReadEntity = entity({
    id: e.field(DO_SEQ_ID, f => f
        .label("restaurant.reservation.id")
        .comment("comments.restaurant.reservation.id")
    ),
    dateReservation: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.reservation.dateReservation")
        .comment("comments.restaurant.reservation.dateReservation")
    ),
    nombrePersonnes: e.field(DO_QUANTITE, f => f
        .label("restaurant.reservation.nombrePersonnes")
        .comment("comments.restaurant.reservation.nombrePersonnes")
    ),
    commentaire: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.reservation.commentaire")
        .comment("comments.restaurant.reservation.commentaire")
    ),
    confirmee: e.field(DO_BOOLEEN, f => f.defaultValue(false)
        .label("restaurant.reservation.confirmee")
        .comment("comments.restaurant.reservation.confirmee")
    ),
    clientId: e.field(DO_ID, f => f
        .label("restaurant.reservation.clientId")
        .comment("comments.restaurant.reservation.clientId")
    ),
    tableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.reservation.tableId")
        .comment("comments.restaurant.reservation.tableId")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.reservation.restaurantId")
        .comment("comments.restaurant.reservation.restaurantId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
        .comment("comments.common.dateCreation.dateCreation")
    ),
    clientNom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.nom")
        .comment("comments.restaurant.reservationRead.clientNom")
    ),
    clientPrenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.prenom")
        .comment("comments.restaurant.reservationRead.clientPrenom")
    ),
    clientEmail: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
        .comment("comments.restaurant.reservationRead.clientEmail")
    ),
    tableNumero: e.field(DO_CODE, f => f
        .label("restaurant.table.numero")
        .comment("comments.restaurant.reservationRead.tableNumero")
    ),
    tableCapacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.table.capacite")
        .comment("comments.restaurant.reservationRead.tableCapacite")
    )
});
