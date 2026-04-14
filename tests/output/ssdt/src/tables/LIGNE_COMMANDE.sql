----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table LIGNE_COMMANDE.
-- ===========================================================================================

create table [dbo].[LIGNE_COMMANDE] (
	[LIG_ID] int identity,
	[LIG_QUANTITE] int not null,
	[LIG_PRIX_UNITAIRE] decimal not null,
	[LIG_PRIX_TOTAL] decimal not null,
	[COM_ID] int not null,
	[PLA_ID] int not null,
	constraint [PK_LIGNE_COMMANDE] primary key clustered ([LIG_ID] ASC),
	constraint [FK_LIGNE_COMMANDE_COM_ID] foreign key ([COM_ID]) references [dbo].[COMMANDE] ([COM_ID]),
	constraint [FK_LIGNE_COMMANDE_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]),
	constraint [UK_LIGNE_COMMANDE_COM_ID_PLA_ID] unique nonclustered ([COM_ID] ASC, [PLA_ID] ASC))
go

/* Index on foreign key column for LIGNE_COMMANDE.COM_ID */
create nonclustered index [IDX_LIG_COM_ID_FK]
	on [dbo].[LIGNE_COMMANDE] ([COM_ID] ASC)
go

/* Index on foreign key column for LIGNE_COMMANDE.PLA_ID */
create nonclustered index [IDX_LIG_PLA_ID_FK]
	on [dbo].[LIGNE_COMMANDE] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table LIGNE_COMMANDE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Ligne d''une commande', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la ligne', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE', 'COLUMN', 'LIG_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Quantité commandée', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE', 'COLUMN', 'LIG_QUANTITE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix unitaire au moment de la commande', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE', 'COLUMN', 'LIG_PRIX_UNITAIRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix total de la ligne', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE', 'COLUMN', 'LIG_PRIX_TOTAL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Commande à laquelle appartient la ligne', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE', 'COLUMN', 'COM_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat commandé', 'SCHEMA', 'dbo', 'TABLE', 'LIGNE_COMMANDE', 'COLUMN', 'PLA_ID'
go
