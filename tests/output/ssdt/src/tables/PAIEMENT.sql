----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table PAIEMENT.
-- ===========================================================================================

create table PAIEMENT (
	ID int,
	SWI_ID int,
	constraint PK_PAIEMENT primary key clustered (ID asc, SWI_ID asc),
	constraint FK_PAIEMENT_ID foreign key (ID) references FACTURE (ID))
go

/* Index on foreign key column for PAIEMENT.ID */
create nonclustered index IDX_PAIEMENT_ID_FK
	on PAIEMENT (ID asc)
go

/* Index on foreign key column for PAIEMENT.SWI_ID */
create nonclustered index IDX_PAIEMENT_SWI_ID_FK
	on PAIEMENT (SWI_ID asc)
go

/**
  * Commentaires pour la table PAIEMENT
 **/
execute sp_addextendedproperty 'MS_Description', 'Paiement', 'SCHEMA', 'dbo', 'TABLE', 'PAIEMENT'
go
execute sp_addextendedproperty 'MS_Description', 'Facture associée au paiement', 'SCHEMA', 'dbo', 'TABLE', 'PAIEMENT', 'COLUMN', 'ID'
go
execute sp_addextendedproperty 'MS_Description', 'Carte Swile utilisée pour le paiement', 'SCHEMA', 'dbo', 'TABLE', 'PAIEMENT', 'COLUMN', 'SWI_ID'
go
