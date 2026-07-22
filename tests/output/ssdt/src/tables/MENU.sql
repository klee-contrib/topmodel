----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table MENU.
-- ===========================================================================================

create table MENU (
	MEN_ID int,
	MEN_NOM varchar(100) not null,
	MEN_DESCRIPTION varchar(100),
	MEN_PRIX decimal not null,
	MEN_DISPONIBLE boolean not null default true,
	MEN_DATE_DEBUT timestamp,
	MEN_DATE_FIN timestamp,
	LIE_ID int not null,
	MEN_DATE_CREATION timestamp not null,
	constraint PK_MENU primary key clustered (MEN_ID asc),
	constraint FK_MENU_LIE_ID foreign key (LIE_ID) references LIEU (LIE_ID))
go

/**
  * Création de la séquence pour la clé primaire de la table MENU
 **/
create sequence SEQ_MENU as int start with 1000 increment by 50
go

/* Index on foreign key column for MENU.LIE_ID */
create nonclustered index IDX_MEN_LIE_ID_FK
	on MENU (LIE_ID asc)
go

/**
  * Commentaires pour la table MENU
 **/
execute sp_addextendedproperty 'MS_Description', 'Menu du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'MENU'
go
execute sp_addextendedproperty 'MS_Description', 'Identifiant du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Nom du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_NOM'
go
execute sp_addextendedproperty 'MS_Description', 'Description du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DESCRIPTION'
go
execute sp_addextendedproperty 'MS_Description', 'Prix du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_PRIX'
go
execute sp_addextendedproperty 'MS_Description', 'Indique si le menu est disponible', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DISPONIBLE'
go
execute sp_addextendedproperty 'MS_Description', 'Date de début de validité du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DATE_DEBUT'
go
execute sp_addextendedproperty 'MS_Description', 'Date de fin de validité du menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DATE_FIN'
go
execute sp_addextendedproperty 'MS_Description', 'Restaurant proposant ce menu', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'LIE_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'MENU', 'COLUMN', 'MEN_DATE_CREATION'
go
