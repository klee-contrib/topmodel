----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PRESTATAIRE.
-- ===========================================================================================

create table [dbo].[PRESTATAIRE] (
	[PST_ID] int identity,
	[PST_NOM] varchar not null,
	[PST_PRENOM] varchar not null,
	[PST_TELEPHONE] varchar,
	constraint [PK_PRESTATAIRE] primary key clustered ([PST_ID] ASC))
go

/* Index IDX_PST_PST_NOM_PST_PRENOM on PRESTATAIRE */
create nonclustered index [IDX_PST_PST_NOM_PST_PRENOM]
	on [dbo].[PRESTATAIRE] ([PST_NOM] ASC, [PST_PRENOM] ASC)
go

/* Index IDX_PST_PST_TELEPHONE on PRESTATAIRE */
create nonclustered index [IDX_PST_PST_TELEPHONE]
	on [dbo].[PRESTATAIRE] ([PST_TELEPHONE] ASC)
go

/**
  * Commentaires pour la table PRESTATAIRE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Prestaire du restaurant', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Identifiant de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Nom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_NOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Prénom de la personne', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_PRENOM'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone de l''employé.', 'SCHEMA', 'dbo', 'TABLE', 'PRESTATAIRE', 'COLUMN', 'PST_TELEPHONE'
go
