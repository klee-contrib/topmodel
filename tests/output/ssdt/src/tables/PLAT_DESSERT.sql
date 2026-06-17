----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PLAT_DESSERT.
-- ===========================================================================================

create table [dbo].[PLAT_DESSERT] (
	[PLA_ID] int,
	constraint [PK_PLAT_DESSERT] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PLAT_DESSERT_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]))
go

/* Index on foreign key column for PLAT_DESSERT.PLA_ID */
create nonclustered index [IDX_PLAT_DESSERT_PLA_ID_FK]
	on [dbo].[PLAT_DESSERT] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table PLAT_DESSERT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Dessert', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_DESSERT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_DESSERT', 'COLUMN', 'PLA_ID'
go
