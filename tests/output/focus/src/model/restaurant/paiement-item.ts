////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_DATE_HEURE, DO_ID, DO_PRIX} from "../../domains";

export type PaiementItem = EntityToType<PaiementItemEntityType>;
export type PaiementItemEntityType = typeof PaiementItemEntity;

export const PaiementItemEntity = entity({
    factureId: e.field(DO_ID, f => f
        .label("restaurant.paiement.factureId")
    ),
    swileCardId: e.field(DO_ID, f => f
        .label("restaurant.paiement.swileCardId")
    ),
    dateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
    ),
    montantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
    )
});
