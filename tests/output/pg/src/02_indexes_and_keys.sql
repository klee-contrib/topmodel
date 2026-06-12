----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	02_indexes_and_keys.sql
--   Description		:	Script de création des indexes et des clés étrangères et uniques.
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
  * Création de l'index de clef étrangère pour CATEGORIE_PLAT_REGION.REG_CODE
 **/
create index IDX_CATEGORIE_PLAT_REGION_REG_CODE_FK on CATEGORIE_PLAT_REGION (
	REG_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour CATEGORIE_PLAT_REGION.REG_CODE
 **/
alter table CATEGORIE_PLAT_REGION
	add constraint FK_CATEGORIE_PLAT_REGION_REG_CODE foreign key (REG_CODE)
		references REGION (REG_CODE);

/**
  * Création de l'index de clef étrangère pour CATEGORIE_PLAT_REGION.CAT_CODE
 **/
create index IDX_CATEGORIE_PLAT_REGION_CAT_CODE_FK on CATEGORIE_PLAT_REGION (
	CAT_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour CATEGORIE_PLAT_REGION.CAT_CODE
 **/
alter table CATEGORIE_PLAT_REGION
	add constraint FK_CATEGORIE_PLAT_REGION_CAT_CODE foreign key (CAT_CODE)
		references CATEGORIE_PLAT (CAT_CODE);

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
		references TABLE_RESTAURANT (TAB_ID);

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
  * Création de l'index de clef étrangère pour DEPARTEMENT.REG_CODE
 **/
create index IDX_DEP_REG_CODE_FK on DEPARTEMENT (
	REG_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour DEPARTEMENT.REG_CODE
 **/
alter table DEPARTEMENT
	add constraint FK_DEPARTEMENT_REG_CODE foreign key (REG_CODE)
		references REGION (REG_CODE);

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
  * Création de l'index de clef étrangère pour LIGNE_COMMANDE_HISTORIQUE.COM_ID
 **/
create index IDX_LIGNE_COMMANDE_HISTORIQUE_COM_ID_FK on LIGNE_COMMANDE_HISTORIQUE (
	COM_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour LIGNE_COMMANDE_HISTORIQUE.COM_ID
 **/
alter table LIGNE_COMMANDE_HISTORIQUE
	add constraint FK_LIGNE_COMMANDE_HISTORIQUE_COM_ID foreign key (COM_ID)
		references COMMANDE_HISTORIQUE (COM_ID);

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
  * Création de l'index de clef étrangère pour PERSONNE.DEP_CODE
 **/
create index IDX_PER_DEP_CODE_FK on PERSONNE (
	DEP_CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour PERSONNE.DEP_CODE
 **/
alter table PERSONNE
	add constraint FK_PERSONNE_DEP_CODE foreign key (DEP_CODE)
		references DEPARTEMENT (DEP_CODE);

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
		references TABLE_RESTAURANT (TAB_ID);

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
  * Création de l'index de clef étrangère pour TABLE_RESTAURANT.RES_ID
 **/
create index IDX_TAB_RES_ID_FK on TABLE_RESTAURANT (
	RES_ID ASC
);

/**
  * Génération de la contrainte de clef étrangère pour TABLE_RESTAURANT.RES_ID
 **/
alter table TABLE_RESTAURANT
	add constraint FK_TABLE_RESTAURANT_RES_ID foreign key (RES_ID)
		references RESTAURANT (RES_ID);

/**
  * Création de l'index UK_AVIS_CLIENT_PER_ID_RES_ID_AVI_DATE_AVIS sur AVIS_CLIENT.
 **/
alter table AVIS_CLIENT add constraint UK_AVIS_CLIENT_PER_ID_RES_ID_AVI_DATE_AVIS unique (PER_ID, RES_ID, AVI_DATE_AVIS);

/**
  * Création de l'index UK_CATEGORIE_PLAT_CAT_ORDRE sur CATEGORIE_PLAT.
 **/
alter table CATEGORIE_PLAT add constraint UK_CATEGORIE_PLAT_CAT_ORDRE unique (CAT_ORDRE);

/**
  * Création de l'index UK_COMMANDE_AVI_ID sur COMMANDE.
 **/
alter table COMMANDE add constraint UK_COMMANDE_AVI_ID unique (AVI_ID);

/**
  * Création de l'index IDX_EMP_EMP_TELEPHONE sur EMPLOYE.
 **/
create index IDX_EMP_EMP_TELEPHONE on EMPLOYE (
	EMP_TELEPHONE ASC
);

/**
  * Création de l'index UK_EMPLOYE_EMP_MATRICULE sur EMPLOYE.
 **/
alter table EMPLOYE add constraint UK_EMPLOYE_EMP_MATRICULE unique (EMP_MATRICULE);

/**
  * Création de l'index UK_LIGNE_COMMANDE_COM_ID_PLA_ID sur LIGNE_COMMANDE.
 **/
alter table LIGNE_COMMANDE add constraint UK_LIGNE_COMMANDE_COM_ID_PLA_ID unique (COM_ID, PLA_ID);

/**
  * Création de l'index UK_MENU_PLAT_MEN_ID_MPL_ORDRE sur MENU_PLAT.
 **/
alter table MENU_PLAT add constraint UK_MENU_PLAT_MEN_ID_MPL_ORDRE unique (MEN_ID, MPL_ORDRE);

/**
  * Création de l'index IDX_PST_PST_NOM_PST_PRENOM sur PRESTATAIRE.
 **/
create index IDX_PST_PST_NOM_PST_PRENOM on PRESTATAIRE (
	PST_NOM ASC, PST_PRENOM ASC
);

/**
  * Création de l'index IDX_PST_PST_TELEPHONE sur PRESTATAIRE.
 **/
create index IDX_PST_PST_TELEPHONE on PRESTATAIRE (
	PST_TELEPHONE ASC
);

/**
  * Création de l'index UK_RESERVATION_TAB_ID_REV_DATE_RESERVATION sur RESERVATION.
 **/
alter table RESERVATION add constraint UK_RESERVATION_TAB_ID_REV_DATE_RESERVATION unique (TAB_ID, REV_DATE_RESERVATION);

/**
  * Création de l'index UK_TABLE_RESTAURANT_RES_ID_TAB_NUMERO sur TABLE_RESTAURANT.
 **/
alter table TABLE_RESTAURANT add constraint UK_TABLE_RESTAURANT_RES_ID_TAB_NUMERO unique (RES_ID, TAB_NUMERO);

/**
  * Création de l'index IDX_CAT_CAT_LIBELLE sur CATEGORIE_PLAT.
 **/
create index IDX_CAT_CAT_LIBELLE on CATEGORIE_PLAT (
	CAT_LIBELLE ASC
);

/**
  * Création de l'index IDX_DEP_DEP_LIBELLE sur DEPARTEMENT.
 **/
create index IDX_DEP_DEP_LIBELLE on DEPARTEMENT (
	DEP_LIBELLE ASC
);

/**
  * Création de l'index IDX_REG_REG_LIBELLE sur REGION.
 **/
create index IDX_REG_REG_LIBELLE on REGION (
	REG_LIBELLE ASC
);
