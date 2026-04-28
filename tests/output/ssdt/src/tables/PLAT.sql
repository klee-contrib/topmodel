----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PLAT.
-- ===========================================================================================

create table [dbo].[PLAT] (
	[PLA_ID] int identity,
	[PLA_NOM] varchar not null,
	[PLA_DESCRIPTION] varchar,
	[PLA_PRIX] decimal not null,
	[PLA_DISPONIBLE] boolean not null default true,
	[CAT_CODE] varchar not null,
	[RES_ID] int not null,
	constraint [PK_PLAT] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PLAT_CAT_CODE] foreign key ([CAT_CODE]) references [dbo].[CATEGORIE_PLAT] ([CAT_CODE]),
	constraint [FK_PLAT_RES_ID] foreign key ([RES_ID]) references [dbo].[RESTAURANT] ([RES_ID]))
go

/* Index on foreign key column for PLAT.CAT_CODE */
create nonclustered index [IDX_PLA_CAT_CODE_FK]
	on [dbo].[PLAT] ([CAT_CODE] ASC)
go

/* Index on foreign key column for PLAT.RES_ID */
create nonclustered index [IDX_PLA_RES_ID_FK]
	on [dbo].[PLAT] ([RES_ID] ASC)
go

/**
  * Commentaires pour la table PLAT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat du menu', 'SCHEMA', 'dbo', 'TABLE', 'PLAT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant du plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PLA_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom du plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PLA_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Description du plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PLA_DESCRIPTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix du plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PLA_PRIX'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si le plat est disponible', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PLA_DISPONIBLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Catégorie du plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'CAT_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant proposant ce plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'RES_ID'
go
