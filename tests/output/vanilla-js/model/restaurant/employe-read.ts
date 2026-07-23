////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {DO_CODE, DO_DATE_HEURE, DO_ID, DO_LIBELLE, DO_PRIX, DO_TELEPHONE} from "../../domains";

import {DepartementCode} from "./enums";

export interface EmployeRead {
    id: number;
    nom: string;
    prenom: string;
    departementCode?: DepartementCode;
    dateCreation: string;
    telephone?: string;
    dateNaissance?: string;
    matricule: string;
    dateEmbauche: string;
    salaire?: number;
    restaurantId: number;
}

export const EmployeReadEntity = {
    id: {
        type: "field",
        name: "id",
        domain: DO_ID,
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
    prenom: {
        type: "field",
        name: "prenom",
        domain: DO_LIBELLE,
        isRequired: true,
        label: "Prénom"
    },
    departementCode: {
        type: "field",
        name: "departementCode",
        domain: DO_CODE,
        isRequired: false,
        label: "DepartementCode"
    },
    dateCreation: {
        type: "field",
        name: "dateCreation",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "Date de création"
    },
    telephone: {
        type: "field",
        name: "telephone",
        domain: DO_TELEPHONE,
        isRequired: false,
        label: "Telephone"
    },
    dateNaissance: {
        type: "field",
        name: "dateNaissance",
        domain: DO_DATE_HEURE,
        isRequired: false,
        label: "DateNaissance"
    },
    matricule: {
        type: "field",
        name: "matricule",
        domain: DO_CODE,
        isRequired: true,
        label: "Matricule"
    },
    dateEmbauche: {
        type: "field",
        name: "dateEmbauche",
        domain: DO_DATE_HEURE,
        isRequired: true,
        label: "DateEmbauche"
    },
    salaire: {
        type: "field",
        name: "salaire",
        domain: DO_PRIX,
        isRequired: false,
        label: "Salaire"
    },
    restaurantId: {
        type: "field",
        name: "restaurantId",
        domain: DO_ID,
        isRequired: true,
        label: "RestaurantId"
    }
} as const;
