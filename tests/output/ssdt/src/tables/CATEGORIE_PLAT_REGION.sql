----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Description		:	Création de la table CATEGORIE_PLAT_REGION.
-- ===========================================================================================

create table CATEGORIE_PLAT_REGION (
	REG_CODE varchar(10),
	CAT_CODE varchar(10),
	constraint PK_CATEGORIE_PLAT_REGION primary key clustered (REG_CODE asc, CAT_CODE asc),
	constraint FK_CATEGORIE_PLAT_REGION_REG_CODE foreign key (REG_CODE) references REGION (REG_CODE),
	constraint FK_CATEGORIE_PLAT_REGION_CAT_CODE foreign key (CAT_CODE) references CATEGORIE_PLAT (CAT_CODE))
go

/* Index on foreign key column for CATEGORIE_PLAT_REGION.REG_CODE */
create nonclustered index IDX_CATEGORIE_PLAT_REGION_REG_CODE_FK
	on CATEGORIE_PLAT_REGION (REG_CODE asc)
go

/* Index on foreign key column for CATEGORIE_PLAT_REGION.CAT_CODE */
create nonclustered index IDX_CATEGORIE_PLAT_REGION_CAT_CODE_FK
	on CATEGORIE_PLAT_REGION (CAT_CODE asc)
go

/**
  * Commentaires pour la table CATEGORIE_PLAT_REGION
 **/
execute sp_addextendedproperty 'MS_Description', 'Catégories de plats disponibles par région', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT_REGION'
go
execute sp_addextendedproperty 'MS_Description', 'Région', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT_REGION', 'COLUMN', 'REG_CODE'
go
execute sp_addextendedproperty 'MS_Description', 'Catégorie de plat', 'SCHEMA', 'dbo', 'TABLE', 'CATEGORIE_PLAT_REGION', 'COLUMN', 'CAT_CODE'
go
