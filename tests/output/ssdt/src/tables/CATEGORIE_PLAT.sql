----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table CATEGORIE_PLAT.
-- ===========================================================================================

create table [dbo].[CATEGORIE_PLAT] (
	[CAT_CODE] varchar(10),
	[CAT_LIBELLE] varchar(100) not null,
	[CAT_ORDRE] int not null,
	[CAT_PRIX_MOYEN] decimal,
	constraint [PK_CATEGORIE_PLAT] primary key clustered ([CAT_CODE] ASC),
	constraint [UK_CATEGORIE_PLAT_CAT_ORDRE] unique nonclustered ([CAT_ORDRE] ASC))
go

create nonclustered index [IDX_CAT_CAT_LIBELLE]
	on [dbo].[CATEGORIE_PLAT] ([CAT_LIBELLE] ASC)
go

/**
  * Commentaires pour la table CATEGORIE_PLAT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Catégorie de plat', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Code de la catégorie', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT', 'COLUMN', 'CAT_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé de la catégorie', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT', 'COLUMN', 'CAT_LIBELLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Ordre d''affichage dans le menu.', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT', 'COLUMN', 'CAT_ORDRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix moyen de la catégorie, à titre indicatif.', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT', 'COLUMN', 'CAT_PRIX_MOYEN'
go
