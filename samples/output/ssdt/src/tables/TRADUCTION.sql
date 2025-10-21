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

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Traduction', 'SCHEMA', 'dbo', 'TABLE', 'TRADUCTION';
