----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_resources.sql
--   Description		:	Scripts d'insertion des ressources (libellés traduits).
-- ===========================================================================================

/**		Initialisation des traductions des valeurs de la table STATUT_COMMANDE		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnAttente', 'En attente');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnPreparation', 'En préparation');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.statutCommande.values.Prete', 'Prête');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.statutCommande.values.Servie', 'Servie');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.statutCommande.values.Annulee', 'Annulée');

/**		Initialisation des traductions des valeurs de la table CATEGORIE_PLAT		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Entree', 'Entrée');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Plat', 'Plat principal');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Dessert', 'Dessert');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Boisson', 'Boisson');

/**		Initialisation des traductions des valeurs de la table DEPARTEMENT		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.departement.values.Paris', 'Paris');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.departement.values.HautsDeSeine', 'Hauts de Seine');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.departement.values.SeineSaintDenis', 'Seine Saint Denis');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.departement.values.SeineEtMarne', 'Seine et Marne');

/**		Initialisation des traductions des valeurs de la table REGION		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_VALUE) VALUES('restaurant.region.values.Idf', 'Île de France');
