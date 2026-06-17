----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PLAT_BOISSON.
-- ===========================================================================================

create table [dbo].[PLAT_BOISSON] (
	[VOLUME] int not null,
	[PLA_ID] int,
	constraint [PK_PLAT_BOISSON] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PLAT_BOISSON_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]))
go

/* Index on foreign key column for PLAT_BOISSON.PLA_ID */
create nonclustered index [IDX_PLAT_BOISSON_PLA_ID_FK]
	on [dbo].[PLAT_BOISSON] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table PLAT_BOISSON
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Boisson', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_BOISSON'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Volume de la boisson', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_BOISSON', 'COLUMN', 'VOLUME'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_BOISSON', 'COLUMN', 'PLA_ID'
go
