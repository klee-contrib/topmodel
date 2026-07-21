----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table COUVERT.
-- ===========================================================================================

create table COUVERT (
	VSL_ID int,
	VSL_DESCRIPTION varchar(100) not null,
	constraint PK_COUVERT primary key clustered (VSL_ID asc))
go

/**
  * Commentaires pour la table COUVERT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Couvert.', 'SCHEMA', 'dbo', 'TABLE', 'COUVERT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Id de la vaisselle', 'SCHEMA', 'dbo', 'TABLE', 'COUVERT', 'COLUMN', 'VSL_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Description de la vaisselle.', 'SCHEMA', 'dbo', 'TABLE', 'COUVERT', 'COLUMN', 'VSL_DESCRIPTION'
go
