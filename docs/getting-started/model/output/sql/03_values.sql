----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	03_values.sql
--   Description		:	Script d'insertion des valeurs initiales.
-- ===========================================================================================

/**		Initialisation de la table TYPE_UTILISATEUR		**/
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('ADM', 'refs.typeUtilisateur.values.ADM');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('GES', 'refs.typeUtilisateur.values.GES');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('CLI', 'refs.typeUtilisateur.values.CLI');
