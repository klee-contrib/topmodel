----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Insertion des valeurs de la table STATUT_COMMANDE.
-- ===========================================================================================

INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES(N'EN_ATT', N'restaurant.statutCommande.values.EnAttente');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES(N'EN_PREP', N'restaurant.statutCommande.values.EnPreparation');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES(N'PRETE', N'restaurant.statutCommande.values.Prete');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES(N'SERVIE', N'restaurant.statutCommande.values.Servie');
INSERT INTO STATUT_COMMANDE(STC_CODE, STC_LIBELLE) VALUES(N'ANNULE', N'restaurant.statutCommande.values.Annulee');
GO
