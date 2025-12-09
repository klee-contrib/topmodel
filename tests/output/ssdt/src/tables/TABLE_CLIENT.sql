----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TABLE_CLIENT.
-- ===========================================================================================

create table [dbo].[TABLE_CLIENT] (
	[TAB_ID] int identity,
	[TAB_NUMERO] varchar not null,
	[TAB_CAPACITE] int not null,
	[TAB_DISPONIBLE] boolean not null default true,
	[RES_ID_RESTAURANT] int not null,
	constraint [PK_TABLE_CLIENT] primary key clustered ([TAB_ID] ASC),
	constraint [FK_TABLE_CLIENT_RESTAURANT_RES_ID_RESTAURANT] foreign key ([RES_ID_RESTAURANT]) references [dbo].[RESTAURANT] ([RES_ID]),
	constraint [UK_TABLE_CLIENT_RES_ID_RESTAURANT_TAB_NUMERO] unique nonclustered ([RES_ID_RESTAURANT] ASC, [TAB_NUMERO] ASC))
go

/* Index on foreign key column for TABLE_CLIENT.RES_ID_RESTAURANT */
create nonclustered index [IDX_TABLE_CLIENT_RES_ID_RESTAURANT_FK]
	on [dbo].[TABLE_CLIENT] ([RES_ID_RESTAURANT] ASC)
go

/**
  * Commentaires pour la table TABLE_CLIENT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Table du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_CLIENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_CLIENT', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_CLIENT', 'COLUMN', 'TAB_NUMERO'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Capacité de la table (nombre de places)', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_CLIENT', 'COLUMN', 'TAB_CAPACITE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si la table est disponible', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_CLIENT', 'COLUMN', 'TAB_DISPONIBLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant auquel appartient la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_CLIENT', 'COLUMN', 'RES_ID_RESTAURANT'
go
