----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table LIGNE_COMMANDE_HISTORIQUE.
-- ===========================================================================================

create table [dbo].[LIGNE_COMMANDE_HISTORIQUE] (
	[LIG_ID] int,
	[LIG_QUANTITE] int not null,
	[LIG_PRIX_UNITAIRE] decimal not null,
	[LIG_PRIX_TOTAL] decimal not null,
	[PLA_ID] int not null,
	[LIG_DATE_CREATION] timestamp not null,
	[COM_ID] int not null,
	constraint [PK_LIGNE_COMMANDE_HISTORIQUE] primary key clustered ([LIG_ID] ASC),
	constraint [FK_LIGNE_COMMANDE_HISTORIQUE_COM_ID] foreign key ([COM_ID]) references [dbo].[COMMANDE_HISTORIQUE] ([COM_ID]))
go

/* Index on foreign key column for LIGNE_COMMANDE_HISTORIQUE.COM_ID */
create nonclustered index [IDX_LIGNE_COMMANDE_HISTORIQUE_COM_ID_FK]
	on [dbo].[LIGNE_COMMANDE_HISTORIQUE] ([COM_ID] ASC)
go

/**
  * Commentaires pour la table LIGNE_COMMANDE_HISTORIQUE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Ligne de commande pour historique avec préservation des clés primaires', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la ligne', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'LIG_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Quantité commandée', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'LIG_QUANTITE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix unitaire au moment de la commande', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'LIG_PRIX_UNITAIRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix total de la ligne', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'LIG_PRIX_TOTAL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat commandé', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'PLA_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'LIG_DATE_CREATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Commande à laquelle appartient la ligne', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE_HISTORIQUE', 'COLUMN', 'COM_ID'
go
