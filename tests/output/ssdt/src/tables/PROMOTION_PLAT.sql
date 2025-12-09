----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROMOTION_PLAT.
-- ===========================================================================================

create table [dbo].[PROMOTION_PLAT] (
	[PPL_ID] int identity,
	[PRO_ID_PROMOTION] int not null,
	[PLA_ID_PLAT] int not null,
	constraint [PK_PROMOTION_PLAT] primary key clustered ([PPL_ID] ASC),
	constraint [FK_PROMOTION_PLAT_PROMOTION_PRO_ID_PROMOTION] foreign key ([PRO_ID_PROMOTION]) references [dbo].[PROMOTION] ([PRO_ID]),
	constraint [FK_PROMOTION_PLAT_PLAT_PLA_ID_PLAT] foreign key ([PLA_ID_PLAT]) references [dbo].[PLAT] ([PLA_ID]),
	constraint [UK_PROMOTION_PLAT_PRO_ID_PROMOTION_PLA_ID_PLAT] unique nonclustered ([PRO_ID_PROMOTION] ASC, [PLA_ID_PLAT] ASC))
go

/* Index on foreign key column for PROMOTION_PLAT.PRO_ID_PROMOTION */
create nonclustered index [IDX_PROMOTION_PLAT_PRO_ID_PROMOTION_FK]
	on [dbo].[PROMOTION_PLAT] ([PRO_ID_PROMOTION] ASC)
go

/* Index on foreign key column for PROMOTION_PLAT.PLA_ID_PLAT */
create nonclustered index [IDX_PROMOTION_PLAT_PLA_ID_PLAT_FK]
	on [dbo].[PROMOTION_PLAT] ([PLA_ID_PLAT] ASC)
go

/**
  * Commentaires pour la table PROMOTION_PLAT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Association entre une promotion et un plat', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de l''association', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT', 'COLUMN', 'PPL_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Promotion concernée', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT', 'COLUMN', 'PRO_ID_PROMOTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat concerné par la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT', 'COLUMN', 'PLA_ID_PLAT'
go
