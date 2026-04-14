----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table MENU.
-- ===========================================================================================

create table [dbo].[MENU] (
	[MEN_ID] int identity,
	[MEN_NOM] varchar not null,
	[MEN_DESCRIPTION] varchar,
	[MEN_PRIX] decimal not null,
	[MEN_DISPONIBLE] boolean not null default true,
	[MEN_DATE_DEBUT] timestamp,
	[MEN_DATE_FIN] timestamp,
	[RES_ID] int not null,
	constraint [PK_MENU] primary key clustered ([MEN_ID] ASC),
	constraint [FK_MENU_RES_ID] foreign key ([RES_ID]) references [dbo].[RESTAURANT] ([RES_ID]))
go

/* Index on foreign key column for MENU.RES_ID */
create nonclustered index [IDX_MEN_RES_ID_FK]
	on [dbo].[MENU] ([RES_ID] ASC)
go

/**
  * Commentaires pour la table MENU
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Menu du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'MENU'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Description du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DESCRIPTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prix du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_PRIX'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si le menu est disponible', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DISPONIBLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de début de validité du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DATE_DEBUT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de fin de validité du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DATE_FIN'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant proposant ce menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'RES_ID'
go
