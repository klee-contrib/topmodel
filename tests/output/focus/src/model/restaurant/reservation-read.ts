////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_QUANTITE} from "../../domains";

export type ReservationRead = EntityToType<ReservationReadEntityType>;
export type ReservationReadEntityType = typeof ReservationReadEntity;

export const ReservationReadEntity = entity({
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
    clientId: e.field(DO_ID, f => f
        .label("restaurant.reservation.clientId")
    ),
    tableId: e.field(DO_ID, f => f.optional()
        .label("restaurant.reservation.tableId")
    ),
    restaurantId: e.field(DO_ID, f => f
        .label("restaurant.reservation.restaurantId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    ),
    clientNom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.nom")
    ),
    clientPrenom: e.field(DO_LIBELLE, f => f
        .label("restaurant.personneBase.prenom")
    ),
    clientEmail: e.field(DO_LIBELLE, f => f.optional()
        .label("restaurant.client.email")
    ),
    tableNumero: e.field(DO_CODE, f => f
        .label("restaurant.tableRestaurant.numero")
    ),
    tableCapacite: e.field(DO_QUANTITE, f => f
        .label("restaurant.tableRestaurant.capacite")
    )
});
