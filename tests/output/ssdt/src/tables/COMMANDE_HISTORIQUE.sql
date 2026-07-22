----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table COMMANDE_HISTORIQUE.
-- ===========================================================================================

create table COMMANDE_HISTORIQUE (
	COM_ID int,
	COM_DATE_COMMANDE timestamp not null,
	COM_DATE_LIVRAISON timestamp,
	COM_MONTANT_TOTAL decimal not null,
	PER_ID int not null,
	TAB_ID int,
	REV_ID int,
	STC_CODE varchar(10) not null default N'EN_ATT',
	AVI_ID int,
	COM_DATE_CREATION timestamp not null,
	constraint PK_COMMANDE_HISTORIQUE primary key clustered (COM_ID asc))
go

/**
  * Commentaires pour la table COMMANDE_HISTORIQUE
 **/
execute sp_addextendedproperty 'MS_Description', 'Commande pour historique avec préservation des clés primaires', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE'
go
execute sp_addextendedproperty 'MS_Description', 'Identifiant de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Date et heure de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_DATE_COMMANDE'
go
execute sp_addextendedproperty 'MS_Description', 'Date et heure de livraison', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_DATE_LIVRAISON'
go
execute sp_addextendedproperty 'MS_Description', 'Montant total de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_MONTANT_TOTAL'
go
execute sp_addextendedproperty 'MS_Description', 'Client ayant passé la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'PER_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Table associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'TAB_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Réservation associée à la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'REV_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Statut de la commande', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'STC_CODE'
go
execute sp_addextendedproperty 'MS_Description', 'Avis laissé par le client sur la commande.', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'AVI_ID'
go
execute sp_addextendedproperty 'MS_Description', 'Date de création de l''enregistrement', 'SCHEMA', 'dbo', 'TABLE', 'COMMANDE_HISTORIQUE', 'COLUMN', 'COM_DATE_CREATION'
go
