----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table RESTAURANT.
-- ===========================================================================================

create table [dbo].[RESTAURANT] (
	[RES_ID] int identity,
	[RES_NOM] varchar not null,
	[RES_ADRESSE] varchar,
	[RES_TELEPHONE] varchar,
	[RES_DATE_CREATION] timestamp not null,
	constraint [PK_RESTAURANT] primary key clustered ([RES_ID] ASC))
go

/**
  * Commentaires pour la table RESTAURANT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Adresse du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_ADRESSE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_DATE_CREATION'
go
