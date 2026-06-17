----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table LIEU.
-- ===========================================================================================

create table [dbo].[LIEU] (
	[LIE_ID] int identity,
	[LIE_NOM] varchar not null,
	[LIE_ADRESSE] varchar,
	[LIE_DISCRIMINATOR] varchar(128) not null,
	[RES_TELEPHONE] varchar,
	[RES_DATE_CREATION] timestamp,
	[FRN_TELEPHONE] varchar,
	[FRN_BIO] boolean,
	constraint [PK_LIEU] primary key clustered ([LIE_ID] ASC))
go

/**
  * Commentaires pour la table LIEU
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Lieu', 'SCHEMA', 'dbo', 'TABLE', 'LIEU'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'LIE_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'LIE_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Adresse du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'LIE_ADRESSE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Discriminateur pour les instances de la hiérarchie de classe', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'LIE_DISCRIMINATOR'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'RES_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'RES_DATE_CREATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'FRN_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Si le fournisseur fait du bio.', 'SCHEMA', 'dbo', 'TABLE', 'LIEU', 'COLUMN', 'FRN_BIO'
go
