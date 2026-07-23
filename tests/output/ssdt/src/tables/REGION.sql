----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table REGION.
-- ===========================================================================================

create table REGION (
	REG_CODE varchar(10),
	REG_LIBELLE varchar(100) not null,
	REG_NOM_RESPONSABLE varchar(100),
	constraint PK_REGION primary key clustered (REG_CODE asc))
go

create nonclustered index IDX_REG_REG_LIBELLE
	on REGION (REG_LIBELLE asc)
go

/**
  * Commentaires pour la table REGION
 **/
execute sp_addextendedproperty 'MS_Description', 'Région', 'SCHEMA', 'dbo', 'TABLE', 'REGION'
go
execute sp_addextendedproperty 'MS_Description', 'Code de la région.', 'SCHEMA', 'dbo', 'TABLE', 'REGION', 'COLUMN', 'REG_CODE'
go
execute sp_addextendedproperty 'MS_Description', 'Libellé de la région.', 'SCHEMA', 'dbo', 'TABLE', 'REGION', 'COLUMN', 'REG_LIBELLE'
go
execute sp_addextendedproperty 'MS_Description', 'Nom du responsable de la région.', 'SCHEMA', 'dbo', 'TABLE', 'REGION', 'COLUMN', 'REG_NOM_RESPONSABLE'
go
