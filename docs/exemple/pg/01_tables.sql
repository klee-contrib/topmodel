----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	01_tables.sql
--   Description		:	Script de création des tables.
-- =========================================================================================== 
/**
  * Création de la table PROFIL_DROITS_APPLI
 **/
create table PROFIL_DROITS_APPLI (
	PRO_ID_APPLI int8 not null,
	CODE_APPLI varchar(3) not null,
	constraint PK_PROFIL_DROITS_APPLI primary key (PRO_ID_APPLI,CODE_APPLI)
)
;

/**
  * Création de la table DROITS
 **/
create table DROITS (
	CODE varchar(3) not null,
	LIBELLE varchar(3) not null,
	constraint PK_DROITS primary key (CODE)
)
;

/**
  * Création de la séquence pour la clé primaire de la table PROFIL
 **/
create sequence SEQ_PROFIL start 1000 increment 50
;
/**
  * Création de la table PROFIL
 **/
create table PROFIL (
	PRO_ID int8 not null,
	CODE varchar(3),
	constraint PK_PROFIL primary key (PRO_ID)
)
;

/**
  * Création de la séquence pour la clé primaire de la table SECTEUR
 **/
create sequence SEQ_SECTEUR start 1000 increment 50
;
/**
  * Création de la table SECTEUR
 **/
create table SECTEUR (
	SEC_ID int8 not null,
	SEC_ID int8,
	constraint PK_SECTEUR primary key (SEC_ID)
)
;

/**
  * Création de la table TYPE_PROFIL
 **/
create table TYPE_PROFIL (
	CODE varchar(3) not null,
	LIBELLE varchar(3) not null,
	constraint PK_TYPE_PROFIL primary key (CODE)
)
;

/**
  * Création de la table TYPE_UTILISATEUR
 **/
create table TYPE_UTILISATEUR (
	TUT_CODE varchar(3) not null,
	TUT_LIBELLE varchar(3) not null,
	constraint PK_TYPE_UTILISATEUR primary key (TUT_CODE)
)
;

/**
  * Création de la séquence pour la clé primaire de la table UTILISATEUR
 **/
create sequence SEQ_UTILISATEUR start 1000 increment 50
;
/**
  * Création de la table UTILISATEUR
 **/
create table UTILISATEUR (
	UTI_DATE_CREATION date,
	UTI_DATE_MODIFICATION date,
	UTI_ID int8 not null,
	UTI_AGE numeric(20, 9),
	PRO_ID int8,
	UTI_EMAIL varchar(50),
	TUT_CODE varchar(3),
	UTI_ID_JUMEAU int8,
	constraint PK_UTILISATEUR primary key (UTI_ID)
)
;

