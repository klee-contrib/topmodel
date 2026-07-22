----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table AVIS_CLIENT.
-- ===========================================================================================

create table AVIS_CLIENT (
	AVI_ID int identity,
	AVI_NOTE int not null,
	AVI_COMMENTAIRE varchar(100),
	AVI_DATE_AVIS timestamp not null,
	AVI_APPROUVE boolean not null default false,
	PER_ID int not null,
	LIE_ID int not null,
	AVI_DATE_CREATION timestamp not null,
	constraint PK_AVIS_CLIENT primary key clustered (AVI_ID asc),
	constraint FK_AVIS_CLIENT_PER_ID foreign key (PER_ID) references CLIENT (PER_ID),
	constraint FK_AVIS_CLIENT_LIE_ID foreign key (LIE_ID) references LIEU (LIE_ID),
	constraint UK_AVIS_CLIENT_PER_ID_LIE_ID_AVI_DATE_AVIS unique nonclustered (PER_ID asc, LIE_ID asc, AVI_DATE_AVIS asc))
go

/* Index on foreign key column for AVIS_CLIENT.PER_ID */
create nonclustered index IDX_AVI_PER_ID_FK
	on AVIS_CLIENT (PER_ID asc)
go

/* Index on foreign key column for AVIS_CLIENT.LIE_ID */
create nonclustered index IDX_AVI_LIE_ID_FK
	on AVIS_CLIENT (LIE_ID asc)
go

/**
  * Commentaires pour la table AVIS_CLIENT
 **/
execute sp_addextendedproperty 'MS_Description', 'Avis d''un client sur un restaurant', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT'
go
execute sp_addextendedproperty 'MS_Description', 'Identifiant de l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Note sur 5', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_NOTE'
go
execute sp_addextendedproperty 'MS_Description', 'Commentaire de l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_COMMENTAIRE'
go
execute sp_addextendedproperty 'MS_Description', 'Date de l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_DATE_AVIS'
go
execute sp_addextendedproperty 'MS_Description', 'Indique si l''avis est approuvé par le restaurant', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_APPROUVE'
go
execute sp_addextendedproperty 'MS_Description', 'Client ayant donné l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'PER_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Restaurant concerné par l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'LIE_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_DATE_CREATION'
go
