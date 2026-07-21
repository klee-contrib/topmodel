----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	03_values.sql
--   Description		:	Script d'insertion des valeurs initiales.
-- ===========================================================================================

/**		Initialisation de la table ASSIETTE		**/
INSERT INTO ASSIETTE(VSL_ID, VSL_DESCRIPTION, AST_TAILLE) VALUES(1, 'Grande assiette', 29);

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

/**		Initialisation de la table COUVERT		**/
INSERT INTO COUVERT(VSL_ID, VSL_DESCRIPTION) VALUES(2, 'Fourchette');
INSERT INTO COUVERT(VSL_ID, VSL_DESCRIPTION) VALUES(3, 'Couteau');

/**		Initialisation de la table DEPARTEMENT		**/
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('75', 'restaurant.departement.values.Paris', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('92', 'restaurant.departement.values.HautsDeSeine', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('93', 'restaurant.departement.values.SeineSaintDenis', 'IDF');
INSERT INTO DEPARTEMENT(DEP_CODE, DEP_LIBELLE, REG_CODE) VALUES('94', 'restaurant.departement.values.SeineEtMarne', 'IDF');

/**		Initialisation de la table LIEU		**/
INSERT INTO LIEU(LIE_ID, LIE_NOM, LIE_ADRESSE, LIE_DISCRIMINATOR, RES_TELEPHONE, RES_DATE_CREATION, FRN_TELEPHONE, FRN_BIO) VALUES(1, 'Burger King', NULL, 'RESTAURANT', NULL, '1954-01-01T00:00:00Z', NULL, NULL);
INSERT INTO LIEU(LIE_ID, LIE_NOM, LIE_ADRESSE, LIE_DISCRIMINATOR, RES_TELEPHONE, RES_DATE_CREATION, FRN_TELEPHONE, FRN_BIO) VALUES(2, 'Pomona', NULL, 'FOURNISSEUR', NULL, NULL, NULL, true);

/**		Initialisation de la table PERSONNE		**/
INSERT INTO PERSONNE(PER_ID, PER_NOM, PER_PRENOM, DEP_CODE, PER_DATE_CREATION) VALUES(1, 'Michel', 'Jean', NULL, '2026-01-01T00:00:00Z');
INSERT INTO PERSONNE(PER_ID, PER_NOM, PER_PRENOM, DEP_CODE, PER_DATE_CREATION) VALUES(2, 'Christophe', 'Michel', NULL, '2026-01-01T00:00:00Z');

/**		Initialisation de la table EMPLOYE		**/
INSERT INTO EMPLOYE(EMP_TELEPHONE, EMP_DATE_NAISSANCE, EMP_MATRICULE, EMP_DATE_EMBAUCHE, EMP_SALAIRE, LIE_ID, PER_ID) VALUES(NULL, NULL, '123456', '2026-01-01T00:00:00Z', NULL, 1, 2);
