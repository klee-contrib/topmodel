----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_resources.sql
--   Description		:	Scripts d'insertion des ressources (libellés traduits).
-- ===========================================================================================

/**		Initialisation des traductions des propriétés de la table avis_client		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('common.dateCreation.dateCreation', 'fr', 'Date de création');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('common.dateCreation.dateCreation', 'de', 'Date de création');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('common.dateCreation.dateCreation', 'en', 'Creation date');

/**		Initialisation des traductions des propriétés de la table client		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.client.email', 'fr', 'Courriel');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.client.email', 'de', 'Courriel');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.client.email', 'en', 'Email');

/**		Initialisation des traductions des propriétés de la table commande		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.commande.montantTotal', 'fr', 'Montant total');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.commande.montantTotal', 'de', 'Montant total');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.commande.montantTotal', 'en', 'Total amount');

/**		Initialisation des traductions des propriétés de la table personne		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.personneBase.nom', 'fr', 'Nom');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.personneBase.prenom', 'fr', 'Prénom');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.personneBase.nom', 'de', 'Nom');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.personneBase.prenom', 'de', 'Prénom');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.personneBase.nom', 'en', 'Nom');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.personneBase.prenom', 'en', 'Prénom');

/**		Initialisation des traductions des valeurs de la table categorie_plat		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Autre', 'fr', 'Autre');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Entree', 'fr', 'Entrée');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Principal', 'fr', 'Plat principal');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Dessert', 'fr', 'Dessert');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Boisson', 'fr', 'Boisson');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Autre', 'de', 'Autre');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Entree', 'de', 'Entrée');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Principal', 'de', 'Plat principal');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Dessert', 'de', 'Dessert');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Boisson', 'de', 'Boisson');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Autre', 'en', 'Autre');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Entree', 'en', 'Starter');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Principal', 'en', 'Plat principal');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Dessert', 'en', 'Dessert');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.categoriePlat.values.Boisson', 'en', 'Drink');

/**		Initialisation des traductions des valeurs de la table departement		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.Paris', 'fr', 'Paris');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.HautsDeSeine', 'fr', 'Hauts de Seine');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.SeineSaintDenis', 'fr', 'Seine Saint Denis');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.SeineEtMarne', 'fr', 'Seine et Marne');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.Paris', 'de', 'Paris');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.HautsDeSeine', 'de', 'Hauts de Seine');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.SeineSaintDenis', 'de', 'Seine Saint Denis');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.SeineEtMarne', 'de', 'Seine et Marne');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.Paris', 'en', 'Paris');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.HautsDeSeine', 'en', 'Hauts de Seine');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.SeineSaintDenis', 'en', 'Seine Saint Denis');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.departement.values.SeineEtMarne', 'en', 'Seine et Marne');

/**		Initialisation des traductions des valeurs de la table region		**/
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.region.values.Idf', 'fr', 'Île de France');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.region.values.Idf', 'de', 'Île de France');
INSERT INTO translation(tra_resource_key, tra_lang, tra_value) VALUES('restaurant.region.values.Idf', 'en', 'Île de France');
