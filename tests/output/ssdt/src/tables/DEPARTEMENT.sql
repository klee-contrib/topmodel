----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table DEPARTEMENT.
-- ===========================================================================================

create table [dbo].[DEPARTEMENT] (
	[DEP_CODE] varchar,
	[DEP_LIBELLE] varchar not null,
	[REG_CODE] varchar not null,
	constraint [PK_DEPARTEMENT] primary key clustered ([DEP_CODE] ASC),
	constraint [FK_DEPARTEMENT_REG_CODE] foreign key ([REG_CODE]) references [dbo].[REGION] ([REG_CODE]))
go

/* Index on foreign key column for DEPARTEMENT.REG_CODE */
create nonclustered index [IDX_DEP_REG_CODE_FK]
	on [dbo].[DEPARTEMENT] ([REG_CODE] ASC)
go

create nonclustered index [IDX_DEP_DEP_LIBELLE]
	on [dbo].[DEPARTEMENT] ([DEP_LIBELLE] ASC)
go

/**
  * Commentaires pour la table DEPARTEMENT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Département', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Code du département.', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT', 'COLUMN', 'DEP_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé du département.', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT', 'COLUMN', 'DEP_LIBELLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Région associée.', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT', 'COLUMN', 'REG_CODE'
go
