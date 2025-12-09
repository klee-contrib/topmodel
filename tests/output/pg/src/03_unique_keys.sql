----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	03_unique_keys.sql
--   Description		:	Script de création des contraintes d'unicité.
-- ===========================================================================================

alter table AVIS_CLIENT add constraint UK_AVIS_CLIENT_CLI_ID_CLIENT_RES_ID_RESTAURANT_AVI_DATE_AVIS unique (CLI_ID_CLIENT, RES_ID_RESTAURANT, AVI_DATE_AVIS);

alter table EMPLOYE add constraint UK_EMPLOYE_EMP_MATRICULE unique (EMP_MATRICULE);

alter table LIGNE_COMMANDE add constraint UK_LIGNE_COMMANDE_COM_ID_PLA_ID unique (COM_ID, PLA_ID);

alter table MENU_PLAT add constraint UK_MENU_PLAT_MEN_ID_MENU_MPL_ORDRE unique (MEN_ID_MENU, MPL_ORDRE);

alter table MENU_PLAT add constraint UK_MENU_PLAT_MEN_ID_MENU_PLA_ID_PLAT unique (MEN_ID_MENU, PLA_ID_PLAT);

alter table PROMOTION_PLAT add constraint UK_PROMOTION_PLAT_PRO_ID_PROMOTION_PLA_ID_PLAT unique (PRO_ID_PROMOTION, PLA_ID_PLAT);

alter table RESERVATION add constraint UK_RESERVATION_TAB_ID_TABLE_REV_DATE_RESERVATION unique (TAB_ID_TABLE, REV_DATE_RESERVATION);

alter table TABLE_CLIENT add constraint UK_TABLE_CLIENT_RES_ID_RESTAURANT_TAB_NUMERO unique (RES_ID_RESTAURANT, TAB_NUMERO);
