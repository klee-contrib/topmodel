----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table VERRE.
-- ===========================================================================================

create table [dbo].[VERRE] (
	[VRR_A_PIED] boolean not null,
	[VSL_ID] int,
	constraint [PK_VERRE] primary key clustered ([VSL_ID] ASC),
	constraint [FK_VERRE_VSL_ID] foreign key ([VSL_ID]) references [dbo].[VAISSELLE] ([VSL_ID]))
go

/* Index on foreign key column for VERRE.VSL_ID */
create nonclustered index [IDX_VRR_VSL_ID_FK]
	on [dbo].[VERRE] ([VSL_ID] ASC)
go

/**
  * Commentaires pour la table VERRE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Verre.', 'SCHEMA', 'dbo', 'TABLE', 'VERRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Si le verre est à pied ou non.', 'SCHEMA', 'dbo', 'TABLE', 'VERRE', 'COLUMN', 'VRR_A_PIED'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'VERRE', 'COLUMN', 'VSL_ID'
go
