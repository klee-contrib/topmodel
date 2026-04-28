----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	01_tables.sql
--   Description		:	Script de création des tables.
-- ===========================================================================================

/**
  * Création de la table AVIS_CLIENT
 **/
create table AVIS_CLIENT (
	AVI_ID int not null,
	AVI_NOTE int not null,
	AVI_COMMENTAIRE varchar(100),
	AVI_DATE_AVIS timestamp not null,
	AVI_APPROUVE boolean not null,
	AVI_NOMBRE_VUES int not null,
	PER_ID int not null,
	RES_ID int not null,
	constraint PK_AVIS_CLIENT primary key (AVI_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table AVIS_CLIENT
 **/
create sequence SEQ_AVIS_CLIENT as INT start 1000 increment 50 owned by AVIS_CLIENT.AVI_ID;

/**
  * Création de la table CATEGORIE_PLAT
 **/
create table CATEGORIE_PLAT (
	CAT_CODE varchar(10) not null,
	CAT_LIBELLE varchar(100) not null,
	CAT_ORDRE int not null,
	CAT_PRIX_MOYEN decimal,
	constraint PK_CATEGORIE_PLAT primary key (CAT_CODE)
);

/**
  * Création de la table CATEGORIE_PLAT_REGION
 **/
create table CATEGORIE_PLAT_REGION (
	REG_CODE varchar(10) not null,
	CAT_CODE varchar(10) not null,
	constraint PK_CATEGORIE_PLAT_REGION primary key (REG_CODE,CAT_CODE)
);

/**
  * Création de la table CLIENT
 **/
create table CLIENT (
	CLI_EMAIL varchar(100),
	PER_ID int not null,
	constraint PK_CLIENT primary key (PER_ID)
);

/**
  * Création de la table COMMANDE
 **/
create table COMMANDE (
	COM_ID int not null,
	COM_DATE_COMMANDE timestamp not null,
	COM_DATE_LIVRAISON timestamp,
	COM_MONTANT_TOTAL decimal not null,
	PER_ID int not null,
	TAB_ID int,
	REV_ID int,
	STC_CODE varchar(10) not null,
	AVI_ID int,
	constraint PK_COMMANDE primary key (COM_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table COMMANDE
 **/
create sequence SEQ_COMMANDE as INT start 1000 increment 50 owned by COMMANDE.COM_ID;

/**
  * Création de la table COMMANDE_HISTORIQUE
 **/
create table COMMANDE_HISTORIQUE (
	COM_ID int not null,
	COM_DATE_COMMANDE timestamp not null,
	COM_DATE_LIVRAISON timestamp,
	COM_MONTANT_TOTAL decimal not null,
	PER_ID int not null,
	TAB_ID int,
	REV_ID int,
	STC_CODE varchar(10) not null,
	AVI_ID int,
	constraint PK_COMMANDE_HISTORIQUE primary key (COM_ID)
);

/**
  * Création de la table DEPARTEMENT
 **/
create table DEPARTEMENT (
	DEP_CODE varchar(10) not null,
	DEP_LIBELLE varchar(100) not null,
	REG_CODE varchar(10) not null,
	constraint PK_DEPARTEMENT primary key (DEP_CODE)
);

/**
  * Création de la table EMPLOYE
 **/
create table EMPLOYE (
	EMP_TELEPHONE varchar(20),
	EMP_DATE_NAISSANCE timestamp,
	EMP_MATRICULE varchar(10) not null,
	EMP_DATE_EMBAUCHE timestamp not null,
	EMP_SALAIRE decimal,
	RES_ID int not null,
	PER_ID int not null,
	constraint PK_EMPLOYE primary key (PER_ID)
);

/**
  * Création de la table LIGNE_COMMANDE
 **/
create table LIGNE_COMMANDE (
	LIG_ID int not null,
	LIG_QUANTITE int not null,
	LIG_PRIX_UNITAIRE decimal not null,
	LIG_PRIX_TOTAL decimal not null,
	COM_ID int not null,
	PLA_ID int not null,
	constraint PK_LIGNE_COMMANDE primary key (LIG_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table LIGNE_COMMANDE
 **/
create sequence SEQ_LIGNE_COMMANDE as INT start 1000 increment 50 owned by LIGNE_COMMANDE.LIG_ID;

/**
  * Création de la table LIGNE_COMMANDE_HISTORIQUE
 **/
create table LIGNE_COMMANDE_HISTORIQUE (
	LIG_ID int not null,
	LIG_QUANTITE int not null,
	LIG_PRIX_UNITAIRE decimal not null,
	LIG_PRIX_TOTAL decimal not null,
	PLA_ID int not null,
	COM_ID int not null,
	constraint PK_LIGNE_COMMANDE_HISTORIQUE primary key (LIG_ID)
);

/**
  * Création de la table MENU
 **/
create table MENU (
	MEN_ID int not null,
	MEN_NOM varchar(100) not null,
	MEN_DESCRIPTION varchar(100),
	MEN_PRIX decimal not null,
	MEN_DISPONIBLE boolean not null,
	MEN_DATE_DEBUT timestamp,
	MEN_DATE_FIN timestamp,
	RES_ID int not null,
	constraint PK_MENU primary key (MEN_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table MENU
 **/
create sequence SEQ_MENU as INT start 1000 increment 50 owned by MENU.MEN_ID;

/**
  * Création de la table MENU_PLAT
 **/
create table MENU_PLAT (
	MEN_ID int not null,
	PLA_ID int not null,
	MPL_ORDRE int not null,
	constraint PK_MENU_PLAT primary key (MEN_ID,PLA_ID)
);

/**
  * Création de la table PERSONNE
 **/
create table PERSONNE (
	PER_ID int not null,
	PER_NOM varchar(100) not null,
	PER_PRENOM varchar(100) not null,
	DEP_CODE varchar(10),
	constraint PK_PERSONNE primary key (PER_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table PERSONNE
 **/
create sequence SEQ_PERSONNE as INT start 1000 increment 50 owned by PERSONNE.PER_ID;

/**
  * Création de la table PLAT
 **/
create table PLAT (
	PLA_ID int not null,
	PLA_NOM varchar(100) not null,
	PLA_DESCRIPTION varchar(100),
	PLA_PRIX decimal not null,
	PLA_DISPONIBLE boolean not null,
	CAT_CODE varchar(10) not null,
	RES_ID int not null,
	PLA_ID int,
	constraint PK_PLAT primary key (PLA_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table PLAT
 **/
create sequence SEQ_PLAT as INT start 1000 increment 50 owned by PLAT.PLA_ID;

/**
  * Création de la table PROMOTION
 **/
create table PROMOTION (
	PLA_ID int not null,
	PRO_LIBELLE varchar(100) not null,
	PRO_POURCENTAGE_REDUCTION int not null,
	PRO_DATE_DEBUT timestamp not null,
	PRO_DATE_FIN timestamp not null,
	PRO_ACTIVE boolean not null,
	RES_ID int,
	constraint PK_PROMOTION primary key (PLA_ID)
);

/**
  * Création de la table REGION
 **/
create table REGION (
	REG_CODE varchar(10) not null,
	REG_LIBELLE varchar(100) not null,
	REG_NOM_RESPONSABLE varchar(100),
	constraint PK_REGION primary key (REG_CODE)
);

/**
  * Création de la table RESERVATION
 **/
create table RESERVATION (
	REV_ID int not null,
	REV_DATE_RESERVATION timestamp not null,
	REV_NOMBRE_PERSONNES int not null,
	REV_COMMENTAIRE varchar(100),
	REV_CONFIRMEE boolean not null,
	PER_ID int not null,
	TAB_ID int,
	RES_ID int not null,
	constraint PK_RESERVATION primary key (REV_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table RESERVATION
 **/
create sequence SEQ_RESERVATION as INT start 1000 increment 50 owned by RESERVATION.REV_ID;

/**
  * Création de la table RESTAURANT
 **/
create table RESTAURANT (
	RES_ID int not null,
	RES_NOM varchar(100) not null,
	RES_ADRESSE varchar(100),
	RES_TELEPHONE varchar(20),
	constraint PK_RESTAURANT primary key (RES_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table RESTAURANT
 **/
create sequence SEQ_RESTAURANT as INT start 1000 increment 50 owned by RESTAURANT.RES_ID;

/**
  * Création de la table STATUT_COMMANDE
 **/
create table STATUT_COMMANDE (
	STC_CODE varchar(10) not null,
	STC_LIBELLE varchar(100) not null,
	constraint PK_STATUT_COMMANDE primary key (STC_CODE)
);

/**
  * Création de la table TABLE
 **/
create table TABLE (
	TAB_ID int not null,
	TAB_NUMERO varchar(10) not null,
	TAB_CAPACITE int not null,
	TAB_DISPONIBLE boolean not null,
	RES_ID int not null,
	constraint PK_TABLE primary key (TAB_ID)
);

/**
  * Création de la séquence pour la clé primaire de la table TABLE
 **/
create sequence SEQ_TABLE as INT start 1000 increment 50 owned by TABLE.TAB_ID;

/**
  * Création de la table TRANSLATION
 **/
create table TRANSLATION (
	TRA_RESOURCE_KEY varchar(100) not null,
	TRA_VALUE varchar(100) not null,
	constraint PK_TRANSLATION primary key (TRA_RESOURCE_KEY)
);
