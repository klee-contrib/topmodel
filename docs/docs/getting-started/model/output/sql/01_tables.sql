----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	01_tables.sql
--   Description		:	Script de création des tables.
-- ===========================================================================================

/**
  * Création de la table TYPE_UTILISATEUR
 **/
create table TYPE_UTILISATEUR (
	TUT_CODE varchar(3),
	TUT_LIBELLE varchar(15) not null,
	constraint PK_TYPE_UTILISATEUR primary key (TUT_CODE)
);

/**
  * Création de la table UTILISATEUR
 **/
create table UTILISATEUR (
	UTI_ID int8,
	UTI_EMAIL varchar(50) not null,
	UTI_NOM varchar(15),
	UTI_DATE_INSCRIPTION timestamp,
	TUT_CODE varchar(3),
	constraint PK_UTILISATEUR primary key (UTI_ID)
);
