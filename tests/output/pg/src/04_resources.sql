----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_resources.sql
--   Description		:	Scripts d'insertion des ressources (libellés traduits).
-- ===========================================================================================

/**		Initialisation des traductions des propriétés de la table avis_client		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.dateCreation.dateCreation', 'fr', 'Date de création');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.dateCreation.dateCreation', 'de', 'Date de création');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.dateCreation.dateCreation', 'en', 'Creation date');

/**		Initialisation des traductions des propriétés de la table client		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.email', 'fr', 'Courriel');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.email', 'de', 'Courriel');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.email', 'en', 'Email');

/**		Initialisation des traductions des propriétés de la table commande		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.montantTotal', 'fr', 'Montant total');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.montantTotal', 'de', 'Montant total');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.montantTotal', 'en', 'Total amount');

/**		Initialisation des traductions des propriétés de la table personne		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.nom', 'fr', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.prenom', 'fr', 'Prénom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.nom', 'de', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.prenom', 'de', 'Prénom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.nom', 'en', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.prenom', 'en', 'Prénom');

/**		Initialisation des traductions des valeurs de la table categorie_plat		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Autre', 'fr', 'Autre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Entree', 'fr', 'Entrée');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Principal', 'fr', 'Plat principal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Dessert', 'fr', 'Dessert');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Boisson', 'fr', 'Boisson');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Autre', 'de', 'Autre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Entree', 'de', 'Entrée');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Principal', 'de', 'Plat principal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Dessert', 'de', 'Dessert');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Boisson', 'de', 'Boisson');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Autre', 'en', 'Autre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Entree', 'en', 'Starter');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Principal', 'en', 'Plat principal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Dessert', 'en', 'Dessert');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Boisson', 'en', 'Drink');

/**		Initialisation des traductions des valeurs de la table departement		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.Paris', 'fr', 'Paris');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.HautsDeSeine', 'fr', 'Hauts de Seine');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineSaintDenis', 'fr', 'Seine Saint Denis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineEtMarne', 'fr', 'Seine et Marne');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.Paris', 'de', 'Paris');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.HautsDeSeine', 'de', 'Hauts de Seine');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineSaintDenis', 'de', 'Seine Saint Denis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineEtMarne', 'de', 'Seine et Marne');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.Paris', 'en', 'Paris');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.HautsDeSeine', 'en', 'Hauts de Seine');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineSaintDenis', 'en', 'Seine Saint Denis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineEtMarne', 'en', 'Seine et Marne');

/**		Initialisation des traductions des valeurs de la table region		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.values.Idf', 'fr', 'Île de France');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.values.Idf', 'de', 'Île de France');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.values.Idf', 'en', 'Île de France');
