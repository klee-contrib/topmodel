----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références
-- ===========================================================================================
/**		Initialisation de la table TYPE_DROIT		**/
INSERT INTO TYPE_DROIT(TDR_CODE, TDR_LIBELLE) VALUES('READ', "securite.profil.typeDroit.values.Read");
INSERT INTO TYPE_DROIT(TDR_CODE, TDR_LIBELLE) VALUES('WRITE', "securite.profil.typeDroit.values.Write");
INSERT INTO TYPE_DROIT(TDR_CODE, TDR_LIBELLE) VALUES('ADMIN', "securite.profil.typeDroit.values.Admin");

/**		Initialisation des traductions des valeurs de la table TYPE_DROIT		**/
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.typeDroit.values.Read", "", "Lecture");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.typeDroit.values.Write", "", "Ecriture");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.typeDroit.values.Admin", "", "Administration");

/**		Initialisation de la table DROIT		**/
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('CREATE', "securite.profil.droit.values.Create", 'WRITE');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('READ', "securite.profil.droit.values.Read", 'READ');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('UPDATE', "securite.profil.droit.values.Update", 'WRITE');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('DELETE', "securite.profil.droit.values.Delete", 'ADMIN');

/**		Initialisation des traductions des valeurs de la table DROIT		**/
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.droit.values.Create", "", "Création");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.droit.values.Read", "", "Lecture");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.droit.values.Update", "", "Mise à jour");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.profil.droit.values.Delete", "", "Suppression");

/**		Initialisation de la table TYPE_UTILISATEUR		**/
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('ADMIN', "securite.utilisateur.typeUtilisateur.values.Admin");
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('GEST', "securite.utilisateur.typeUtilisateur.values.Gestionnaire");
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('CLIENT', "securite.utilisateur.typeUtilisateur.values.Client");

/**		Initialisation des traductions des valeurs de la table TYPE_UTILISATEUR		**/
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.utilisateur.typeUtilisateur.values.Admin", "", "Administrateur");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.utilisateur.typeUtilisateur.values.Gestionnaire", "", "Gestionnaire");
INSERT INTO TRANSLATIONS(TRAD_KEY, LOCALE, LABEL) VALUES("securite.utilisateur.typeUtilisateur.values.Client", "", "Client");

