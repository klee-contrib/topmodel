----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PERSONNE.
-- ===========================================================================================

create table [dbo].[PERSONNE] (
	[PER_ID] int identity,
	[PER_NOM] varchar not null,
	[PER_PRENOM] varchar not null,
	[DEP_CODE] varchar default N'75',
	constraint [PK_PERSONNE] primary key clustered ([PER_ID] ASC),
	constraint [FK_PERSONNE_DEP_CODE] foreign key ([DEP_CODE]) references [dbo].[DEPARTEMENT] ([DEP_CODE]))
go

/* Index on foreign key column for PERSONNE.DEP_CODE */
create nonclustered index [IDX_PER_DEP_CODE_FK]
	on [dbo].[PERSONNE] ([DEP_CODE] ASC)
go

/**
  * Commentaires pour la table PERSONNE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Classe de base représentant une personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prénom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_PRENOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Département de résidence de la personne.', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'DEP_CODE'
go
