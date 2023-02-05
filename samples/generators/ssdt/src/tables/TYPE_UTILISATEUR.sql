----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TYPE_UTILISATEUR.
-- ===========================================================================================

create table [dbo].[TYPE_UTILISATEUR] (
	[TUT_CODE] varchar,
	[TUT_LIBELLE] varchar not null,
	constraint [PK_TYPE_UTILISATEUR] primary key clustered ([TUT_CODE] ASC))
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'TypeUtilisateur', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_UTILISATEUR';
