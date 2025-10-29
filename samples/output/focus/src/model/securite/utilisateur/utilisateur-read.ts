////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {e, entity, EntityToType} from "@focus4/entities";
import {DO_BOOLEEN, DO_CODE, DO_DATE, DO_DATE_HEURE, DO_EMAIL, DO_ID, DO_LIBELLE} from "../../../domains";

import {TypeUtilisateurCode} from "./references";

export type UtilisateurRead = EntityToType<UtilisateurReadEntityType>;
export type UtilisateurReadEntityType = typeof UtilisateurReadEntity;

export const UtilisateurReadEntity = entity({
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
    dateNaissance: e.field(DO_DATE, f => f.optional()
        .label("securite.utilisateur.utilisateur.dateNaissance")
    ),
    adresse: e.field(DO_LIBELLE, f => f.optional()
        .label("securite.utilisateur.utilisateur.adresse")
    ),
    actif: e.field(DO_BOOLEEN, f => f.defaultValue(true)
        .label("securite.utilisateur.utilisateur.actif")
    ),
    profilId: e.field(DO_ID, f => f
        .label("securite.utilisateur.utilisateur.profilId")
    ),
    typeUtilisateurCode: e.field(DO_CODE, f => f.type<TypeUtilisateurCode>().defaultValue("GEST")
        .label("securite.utilisateur.utilisateur.typeUtilisateurCode")
    ),
    dateCreation: e.field(DO_DATE_HEURE, f => f
        .label("common.entityListeners.dateCreation")
    ),
    dateModification: e.field(DO_DATE_HEURE, f => f.optional()
        .label("common.entityListeners.dateModification")
    )
});
