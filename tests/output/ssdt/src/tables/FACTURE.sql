----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table FACTURE.
-- ===========================================================================================

create table FACTURE (
	ID int identity,
	constraint PK_FACTURE primary key clustered (ID asc))
go

/**
  * Commentaires pour la table FACTURE
 **/
execute sp_addextendedproperty 'MS_Description', 'Facture', 'SCHEMA', 'dbo', 'TABLE', 'FACTURE'
go
execute sp_addextendedproperty 'MS_Description', 'Identifiant de la facture', 'SCHEMA', 'dbo', 'TABLE', 'FACTURE', 'COLUMN', 'ID'
go
