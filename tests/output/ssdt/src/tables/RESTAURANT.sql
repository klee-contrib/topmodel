----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table RESTAURANT.
-- ===========================================================================================

create table [dbo].[RESTAURANT] (
	[RES_TELEPHONE] varchar,
	[RES_DATE_CREATION] timestamp not null,
	[LIE_ID] int,
	constraint [PK_RESTAURANT] primary key clustered ([LIE_ID] ASC),
	constraint [FK_RESTAURANT_LIE_ID] foreign key ([LIE_ID]) references [dbo].[LIEU] ([LIE_ID]))
go

/* Index on foreign key column for RESTAURANT.LIE_ID */
create nonclustered index [IDX_RES_LIE_ID_FK]
	on [dbo].[RESTAURANT] ([LIE_ID] ASC)
go

/**
  * Commentaires pour la table RESTAURANT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'RES_DATE_CREATION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'RESTAURANT', 'COLUMN', 'LIE_ID'
go
