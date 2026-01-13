----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères.
-- ===========================================================================================

/**
  * Création de l'index de clef étrangère pour AVIS_CLIENT.PER_ID
 **/
create index IDX_AVI_PER_ID_FK on AVIS_CLIENT (
	PER_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour AVIS_CLIENT.PER_ID
 **/
alter table AVIS_CLIENT
	add constraint FK_AVIS_CLIENT_PER_ID foreign key (PER_ID)
		references CLIENT (PER_ID);

/**
  * Création de l'index de clef étrangère pour AVIS_CLIENT.RES_ID
 **/
create index IDX_AVI_RES_ID_FK on AVIS_CLIENT (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour AVIS_CLIENT.RES_ID
 **/
alter table AVIS_CLIENT
	add constraint FK_AVIS_CLIENT_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour CLIENT.PER_ID
 **/
create index IDX_CLI_PER_ID_FK on CLIENT (
	PER_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour CLIENT.PER_ID
 **/
alter table CLIENT
	add constraint FK_CLIENT_PER_ID foreign key (PER_ID)
		references PERSONNE (PER_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE.PER_ID
 **/
create index IDX_COM_PER_ID_FK on COMMANDE (
	PER_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE.PER_ID
 **/
alter table COMMANDE
	add constraint FK_COMMANDE_PER_ID foreign key (PER_ID)
		references CLIENT (PER_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE.TAB_ID
 **/
create index IDX_COM_TAB_ID_FK on COMMANDE (
	TAB_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE.TAB_ID
 **/
alter table COMMANDE
	add constraint FK_COMMANDE_TAB_ID foreign key (TAB_ID)
		references TABLE (TAB_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE.REV_ID
 **/
create index IDX_COM_REV_ID_FK on COMMANDE (
	REV_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE.REV_ID
 **/
alter table COMMANDE
	add constraint FK_COMMANDE_REV_ID foreign key (REV_ID)
		references RESERVATION (REV_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE.STC_CODE
 **/
create index IDX_COM_STC_CODE_FK on COMMANDE (
	STC_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE.STC_CODE
 **/
alter table COMMANDE
	add constraint FK_COMMANDE_STC_CODE foreign key (STC_CODE)
		references STATUT_COMMANDE (STC_CODE);

/**
  * Création de l'index de clef étrangère pour COMMANDE.AVI_ID
 **/
create index IDX_COM_AVI_ID_FK on COMMANDE (
	AVI_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE.AVI_ID
 **/
alter table COMMANDE
	add constraint FK_COMMANDE_AVI_ID foreign key (AVI_ID)
		references AVIS_CLIENT (AVI_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE_EXPORT.PER_ID
 **/
create index IDX_COMMANDE_EXPORT_PER_ID_FK on COMMANDE_EXPORT (
	PER_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE_EXPORT.PER_ID
 **/
alter table COMMANDE_EXPORT
	add constraint FK_COMMANDE_EXPORT_PER_ID foreign key (PER_ID)
		references CLIENT (PER_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE_EXPORT.TAB_ID
 **/
create index IDX_COMMANDE_EXPORT_TAB_ID_FK on COMMANDE_EXPORT (
	TAB_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE_EXPORT.TAB_ID
 **/
alter table COMMANDE_EXPORT
	add constraint FK_COMMANDE_EXPORT_TAB_ID foreign key (TAB_ID)
		references TABLE (TAB_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE_EXPORT.REV_ID
 **/
create index IDX_COMMANDE_EXPORT_REV_ID_FK on COMMANDE_EXPORT (
	REV_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE_EXPORT.REV_ID
 **/
alter table COMMANDE_EXPORT
	add constraint FK_COMMANDE_EXPORT_REV_ID foreign key (REV_ID)
		references RESERVATION (REV_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE_EXPORT.STC_CODE
 **/
create index IDX_COMMANDE_EXPORT_STC_CODE_FK on COMMANDE_EXPORT (
	STC_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE_EXPORT.STC_CODE
 **/
alter table COMMANDE_EXPORT
	add constraint FK_COMMANDE_EXPORT_STC_CODE foreign key (STC_CODE)
		references STATUT_COMMANDE (STC_CODE);

/**
  * Création de l'index de clef étrangère pour COMMANDE_EXPORT.AVI_ID
 **/
create index IDX_COMMANDE_EXPORT_AVI_ID_FK on COMMANDE_EXPORT (
	AVI_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE_EXPORT.AVI_ID
 **/
alter table COMMANDE_EXPORT
	add constraint FK_COMMANDE_EXPORT_AVI_ID foreign key (AVI_ID)
		references AVIS_CLIENT (AVI_ID);

/**
  * Création de l'index de clef étrangère pour EMPLOYE.RES_ID
 **/
create index IDX_EMP_RES_ID_FK on EMPLOYE (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour EMPLOYE.RES_ID
 **/
alter table EMPLOYE
	add constraint FK_EMPLOYE_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour EMPLOYE.PER_ID
 **/
create index IDX_EMP_PER_ID_FK on EMPLOYE (
	PER_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour EMPLOYE.PER_ID
 **/
alter table EMPLOYE
	add constraint FK_EMPLOYE_PER_ID foreign key (PER_ID)
		references PERSONNE (PER_ID);

/**
  * Création de l'index de clef étrangère pour LIGNE_COMMANDE.COM_ID
 **/
create index IDX_LIG_COM_ID_FK on LIGNE_COMMANDE (
	COM_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour LIGNE_COMMANDE.COM_ID
 **/
alter table LIGNE_COMMANDE
	add constraint FK_LIGNE_COMMANDE_COM_ID foreign key (COM_ID)
		references COMMANDE (COM_ID);

/**
  * Création de l'index de clef étrangère pour LIGNE_COMMANDE.PLA_ID
 **/
create index IDX_LIG_PLA_ID_FK on LIGNE_COMMANDE (
	PLA_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour LIGNE_COMMANDE.PLA_ID
 **/
alter table LIGNE_COMMANDE
	add constraint FK_LIGNE_COMMANDE_PLA_ID foreign key (PLA_ID)
		references PLAT (PLA_ID);

/**
  * Création de l'index de clef étrangère pour MENU.RES_ID
 **/
create index IDX_MEN_RES_ID_FK on MENU (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour MENU.RES_ID
 **/
alter table MENU
	add constraint FK_MENU_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour MENU_PLAT.MEN_ID
 **/
create index IDX_MPL_MEN_ID_FK on MENU_PLAT (
	MEN_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour MENU_PLAT.MEN_ID
 **/
alter table MENU_PLAT
	add constraint FK_MENU_PLAT_MEN_ID foreign key (MEN_ID)
		references MENU (MEN_ID);

/**
  * Création de l'index de clef étrangère pour MENU_PLAT.PLA_ID
 **/
create index IDX_MPL_PLA_ID_FK on MENU_PLAT (
	PLA_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour MENU_PLAT.PLA_ID
 **/
alter table MENU_PLAT
	add constraint FK_MENU_PLAT_PLA_ID foreign key (PLA_ID)
		references PLAT (PLA_ID);

/**
  * Création de l'index de clef étrangère pour PLAT.CAT_CODE
 **/
create index IDX_PLA_CAT_CODE_FK on PLAT (
	CAT_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PLAT.CAT_CODE
 **/
alter table PLAT
	add constraint FK_PLAT_CAT_CODE foreign key (CAT_CODE)
		references CATEGORIE_PLAT (CAT_CODE);

/**
  * Création de l'index de clef étrangère pour PLAT.RES_ID
 **/
create index IDX_PLA_RES_ID_FK on PLAT (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PLAT.RES_ID
 **/
alter table PLAT
	add constraint FK_PLAT_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Génération de la contrainte de clef étrangère pour PROMOTION.PLA_ID
 **/
alter table PROMOTION
	add constraint FK_PROMOTION_PLA_ID foreign key (PLA_ID)
		references PLAT (PLA_ID);

/**
  * Création de l'index de clef étrangère pour PROMOTION.RES_ID
 **/
create index IDX_PRO_RES_ID_FK on PROMOTION (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PROMOTION.RES_ID
 **/
alter table PROMOTION
	add constraint FK_PROMOTION_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour RESERVATION.PER_ID
 **/
create index IDX_REV_PER_ID_FK on RESERVATION (
	PER_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour RESERVATION.PER_ID
 **/
alter table RESERVATION
	add constraint FK_RESERVATION_PER_ID foreign key (PER_ID)
		references CLIENT (PER_ID);

/**
  * Création de l'index de clef étrangère pour RESERVATION.TAB_ID
 **/
create index IDX_REV_TAB_ID_FK on RESERVATION (
	TAB_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour RESERVATION.TAB_ID
 **/
alter table RESERVATION
	add constraint FK_RESERVATION_TAB_ID foreign key (TAB_ID)
		references TABLE (TAB_ID);

/**
  * Création de l'index de clef étrangère pour RESERVATION.RES_ID
 **/
create index IDX_REV_RES_ID_FK on RESERVATION (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour RESERVATION.RES_ID
 **/
alter table RESERVATION
	add constraint FK_RESERVATION_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour TABLE.RES_ID
 **/
create index IDX_TAB_RES_ID_FK on TABLE (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour TABLE.RES_ID
 **/
alter table TABLE
	add constraint FK_TABLE_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);
