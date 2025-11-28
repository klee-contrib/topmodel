----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TYPE_DROIT.
-- ===========================================================================================

create table [dbo].[TYPE_DROIT] (
	[TDR_CODE] varchar,
	[TDR_LIBELLE] varchar not null,
	constraint [PK_TYPE_DROIT] primary key clustered ([TDR_CODE] ASC))
go

/**
  * Commentaires pour la table TYPE_DROIT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Type de droit', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_DROIT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Code du type de droit', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_DROIT', 'COLUMN', 'TDR_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé du type de droit', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_DROIT', 'COLUMN', 'TDR_LIBELLE'
go
