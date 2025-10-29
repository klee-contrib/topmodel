////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_CODE, DO_EMAIL, DO_ID, DO_LIBELLE} from "../../../domains";

import {TypeUtilisateurCode} from "./references";

export type UtilisateurItem = EntityToType<UtilisateurItemEntityType>;
export type UtilisateurItemEntityType = typeof UtilisateurItemEntity;

export const UtilisateurItemEntity = entity({
    id: e.field(DO_ID, f => f.optional()
        .label("securite.utilisateur.utilisateur.id")
    ),
    nom: e.field(DO_LIBELLE, f => f
        .label("securite.utilisateur.utilisateur.nom")
    ),
    prenom: e.field(DO_LIBELLE, f => f
        .label("securite.utilisateur.utilisateur.prenom")
    ),
    email: e.field(DO_EMAIL, f => f
        .label("securite.utilisateur.utilisateur.email")
    ),
    typeUtilisateurCode: e.field(DO_CODE, f => f.type<TypeUtilisateurCode>().defaultValue("GEST")
        .label("securite.utilisateur.utilisateur.typeUtilisateurCode")
    )
});
