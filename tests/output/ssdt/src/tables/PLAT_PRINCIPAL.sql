----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PLAT_PRINCIPAL.
-- ===========================================================================================

create table [dbo].[PLAT_PRINCIPAL] (
	[VEGETARIEN] boolean not null,
	[PLA_ID] int,
	constraint [PK_PLAT_PRINCIPAL] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PLAT_PRINCIPAL_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]))
go

/* Index on foreign key column for PLAT_PRINCIPAL.PLA_ID */
create nonclustered index [IDX_PLAT_PRINCIPAL_PLA_ID_FK]
	on [dbo].[PLAT_PRINCIPAL] ([PLA_ID] ASC)
go

/**
  * Commentaires pour la table PLAT_PRINCIPAL
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat principal', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_PRINCIPAL'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Si le plat est végétarien.', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_PRINCIPAL', 'COLUMN', 'VEGETARIEN'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'PLAT_PRINCIPAL', 'COLUMN', 'PLA_ID'
go
