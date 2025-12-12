////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_DATE_HEURE, DO_ID, DO_QUANTITE} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type ReservationItem = EntityToType<ReservationItemEntityType>;
export type ReservationItemEntityType = typeof ReservationItemEntity;

export const ReservationItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.reservation.id")
    ),
    dateReservation: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.reservation.dateReservation")
    ),
    nombrePersonnes: e.field(DO_QUANTITE, f => f
        .label("restaurant.reservation.nombrePersonnes")
    ),
    confirmee: e.field(DO_BOOLEEN, f => f.defaultValue(false)
        .label("restaurant.reservation.confirmee")
    ),
    clientId: e.field(DO_ID, f => f
        .label("restaurant.reservation.clientId")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.reservation.restaurantId")
    )
});
