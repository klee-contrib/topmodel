----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table SECTEUR.
-- ===========================================================================================

create table [dbo].[SECTEUR] (
	[SEC_ID] int8 identity,
	[PRO_ID] int8,
	constraint [PK_SECTEUR] primary key clustered ([SEC_ID] ASC),
	constraint [FK_SECTEUR_PROFIL_PRO_ID] foreign key ([PRO_ID]) references [dbo].[PROFIL] ([PRO_ID]))
go

/* Index on foreign key column for SECTEUR.PRO_ID */
create nonclustered index [IDX_SECTEUR_PRO_ID_FK]
	on [dbo].[SECTEUR] ([PRO_ID] ASC)
go

/* Description property. */
EXECUTE sp_addextendedproperty 'Description', 'Secteur', 'SCHEMA', 'dbo', 'TABLE', 'SECTEUR';
