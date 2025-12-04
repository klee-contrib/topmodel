////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

export type ReservationWrite = EntityToType<ReservationWriteEntityType>;
export type ReservationWriteEntityType = typeof ReservationWriteEntity;

export const ReservationWriteEntity = entity({
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
    )
});
