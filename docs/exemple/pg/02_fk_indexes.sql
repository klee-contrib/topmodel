----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères. 
-- =========================================================================================== 
/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS_APPLI.ID_APPLI
 **/
create index IDX_PROFIL_DROITS_APPLI_ID_APPLI_FK on PROFIL_DROITS_APPLI (
	ID_APPLI ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS_APPLI.ID_APPLI
 **/
alter table PROFIL_DROITS_APPLI
	add constraint FK_PROFIL_DROITS_APPLI_ID_APPLI foreign key (ID_APPLI)
		references PROFIL (ID)
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
create index IDX_PROFIL_CODE_FK on PROFIL (
	CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL.CODE
 **/
alter table PROFIL
	add constraint FK_PROFIL_CODE foreign key (CODE)
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
  * Création de l'index de clef étrangère pour UTILISATEUR.ID
 **/
create index IDX_UTILISATEUR_ID_FK on UTILISATEUR (
	ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.ID
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_ID foreign key (ID)
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
  * Création de l'index de clef étrangère pour UTILISATEUR.ID_PARENT
 **/
create index IDX_UTILISATEUR_ID_PARENT_FK on UTILISATEUR (
	ID_PARENT ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.ID_PARENT
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_ID_PARENT foreign key (ID_PARENT)
		references UTILISATEUR (ID)
;

