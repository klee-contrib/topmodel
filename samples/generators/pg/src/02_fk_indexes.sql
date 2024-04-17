----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères. 
-- =========================================================================================== 
/**
  * Création de l'index de clef étrangère pour DROIT.TDR_CODE
 **/
create index IDX_DRO_TDR_CODE_FK on DROIT (
	TDR_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour DROIT.TDR_CODE
 **/
alter table DROIT
	add constraint FK_DROIT_TDR_CODE foreign key (TDR_CODE)
		references TYPE_DROIT (TDR_CODE);

/**
  * Création de l'index de clef étrangère pour PROFIL_DROIT.PRO_ID
 **/
create index IDX_PROFIL_DROIT_PRO_ID_FK on PROFIL_DROIT (
	PRO_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROIT.PRO_ID
 **/
alter table PROFIL_DROIT
	add constraint FK_PROFIL_DROIT_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID);

/**
  * Création de l'index de clef étrangère pour PROFIL_DROIT.DRO_CODE
 **/
create index IDX_PROFIL_DROIT_DRO_CODE_FK on PROFIL_DROIT (
	DRO_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PROFIL_DROIT.DRO_CODE
 **/
alter table PROFIL_DROIT
	add constraint FK_PROFIL_DROIT_DRO_CODE foreign key (DRO_CODE)
		references DROIT (DRO_CODE);

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.PRO_ID
 **/
create index IDX_UTI_PRO_ID_FK on UTILISATEUR (
	PRO_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.PRO_ID
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_PRO_ID foreign key (PRO_ID)
		references PROFIL (PRO_ID);

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.TUT_CODE
 **/
create index IDX_UTI_TUT_CODE_FK on UTILISATEUR (
	TUT_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.TUT_CODE
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_TUT_CODE foreign key (TUT_CODE)
		references TYPE_UTILISATEUR (TUT_CODE);

/**
  * Création de l'index de clef étrangère pour DROIT.DRO_LIBELLE
 **/
create index IDX_DRO_DRO_LIBELLE_FK on DROIT (
	DRO_LIBELLE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour DROIT.DRO_LIBELLE
 **/
alter table DROIT
	add constraint FK_DROIT_DRO_LIBELLE foreign key (DRO_LIBELLE)
		references TRANSLATION (RESOURCE_KEY);

/**
  * Création de l'index de clef étrangère pour TYPE_DROIT.TDR_LIBELLE
 **/
create index IDX_TDR_TDR_LIBELLE_FK on TYPE_DROIT (
	TDR_LIBELLE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour TYPE_DROIT.TDR_LIBELLE
 **/
alter table TYPE_DROIT
	add constraint FK_TYPE_DROIT_TDR_LIBELLE foreign key (TDR_LIBELLE)
		references TRANSLATION (RESOURCE_KEY);

/**
  * Création de l'index de clef étrangère pour TYPE_UTILISATEUR.TUT_LIBELLE
 **/
create index IDX_TUT_TUT_LIBELLE_FK on TYPE_UTILISATEUR (
	TUT_LIBELLE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour TYPE_UTILISATEUR.TUT_LIBELLE
 **/
alter table TYPE_UTILISATEUR
	add constraint FK_TYPE_UTILISATEUR_TUT_LIBELLE foreign key (TUT_LIBELLE)
		references TRANSLATION (RESOURCE_KEY);

