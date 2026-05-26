----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_resources.sql
--   Description		:	Scripts d'insertion des ressources (libellés traduits).
-- ===========================================================================================

/**		Initialisation des traductions des propriétés de la table CLIENT		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.client.email', 'fr', 'Courriel');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.client.email', 'de', 'Courriel');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.client.email', 'en', 'Courriel');

/**		Initialisation des traductions des propriétés de la table COMMANDE		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.commande.montantTotal', 'fr', 'Montant total');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.commande.montantTotal', 'de', 'Montant total');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.commande.montantTotal', 'en', 'Montant total');

/**		Initialisation des traductions des propriétés de la table PERSONNE		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.personne.nom', 'fr', 'Nom');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.personne.prenom', 'fr', 'Prénom');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.personne.nom', 'de', 'Nom');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.personne.prenom', 'de', 'Prénom');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.personne.nom', 'en', 'Nom');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.personne.prenom', 'en', 'Prénom');

/**		Initialisation des traductions des valeurs de la table STATUT_COMMANDE		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnAttente', 'fr', 'En attente');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnPreparation', 'fr', 'En préparation');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Prete', 'fr', 'Prête');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Servie', 'fr', 'Servie');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Annulee', 'fr', 'Annulée');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnAttente', 'de', 'En attente');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnPreparation', 'de', 'En préparation');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Prete', 'de', 'Prête');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Servie', 'de', 'Servie');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Annulee', 'de', 'Annulée');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnAttente', 'en', 'En attente');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.EnPreparation', 'en', 'En préparation');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Prete', 'en', 'Prête');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Servie', 'en', 'Servie');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.statutCommande.values.Annulee', 'en', 'Annulée');

/**		Initialisation des traductions des valeurs de la table CATEGORIE_PLAT		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Entree', 'fr', 'Entrée');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Plat', 'fr', 'Plat principal');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Dessert', 'fr', 'Dessert');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Boisson', 'fr', 'Boisson');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Entree', 'de', 'Entrée');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Plat', 'de', 'Plat principal');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Dessert', 'de', 'Dessert');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Boisson', 'de', 'Boisson');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Entree', 'en', 'Entrée');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Plat', 'en', 'Plat principal');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Dessert', 'en', 'Dessert');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.categoriePlat.values.Boisson', 'en', 'Boisson');

/**		Initialisation des traductions des valeurs de la table DEPARTEMENT		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.Paris', 'fr', 'Paris');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.HautsDeSeine', 'fr', 'Hauts de Seine');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.SeineSaintDenis', 'fr', 'Seine Saint Denis');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.SeineEtMarne', 'fr', 'Seine et Marne');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.Paris', 'de', 'Paris');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.HautsDeSeine', 'de', 'Hauts de Seine');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.SeineSaintDenis', 'de', 'Seine Saint Denis');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.SeineEtMarne', 'de', 'Seine et Marne');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.Paris', 'en', 'Paris');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.HautsDeSeine', 'en', 'Hauts de Seine');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.SeineSaintDenis', 'en', 'Seine Saint Denis');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.departement.values.SeineEtMarne', 'en', 'Seine et Marne');

/**		Initialisation des traductions des valeurs de la table REGION		**/
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.region.values.Idf', 'fr', 'Île de France');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.region.values.Idf', 'de', 'Île de France');
INSERT INTO TRANSLATION(TRA_RESOURCE_KEY, TRA_LANG, TRA_VALUE) VALUES('restaurant.region.values.Idf', 'en', 'Île de France');
