----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références.
-- ===========================================================================================

/**		Initialisation de la table CATEGORIE_PLAT		**/
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE) VALUES('ENTREE', 'restaurant.categoriePlat.values.Entree', 2);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE) VALUES('PLAT', 'restaurant.categoriePlat.values.Plat', 3);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE) VALUES('DESSERT', 'restaurant.categoriePlat.values.Dessert', 4);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE) VALUES('BOISSON', 'restaurant.categoriePlat.values.Boisson', 1);

/**		Initialisation de la table DEPARTEMENT		**/
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE) VALUES('75', 'restaurant.departement.values.Paris');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE) VALUES('92', 'restaurant.departement.values.HautsDeSeine');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE) VALUES('93', 'restaurant.departement.values.SeineSaintDenis');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE) VALUES('94', 'restaurant.departement.values.SeineEtMarne');

/**		Initialisation de la table STATUT_COMMANDE		**/
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('EN_ATT', 'restaurant.statutCommande.values.EnAttente');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('EN_PREP', 'restaurant.statutCommande.values.EnPreparation');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('PRETE', 'restaurant.statutCommande.values.Prete');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('SERVIE', 'restaurant.statutCommande.values.Servie');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('ANNULE', 'restaurant.statutCommande.values.Annulee');
