----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table CLIENT.
-- ===========================================================================================

create table CLIENT (
	CLI_EMAIL varchar(100),
	SWI_ID int,
	PER_ID int,
	constraint PK_CLIENT primary key clustered (PER_ID asc),
	constraint FK_CLIENT_PER_ID foreign key (PER_ID) references PERSONNE (PER_ID))
go

/* Index on foreign key column for CLIENT.SWI_ID */
create nonclustered index IDX_CLI_SWI_ID_FK
	on CLIENT (SWI_ID asc)
go

/* Index on foreign key column for CLIENT.PER_ID */
create nonclustered index IDX_CLI_PER_ID_FK
	on CLIENT (PER_ID asc)
go

/**
  * Commentaires pour la table CLIENT
 **/
execute sp_addextendedproperty 'MS_Description', 'Client du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT'
go
execute sp_addextendedproperty 'MS_Description', 'Adresse email du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'CLI_EMAIL'
go
execute sp_addextendedproperty 'MS_Description', 'Carte Swile du client', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'SWI_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'CLIENT', 'COLUMN', 'PER_ID'
go
