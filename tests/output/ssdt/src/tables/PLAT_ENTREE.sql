----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PLAT_ENTREE.
-- ===========================================================================================

create table [dbo].[PLAT_ENTREE] (
	[PLA_ID] int,
	constraint [PK_PLAT_ENTREE] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PLAT_ENTREE_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]))
go

/* Index on foreign key column for PLAT_ENTREE.PLA_ID */
create nonclustered index [IDX_PLAT_ENTREE_PLA_ID_FK]
	on [dbo].[PLAT_ENTREE] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table PLAT_ENTREE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Entrée', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_ENTREE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_ENTREE', 'COLUMN', 'PLA_ID'
go
