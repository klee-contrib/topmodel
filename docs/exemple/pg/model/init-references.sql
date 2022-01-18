----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	init-references.sql
--   Description		:	Script d'insertion des données de références
-- ===========================================================================================
/**		Initialisation de la table TypeProfil		**/
INSERT INTO TYPE_PROFIL(CODE, LIBELLE) VALUES('ADM', 'Administrateur');
INSERT INTO TYPE_PROFIL(CODE, LIBELLE) VALUES('GES', 'Gestionnaire');

/**		Initialisation de la table TypeUtilisateur		**/
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('ADM', 'Administrateur');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('GES', 'Gestionnaire');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('CLI', 'Client');

