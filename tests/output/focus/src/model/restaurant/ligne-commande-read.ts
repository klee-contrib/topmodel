////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_DATE_HEURE, DO_ID, DO_PRIX, DO_QUANTITE, DO_SEQ_ID} from "../../domains";

export type LigneCommandeRead = EntityToType<LigneCommandeReadEntityType>;
export type LigneCommandeReadEntityType = typeof LigneCommandeReadEntity;

export const LigneCommandeReadEntity = entity({
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
    platId: e.field(DO_SEQ_ID, f => f
        .label("restaurant.ligneCommande.platId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
    )
});
