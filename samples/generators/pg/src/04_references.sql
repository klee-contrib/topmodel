----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références
-- ===========================================================================================
/**		Initialisation de la table TYPE_DROIT		**/
INSERT INTO TYPE_DROIT(TDR_CODE, TDR_LIBELLE) VALUES('READ', 'Lecture');
INSERT INTO TYPE_DROIT(TDR_CODE, TDR_LIBELLE) VALUES('WRITE', 'Ecriture');
INSERT INTO TYPE_DROIT(TDR_CODE, TDR_LIBELLE) VALUES('ADMIN', 'Administration');

/**		Initialisation de la table DROIT		**/
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('CREATE', 'Création', 'WRITE');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('READ', 'Lecture', 'READ');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('UPDATE', 'Mise à jour', 'WRITE');
INSERT INTO DROIT(DRO_CODE, DRO_LIBELLE, TDR_CODE) VALUES('DELETE', 'Suppression', 'ADMIN');

/**		Initialisation de la table TYPE_UTILISATEUR		**/
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('ADMIN', 'Administrateur');
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('GEST', 'Gestionnaire');
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('CLIENT', 'Client');

