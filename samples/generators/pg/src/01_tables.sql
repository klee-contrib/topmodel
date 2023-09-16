----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	01_tables.sql
--   Description		:	Script de création des tables.
-- =========================================================================================== 
/**
  * Création de la table DROIT
 **/
create table DROIT (
	DRO_CODE varchar(10) not null,
	DRO_LIBELLE varchar(100) not null,
	TDR_CODE varchar(10) not null,
	constraint PK_DROIT primary key (DRO_CODE)
);

/**
  * Création de la séquence pour la clé primaire de la table PROFIL
 **/
create sequence SEQ_PROFIL start 1000 increment 50;
/**
  * Création de la table PROFIL
 **/
create table PROFIL (
	PRO_ID int not null,
	PRO_LIBELLE varchar(100) not null,
	PRO_DATE_CREATION date not null,
	PRO_DATE_MODIFICATION date,
	constraint PK_PROFIL primary key (PRO_ID)
);

/**
  * Création de la table PROFIL_DROIT
 **/
create table PROFIL_DROIT (
	PRO_ID int not null,
	DRO_CODE varchar(10) not null,
	constraint PK_PROFIL_DROIT primary key (PRO_ID,DRO_CODE)
);

/**
  * Création de la table TYPE_DROIT
 **/
create table TYPE_DROIT (
	TDR_CODE varchar(10) not null,
	TDR_LIBELLE varchar(100) not null,
	constraint PK_TYPE_DROIT primary key (TDR_CODE)
);

/**
  * Création de la table TYPE_UTILISATEUR
 **/
create table TYPE_UTILISATEUR (
	TUT_CODE varchar(10) not null,
	TUT_LIBELLE varchar(100) not null,
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
	UTI_ID int not null,
	UTI_NOM varchar(100) not null,
	UTI_PRENOM varchar(100) not null,
	UTI_EMAIL varchar(50) not null,
	UTI_DATE_NAISSANCE date,
	UTI_ACTIF boolean not null,
	PRO_ID int not null,
	TUT_CODE varchar(10) not null,
	UTI_DATE_CREATION date not null,
	UTI_DATE_MODIFICATION date,
	constraint PK_UTILISATEUR primary key (UTI_ID)
);

