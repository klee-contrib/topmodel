----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	06_resources.sql
--   Description		:	Script de création des resources (libellés traduits). 
-- =========================================================================================== 

/**		Initialisation des traductions des propriétés de la table DROIT		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.droit.code', 'Droit');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.droit.libelle', 'Droit');

/**		Initialisation des traductions des valeurs de la table DROIT		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.droit.values.Create', 'Création');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.droit.values.Read', 'Lecture');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.droit.values.Update', 'Mise à jour');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.droit.values.Delete', 'Suppression');

/**		Initialisation des traductions des propriétés de la table PROFIL		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.profil.id', 'Id technique du profil');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.profil.libelle', 'Libellé du profil');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.profil.droits', 'Droits');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('common.entityListeners.dateCreation', 'Date de création');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('common.entityListeners.dateModification', 'Date de modification');

/**		Initialisation des traductions des propriétés de la table TYPE_DROIT		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.typeDroit.code', 'Type de droit');

/**		Initialisation des traductions des valeurs de la table TYPE_DROIT		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.typeDroit.values.Read', 'Lecture');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.typeDroit.values.Write', 'Ecriture');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.profil.typeDroit.values.Admin', 'Administration');

/**		Initialisation des traductions des propriétés de la table TYPE_UTILISATEUR		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.typeUtilisateur.code', 'Type d''utilisateur');

/**		Initialisation des traductions des valeurs de la table TYPE_UTILISATEUR		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.typeUtilisateur.values.Admin', 'Administrateur');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.typeUtilisateur.values.Gestionnaire', 'Gestionnaire');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.typeUtilisateur.values.Client', 'Client');

/**		Initialisation des traductions des propriétés de la table UTILISATEUR		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.id', 'Id technique');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.prenom', 'Prénom');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.email', 'Adresse email');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.dateNaissance', 'Date de naissance');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.adresse', 'Adresse');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.profilId', 'Profil');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('securite.utilisateur.utilisateur.typeUtilisateurCode', 'Type d''utilisateur');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('common.entityListeners.dateCreation', 'Date de création');
INSERT INTO TRANSLATION(RESOURCE_KEY, LABEL) VALUES('common.entityListeners.dateModification', 'Date de modification');
