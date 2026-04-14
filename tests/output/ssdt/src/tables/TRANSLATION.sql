----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TRANSLATION.
-- ===========================================================================================

create table [dbo].[TRANSLATION] (
	[TRA_RESOURCE_KEY] varchar,
	[TRA_VALUE] varchar not null,
	constraint [PK_TRANSLATION] primary key clustered ([TRA_RESOURCE_KEY] ASC))
go

/**
  * Commentaires pour la table TRANSLATION
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Table pour stocker les traductions en SQL.', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Clé de traduction.', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION', 'COLUMN', 'TRA_RESOURCE_KEY'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Valeur de la clé de traduction.', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION', 'COLUMN', 'TRA_VALUE'
go
