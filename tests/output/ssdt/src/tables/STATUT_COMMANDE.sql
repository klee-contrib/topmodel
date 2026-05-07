----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table STATUT_COMMANDE.
-- ===========================================================================================

create table [dbo].[STATUT_COMMANDE] (
	[STC_CODE] varchar,
	[STC_LIBELLE] varchar not null,
	constraint [PK_STATUT_COMMANDE] primary key clustered ([STC_CODE] ASC))
go

create nonclustered index [IDX_STC_STC_LIBELLE]
	on [dbo].[STATUT_COMMANDE] ([STC_LIBELLE] ASC)
go

/**
  * Commentaires pour la table STATUT_COMMANDE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Statut d''une commande', 'SCHEMA', 'dbo', 'TABLE', 'STATUT_COMMANDE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Code du statut', 'SCHEMA', 'dbo', 'TABLE', 'STATUT_COMMANDE', 'COLUMN', 'STC_CODE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Libellé du statut', 'SCHEMA', 'dbo', 'TABLE', 'STATUT_COMMANDE', 'COLUMN', 'STC_LIBELLE'
go
