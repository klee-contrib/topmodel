----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères. 
-- =========================================================================================== 
/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS_APPLI.PROFIL_ID_APPLI
 **/
create index IDX_PROFIL_DROITS_APPLI_PROFIL_ID_APPLI_FK on PROFIL_DROITS_APPLI (
	PROFIL_ID_APPLI ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS_APPLI.PROFIL_ID_APPLI
 **/
alter table PROFIL_DROITS_APPLI
	add constraint FK_PROFIL_DROITS_APPLI_PROFIL_ID_APPLI foreign key (PROFIL_ID_APPLI)
		references PROFIL (ID)
;

/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS_APPLI.DROITS_CODE_APPLI
 **/
create index IDX_PROFIL_DROITS_APPLI_DROITS_CODE_APPLI_FK on PROFIL_DROITS_APPLI (
	DROITS_CODE_APPLI ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS_APPLI.DROITS_CODE_APPLI
 **/
alter table PROFIL_DROITS_APPLI
	add constraint FK_PROFIL_DROITS_APPLI_DROITS_CODE_APPLI foreign key (DROITS_CODE_APPLI)
		references DROITS (CODE)
;

/**
  * Création de l'index de clef étrangère pour PROFIL.TYPE_PROFIL_CODE
 **/
create index IDX_PROFIL_TYPE_PROFIL_CODE_FK on PROFIL (
	TYPE_PROFIL_CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL.TYPE_PROFIL_CODE
 **/
alter table PROFIL
	add constraint FK_PROFIL_TYPE_PROFIL_CODE foreign key (TYPE_PROFIL_CODE)
		references TYPE_PROFIL (CODE)
;

/**
  * Création de l'index de clef étrangère pour SECTEUR.ID
 **/
create index IDX_SEC_ID_FK on SECTEUR (
	ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour SECTEUR.ID
 **/
alter table SECTEUR
	add constraint FK_SEC_ID foreign key (ID)
		references PROFIL (ID)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.PROFIL_ID
 **/
create index IDX_UTILISATEUR_PROFIL_ID_FK on UTILISATEUR (
	PROFIL_ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.PROFIL_ID
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_PROFIL_ID foreign key (PROFIL_ID)
		references PROFIL (ID)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.TUT_CODE
 **/
create index IDX_UTILISATEUR_TUT_CODE_FK on UTILISATEUR (
	TUT_CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.TUT_CODE
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_TUT_CODE foreign key (TUT_CODE)
		references TYPE_UTILISATEUR (TUT_CODE)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.UTILISATEUR_ID_PARENT
 **/
create index IDX_UTILISATEUR_UTILISATEUR_ID_PARENT_FK on UTILISATEUR (
	UTILISATEUR_ID_PARENT ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.UTILISATEUR_ID_PARENT
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_UTILISATEUR_ID_PARENT foreign key (UTILISATEUR_ID_PARENT)
		references UTILISATEUR (ID)
;

