----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table MENU_PLAT.
-- ===========================================================================================

create table [dbo].[MENU_PLAT] (
	[MEN_ID] int,
	[PLA_ID] int,
	[MPL_ORDRE] int not null,
	constraint [PK_MENU_PLAT] primary key clustered ([MEN_ID] ASC, [PLA_ID] ASC),
	constraint [FK_MENU_PLAT_MEN_ID] foreign key ([MEN_ID]) references [dbo].[MENU] ([MEN_ID]),
	constraint [FK_MENU_PLAT_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]),
	constraint [UK_MENU_PLAT_MEN_ID_MPL_ORDRE] unique nonclustered ([MEN_ID] ASC, [MPL_ORDRE] ASC))
go

/* Index on foreign key column for MENU_PLAT.MEN_ID */
create nonclustered index [IDX_MPL_MEN_ID_FK]
	on [dbo].[MENU_PLAT] ([MEN_ID] ASC)
go

/* Index on foreign key column for MENU_PLAT.PLA_ID */
create nonclustered index [IDX_MPL_PLA_ID_FK]
	on [dbo].[MENU_PLAT] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table MENU_PLAT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat dans un menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Menu contenant ce plat', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'MEN_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'PLA_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Ordre d''affichage du plat dans le menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'MPL_ORDRE'
go
