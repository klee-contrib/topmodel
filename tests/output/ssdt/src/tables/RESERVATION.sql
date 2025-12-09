----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table RESERVATION.
-- ===========================================================================================

create table [dbo].[RESERVATION] (
	[REV_ID] int identity,
	[REV_DATE_RESERVATION] timestamp not null,
	[REV_NOMBRE_PERSONNES] int not null,
	[REV_COMMENTAIRE] varchar,
	[REV_CONFIRMEE] boolean not null default false,
	[CLI_ID_CLIENT] int not null,
	[TAB_ID_TABLE] int,
	[RES_ID_RESTAURANT] int not null,
	constraint [PK_RESERVATION] primary key clustered ([REV_ID] ASC),
	constraint [FK_RESERVATION_CLIENT_CLI_ID_CLIENT] foreign key ([CLI_ID_CLIENT]) references [dbo].[CLIENT] ([CLI_ID]),
	constraint [FK_RESERVATION_TABLE_CLIENT_TAB_ID_TABLE] foreign key ([TAB_ID_TABLE]) references [dbo].[TABLE_CLIENT] ([TAB_ID]),
	constraint [FK_RESERVATION_RESTAURANT_RES_ID_RESTAURANT] foreign key ([RES_ID_RESTAURANT]) references [dbo].[RESTAURANT] ([RES_ID]),
	constraint [UK_RESERVATION_TAB_ID_TABLE_REV_DATE_RESERVATION] unique nonclustered ([TAB_ID_TABLE] ASC, [REV_DATE_RESERVATION] ASC))
go

/* Index on foreign key column for RESERVATION.CLI_ID_CLIENT */
create nonclustered index [IDX_RESERVATION_CLI_ID_CLIENT_FK]
	on [dbo].[RESERVATION] ([CLI_ID_CLIENT] ASC)
go

/* Index on foreign key column for RESERVATION.TAB_ID_TABLE */
create nonclustered index [IDX_RESERVATION_TAB_ID_TABLE_FK]
	on [dbo].[RESERVATION] ([TAB_ID_TABLE] ASC)
go

/* Index on foreign key column for RESERVATION.RES_ID_RESTAURANT */
create nonclustered index [IDX_RESERVATION_RES_ID_RESTAURANT_FK]
	on [dbo].[RESERVATION] ([RES_ID_RESTAURANT] ASC)
go

/**
  * Commentaires pour la table RESERVATION
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Réservation d''une table', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'REV_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date et heure de la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'REV_DATE_RESERVATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nombre de personnes', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'REV_NOMBRE_PERSONNES'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Commentaire sur la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'REV_COMMENTAIRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si la réservation est confirmée', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'REV_CONFIRMEE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Client ayant fait la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'CLI_ID_CLIENT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Table réservée', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'TAB_ID_TABLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant concerné par la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'RES_ID_RESTAURANT'
go
