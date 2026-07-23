----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TRANSLATION.
-- ===========================================================================================

create table TRANSLATION (
	TRA_RESOURCE_KEY varchar(100),
	TRA_VALUE varchar(100) not null,
	TRA_LANG varchar(100),
	constraint PK_TRANSLATION primary key clustered (TRA_RESOURCE_KEY asc, TRA_LANG asc))
go

/**
  * Commentaires pour la table TRANSLATION
 **/
execute sp_addextendedproperty 'MS_Description', 'Table pour stocker les traductions en SQL.', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION'
go
execute sp_addextendedproperty 'MS_Description', 'Clé de traduction.', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION', 'COLUMN', 'TRA_RESOURCE_KEY'
go
execute sp_addextendedproperty 'MS_Description', 'Valeur de la clé de traduction.', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION', 'COLUMN', 'TRA_VALUE'
go
execute sp_addextendedproperty 'MS_Description', 'Langue de traduction', 'SCHEMA', 'dbo', 'TABLE', 'TRANSLATION', 'COLUMN', 'TRA_LANG'
go
