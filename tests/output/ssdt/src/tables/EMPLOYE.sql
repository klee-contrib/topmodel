----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table EMPLOYE.
-- ===========================================================================================

create table [dbo].[EMPLOYE] (
	[EMP_TELEPHONE] varchar(20),
	[EMP_DATE_NAISSANCE] timestamp,
	[EMP_MATRICULE] varchar(10) not null,
	[EMP_DATE_EMBAUCHE] timestamp not null,
	[EMP_SALAIRE] decimal,
	[LIE_ID] int not null,
	[PER_ID] int,
	constraint [PK_EMPLOYE] primary key clustered ([PER_ID] ASC),
	constraint [FK_EMPLOYE_LIE_ID] foreign key ([LIE_ID]) references [dbo].[LIEU] ([LIE_ID]),
	constraint [FK_EMPLOYE_PER_ID] foreign key ([PER_ID]) references [dbo].[PERSONNE] ([PER_ID]),
	constraint [UK_EMPLOYE_EMP_MATRICULE] unique nonclustered ([EMP_MATRICULE] ASC))
go

/* Index on foreign key column for EMPLOYE.LIE_ID */
create nonclustered index [IDX_EMP_LIE_ID_FK]
	on [dbo].[EMPLOYE] ([LIE_ID] ASC)
go

/* Index on foreign key column for EMPLOYE.PER_ID */
create nonclustered index [IDX_EMP_PER_ID_FK]
	on [dbo].[EMPLOYE] ([PER_ID] ASC)
go

/* Index IDX_EMP_EMP_TELEPHONE on EMPLOYE */
create nonclustered index [IDX_EMP_EMP_TELEPHONE]
	on [dbo].[EMPLOYE] ([EMP_TELEPHONE] ASC)
go

/**
  * Commentaires pour la table EMPLOYE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Employé du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone de l''employé.', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'EMP_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de naissance', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'EMP_DATE_NAISSANCE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Matricule de l''employé', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'EMP_MATRICULE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date d''embauche', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'EMP_DATE_EMBAUCHE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Salaire de l''employé', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'EMP_SALAIRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant où travaille l''employé', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'LIE_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'EMPLOYE', 'COLUMN', 'PER_ID'
go
