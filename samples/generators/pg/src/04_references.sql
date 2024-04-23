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

/**		Initialisation de la table DROIT		**/
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('CREATE', "securite.profil.droit.values.Create", 'WRITE');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('READ', "securite.profil.droit.values.Read", 'READ');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('UPDATE', "securite.profil.droit.values.Update", 'WRITE');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('DELETE', "securite.profil.droit.values.Delete", 'ADMIN');

/**		Initialisation de la table TYPE_UTILISATEUR		**/
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('ADMIN', "securite.utilisateur.typeUtilisateur.values.Admin");
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('GEST', "securite.utilisateur.typeUtilisateur.values.Gestionnaire");
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('CLIENT', "securite.utilisateur.typeUtilisateur.values.Client");

