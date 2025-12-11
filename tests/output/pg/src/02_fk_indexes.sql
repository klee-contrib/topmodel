----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	02_fk_indexes.sql
--   Description		:	Script de création des indexes et des clef étrangères.
-- ===========================================================================================

/**
  * Création de l'index de clef étrangère pour AVIS_CLIENT.CLI_ID_CLIENT
 **/
create index IDX_AVI_CLI_ID_CLIENT_FK on AVIS_CLIENT (
	CLI_ID_CLIENT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour AVIS_CLIENT.CLI_ID_CLIENT
 **/
alter table AVIS_CLIENT
	add constraint FK_AVIS_CLIENT_CLI_ID_CLIENT foreign key (CLI_ID_CLIENT)
		references CLIENT (CLI_ID);

/**
  * Création de l'index de clef étrangère pour AVIS_CLIENT.RES_ID_RESTAURANT
 **/
create index IDX_AVI_RES_ID_RESTAURANT_FK on AVIS_CLIENT (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour AVIS_CLIENT.RES_ID_RESTAURANT
 **/
alter table AVIS_CLIENT
	add constraint FK_AVIS_CLIENT_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour COMMANDE.CLI_ID
 **/
create index IDX_COM_CLI_ID_FK on COMMANDE (
	CLI_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE.CLI_ID
 **/
alter table COMMANDE
	add constraint FK_COMMANDE_CLI_ID foreign key (CLI_ID)
		references CLIENT (CLI_ID);

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
		references TABLE_CLIENT (TAB_ID);

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
  * Création de l'index de clef étrangère pour COMMANDE_EXPORT.CLI_ID
 **/
create index IDX_COMMANDE_EXPORT_CLI_ID_FK on COMMANDE_EXPORT (
	CLI_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour COMMANDE_EXPORT.CLI_ID
 **/
alter table COMMANDE_EXPORT
	add constraint FK_COMMANDE_EXPORT_CLI_ID foreign key (CLI_ID)
		references CLIENT (CLI_ID);

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
		references TABLE_CLIENT (TAB_ID);

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
  * Création de l'index de clef étrangère pour EMPLOYE.RES_ID_RESTAURANT
 **/
create index IDX_EMP_RES_ID_RESTAURANT_FK on EMPLOYE (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour EMPLOYE.RES_ID_RESTAURANT
 **/
alter table EMPLOYE
	add constraint FK_EMPLOYE_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
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
  * Création de l'index de clef étrangère pour MENU.RES_ID_RESTAURANT
 **/
create index IDX_MEN_RES_ID_RESTAURANT_FK on MENU (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour MENU.RES_ID_RESTAURANT
 **/
alter table MENU
	add constraint FK_MENU_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour MENU_PLAT.MEN_ID_MENU
 **/
create index IDX_MPL_MEN_ID_MENU_FK on MENU_PLAT (
	MEN_ID_MENU ASC
);

/**
  * Génération de la contrainte de clef étrangère pour MENU_PLAT.MEN_ID_MENU
 **/
alter table MENU_PLAT
	add constraint FK_MENU_PLAT_MEN_ID_MENU foreign key (MEN_ID_MENU)
		references MENU (MEN_ID);

/**
  * Création de l'index de clef étrangère pour MENU_PLAT.PLA_ID_PLAT
 **/
create index IDX_MPL_PLA_ID_PLAT_FK on MENU_PLAT (
	PLA_ID_PLAT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour MENU_PLAT.PLA_ID_PLAT
 **/
alter table MENU_PLAT
	add constraint FK_MENU_PLAT_PLA_ID_PLAT foreign key (PLA_ID_PLAT)
		references PLAT (PLA_ID);

/**
  * Création de l'index de clef étrangère pour PLAT.CAT_CODE_CATEGORIE_PLAT
 **/
create index IDX_PLA_CAT_CODE_CATEGORIE_PLAT_FK on PLAT (
	CAT_CODE_CATEGORIE_PLAT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PLAT.CAT_CODE_CATEGORIE_PLAT
 **/
alter table PLAT
	add constraint FK_PLAT_CAT_CODE_CATEGORIE_PLAT foreign key (CAT_CODE_CATEGORIE_PLAT)
		references CATEGORIE_PLAT (CAT_CODE);

/**
  * Création de l'index de clef étrangère pour PLAT.RES_ID_RESTAURANT
 **/
create index IDX_PLA_RES_ID_RESTAURANT_FK on PLAT (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PLAT.RES_ID_RESTAURANT
 **/
alter table PLAT
	add constraint FK_PLAT_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour PROMOTION.RES_ID_RESTAURANT
 **/
create index IDX_PRO_RES_ID_RESTAURANT_FK on PROMOTION (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PROMOTION.RES_ID_RESTAURANT
 **/
alter table PROMOTION
	add constraint FK_PROMOTION_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour PROMOTION_PLAT.PRO_ID_PROMOTION
 **/
create index IDX_PPL_PRO_ID_PROMOTION_FK on PROMOTION_PLAT (
	PRO_ID_PROMOTION ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PROMOTION_PLAT.PRO_ID_PROMOTION
 **/
alter table PROMOTION_PLAT
	add constraint FK_PROMOTION_PLAT_PRO_ID_PROMOTION foreign key (PRO_ID_PROMOTION)
		references PROMOTION (PRO_ID);

/**
  * Création de l'index de clef étrangère pour PROMOTION_PLAT.PLA_ID_PLAT
 **/
create index IDX_PPL_PLA_ID_PLAT_FK on PROMOTION_PLAT (
	PLA_ID_PLAT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PROMOTION_PLAT.PLA_ID_PLAT
 **/
alter table PROMOTION_PLAT
	add constraint FK_PROMOTION_PLAT_PLA_ID_PLAT foreign key (PLA_ID_PLAT)
		references PLAT (PLA_ID);

/**
  * Création de l'index de clef étrangère pour RESERVATION.CLI_ID_CLIENT
 **/
create index IDX_REV_CLI_ID_CLIENT_FK on RESERVATION (
	CLI_ID_CLIENT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour RESERVATION.CLI_ID_CLIENT
 **/
alter table RESERVATION
	add constraint FK_RESERVATION_CLI_ID_CLIENT foreign key (CLI_ID_CLIENT)
		references CLIENT (CLI_ID);

/**
  * Création de l'index de clef étrangère pour RESERVATION.TAB_ID_TABLE
 **/
create index IDX_REV_TAB_ID_TABLE_FK on RESERVATION (
	TAB_ID_TABLE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour RESERVATION.TAB_ID_TABLE
 **/
alter table RESERVATION
	add constraint FK_RESERVATION_TAB_ID_TABLE foreign key (TAB_ID_TABLE)
		references TABLE_CLIENT (TAB_ID);

/**
  * Création de l'index de clef étrangère pour RESERVATION.RES_ID_RESTAURANT
 **/
create index IDX_REV_RES_ID_RESTAURANT_FK on RESERVATION (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour RESERVATION.RES_ID_RESTAURANT
 **/
alter table RESERVATION
	add constraint FK_RESERVATION_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index de clef étrangère pour TABLE_CLIENT.RES_ID_RESTAURANT
 **/
create index IDX_TAB_RES_ID_RESTAURANT_FK on TABLE_CLIENT (
	RES_ID_RESTAURANT ASC
);

/**
  * Génération de la contrainte de clef étrangère pour TABLE_CLIENT.RES_ID_RESTAURANT
 **/
alter table TABLE_CLIENT
	add constraint FK_TABLE_CLIENT_RES_ID_RESTAURANT foreign key (RES_ID_RESTAURANT)
		references RESTAURANT (RES_ID);
