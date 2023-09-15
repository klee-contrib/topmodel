----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROFIL.
-- ===========================================================================================

create table [dbo].[PROFIL] (
	[PRO_ID] int identity,
	[PRO_LIBELLE] varchar not null,
	[PRO_DATE_CREATION] date not null default now(),
	[PRO_DATE_MODIFICATION] date default now(),
	constraint [PK_PROFIL] primary key clustered ([PRO_ID] ASC))
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Profil', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL';
