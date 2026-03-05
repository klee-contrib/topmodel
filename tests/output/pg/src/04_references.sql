----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références.
-- ===========================================================================================

/**		Initialisation de la table CATEGORIE_PLAT		**/
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('ENTREE', 'restaurant.categoriePlat.values.Entree', 2, null);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('PLAT', 'restaurant.categoriePlat.values.Plat', 3, 10);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('DESSERT', 'restaurant.categoriePlat.values.Dessert', 4, null);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('BOISSON', 'restaurant.categoriePlat.values.Boisson', 1, 2);

/**		Initialisation de la table REGION		**/
INSERT INTO REGION(REG_CODE, REG_LIBELLE, REG_NOM_RESPONSABLE) VALUES('IDF', 'restaurant.region.values.Idf', null);

/**		Initialisation de la table DEPARTEMENT		**/
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('75', 'restaurant.departement.values.Paris', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('92', 'restaurant.departement.values.HautsDeSeine', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('93', 'restaurant.departement.values.SeineSaintDenis', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('94', 'restaurant.departement.values.SeineEtMarne', 'IDF');

/**		Initialisation de la table STATUT_COMMANDE		**/
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('EN_ATT', 'restaurant.statutCommande.values.EnAttente');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('EN_PREP', 'restaurant.statutCommande.values.EnPreparation');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('PRETE', 'restaurant.statutCommande.values.Prete');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('SERVIE', 'restaurant.statutCommande.values.Servie');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('ANNULE', 'restaurant.statutCommande.values.Annulee');
