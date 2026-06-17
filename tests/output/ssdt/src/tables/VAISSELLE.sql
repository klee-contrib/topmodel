----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table VAISSELLE.
-- ===========================================================================================

create table [dbo].[VAISSELLE] (
	[VSL_ID] int identity,
	[VSL_DESCRIPTION] varchar not null,
	constraint [PK_VAISSELLE] primary key clustered ([VSL_ID] ASC))
go

/**
  * Commentaires pour la table VAISSELLE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Vaisselle de restaurant', 'SCHEMA', 'dbo', 'TABLE', 'VAISSELLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Id de la vaisselle', 'SCHEMA', 'dbo', 'TABLE', 'VAISSELLE', 'COLUMN', 'VSL_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Description de la vaisselle.', 'SCHEMA', 'dbo', 'TABLE', 'VAISSELLE', 'COLUMN', 'VSL_DESCRIPTION'
go
