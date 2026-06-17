----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table FOURNISSEUR.
-- ===========================================================================================

create table [dbo].[FOURNISSEUR] (
	[FRN_TELEPHONE] varchar,
	[FRN_BIO] boolean,
	[LIE_ID] int,
	constraint [PK_FOURNISSEUR] primary key clustered ([LIE_ID] ASC),
	constraint [FK_FOURNISSEUR_LIE_ID] foreign key ([LIE_ID]) references [dbo].[LIEU] ([LIE_ID]))
go

/* Index on foreign key column for FOURNISSEUR.LIE_ID */
create nonclustered index [IDX_FRN_LIE_ID_FK]
	on [dbo].[FOURNISSEUR] ([LIE_ID] ASC)
go

/**
  * Commentaires pour la table FOURNISSEUR
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Restaurant', 'SCHEMA', 'dbo', 'TABLE', 'FOURNISSEUR'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Numéro de téléphone', 'SCHEMA', 'dbo', 'TABLE', 'FOURNISSEUR', 'COLUMN', 'FRN_TELEPHONE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Si le fournisseur fait du bio.', 'SCHEMA', 'dbo', 'TABLE', 'FOURNISSEUR', 'COLUMN', 'FRN_BIO'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Association vers la clé primaire de la classe parente', 'SCHEMA', 'dbo', 'TABLE', 'FOURNISSEUR', 'COLUMN', 'LIE_ID'
go
