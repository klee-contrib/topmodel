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
