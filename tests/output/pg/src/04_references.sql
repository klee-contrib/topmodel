----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références.
-- ===========================================================================================

/**		Initialisation de la table CATEGORIE_PLAT		**/
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE) VALUES('ENTREE', 'restaurant.categoriePlat.values.Entree');
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE) VALUES('PLAT', 'restaurant.categoriePlat.values.Plat');
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE) VALUES('DESSERT', 'restaurant.categoriePlat.values.Dessert');
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE) VALUES('BOISSON', 'restaurant.categoriePlat.values.Boisson');

/**		Initialisation de la table STATUT_COMMANDE		**/
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('EN_ATT', 'restaurant.statutCommande.values.EnAttente');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('EN_PREP', 'restaurant.statutCommande.values.EnPreparation');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('PRETE', 'restaurant.statutCommande.values.Prete');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('SERVIE', 'restaurant.statutCommande.values.Servie');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES('ANNULE', 'restaurant.statutCommande.values.Annulee');
