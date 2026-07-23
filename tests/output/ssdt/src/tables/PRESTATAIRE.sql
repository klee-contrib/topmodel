----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PRESTATAIRE.
-- ===========================================================================================

create table PRESTATAIRE (
	PST_ID int identity,
	PST_NOM varchar(100) not null,
	PST_PRENOM varchar(100) not null,
	PST_TELEPHONE varchar(20),
	constraint PK_PRESTATAIRE primary key clustered (PST_ID asc))
go

/* Index IDX_PST_PST_NOM_PST_PRENOM on PRESTATAIRE */
create nonclustered index IDX_PST_PST_NOM_PST_PRENOM
	on PRESTATAIRE (PST_NOM asc, PST_PRENOM asc)
go

/* Index IDX_PST_PST_TELEPHONE on PRESTATAIRE */
create nonclustered index IDX_PST_PST_TELEPHONE
	on PRESTATAIRE (PST_TELEPHONE asc)
go

/**
  * Commentaires pour la table PRESTATAIRE
 **/
execute sp_addextendedproperty 'MS_Description', 'Prestaire du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE'
go
execute sp_addextendedproperty 'MS_Description', 'Identifiant de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Nom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_NOM'
go
execute sp_addextendedproperty 'MS_Description', 'Prénom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_PRENOM'
go
execute sp_addextendedproperty 'MS_Description', 'Numéro de téléphone de l''employé.', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_TELEPHONE'
go
