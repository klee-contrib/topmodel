----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	03_values.sql
--   Description		:	Script d'insertion des valeurs initiales.
-- ===========================================================================================

/**		Initialisation de la table CATEGORIE_PLAT		**/
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('AUTRE', 'restaurant.categoriePlat.values.Autre', 5, NULL);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('ENTREE', 'restaurant.categoriePlat.values.Entree', 2, NULL);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('PRINCIPAL', 'restaurant.categoriePlat.values.Principal', 3, 10);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('DESSERT', 'restaurant.categoriePlat.values.Dessert', 4, NULL);
INSERT INTO CATEGORIE_PLAT(CAT_CODE, CAT_LIBELLE, CAT_ORDRE, CAT_PRIX_MOYEN) VALUES('BOISSON', 'restaurant.categoriePlat.values.Boisson', 1, 2);

/**		Initialisation de la table REGION		**/
INSERT INTO REGION(REG_CODE, REG_LIBELLE, REG_NOM_RESPONSABLE) VALUES('IDF', 'restaurant.region.values.Idf', NULL);

/**		Initialisation de la table CATEGORIE_PLAT_REGION		**/
INSERT INTO CATEGORIE_PLAT_REGION(REG_CODE, CAT_CODE) VALUES('IDF', 'ENTREE');
INSERT INTO CATEGORIE_PLAT_REGION(REG_CODE, CAT_CODE) VALUES('IDF', 'DESSERT');

/**		Initialisation de la table DEPARTEMENT		**/
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('75', 'restaurant.departement.values.Paris', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('92', 'restaurant.departement.values.HautsDeSeine', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('93', 'restaurant.departement.values.SeineSaintDenis', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('94', 'restaurant.departement.values.SeineEtMarne', 'IDF');
