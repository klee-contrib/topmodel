////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID, DO_PRIX, DO_QUANTITE} from "../../domains";

export type LigneCommandeItem = EntityToType<LigneCommandeItemEntityType>;
export type LigneCommandeItemEntityType = typeof LigneCommandeItemEntity;

export const LigneCommandeItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("restaurant.ligneCommande.id")
    ),
    quantite: e.field(DO_QUANTITE, f => f
        .label("restaurant.ligneCommande.quantite")
    ),
    prixUnitaire: e.field(DO_PRIX, f => f
        .label("restaurant.ligneCommande.prixUnitaire")
    ),
    prixTotal: e.field(DO_PRIX, f => f
        .label("restaurant.ligneCommande.prixTotal")
    ),
    commandeId: e.field(DO_ID, f => f
        .label("restaurant.ligneCommande.commandeId")
    ),
    platId: e.field(DO_ID, f => f
        .label("restaurant.ligneCommande.platId")
    )
});
