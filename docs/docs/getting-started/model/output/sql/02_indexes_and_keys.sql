----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	02_indexes_and_keys.sql
--   Description		:	Script de création des indexes et des clés étrangères et uniques.
-- ===========================================================================================

/**
  * Création de l'index de clef étrangère pour UTILISATEUR.CODE
 **/
create index IDX_UTILISATEUR_CODE_FK on UTILISATEUR (
	CODE ASC
);

/**
  * Génération de la contrainte de clef étrangère pour UTILISATEUR.CODE
 **/
alter table UTILISATEUR
	add constraint FK_UTILISATEUR_CODE foreign key (CODE)
		references TYPE_UTILISATEUR (CODE);
