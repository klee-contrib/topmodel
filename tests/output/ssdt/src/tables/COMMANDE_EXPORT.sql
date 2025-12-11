----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table COMMANDE_EXPORT.
-- ===========================================================================================

create table [dbo].[COMMANDE_EXPORT] (
	[COM_ID] int,
	[COM_DATE_COMMANDE] timestamp not null,
	[COM_DATE_LIVRAISON] timestamp,
	[COM_MONTANT_TOTAL] decimal not null,
	[CLI_ID] int not null,
	[TAB_ID] int,
	[STC_CODE] varchar not null default N'EN_ATT',
	constraint [PK_COMMANDE_EXPORT] primary key clustered ([COM_ID] ASC),
	constraint [FK_COMMANDE_EXPORT_CLIENT_CLI_ID] foreign key ([CLI_ID]) references [dbo].[CLIENT] ([CLI_ID]),
	constraint [FK_COMMANDE_EXPORT_TABLE_CLIENT_TAB_ID] foreign key ([TAB_ID]) references [dbo].[TABLE_CLIENT] ([TAB_ID]),
	constraint [FK_COMMANDE_EXPORT_STATUT_COMMANDE_STC_CODE] foreign key ([STC_CODE]) references [dbo].[STATUT_COMMANDE] ([STC_CODE]))
go

/* Index on foreign key column for COMMANDE_EXPORT.CLI_ID */
create nonclustered index [IDX_COMMANDE_EXPORT_CLI_ID_FK]
	on [dbo].[COMMANDE_EXPORT] ([CLI_ID] ASC)
go

/* Index on foreign key column for COMMANDE_EXPORT.TAB_ID */
create nonclustered index [IDX_COMMANDE_EXPORT_TAB_ID_FK]
	on [dbo].[COMMANDE_EXPORT] ([TAB_ID] ASC)
go

/* Index on foreign key column for COMMANDE_EXPORT.STC_CODE */
create nonclustered index [IDX_COMMANDE_EXPORT_STC_CODE_FK]
	on [dbo].[COMMANDE_EXPORT] ([STC_CODE] ASC)
go

/**
  * Commentaires pour la table COMMANDE_EXPORT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Commande pour export avec préservation des clés primaires', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'COM_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'COM_DATE_COMMANDE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de livraison', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'COM_DATE_LIVRAISON'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Montant total de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'COM_MONTANT_TOTAL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Client ayant passé la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'CLI_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Table associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Statut de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_EXPORT', 'COLUMN', 'STC_CODE'
go
