----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères. 
-- =========================================================================================== 
/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS_APPLI.PRO_ID_APPLI
 **/
create index IDX_PROFIL_DROITS_APPLI_PRO_ID_APPLI_FK on PROFIL_DROITS_APPLI (
	PRO_ID_APPLI ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS_APPLI.PRO_ID_APPLI
 **/
alter table PROFIL_DROITS_APPLI
	add constraint FK_PROFIL_DROITS_APPLI_PRO_ID_APPLI foreign key (PRO_ID_APPLI)
		references PROFIL (PRO_ID)
;

/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS_APPLI.CODE_APPLI
 **/
create index IDX_PROFIL_DROITS_APPLI_CODE_APPLI_FK on PROFIL_DROITS_APPLI (
	CODE_APPLI ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS_APPLI.CODE_APPLI
 **/
alter table PROFIL_DROITS_APPLI
	add constraint FK_PROFIL_DROITS_APPLI_CODE_APPLI foreign key (CODE_APPLI)
		references DROITS (CODE)
;

/**
  * Création de l'index de clef étrangère pour PROFIL.CODE
 **/
create index IDX_PRO_CODE_FK on PROFIL (
	CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL.CODE
 **/
alter table PROFIL
	add constraint FK_PRO_CODE foreign key (CODE)
		references TYPE_PROFIL (CODE)
;

/**
  * Création de l'index de clef étrangère pour SECTEUR.PRO_ID
 **/
create index IDX_SEC_PRO_ID_FK on SECTEUR (
	PRO_ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour SECTEUR.PRO_ID
 **/
alter table SECTEUR
	add constraint FK_SEC_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.PRO_ID
 **/
create index IDX_UTI_PRO_ID_FK on UTILISATEUR (
	PRO_ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.PRO_ID
 **/
alter table UTILISATEUR
	add constraint FK_UTI_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.TUT_CODE
 **/
create index IDX_UTI_TUT_CODE_FK on UTILISATEUR (
	TUT_CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.TUT_CODE
 **/
alter table UTILISATEUR
	add constraint FK_UTI_TUT_CODE foreign key (TUT_CODE)
		references TYPE_UTILISATEUR (TUT_CODE)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.UTI_ID_PARENT
 **/
create index IDX_UTI_UTI_ID_PARENT_FK on UTILISATEUR (
	UTI_ID_PARENT ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.UTI_ID_PARENT
 **/
alter table UTILISATEUR
	add constraint FK_UTI_UTI_ID_PARENT foreign key (UTI_ID_PARENT)
		references UTILISATEUR (UTI_ID)
;

