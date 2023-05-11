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
	DRO_CODE varchar(3) not null,
	DRO_LIBELLE varchar(3) not null,
	TPR_CODE varchar(3),
	constraint PK_DROIT primary key (DRO_CODE)
);

COMMENT ON TABLE DROIT IS 'Droits de l''application';
COMMENT ON COLUMN DROIT.DRO_CODE IS 'Code du droit';
COMMENT ON COLUMN DROIT.DRO_LIBELLE IS 'Libellé du droit';
COMMENT ON COLUMN DROIT.TPR_CODE IS 'Type de profil pouvant faire l''action';

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

COMMENT ON TABLE PROFIL IS 'Profil des utilisateurs';
COMMENT ON COLUMN PROFIL.PRO_ID IS 'Id technique';
COMMENT ON COLUMN PROFIL.TPR_CODE IS 'Type de profil';

/**
  * Création de la table PROFIL_DROIT
 **/
create table PROFIL_DROIT (
	PRO_ID int8 not null,
	DRO_CODE varchar(3) not null,
	constraint PK_PROFIL_DROIT primary key (PRO_ID,DRO_CODE)
);

COMMENT ON TABLE PROFIL_DROIT IS 'Liste des droits de l''utilisateur';
COMMENT ON COLUMN PROFIL_DROIT.PRO_ID IS 'Liste des droits de l''utilisateur';
COMMENT ON COLUMN PROFIL_DROIT.DRO_CODE IS 'Liste des droits de l''utilisateur';

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

COMMENT ON TABLE SECTEUR IS 'Secteur d''application du profil';
COMMENT ON COLUMN SECTEUR.SEC_ID IS 'Id technique';
COMMENT ON COLUMN SECTEUR.PRO_ID IS 'Liste des secteurs de l''utilisateur';

/**
  * Création de la table TYPE_PROFIL
 **/
create table TYPE_PROFIL (
	TPR_CODE varchar(3) not null,
	TPR_LIBELLE varchar(3) not null,
	constraint PK_TYPE_PROFIL primary key (TPR_CODE)
);

COMMENT ON TABLE TYPE_PROFIL IS 'Type d''utilisateur';
COMMENT ON COLUMN TYPE_PROFIL.TPR_CODE IS 'Code du type d''utilisateur';
COMMENT ON COLUMN TYPE_PROFIL.TPR_LIBELLE IS 'Libellé du type d''utilisateur';

/**
  * Création de la table TYPE_UTILISATEUR
 **/
create table TYPE_UTILISATEUR (
	TUT_CODE varchar(3) not null,
	TUT_LIBELLE varchar(3) not null,
	constraint PK_TYPE_UTILISATEUR primary key (TUT_CODE)
);

COMMENT ON TABLE TYPE_UTILISATEUR IS 'Type d''utilisateur';
COMMENT ON COLUMN TYPE_UTILISATEUR.TUT_CODE IS 'Code du type d''utilisateur';
COMMENT ON COLUMN TYPE_UTILISATEUR.TUT_LIBELLE IS 'Libellé du type d''utilisateur';

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
	UTI_ACTIF boolean,
	TUT_CODE varchar(3),
	UTI_ID_PARENT int8,
	UTI_DATE_CREATION date,
	UTI_DATE_MODIFICATION date,
	UTI_ID_ENFANT int8,
	constraint PK_UTILISATEUR primary key (UTI_ID)
);

COMMENT ON TABLE UTILISATEUR IS 'Utilisateur de l''application';
COMMENT ON COLUMN UTILISATEUR.UTI_ID IS 'Id technique';
COMMENT ON COLUMN UTILISATEUR.UTI_AGE IS 'Age en années de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.PRO_ID IS 'Profil de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_EMAIL IS 'Email de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_NOM IS 'Nom de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_ACTIF IS 'Si l''utilisateur est actif';
COMMENT ON COLUMN UTILISATEUR.TUT_CODE IS 'Type d''utilisateur en Many to one';
COMMENT ON COLUMN UTILISATEUR.UTI_ID_PARENT IS 'Utilisateur parent';
COMMENT ON COLUMN UTILISATEUR.UTI_DATE_CREATION IS 'Date de création de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_DATE_MODIFICATION IS 'Date de modification de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_ID_ENFANT IS 'Utilisateur enfants';

