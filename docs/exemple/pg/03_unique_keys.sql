----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	03_unique_keys.sql
--   Description		:	Script de création des index uniques.
-- =========================================================================================== 
alter table UTILISATEUR add constraint UK_UTILISATEUR_UTI_ID_PARENT unique (UTI_ID_PARENT);

