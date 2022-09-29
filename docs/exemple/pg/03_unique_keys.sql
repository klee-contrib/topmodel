----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	03_unique_keys.sql
--   Description		:	Script de création des index uniques.
-- =========================================================================================== 
alter table UTILISATEUR add constraint UK_UTILISATEUR_EMAIL_ID_PARENT unique (EMAIL, ID_PARENT);

alter table UTILISATEUR add constraint UK_UTILISATEUR_ID_PARENT unique (ID_PARENT);

