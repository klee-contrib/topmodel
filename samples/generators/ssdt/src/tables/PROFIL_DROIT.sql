----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROFIL_DROIT.
-- ===========================================================================================

create table [dbo].[PROFIL_DROIT] (
	[PRO_ID] int8 identity,
	[DRO_CODE] varchar,
	constraint [PK_PROFIL_DROIT] primary key clustered ([PRO_ID] ASC, [DRO_CODE] ASC),
	constraint [FK_PROFIL_DROIT_PROFIL_PRO_ID] foreign key ([PRO_ID]) references [dbo].[PROFIL] ([PRO_ID]),
	constraint [FK_PROFIL_DROIT_DROIT_DRO_CODE] foreign key ([DRO_CODE]) references [dbo].[DROIT] ([DRO_CODE]))
go

/* Index on foreign key column for PROFIL_DROIT.PRO_ID */
create nonclustered index [IDX_PROFIL_DROIT_PRO_ID_FK]
	on [dbo].[PROFIL_DROIT] ([PRO_ID] ASC)
go

/* Index on foreign key column for PROFIL_DROIT.DRO_CODE */
create nonclustered index [IDX_PROFIL_DROIT_DRO_CODE_FK]
	on [dbo].[PROFIL_DROIT] ([DRO_CODE] ASC)
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Droits', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL_DROIT';
