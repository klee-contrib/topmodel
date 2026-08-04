////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ReservationWrite = EntityToType<ReservationWriteEntityType>;
export type ReservationWriteEntityType = typeof ReservationWriteEntity;

export const ReservationWriteEntity = entity({
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
    )
});
