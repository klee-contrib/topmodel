----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TYPE_TERRASSE.
-- ===========================================================================================

create table [dbo].[TYPE_TERRASSE] (
	[CODE] varchar,
	constraint [PK_TYPE_TERRASSE] primary key clustered ([CODE] ASC))
go

/**
  * Commentaires pour la table TYPE_TERRASSE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Type de terrasse', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_TERRASSE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Code du type de terrase', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_TERRASSE', 'COLUMN', 'CODE'
go
