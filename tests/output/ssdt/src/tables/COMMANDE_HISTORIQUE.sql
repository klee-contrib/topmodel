----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table COMMANDE_HISTORIQUE.
-- ===========================================================================================

create table [dbo].[COMMANDE_HISTORIQUE] (
	[COM_ID] int,
	[COM_DATE_COMMANDE] timestamp not null,
	[COM_DATE_LIVRAISON] timestamp,
	[COM_MONTANT_TOTAL] decimal not null,
	[PER_ID] int not null,
	[TAB_ID] int,
	[REV_ID] int,
	[STC_CODE] varchar not null default N'EN_ATT',
	[AVI_ID] int,
	constraint [PK_COMMANDE_HISTORIQUE] primary key clustered ([COM_ID] ASC),
	constraint [FK_COMMANDE_HISTORIQUE_PER_ID] foreign key ([PER_ID]) references [dbo].[CLIENT] ([PER_ID]),
	constraint [FK_COMMANDE_HISTORIQUE_TAB_ID] foreign key ([TAB_ID]) references [dbo].[TABLE] ([TAB_ID]),
	constraint [FK_COMMANDE_HISTORIQUE_REV_ID] foreign key ([REV_ID]) references [dbo].[RESERVATION] ([REV_ID]),
	constraint [FK_COMMANDE_HISTORIQUE_STC_CODE] foreign key ([STC_CODE]) references [dbo].[STATUT_COMMANDE] ([STC_CODE]),
	constraint [FK_COMMANDE_HISTORIQUE_AVI_ID] foreign key ([AVI_ID]) references [dbo].[AVIS_CLIENT] ([AVI_ID]),
	constraint [UK_COMMANDE_HISTORIQUE_AVI_ID] unique nonclustered ([AVI_ID] ASC))
go

/* Index on foreign key column for COMMANDE_HISTORIQUE.PER_ID */
create nonclustered index [IDX_COMMANDE_HISTORIQUE_PER_ID_FK]
	on [dbo].[COMMANDE_HISTORIQUE] ([PER_ID] ASC)
go

/* Index on foreign key column for COMMANDE_HISTORIQUE.TAB_ID */
create nonclustered index [IDX_COMMANDE_HISTORIQUE_TAB_ID_FK]
	on [dbo].[COMMANDE_HISTORIQUE] ([TAB_ID] ASC)
go

/* Index on foreign key column for COMMANDE_HISTORIQUE.REV_ID */
create nonclustered index [IDX_COMMANDE_HISTORIQUE_REV_ID_FK]
	on [dbo].[COMMANDE_HISTORIQUE] ([REV_ID] ASC)
go

/* Index on foreign key column for COMMANDE_HISTORIQUE.STC_CODE */
create nonclustered index [IDX_COMMANDE_HISTORIQUE_STC_CODE_FK]
	on [dbo].[COMMANDE_HISTORIQUE] ([STC_CODE] ASC)
go

/* Index on foreign key column for COMMANDE_HISTORIQUE.AVI_ID */
create nonclustered index [IDX_COMMANDE_HISTORIQUE_AVI_ID_FK]
	on [dbo].[COMMANDE_HISTORIQUE] ([AVI_ID] ASC)
go

/**
  * Commentaires pour la table COMMANDE_HISTORIQUE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Commande pour historique avec préservation des clés primaires', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_DATE_COMMANDE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de livraison', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_DATE_LIVRAISON'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Montant total de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_MONTANT_TOTAL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Client ayant passé la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'PER_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Table associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Réservation associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'REV_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Statut de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'STC_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Avis laissé par le client sur la commande.', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'AVI_ID'
go
