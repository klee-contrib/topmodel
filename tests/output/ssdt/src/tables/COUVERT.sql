----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table COUVERT.
-- ===========================================================================================

create table [dbo].[COUVERT] (
	[VSL_ID] int,
	constraint [PK_COUVERT] primary key clustered ([VSL_ID] ASC),
	constraint [FK_COUVERT_VSL_ID] foreign key ([VSL_ID]) references [dbo].[VAISSELLE] ([VSL_ID]))
go

/* Index on foreign key column for COUVERT.VSL_ID */
create nonclustered index [IDX_CVT_VSL_ID_FK]
	on [dbo].[COUVERT] ([VSL_ID] ASC)
go

/**
  * Commentaires pour la table COUVERT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Couvert.', 'SCHEMA', 'dbo', 'TABLE', 'COUVERT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'COUVERT', 'COLUMN', 'VSL_ID'
go
