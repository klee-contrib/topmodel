----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table DROIT.
-- ===========================================================================================

create table [dbo].[DROIT] (
	[DRO_CODE] varchar,
	[DRO_LIBELLE] varchar not null,
	[TDR_CODE] varchar not null,
	constraint [PK_DROIT] primary key clustered ([DRO_CODE] ASC),
	constraint [FK_DROIT_TYPE_DROIT_TDR_CODE] foreign key ([TDR_CODE]) references [dbo].[TYPE_DROIT] ([TDR_CODE]))
go

/* Index on foreign key column for DROIT.TDR_CODE */
create nonclustered index [IDX_DROIT_TDR_CODE_FK]
	on [dbo].[DROIT] ([TDR_CODE] ASC)
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Droit', 'SCHEMA', 'dbo', 'TABLE', 'DROIT';
