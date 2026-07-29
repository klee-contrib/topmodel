----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	01_tables.sql
--   Description		:	Script de création des tables.
-- ===========================================================================================

/**
  * Création de la table type_utilisateur
 **/
create table type_utilisateur (
	tut_code varchar(3),
	tut_libelle varchar(15) not null,
	constraint pk_type_utilisateur primary key (tut_code)
);

/**
  * Création de la table utilisateur
 **/
create table utilisateur (
	uti_id int8,
	uti_email varchar(50) not null,
	uti_nom varchar(15),
	uti_date_inscription timestamp,
	tut_code varchar(3),
	constraint pk_utilisateur primary key (uti_id)
);
