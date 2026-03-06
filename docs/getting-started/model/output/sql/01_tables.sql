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
	CODE varchar(3) not null,
	LIBELLE varchar(15) not null,
	constraint PK_TYPE_UTILISATEUR primary key (CODE)
);

/**
  * Création de la table UTILISATEUR
 **/
create table UTILISATEUR (
	ID int8 not null,
	EMAIL varchar(50) not null,
	NOM varchar(15),
	DATE_INSCRIPTION timestamp,
	CODE varchar(3),
	constraint PK_UTILISATEUR primary key (ID)
);
