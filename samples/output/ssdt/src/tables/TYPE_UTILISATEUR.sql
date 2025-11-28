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

/**
  * Commentaires pour la table TYPE_UTILISATEUR
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Type d''utilisateur', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_UTILISATEUR';
EXECUTE sp_addextendedproperty 'MS_Description', 'Code du type d''utilisateur', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_UTILISATEUR', 'COLUMN', 'TUT_CODE';
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé du type d''utilisateur', 'SCHEMA', 'dbo', 'TABLE', 'TYPE_UTILISATEUR', 'COLUMN', 'TUT_LIBELLE';
