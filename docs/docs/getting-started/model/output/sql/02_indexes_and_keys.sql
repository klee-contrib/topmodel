----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Hello World 
--   Script Name		:	02_indexes_and_keys.sql
--   Description		:	Script de création des indexes et des clés étrangères et uniques.
-- ===========================================================================================

/**
  * Création de l'index de clef étrangère pour utilisateur.tut_code
 **/
create index idx_uti_tut_code_fk on utilisateur (
	tut_code ASC
);

/**
  * Génération de la contrainte de clef étrangère pour utilisateur.tut_code
 **/
alter table utilisateur
	add constraint fk_utilisateur_tut_code foreign key (tut_code)
		references type_utilisateur (tut_code);
