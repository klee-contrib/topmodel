----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table CLIENT.
-- ===========================================================================================

create table [dbo].[CLIENT] (
	[CLI_EMAIL] varchar,
	[PER_ID] int,
	constraint [PK_CLIENT] primary key clustered ([PER_ID] ASC),
	constraint [FK_CLIENT_PERSONNE_PER_ID] foreign key ([PER_ID]) references [dbo].[PERSONNE] ([PER_ID]))
go

/* Index on foreign key column for CLIENT.PER_ID */
create nonclustered index [IDX_CLIENT_PER_ID_FK]
	on [dbo].[CLIENT] ([PER_ID] ASC)
go

/**
  * Commentaires pour la table CLIENT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Client du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Adresse email du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_EMAIL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'PER_ID'
go
