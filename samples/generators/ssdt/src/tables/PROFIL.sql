----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROFIL.
-- ===========================================================================================

create table [dbo].[PROFIL] (
	[PRO_ID] int identity,
	[TPR_CODE] varchar,
	constraint [PK_PROFIL] primary key clustered ([PRO_ID] ASC),
	constraint [FK_PROFIL_TYPE_PROFIL_TPR_CODE] foreign key ([TPR_CODE]) references [dbo].[TYPE_PROFIL] ([TPR_CODE]))
go

/* Index on foreign key column for PROFIL.TPR_CODE */
create nonclustered index [IDX_PROFIL_TPR_CODE_FK]
	on [dbo].[PROFIL] ([TPR_CODE] ASC)
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Profil', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL';
