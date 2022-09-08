----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	indexFk.sql
--   Description		:	Script de création des indexes et des clef étrangères. 
-- =========================================================================================== 
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

