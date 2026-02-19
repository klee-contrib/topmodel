----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table DEPARTEMENT.
-- ===========================================================================================

create table [dbo].[DEPARTEMENT] (
	[DEP_CODE] varchar,
	[DEP_LIBELLE] varchar not null,
	constraint [PK_DEPARTEMENT] primary key clustered ([DEP_CODE] ASC))
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
