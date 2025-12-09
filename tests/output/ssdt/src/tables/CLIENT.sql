----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table CLIENT.
-- ===========================================================================================

create table [dbo].[CLIENT] (
	[CLI_ID] int identity,
	[CLI_NOM] varchar not null,
	[CLI_PRENOM] varchar not null,
	[CLI_TELEPHONE] varchar,
	[CLI_EMAIL] varchar,
	constraint [PK_CLIENT] primary key clustered ([CLI_ID] ASC))
go

/**
  * Commentaires pour la table CLIENT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Client du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prénom du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_PRENOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Adresse email du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_EMAIL'
go
