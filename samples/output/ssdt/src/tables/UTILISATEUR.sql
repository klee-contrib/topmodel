----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table UTILISATEUR.
-- ===========================================================================================

create table [dbo].[UTILISATEUR] (
	[UTI_ID] int identity,
	[UTI_NOM] varchar not null,
	[UTI_PRENOM] varchar not null,
	[UTI_EMAIL] varchar not null,
	[UTI_DATE_NAISSANCE] date,
	[UTI_ADRESSE] varchar,
	[UTI_ACTIF] boolean not null default true,
	[PRO_ID] int not null,
	[TUT_CODE] varchar not null default N'GEST',
	[UTI_DATE_CREATION] date not null default now(),
	[UTI_DATE_MODIFICATION] date default now(),
	constraint [PK_UTILISATEUR] primary key clustered ([UTI_ID] ASC),
	constraint [FK_UTILISATEUR_PROFIL_PRO_ID] foreign key ([PRO_ID]) references [dbo].[PROFIL] ([PRO_ID]),
	constraint [FK_UTILISATEUR_TYPE_UTILISATEUR_TUT_CODE] foreign key ([TUT_CODE]) references [dbo].[TYPE_UTILISATEUR] ([TUT_CODE]),
	constraint [UK_UTILISATEUR_UTI_EMAIL] unique nonclustered ([UTI_EMAIL] ASC))
go

/* Index on foreign key column for UTILISATEUR.PRO_ID */
create nonclustered index [IDX_UTILISATEUR_PRO_ID_FK]
	on [dbo].[UTILISATEUR] ([PRO_ID] ASC)
go

/* Index on foreign key column for UTILISATEUR.TUT_CODE */
create nonclustered index [IDX_UTILISATEUR_TUT_CODE_FK]
	on [dbo].[UTILISATEUR] ([TUT_CODE] ASC)
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Utilisateur', 'SCHEMA', 'dbo', 'TABLE', 'UTILISATEUR';
