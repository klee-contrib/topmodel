----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table TABLE_RESTAURANT.
-- ===========================================================================================

create table TABLE_RESTAURANT (
	TAB_ID int identity,
	TAB_NUMERO varchar(10) not null,
	TAB_CAPACITE int not null,
	TAB_DISPONIBLE boolean not null default true,
	LIE_ID int not null,
	TAB_DATE_CREATION timestamp not null,
	constraint PK_TABLE_RESTAURANT primary key clustered (TAB_ID asc),
	constraint FK_TABLE_RESTAURANT_LIE_ID foreign key (LIE_ID) references LIEU (LIE_ID),
	constraint UK_TABLE_RESTAURANT_LIE_ID_TAB_NUMERO unique nonclustered (LIE_ID asc, TAB_NUMERO asc))
go

/* Index on foreign key column for TABLE_RESTAURANT.LIE_ID */
create nonclustered index IDX_TAB_LIE_ID_FK
	on TABLE_RESTAURANT (LIE_ID asc)
go

/**
  * Commentaires pour la table TABLE_RESTAURANT
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Table du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT', 'COLUMN', 'TAB_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT', 'COLUMN', 'TAB_NUMERO'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Capacité de la table (nombre de places)', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT', 'COLUMN', 'TAB_CAPACITE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Indique si la table est disponible', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT', 'COLUMN', 'TAB_DISPONIBLE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant auquel appartient la table', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT', 'COLUMN', 'LIE_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'TABLE_RESTAURANT', 'COLUMN', 'TAB_DATE_CREATION'
go
