----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table ASSIETTE.
-- ===========================================================================================

create table [dbo].[ASSIETTE] (
	[VSL_ID] int,
	[VSL_DESCRIPTION] varchar(100) not null,
	[AST_TAILLE] int not null,
	constraint [PK_ASSIETTE] primary key clustered ([VSL_ID] ASC))
go

/**
  * Création de la séquence pour la clé primaire de la table VAISSELLE
 **/
create sequence SEQ_VAISSELLE as int start with 1000 increment by 50
go

/**
  * Commentaires pour la table ASSIETTE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Assiette.', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Id de la vaisselle', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE', 'COLUMN', 'VSL_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Description de la vaisselle.', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE', 'COLUMN', 'VSL_DESCRIPTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Taille de l''assiette.', 'SCHEMA', 'dbo', 'TABLE', 'ASSIETTE', 'COLUMN', 'AST_TAILLE'
go
