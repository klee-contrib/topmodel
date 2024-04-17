----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	06_resources.sql
--   Description		:	Script de création des resources (libellés traduits). 
-- =========================================================================================== 

/**		Initialisation des traductions des valeurs de la table DROIT		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.droit.values.Create', null, 'Création');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.droit.values.Read', null, 'Lecture');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.droit.values.Update', null, 'Mise à jour');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.droit.values.Delete', null, 'Suppression');

/**		Initialisation des traductions des valeurs de la table TYPE_DROIT		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.typeDroit.values.Read', null, 'Lecture');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.typeDroit.values.Write', null, 'Ecriture');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.profil.typeDroit.values.Admin', null, 'Administration');

/**		Initialisation des traductions des valeurs de la table TYPE_UTILISATEUR		**/
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.utilisateur.typeUtilisateur.values.Admin', null, 'Administrateur');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.utilisateur.typeUtilisateur.values.Gestionnaire', null, 'Gestionnaire');
INSERT INTO TRANSLATION(RESOURCE_KEY, LOCALE, LABEL) VALUES('securite.utilisateur.typeUtilisateur.values.Client', null, 'Client');
