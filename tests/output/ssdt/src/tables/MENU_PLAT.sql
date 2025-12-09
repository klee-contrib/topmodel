----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table MENU_PLAT.
-- ===========================================================================================

create table [dbo].[MENU_PLAT] (
	[MPL_ID] int identity,
	[MPL_ORDRE] int not null,
	[MEN_ID_MENU] int not null,
	[PLA_ID_PLAT] int not null,
	constraint [PK_MENU_PLAT] primary key clustered ([MPL_ID] ASC),
	constraint [FK_MENU_PLAT_MENU_MEN_ID_MENU] foreign key ([MEN_ID_MENU]) references [dbo].[MENU] ([MEN_ID]),
	constraint [FK_MENU_PLAT_PLAT_PLA_ID_PLAT] foreign key ([PLA_ID_PLAT]) references [dbo].[PLAT] ([PLA_ID]),
	constraint [UK_MENU_PLAT_MEN_ID_MENU_MPL_ORDRE] unique nonclustered ([MEN_ID_MENU] ASC, [MPL_ORDRE] ASC),
	constraint [UK_MENU_PLAT_MEN_ID_MENU_PLA_ID_PLAT] unique nonclustered ([MEN_ID_MENU] ASC, [PLA_ID_PLAT] ASC))
go

/* Index on foreign key column for MENU_PLAT.MEN_ID_MENU */
create nonclustered index [IDX_MENU_PLAT_MEN_ID_MENU_FK]
	on [dbo].[MENU_PLAT] ([MEN_ID_MENU] ASC)
go

/* Index on foreign key column for MENU_PLAT.PLA_ID_PLAT */
create nonclustered index [IDX_MENU_PLAT_PLA_ID_PLAT_FK]
	on [dbo].[MENU_PLAT] ([PLA_ID_PLAT] ASC)
go

/**
  * Commentaires pour la table MENU_PLAT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat dans un menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la relation', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'MPL_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Ordre d''affichage du plat dans le menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'MPL_ORDRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Menu contenant ce plat', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'MEN_ID_MENU'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU_PLAT', 'COLUMN', 'PLA_ID_PLAT'
go
