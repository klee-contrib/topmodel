----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table FACTURE.
-- ===========================================================================================

create table FACTURE (
	ID int identity,
	COM_ID int not null,
	constraint PK_FACTURE primary key clustered (ID asc),
	constraint FK_FACTURE_COM_ID foreign key (COM_ID) references COMMANDE (COM_ID))
go

/* Index on foreign key column for FACTURE.COM_ID */
create nonclustered index IDX_FACTURE_COM_ID_FK
	on FACTURE (COM_ID asc)
go

/**
  * Commentaires pour la table FACTURE
 **/
execute sp_addextendedproperty 'MS_Description', 'Facture', 'SCHEMA', 'dbo', 'TABLE', 'FACTURE'
go
execute sp_addextendedproperty 'MS_Description', 'Identifiant de la facture', 'SCHEMA', 'dbo', 'TABLE', 'FACTURE', 'COLUMN', 'ID'
go
execute sp_addextendedproperty 'MS_Description', 'Commande associée à la facture', 'SCHEMA', 'dbo', 'TABLE', 'FACTURE', 'COLUMN', 'COM_ID'
go
