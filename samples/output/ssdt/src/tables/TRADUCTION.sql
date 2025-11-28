----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TRADUCTION.
-- ===========================================================================================

create table [dbo].[TRADUCTION] (
	[TRD_RESOURCE_KEY] varchar,
	[TRD_LABEL] varchar not null,
	constraint [PK_TRADUCTION] primary key clustered ([TRD_RESOURCE_KEY] ASC))
go

/**
  * Commentaires pour la table TRADUCTION
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Classe pour contenir les traductions en base de données.', 'SCHEMA', 'dbo', 'TABLE', 'TRADUCTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Clé de traduction.', 'SCHEMA', 'dbo', 'TABLE', 'TRADUCTION', 'COLUMN', 'TRD_RESOURCE_KEY'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Valeur.', 'SCHEMA', 'dbo', 'TABLE', 'TRADUCTION', 'COLUMN', 'TRD_LABEL'
go
