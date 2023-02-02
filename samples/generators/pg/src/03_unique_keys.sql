----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	03_unique_keys.sql
--   Description		:	Script de création des index uniques.
-- =========================================================================================== 
alter table UTILISATEUR add constraint UK_UTILISATEUR_UTI_EMAIL_UTI_ID_PARENT unique (UTI_EMAIL, UTI_ID_PARENT);

alter table UTILISATEUR add constraint UK_UTILISATEUR_UTI_ID_PARENT unique (UTI_ID_PARENT);

