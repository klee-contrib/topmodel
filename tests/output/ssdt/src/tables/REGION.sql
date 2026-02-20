----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table REGION.
-- ===========================================================================================

create table [dbo].[REGION] (
	[REG_CODE] varchar,
	[REG_LIBELLE] varchar not null,
	[REG_NOM_RESPONSABLE] varchar,
	constraint [PK_REGION] primary key clustered ([REG_CODE] ASC))
go

/**
  * Commentaires pour la table REGION
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Région', 'SCHEMA', 'dbo', 'TABLE', 'REGION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Code de la région.', 'SCHEMA', 'dbo', 'TABLE', 'REGION', 'COLUMN', 'REG_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé de la région.', 'SCHEMA', 'dbo', 'TABLE', 'REGION', 'COLUMN', 'REG_LIBELLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom du responsable de la région.', 'SCHEMA', 'dbo', 'TABLE', 'REGION', 'COLUMN', 'REG_NOM_RESPONSABLE'
go
