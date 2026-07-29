----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	03_values.sql
--   Description		:	Script d'insertion des valeurs initiales.
-- ===========================================================================================

/**		Initialisation de la table type_utilisateur		**/
insert into type_utilisateur(tut_code, tut_libelle) values('ADM', 'refs.typeUtilisateur.values.ADM');
insert into type_utilisateur(tut_code, tut_libelle) values('GES', 'refs.typeUtilisateur.values.GES');
insert into type_utilisateur(tut_code, tut_libelle) values('CLI', 'refs.typeUtilisateur.values.CLI');
