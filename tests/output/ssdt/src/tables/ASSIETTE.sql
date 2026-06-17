----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table ASSIETTE.
-- ===========================================================================================

create table [dbo].[ASSIETTE] (
	[AST_TAILLE] int not null,
	[VSL_ID] int,
	constraint [PK_ASSIETTE] primary key clustered ([VSL_ID] ASC),
	constraint [FK_ASSIETTE_VSL_ID] foreign key ([VSL_ID]) references [dbo].[VAISSELLE] ([VSL_ID]))
go

/* Index on foreign key column for ASSIETTE.VSL_ID */
create nonclustered index [IDX_AST_VSL_ID_FK]
	on [dbo].[ASSIETTE] ([VSL_ID] ASC)
go

/**
  * Commentaires pour la table ASSIETTE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Assiette.', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Taille de l''assiette.', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE', 'COLUMN', 'AST_TAILLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE', 'COLUMN', 'VSL_ID'
go
