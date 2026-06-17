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
	[LIE_ID] int not null,
	[PLA_DATE_CREATION] timestamp not null,
	[PBO_VOLUME] int,
	[PPR_VEGETARIEN] boolean,
	constraint [PK_PLAT] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PLAT_CAT_CODE] foreign key ([CAT_CODE]) references [dbo].[CATEGORIE_PLAT] ([CAT_CODE]),
	constraint [FK_PLAT_LIE_ID] foreign key ([LIE_ID]) references [dbo].[LIEU] ([LIE_ID]))
go

/* Index on foreign key column for PLAT.CAT_CODE */
create nonclustered index [IDX_PLA_CAT_CODE_FK]
	on [dbo].[PLAT] ([CAT_CODE] ASC)
go

/* Index on foreign key column for PLAT.LIE_ID */
create nonclustered index [IDX_PLA_LIE_ID_FK]
	on [dbo].[PLAT] ([LIE_ID] ASC)
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
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant proposant ce plat', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'LIE_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PLA_DATE_CREATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Volume de la boisson', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PBO_VOLUME'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Si le plat est végétarien.', 'SCHEMA', 'dbo', 'TABLE', 'PLAT', 'COLUMN', 'PPR_VEGETARIEN'
go
