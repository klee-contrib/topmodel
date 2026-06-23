----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table RESERVATION.
-- ===========================================================================================

create table [dbo].[RESERVATION] (
	[REV_ID] int,
	[REV_DATE_RESERVATION] timestamp not null,
	[REV_NOMBRE_PERSONNES] int not null,
	[REV_COMMENTAIRE] varchar(100),
	[REV_CONFIRMEE] boolean not null default false,
	[PER_ID] int not null,
	[TAB_ID] int,
	[LIE_ID] int not null,
	[REV_DATE_CREATION] timestamp not null,
	constraint [PK_RESERVATION] primary key clustered ([REV_ID] ASC),
	constraint [FK_RESERVATION_PER_ID] foreign key ([PER_ID]) references [dbo].[CLIENT] ([PER_ID]),
	constraint [FK_RESERVATION_TAB_ID] foreign key ([TAB_ID]) references [dbo].[TABLE_RESTAURANT] ([TAB_ID]),
	constraint [FK_RESERVATION_LIE_ID] foreign key ([LIE_ID]) references [dbo].[LIEU] ([LIE_ID]),
	constraint [UK_RESERVATION_TAB_ID_REV_DATE_RESERVATION] unique nonclustered ([TAB_ID] ASC, [REV_DATE_RESERVATION] ASC))
go

/**
  * Création de la séquence pour la clé primaire de la table RESERVATION
 **/
create sequence SEQ_RESERVATION as int start with 1000 increment by 50
go

/* Index on foreign key column for RESERVATION.PER_ID */
create nonclustered index [IDX_REV_PER_ID_FK]
	on [dbo].[RESERVATION] ([PER_ID] ASC)
go

/* Index on foreign key column for RESERVATION.TAB_ID */
create nonclustered index [IDX_REV_TAB_ID_FK]
	on [dbo].[RESERVATION] ([TAB_ID] ASC)
go

/* Index on foreign key column for RESERVATION.LIE_ID */
create nonclustered index [IDX_REV_LIE_ID_FK]
	on [dbo].[RESERVATION] ([LIE_ID] ASC)
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
EXECUTE sp_addextendedproperty 'MS_Description', 'Client ayant fait la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'PER_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Table réservée', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant concerné par la réservation', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'LIE_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'RESERVATION', 'COLUMN', 'REV_DATE_CREATION'
go
