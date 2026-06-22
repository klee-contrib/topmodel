////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_ID, DO_PRIX, DO_QUANTITE, DO_SEQ_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type LigneCommandeWrite = EntityToType<LigneCommandeWriteEntityType>;
export type LigneCommandeWriteEntityType = typeof LigneCommandeWriteEntity;

export const LigneCommandeWriteEntity = entity({
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
    platId: e.field(DO_SEQ_ID, f => f
        .label("restaurant.ligneCommande.platId")
    )
});
