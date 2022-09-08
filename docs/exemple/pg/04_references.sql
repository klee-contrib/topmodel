----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	04_references.sql
--   Description		:	Script d'insertion des données de références
-- ===========================================================================================
/**		Initialisation de la table Droits		**/
INSERT INTO DROITS(CODE, LIBELLE) VALUES('CRE', 'Créer');
INSERT INTO DROITS(CODE, LIBELLE) VALUES('MOD', 'Modifier');
INSERT INTO DROITS(CODE, LIBELLE) VALUES('SUP', 'Supprimer');

/**		Initialisation de la table TypeProfil		**/
INSERT INTO TYPE_PROFIL(CODE, LIBELLE) VALUES('ADM', 'Administrateur');
INSERT INTO TYPE_PROFIL(CODE, LIBELLE) VALUES('GES', 'Gestionnaire');

/**		Initialisation de la table TypeUtilisateur		**/
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('ADM', 'Administrateur');
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('GES', 'Gestionnaire');
INSERT INTO TYPE_UTILISATEUR(TUT_CODE, TUT_LIBELLE) VALUES('CLI', 'Client');

