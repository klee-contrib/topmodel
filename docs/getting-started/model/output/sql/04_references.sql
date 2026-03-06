----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références.
-- ===========================================================================================

/**		Initialisation de la table TYPE_UTILISATEUR		**/
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('ADM', 'refs.typeUtilisateur.values.ADM');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('GES', 'refs.typeUtilisateur.values.GES');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('CLI', 'refs.typeUtilisateur.values.CLI');
