----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table DEPARTEMENT.
-- ===========================================================================================

create table DEPARTEMENT (
	DEP_CODE varchar(10),
	DEP_LIBELLE varchar(100) not null,
	REG_CODE varchar(10) not null,
	constraint PK_DEPARTEMENT primary key clustered (DEP_CODE asc),
	constraint FK_DEPARTEMENT_REG_CODE foreign key (REG_CODE) references REGION (REG_CODE))
go

/* Index on foreign key column for DEPARTEMENT.REG_CODE */
create nonclustered index IDX_DEP_REG_CODE_FK
	on DEPARTEMENT (REG_CODE asc)
go

create nonclustered index IDX_DEP_DEP_LIBELLE
	on DEPARTEMENT (DEP_LIBELLE asc)
go

/**
  * Commentaires pour la table DEPARTEMENT
 **/
execute sp_addextendedproperty 'MS_Description', 'Département', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT'
go
execute sp_addextendedproperty 'MS_Description', 'Code du département.', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT', 'COLUMN', 'DEP_CODE'
go
execute sp_addextendedproperty 'MS_Description', 'Libellé du département.', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT', 'COLUMN', 'DEP_LIBELLE'
go
execute sp_addextendedproperty 'MS_Description', 'Région associée.', 'SCHEMA', 'dbo', 'TABLE', 'DEPARTEMENT', 'COLUMN', 'REG_CODE'
go
