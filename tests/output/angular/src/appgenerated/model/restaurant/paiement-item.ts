////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID, DO_PRIX} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type PaiementItem = EntityToType<PaiementItemEntityType>;
export type PaiementItemEntityType = typeof PaiementItemEntity;

export const PaiementItemEntity = entity({
    factureId: e.field(DO_ID, f => f
        .label("restaurant.paiement.factureId")
        .comment("comments.restaurant.paiement.factureId")
    ),
    swileCardId: e.field(DO_ID, f => f
        .label("restaurant.paiement.swileCardId")
        .comment("comments.restaurant.paiement.swileCardId")
    ),
    dateCommande: e.field(DO_DATE_HEURE, f => f
        .label("restaurant.commande.dateCommande")
        .comment("comments.restaurant.commande.dateCommande")
    ),
    montantTotal: e.field(DO_PRIX, f => f
        .label("restaurant.commande.montantTotal")
        .comment("comments.restaurant.commande.montantTotal")
    )
});
