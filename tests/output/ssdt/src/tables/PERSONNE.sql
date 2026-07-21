----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PERSONNE.
-- ===========================================================================================

create table PERSONNE (
	PER_ID int identity,
	PER_NOM varchar(100) not null,
	PER_PRENOM varchar(100) not null,
	DEP_CODE varchar(10) default N'75',
	PER_DATE_CREATION timestamp not null,
	constraint PK_PERSONNE primary key clustered (PER_ID asc),
	constraint FK_PERSONNE_DEP_CODE foreign key (DEP_CODE) references DEPARTEMENT (DEP_CODE))
go

/* Index on foreign key column for PERSONNE.DEP_CODE */
create nonclustered index IDX_PER_DEP_CODE_FK
	on PERSONNE (DEP_CODE asc)
go

/**
  * Commentaires pour la table PERSONNE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Classe de base représentant une personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prénom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_PRENOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Département de résidence de la personne.', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'DEP_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'PERSONNE', 'COLUMN', 'PER_DATE_CREATION'
go
