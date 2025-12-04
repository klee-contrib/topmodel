----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROMOTION.
-- ===========================================================================================

create table [dbo].[PROMOTION] (
	[PRO_ID] int identity,
	[PRO_LIBELLE] varchar not null,
	[PRO_POURCENTAGE_REDUCTION] int not null,
	[PRO_DATE_DEBUT] timestamp not null,
	[PRO_DATE_FIN] timestamp not null,
	[PRO_ACTIVE] boolean not null default true,
	[RES_ID_RESTAURANT] int,
	constraint [PK_PROMOTION] primary key clustered ([PRO_ID] ASC),
	constraint [FK_PROMOTION_RESTAURANT_RES_ID_RESTAURANT] foreign key ([RES_ID_RESTAURANT]) references [dbo].[RESTAURANT] ([RES_ID]))
go

/* Index on foreign key column for PROMOTION.RES_ID_RESTAURANT */
create nonclustered index [IDX_PROMOTION_RES_ID_RESTAURANT_FK]
	on [dbo].[PROMOTION] ([RES_ID_RESTAURANT] ASC)
go

/**
  * Commentaires pour la table PROMOTION
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Promotion sur les plats', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_LIBELLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Pourcentage de réduction (0-100)', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_POURCENTAGE_REDUCTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de début de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_DATE_DEBUT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de fin de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_DATE_FIN'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si la promotion est active', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_ACTIVE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant concerné par la promotion (null si globale)', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'RES_ID_RESTAURANT'
go
