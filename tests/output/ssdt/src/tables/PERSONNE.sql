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
	[PER_EMAIL] varchar,
	[PER_TELEPHONE] varchar,
	[PER_DATE_NAISSANCE] timestamp,
	constraint [PK_PERSONNE] primary key clustered ([PER_ID] ASC))
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
EXECUTE sp_addextendedproperty 'MS_Description', 'Adresse email', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_EMAIL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de naissance', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_DATE_NAISSANCE'
go
