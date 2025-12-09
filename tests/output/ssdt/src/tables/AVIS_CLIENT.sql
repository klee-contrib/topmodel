----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table AVIS_CLIENT.
-- ===========================================================================================

create table [dbo].[AVIS_CLIENT] (
	[AVI_ID] int identity,
	[AVI_NOTE] int not null,
	[AVI_COMMENTAIRE] varchar,
	[AVI_DATE_AVIS] timestamp not null,
	[AVI_APPROUVE] boolean not null default false,
	[AVI_NOMBRE_VUES] int not null default 0,
	[CLI_ID_CLIENT] int not null,
	[RES_ID_RESTAURANT] int not null,
	constraint [PK_AVIS_CLIENT] primary key clustered ([AVI_ID] ASC),
	constraint [FK_AVIS_CLIENT_CLIENT_CLI_ID_CLIENT] foreign key ([CLI_ID_CLIENT]) references [dbo].[CLIENT] ([CLI_ID]),
	constraint [FK_AVIS_CLIENT_RESTAURANT_RES_ID_RESTAURANT] foreign key ([RES_ID_RESTAURANT]) references [dbo].[RESTAURANT] ([RES_ID]),
	constraint [UK_AVIS_CLIENT_CLI_ID_CLIENT_RES_ID_RESTAURANT_AVI_DATE_AVIS] unique nonclustered ([CLI_ID_CLIENT] ASC, [RES_ID_RESTAURANT] ASC, [AVI_DATE_AVIS] ASC))
go

/* Index on foreign key column for AVIS_CLIENT.CLI_ID_CLIENT */
create nonclustered index [IDX_AVIS_CLIENT_CLI_ID_CLIENT_FK]
	on [dbo].[AVIS_CLIENT] ([CLI_ID_CLIENT] ASC)
go

/* Index on foreign key column for AVIS_CLIENT.RES_ID_RESTAURANT */
create nonclustered index [IDX_AVIS_CLIENT_RES_ID_RESTAURANT_FK]
	on [dbo].[AVIS_CLIENT] ([RES_ID_RESTAURANT] ASC)
go

/**
  * Commentaires pour la table AVIS_CLIENT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Avis d''un client sur un restaurant', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Note sur 5', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_NOTE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Commentaire de l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_COMMENTAIRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_DATE_AVIS'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si l''avis est approuvé par le restaurant', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_APPROUVE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nombre de vues de l''avis (calculé)', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'AVI_NOMBRE_VUES'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Client ayant donné l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'CLI_ID_CLIENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant concerné par l''avis', 'SCHEMA', 'dbo', 'TABLE', 'AVIS_CLIENT', 'COLUMN', 'RES_ID_RESTAURANT'
go
