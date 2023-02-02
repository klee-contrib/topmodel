----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	01_tables.sql
--   Description		:	Script de création des tables.
-- =========================================================================================== 
/**
  * Création de la table PROFIL_DROITS
 **/
create table PROFIL_DROITS (
	PRO_ID int8 not null,
	DRO_CODE varchar(3) not null,
	constraint PK_PROFIL_DROITS primary key (PRO_ID,DRO_CODE)
);

/**
  * Création de la table DROITS
 **/
create table DROITS (
	DRO_CODE varchar(3) not null,
	DRO_LIBELLE varchar(3) not null,
	TPR_CODE varchar(3),
	constraint PK_DROITS primary key (DRO_CODE)
);

/**
  * Création de la séquence pour la clé primaire de la table PROFIL
 **/
create sequence SEQ_PROFIL start 1000 increment 50;
/**
  * Création de la table PROFIL
 **/
create table PROFIL (
	PRO_ID int8 not null,
	TPR_CODE varchar(3),
	constraint PK_PROFIL primary key (PRO_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table SECTEUR
 **/
create sequence SEQ_SECTEUR start 1000 increment 50;
/**
  * Création de la table SECTEUR
 **/
create table SECTEUR (
	SEC_ID int8 not null,
	PRO_ID int8,
	constraint PK_SECTEUR primary key (SEC_ID)
);

/**
  * Création de la table TYPE_PROFIL
 **/
create table TYPE_PROFIL (
	TPR_CODE varchar(3) not null,
	TPR_LIBELLE varchar(3) not null,
	constraint PK_TYPE_PROFIL primary key (TPR_CODE)
);

/**
  * Création de la table TYPE_UTILISATEUR
 **/
create table TYPE_UTILISATEUR (
	TUT_CODE varchar(3) not null,
	TUT_LIBELLE varchar(3) not null,
	constraint PK_TYPE_UTILISATEUR primary key (TUT_CODE)
);

/**
  * Création de la séquence pour la clé primaire de la table UTILISATEUR
 **/
create sequence SEQ_UTILISATEUR start 1000 increment 50;
/**
  * Création de la table UTILISATEUR
 **/
create table UTILISATEUR (
	UTI_ID int8 not null,
	UTI_AGE numeric(20, 9),
	PRO_ID int8,
	UTI_EMAIL varchar(50),
	UTI_NOM varchar(3),
	TUT_CODE varchar(3),
	UTI_ID_PARENT int8,
	UTI_DATE_CREATION date,
	UTI_DATE_MODIFICATION date,
	constraint PK_UTILISATEUR primary key (UTI_ID)
);

