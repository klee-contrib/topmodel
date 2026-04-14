----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TABLE.
-- ===========================================================================================

create table [dbo].[TABLE] (
	[TAB_ID] int identity,
	[TAB_NUMERO] varchar not null,
	[TAB_CAPACITE] int not null,
	[TAB_DISPONIBLE] boolean not null default true,
	[RES_ID] int not null,
	constraint [PK_TABLE] primary key clustered ([TAB_ID] ASC),
	constraint [FK_TABLE_RES_ID] foreign key ([RES_ID]) references [dbo].[RESTAURANT] ([RES_ID]),
	constraint [UK_TABLE_RES_ID_TAB_NUMERO] unique nonclustered ([RES_ID] ASC, [TAB_NUMERO] ASC))
go

/* Index on foreign key column for TABLE.RES_ID */
create nonclustered index [IDX_TAB_RES_ID_FK]
	on [dbo].[TABLE] ([RES_ID] ASC)
go

/**
  * Commentaires pour la table TABLE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Table du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'TABLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE', 'COLUMN', 'TAB_NUMERO'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Capacité de la table (nombre de places)', 'SCHEMA', 'dbo', 'TABLE', 'TABLE', 'COLUMN', 'TAB_CAPACITE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si la table est disponible', 'SCHEMA', 'dbo', 'TABLE', 'TABLE', 'COLUMN', 'TAB_DISPONIBLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant auquel appartient la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE', 'COLUMN', 'RES_ID'
go
