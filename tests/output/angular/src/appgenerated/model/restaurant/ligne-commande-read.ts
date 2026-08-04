////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_DATE_HEURE, DO_ID_2, DO_PRIX, DO_QUANTITE, DO_SEQ_ID} from "@/domains";
import {e, entity, EntityToType} from "@focus4/entities";

export type LigneCommandeRead = EntityToType<LigneCommandeReadEntityType>;
export type LigneCommandeReadEntityType = typeof LigneCommandeReadEntity;

export const LigneCommandeReadEntity = entity({
    id: e.field(DO_ID_2, f => f
        .label("restaurant.ligneCommande.id")
        .comment("comments.restaurant.ligneCommande.id")
    ),
    quantite: e.field(DO_QUANTITE, f => f
        .label("restaurant.ligneCommande.quantite")
        .comment("comments.restaurant.ligneCommande.quantite")
    ),
    prixUnitaire: e.field(DO_PRIX, f => f
        .label("restaurant.ligneCommande.prixUnitaire")
        .comment("comments.restaurant.ligneCommande.prixUnitaire")
    ),
    prixTotal: e.field(DO_PRIX, f => f
        .label("restaurant.ligneCommande.prixTotal")
        .comment("comments.restaurant.ligneCommande.prixTotal")
    ),
    commandeId: e.field(DO_ID_2, f => f
        .label("restaurant.ligneCommande.commandeId")
        .comment("comments.restaurant.ligneCommande.commandeId")
    ),
    platId: e.field(DO_SEQ_ID, f => f
        .label("restaurant.ligneCommande.platId")
        .comment("comments.restaurant.ligneCommande.platId")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.dateCreation.dateCreation")
        .comment("comments.common.dateCreation.dateCreation")
    )
});
