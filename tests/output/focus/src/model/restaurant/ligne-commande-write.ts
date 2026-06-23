////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_ID_2, DO_PRIX, DO_QUANTITE, DO_SEQ_ID} from "../../domains";

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
    commandeId: e.field(DO_ID_2, f => f
        .label("restaurant.ligneCommande.commandeId")
    ),
    platId: e.field(DO_SEQ_ID, f => f
        .label("restaurant.ligneCommande.platId")
    )
});
