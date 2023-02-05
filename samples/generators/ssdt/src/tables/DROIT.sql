----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table DROIT.
-- ===========================================================================================

create table [dbo].[DROIT] (
	[DRO_CODE] varchar,
	[DRO_LIBELLE] varchar not null,
	[TPR_CODE] varchar,
	constraint [PK_DROIT] primary key clustered ([DRO_CODE] ASC),
	constraint [FK_DROIT_TYPE_PROFIL_TPR_CODE] foreign key ([TPR_CODE]) references [dbo].[TYPE_PROFIL] ([TPR_CODE]))
go

/* Index on foreign key column for DROIT.TPR_CODE */
create nonclustered index [IDX_DROIT_TPR_CODE_FK]
	on [dbo].[DROIT] ([TPR_CODE] ASC)
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Droit', 'SCHEMA', 'dbo', 'TABLE', 'DROIT';
