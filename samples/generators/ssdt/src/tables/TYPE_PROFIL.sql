----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TYPE_PROFIL.
-- ===========================================================================================

create table [dbo].[TYPE_PROFIL] (
	[TPR_CODE] varchar,
	[TPR_LIBELLE] varchar not null,
	constraint [PK_TYPE_PROFIL] primary key clustered ([TPR_CODE] ASC))
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'TypeProfil', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_PROFIL';
