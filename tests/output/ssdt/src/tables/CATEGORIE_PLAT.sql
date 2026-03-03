----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table CATEGORIE_PLAT.
-- ===========================================================================================

create table [dbo].[CATEGORIE_PLAT] (
	[CAT_CODE] varchar,
	[CAT_LIBELLE] varchar not null,
	[CAT_ORDRE] int not null,
	constraint [PK_CATEGORIE_PLAT] primary key clustered ([CAT_CODE] ASC),
	constraint [UK_CATEGORIE_PLAT_CAT_ORDRE] unique nonclustered ([CAT_ORDRE] ASC))
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
