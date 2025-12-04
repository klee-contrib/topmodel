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
	constraint [PK_COMMANDE_EXPORT] primary key clustered ([COM_ID] ASC))
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
