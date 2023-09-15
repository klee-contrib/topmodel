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

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'TypeDroit', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_DROIT';
