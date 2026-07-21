----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table VERRE.
-- ===========================================================================================

create table VERRE (
	VSL_ID int,
	VSL_DESCRIPTION varchar(100) not null,
	VRR_A_PIED boolean not null,
	constraint PK_VERRE primary key clustered (VSL_ID asc))
go

/**
  * Commentaires pour la table VERRE
 **/
EXECUTE sp_addextendedproperty 'MS_Description', 'Verre.', 'SCHEMA', 'dbo', 'TABLE', 'VERRE'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Id de la vaisselle', 'SCHEMA', 'dbo', 'TABLE', 'VERRE', 'COLUMN', 'VSL_ID'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Description de la vaisselle.', 'SCHEMA', 'dbo', 'TABLE', 'VERRE', 'COLUMN', 'VSL_DESCRIPTION'
go
EXECUTE sp_addextendedproperty 'MS_Description', 'Si le verre est à pied ou non.', 'SCHEMA', 'dbo', 'TABLE', 'VERRE', 'COLUMN', 'VRR_A_PIED'
go
