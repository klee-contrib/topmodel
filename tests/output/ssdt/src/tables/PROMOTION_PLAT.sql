----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROMOTION_PLAT.
-- ===========================================================================================

create table [dbo].[PROMOTION_PLAT] (
	[PRO_ID] int,
	[PLA_ID] int,
	constraint [PK_PROMOTION_PLAT] primary key clustered ([PRO_ID] ASC, [PLA_ID] ASC),
	constraint [FK_PROMOTION_PLAT_PROMOTION_PRO_ID] foreign key ([PRO_ID]) references [dbo].[PROMOTION] ([PRO_ID]),
	constraint [FK_PROMOTION_PLAT_PLAT_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]))
go

/* Index on foreign key column for PROMOTION_PLAT.PRO_ID */
create nonclustered index [IDX_PROMOTION_PLAT_PRO_ID_FK]
	on [dbo].[PROMOTION_PLAT] ([PRO_ID] ASC)
go

/* Index on foreign key column for PROMOTION_PLAT.PLA_ID */
create nonclustered index [IDX_PROMOTION_PLAT_PLA_ID_FK]
	on [dbo].[PROMOTION_PLAT] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table PROMOTION_PLAT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Association entre une promotion et un plat', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Promotion concernée', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT', 'COLUMN', 'PRO_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat concerné par la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION_PLAT', 'COLUMN', 'PLA_ID'
go
