----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	02_indexes_and_keys.sql
--   Description		:	Script de création des indexes et des clés étrangères et uniques.
-- ===========================================================================================

/**
  * Création de l'index de clef étrangère pour avis_client.per_id
 **/
create index idx_avi_per_id_fk on avis_client (
	per_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour avis_client.per_id
 **/
alter table avis_client
	add constraint fk_avis_client_per_id foreign key (per_id)
		references client (per_id);

/**
  * Création de l'index de clef étrangère pour avis_client.lie_id
 **/
create index idx_avi_lie_id_fk on avis_client (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour avis_client.lie_id
 **/
alter table avis_client
	add constraint fk_avis_client_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Création de l'index de clef étrangère pour categorie_plat_region.reg_code
 **/
create index idx_categorie_plat_region_reg_code_fk on categorie_plat_region (
	reg_code ASC
);

/**
  * Génération de la contrainte de clef étrangère pour categorie_plat_region.reg_code
 **/
alter table categorie_plat_region
	add constraint fk_categorie_plat_region_reg_code foreign key (reg_code)
		references region (reg_code);

/**
  * Création de l'index de clef étrangère pour categorie_plat_region.cat_code
 **/
create index idx_categorie_plat_region_cat_code_fk on categorie_plat_region (
	cat_code ASC
);

/**
  * Génération de la contrainte de clef étrangère pour categorie_plat_region.cat_code
 **/
alter table categorie_plat_region
	add constraint fk_categorie_plat_region_cat_code foreign key (cat_code)
		references categorie_plat (cat_code);

/**
  * Création de l'index de clef étrangère pour client.per_id
 **/
create index idx_cli_per_id_fk on client (
	per_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour client.per_id
 **/
alter table client
	add constraint fk_client_per_id foreign key (per_id)
		references personne (per_id);

/**
  * Création de l'index de clef étrangère pour commande.per_id
 **/
create index idx_com_per_id_fk on commande (
	per_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour commande.per_id
 **/
alter table commande
	add constraint fk_commande_per_id foreign key (per_id)
		references client (per_id);

/**
  * Création de l'index de clef étrangère pour commande.tab_id
 **/
create index idx_com_tab_id_fk on commande (
	tab_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour commande.tab_id
 **/
alter table commande
	add constraint fk_commande_tab_id foreign key (tab_id)
		references table_restaurant (tab_id);

/**
  * Création de l'index de clef étrangère pour commande.rev_id
 **/
create index idx_com_rev_id_fk on commande (
	rev_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour commande.rev_id
 **/
alter table commande
	add constraint fk_commande_rev_id foreign key (rev_id)
		references reservation (rev_id);

/**
  * Création de l'index de clef étrangère pour commande.stc_code
 **/
create index idx_com_stc_code_fk on commande (
	stc_code ASC
);

/**
  * Création de l'index de clef étrangère pour commande.avi_id
 **/
create index idx_com_avi_id_fk on commande (
	avi_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour commande.avi_id
 **/
alter table commande
	add constraint fk_commande_avi_id foreign key (avi_id)
		references avis_client (avi_id);

/**
  * Création de l'index de clef étrangère pour departement.reg_code
 **/
create index idx_dep_reg_code_fk on departement (
	reg_code ASC
);

/**
  * Génération de la contrainte de clef étrangère pour departement.reg_code
 **/
alter table departement
	add constraint fk_departement_reg_code foreign key (reg_code)
		references region (reg_code);

/**
  * Création de l'index de clef étrangère pour employe.lie_id
 **/
create index idx_emp_lie_id_fk on employe (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour employe.lie_id
 **/
alter table employe
	add constraint fk_employe_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Création de l'index de clef étrangère pour employe.per_id
 **/
create index idx_emp_per_id_fk on employe (
	per_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour employe.per_id
 **/
alter table employe
	add constraint fk_employe_per_id foreign key (per_id)
		references personne (per_id);

/**
  * Création de l'index de clef étrangère pour ligne_commande.com_id
 **/
create index idx_lig_com_id_fk on ligne_commande (
	com_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour ligne_commande.com_id
 **/
alter table ligne_commande
	add constraint fk_ligne_commande_com_id foreign key (com_id)
		references commande (com_id);

/**
  * Création de l'index de clef étrangère pour ligne_commande.pla_id
 **/
create index idx_lig_pla_id_fk on ligne_commande (
	pla_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour ligne_commande.pla_id
 **/
alter table ligne_commande
	add constraint fk_ligne_commande_pla_id foreign key (pla_id)
		references plat (pla_id);

/**
  * Création de l'index de clef étrangère pour ligne_commande_historique.com_id
 **/
create index idx_ligne_commande_historique_com_id_fk on ligne_commande_historique (
	com_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour ligne_commande_historique.com_id
 **/
alter table ligne_commande_historique
	add constraint fk_ligne_commande_historique_com_id foreign key (com_id)
		references commande_historique (com_id);

/**
  * Création de l'index de clef étrangère pour menu.lie_id
 **/
create index idx_men_lie_id_fk on menu (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour menu.lie_id
 **/
alter table menu
	add constraint fk_menu_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Création de l'index de clef étrangère pour menu_plat.men_id
 **/
create index idx_mpl_men_id_fk on menu_plat (
	men_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour menu_plat.men_id
 **/
alter table menu_plat
	add constraint fk_menu_plat_men_id foreign key (men_id)
		references menu (men_id);

/**
  * Création de l'index de clef étrangère pour menu_plat.pla_id
 **/
create index idx_mpl_pla_id_fk on menu_plat (
	pla_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour menu_plat.pla_id
 **/
alter table menu_plat
	add constraint fk_menu_plat_pla_id foreign key (pla_id)
		references plat (pla_id);

/**
  * Création de l'index de clef étrangère pour personne.dep_code
 **/
create index idx_per_dep_code_fk on personne (
	dep_code ASC
);

/**
  * Génération de la contrainte de clef étrangère pour personne.dep_code
 **/
alter table personne
	add constraint fk_personne_dep_code foreign key (dep_code)
		references departement (dep_code);

/**
  * Création de l'index de clef étrangère pour plat.cat_code
 **/
create index idx_pla_cat_code_fk on plat (
	cat_code ASC
);

/**
  * Génération de la contrainte de clef étrangère pour plat.cat_code
 **/
alter table plat
	add constraint fk_plat_cat_code foreign key (cat_code)
		references categorie_plat (cat_code);

/**
  * Création de l'index de clef étrangère pour plat.lie_id
 **/
create index idx_pla_lie_id_fk on plat (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour plat.lie_id
 **/
alter table plat
	add constraint fk_plat_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Génération de la contrainte de clef étrangère pour promotion.pla_id
 **/
alter table promotion
	add constraint fk_promotion_pla_id foreign key (pla_id)
		references plat (pla_id);

/**
  * Création de l'index de clef étrangère pour promotion.lie_id
 **/
create index idx_pro_lie_id_fk on promotion (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour promotion.lie_id
 **/
alter table promotion
	add constraint fk_promotion_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Création de l'index de clef étrangère pour reservation.per_id
 **/
create index idx_rev_per_id_fk on reservation (
	per_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour reservation.per_id
 **/
alter table reservation
	add constraint fk_reservation_per_id foreign key (per_id)
		references client (per_id);

/**
  * Création de l'index de clef étrangère pour reservation.tab_id
 **/
create index idx_rev_tab_id_fk on reservation (
	tab_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour reservation.tab_id
 **/
alter table reservation
	add constraint fk_reservation_tab_id foreign key (tab_id)
		references table_restaurant (tab_id);

/**
  * Création de l'index de clef étrangère pour reservation.lie_id
 **/
create index idx_rev_lie_id_fk on reservation (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour reservation.lie_id
 **/
alter table reservation
	add constraint fk_reservation_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Création de l'index de clef étrangère pour table_restaurant.lie_id
 **/
create index idx_tab_lie_id_fk on table_restaurant (
	lie_id ASC
);

/**
  * Génération de la contrainte de clef étrangère pour table_restaurant.lie_id
 **/
alter table table_restaurant
	add constraint fk_table_restaurant_lie_id foreign key (lie_id)
		references lieu (lie_id);

/**
  * Création de l'index uk_avis_client_per_id_lie_id_avi_date_avis sur avis_client.
 **/
alter table avis_client add constraint uk_avis_client_per_id_lie_id_avi_date_avis unique (per_id, lie_id, avi_date_avis);

/**
  * Création de l'index uk_categorie_plat_cat_ordre sur categorie_plat.
 **/
alter table categorie_plat add constraint uk_categorie_plat_cat_ordre unique (cat_ordre);

/**
  * Création de l'index uk_commande_avi_id sur commande.
 **/
alter table commande add constraint uk_commande_avi_id unique (avi_id);

/**
  * Création de l'index idx_emp_emp_telephone sur employe.
 **/
create index idx_emp_emp_telephone on employe (
	emp_telephone ASC
);

/**
  * Création de l'index uk_employe_emp_matricule sur employe.
 **/
alter table employe add constraint uk_employe_emp_matricule unique (emp_matricule);

/**
  * Création de l'index uk_ligne_commande_com_id_pla_id sur ligne_commande.
 **/
alter table ligne_commande add constraint uk_ligne_commande_com_id_pla_id unique (com_id, pla_id);

/**
  * Création de l'index uk_menu_plat_men_id_mpl_ordre sur menu_plat.
 **/
alter table menu_plat add constraint uk_menu_plat_men_id_mpl_ordre unique (men_id, mpl_ordre);

/**
  * Création de l'index idx_pst_pst_nom_pst_prenom sur prestataire.
 **/
create index idx_pst_pst_nom_pst_prenom on prestataire (
	pst_nom ASC, pst_prenom ASC
);

/**
  * Création de l'index idx_pst_pst_telephone sur prestataire.
 **/
create index idx_pst_pst_telephone on prestataire (
	pst_telephone ASC
);

/**
  * Création de l'index uk_reservation_tab_id_rev_date_reservation sur reservation.
 **/
alter table reservation add constraint uk_reservation_tab_id_rev_date_reservation unique (tab_id, rev_date_reservation);

/**
  * Création de l'index uk_table_restaurant_lie_id_tab_numero sur table_restaurant.
 **/
alter table table_restaurant add constraint uk_table_restaurant_lie_id_tab_numero unique (lie_id, tab_numero);

/**
  * Création de l'index idx_cat_cat_libelle sur categorie_plat.
 **/
create index idx_cat_cat_libelle on categorie_plat (
	cat_libelle ASC
);

/**
  * Création de l'index idx_dep_dep_libelle sur departement.
 **/
create index idx_dep_dep_libelle on departement (
	dep_libelle ASC
);

/**
  * Création de l'index idx_reg_reg_libelle sur region.
 **/
create index idx_reg_reg_libelle on region (
	reg_libelle ASC
);
