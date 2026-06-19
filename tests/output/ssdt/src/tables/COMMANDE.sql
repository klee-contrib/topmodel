----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table COMMANDE.
-- ===========================================================================================

create table [dbo].[COMMANDE] (
	[COM_ID] int identity,
	[COM_DATE_COMMANDE] timestamp not null,
	[COM_DATE_LIVRAISON] timestamp,
	[COM_MONTANT_TOTAL] decimal not null,
	[PER_ID] int not null,
	[TAB_ID] int,
	[REV_ID] int,
	[STC_CODE] varchar not null default N'EN_ATT',
	[AVI_ID] int,
	[COM_DATE_CREATION] timestamp not null,
	constraint [PK_COMMANDE] primary key clustered ([COM_ID] ASC),
	constraint [FK_COMMANDE_PER_ID] foreign key ([PER_ID]) references [dbo].[CLIENT] ([PER_ID]),
	constraint [FK_COMMANDE_TAB_ID] foreign key ([TAB_ID]) references [dbo].[TABLE_RESTAURANT] ([TAB_ID]),
	constraint [FK_COMMANDE_REV_ID] foreign key ([REV_ID]) references [dbo].[RESERVATION] ([REV_ID]),
	constraint [FK_COMMANDE_AVI_ID] foreign key ([AVI_ID]) references [dbo].[AVIS_CLIENT] ([AVI_ID]),
	constraint [UK_COMMANDE_AVI_ID] unique nonclustered ([AVI_ID] ASC))
go

/* Index on foreign key column for COMMANDE.PER_ID */
create nonclustered index [IDX_COM_PER_ID_FK]
	on [dbo].[COMMANDE] ([PER_ID] ASC)
go

/* Index on foreign key column for COMMANDE.TAB_ID */
create nonclustered index [IDX_COM_TAB_ID_FK]
	on [dbo].[COMMANDE] ([TAB_ID] ASC)
go

/* Index on foreign key column for COMMANDE.REV_ID */
create nonclustered index [IDX_COM_REV_ID_FK]
	on [dbo].[COMMANDE] ([REV_ID] ASC)
go

/* Index on foreign key column for COMMANDE.STC_CODE */
create nonclustered index [IDX_COM_STC_CODE_FK]
	on [dbo].[COMMANDE] ([STC_CODE] ASC)
go

/* Index on foreign key column for COMMANDE.AVI_ID */
create nonclustered index [IDX_COM_AVI_ID_FK]
	on [dbo].[COMMANDE] ([AVI_ID] ASC)
go

/**
  * Commentaires pour la table COMMANDE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Commande d''un client', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'COM_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'COM_DATE_COMMANDE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de livraison', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'COM_DATE_LIVRAISON'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Montant total de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'COM_MONTANT_TOTAL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Client ayant passé la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'PER_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Table associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Réservation associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'REV_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Statut de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'STC_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Avis laissé par le client sur la commande.', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'AVI_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE', 'COLUMN', 'COM_DATE_CREATION'
go
