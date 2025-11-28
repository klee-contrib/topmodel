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

/**
  * Commentaires pour la table PROFIL
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Profil des utilisateurs', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL';
EXECUTE sp_addextendedproperty 'MS_Description', 'Id technique', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL', 'COLUMN', 'PRO_ID';
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé du profil.', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL', 'COLUMN', 'PRO_LIBELLE';
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''utilisateur.', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL', 'COLUMN', 'PRO_DATE_CREATION';
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de modification de l''utilisateur.', 'SCHEMA', 'dbo', 'TABLE', 'PROFIL', 'COLUMN', 'PRO_DATE_MODIFICATION';
