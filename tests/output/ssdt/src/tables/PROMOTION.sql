----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PROMOTION.
-- ===========================================================================================

create table [dbo].[PROMOTION] (
	[PLA_ID] int,
	[PRO_LIBELLE] varchar(100) not null,
	[PRO_POURCENTAGE_REDUCTION] int not null,
	[PRO_DATE_DEBUT] timestamp not null,
	[PRO_DATE_FIN] timestamp not null,
	[PRO_ACTIVE] boolean not null default true,
	[LIE_ID] int,
	[PRO_DATE_CREATION] timestamp not null,
	constraint [PK_PROMOTION] primary key clustered ([PLA_ID] ASC),
	constraint [FK_PROMOTION_PLA_ID] foreign key ([PLA_ID]) references [dbo].[PLAT] ([PLA_ID]),
	constraint [FK_PROMOTION_LIE_ID] foreign key ([LIE_ID]) references [dbo].[LIEU] ([LIE_ID]))
go

/**
  * Création de la séquence pour la clé primaire de la table PROMOTION
 **/
create sequence SEQ_PROMOTION as int start with 1000 increment by 50
go

/* Index on foreign key column for PROMOTION.LIE_ID */
create nonclustered index [IDX_PRO_LIE_ID_FK]
	on [dbo].[PROMOTION] ([LIE_ID] ASC)
go

/**
  * Commentaires pour la table PROMOTION
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Promotion sur un plat', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Plat concerné par la promotion.', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PLA_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_LIBELLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Pourcentage de réduction (0-100)', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_POURCENTAGE_REDUCTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de début de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_DATE_DEBUT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de fin de la promotion', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_DATE_FIN'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si la promotion est active', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_ACTIVE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant concerné par la promotion (null si globale)', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'LIE_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'PROMOTION', 'COLUMN', 'PRO_DATE_CREATION'
go
