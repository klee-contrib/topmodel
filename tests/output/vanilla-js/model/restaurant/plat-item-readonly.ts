////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_BOOLEEN, DO_CODE, DO_LIBELLE, DO_PRIX, DO_SEQ_ID} from "../../domains";

import {CategoriePlatCode} from "./enums";

export interface PlatItemReadonly {
    id: number;
    nom: string;
    categoriePlatCode: CategoriePlatCode;
    prix: number;
    disponible: boolean;
}

export const PlatItemReadonlyEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_SEQ_ID,
        isRequired: true,
        label: "Id"
    },
    nom: {
        type: "field",
        name: "nom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Nom"
    },
    categoriePlatCode: {
        type: "field",
        name: "categoriePlatCode",
        domain: DO_CODE,
        isRequired: true,
        label: "CategoriePlatCode"
    },
    prix: {
        type: "field",
        name: "prix",
        domain: DO_PRIX,
        isRequired: true,
        label: "Prix"
    },
    disponible: {
        type: "field",
        name: "disponible",
        domain: DO_BOOLEEN,
        defaultValue: true,
        isRequired: true,
        label: "Disponible"
    }
} as const;
