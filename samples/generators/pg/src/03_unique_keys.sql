----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	03_unique_keys.sql
--   Description		:	Script de création des index uniques.
-- =========================================================================================== 
alter table UTILISATEUR add constraint UK_UTILISATEUR_UTI_EMAIL unique (UTI_EMAIL);

