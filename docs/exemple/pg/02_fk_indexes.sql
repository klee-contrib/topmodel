----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	Exemple 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères. 
-- =========================================================================================== 
/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS.PRO_ID
 **/
create index IDX_PROFIL_DROITS_PRO_ID_FK on PROFIL_DROITS (
	PRO_ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS.PRO_ID
 **/
alter table PROFIL_DROITS
	add constraint FK_PROFIL_DROITS_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID)
;

/**
  * Création de l'index de clef étrangère pour PROFIL_DROITS.DRO_CODE
 **/
create index IDX_PROFIL_DROITS_DRO_CODE_FK on PROFIL_DROITS (
	DRO_CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROITS.DRO_CODE
 **/
alter table PROFIL_DROITS
	add constraint FK_PROFIL_DROITS_DRO_CODE foreign key (DRO_CODE)
		references DROITS (DRO_CODE)
;

/**
  * Création de l'index de clef étrangère pour DROITS.TPR_CODE
 **/
create index IDX_DRO_TPR_CODE_FK on DROITS (
	TPR_CODE ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour DROITS.TPR_CODE
 **/
alter table DROITS
	add constraint FK_DRO_TPR_CODE foreign key (TPR_CODE)
		references TYPE_PROFIL (TPR_CODE)
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
  * Création de l'index de clef étrangère pour TYPE_PROFIL.PRO_ID
 **/
create index IDX_TPR_PRO_ID_FK on TYPE_PROFIL (
	PRO_ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour TYPE_PROFIL.PRO_ID
 **/
alter table TYPE_PROFIL
	add constraint FK_TPR_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID)
;

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.PRO_ID
 **/
create index IDX_UTILISATEUR_PRO_ID_FK on UTILISATEUR (
	PRO_ID ASC
)
;

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.PRO_ID
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID)
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

