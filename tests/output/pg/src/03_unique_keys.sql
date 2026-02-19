----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	03_unique_keys.sql
--   Description		:	Script de création des contraintes d'unicité.
-- ===========================================================================================

alter table AVIS_CLIENT add constraint UK_AVIS_CLIENT_PER_ID_RES_ID_AVI_DATE_AVIS unique (PER_ID, RES_ID, AVI_DATE_AVIS);

alter table CATEGORIE_PLAT add constraint UK_CATEGORIE_PLAT_CAT_ORDRE unique (CAT_ORDRE);

alter table COMMANDE add constraint UK_COMMANDE_AVI_ID unique (AVI_ID);

alter table COMMANDE_HISTORIQUE add constraint UK_COMMANDE_HISTORIQUE_AVI_ID unique (AVI_ID);

alter table EMPLOYE add constraint UK_EMPLOYE_EMP_MATRICULE unique (EMP_MATRICULE);

alter table LIGNE_COMMANDE add constraint UK_LIGNE_COMMANDE_COM_ID_PLA_ID unique (COM_ID, PLA_ID);

alter table MENU_PLAT add constraint UK_MENU_PLAT_MEN_ID_MPL_ORDRE unique (MEN_ID, MPL_ORDRE);

alter table RESERVATION add constraint UK_RESERVATION_TAB_ID_REV_DATE_RESERVATION unique (TAB_ID, REV_DATE_RESERVATION);

alter table TABLE add constraint UK_TABLE_RES_ID_TAB_NUMERO unique (RES_ID, TAB_NUMERO);
