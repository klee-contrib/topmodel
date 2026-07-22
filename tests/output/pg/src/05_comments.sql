----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	05_comments.sql
--   Description		:	Script de création de commentaires sur les tables et les colonnes.
-- ===========================================================================================

/**
  * Commentaires pour la table assiette
 **/
comment on table assiette is 'Assiette.';
comment on column assiette.vsl_id is 'Id de la vaisselle';
comment on column assiette.vsl_description is 'Description de la vaisselle.';
comment on column assiette.ast_taille is 'Taille de l''assiette.';

/**
  * Commentaires pour la table avis_client
 **/
comment on table avis_client is 'Avis d''un client sur un restaurant';
comment on column avis_client.avi_id is 'Identifiant de l''avis';
comment on column avis_client.avi_note is 'Note sur 5';
comment on column avis_client.avi_commentaire is 'Commentaire de l''avis';
comment on column avis_client.avi_date_avis is 'Date de l''avis';
comment on column avis_client.avi_approuve is 'Indique si l''avis est approuvé par le restaurant';
comment on column avis_client.per_id is 'Client ayant donné l''avis';
comment on column avis_client.lie_id is 'Restaurant concerné par l''avis';
comment on column avis_client.avi_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table categorie_plat
 **/
comment on table categorie_plat is 'Catégorie de plat';
comment on column categorie_plat.cat_code is 'Code de la catégorie';
comment on column categorie_plat.cat_libelle is 'Libellé de la catégorie';
comment on column categorie_plat.cat_ordre is 'Ordre d''affichage dans le menu.';
comment on column categorie_plat.cat_prix_moyen is 'Prix moyen de la catégorie, à titre indicatif.';

/**
  * Commentaires pour la table categorie_plat_region
 **/
comment on table categorie_plat_region is 'Catégories de plats disponibles par région';
comment on column categorie_plat_region.reg_code is 'Région';
comment on column categorie_plat_region.cat_code is 'Catégorie de plat';

/**
  * Commentaires pour la table client
 **/
comment on table client is 'Client du restaurant';
comment on column client.cli_email is 'Adresse email du client';
comment on column client.per_id is 'Association vers la clé primaire de la classe parente';

/**
  * Commentaires pour la table commande
 **/
comment on table commande is 'Commande d''un client';
comment on column commande.com_id is 'Identifiant de la commande';
comment on column commande.com_date_commande is 'Date et heure de la commande';
comment on column commande.com_date_livraison is 'Date et heure de livraison';
comment on column commande.com_montant_total is 'Montant total de la commande';
comment on column commande.per_id is 'Client ayant passé la commande';
comment on column commande.tab_id is 'Table associée à la commande';
comment on column commande.rev_id is 'Réservation associée à la commande';
comment on column commande.stc_code is 'Statut de la commande';
comment on column commande.avi_id is 'Avis laissé par le client sur la commande.';
comment on column commande.com_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table commande_historique
 **/
comment on table commande_historique is 'Commande pour historique avec préservation des clés primaires';
comment on column commande_historique.com_id is 'Identifiant de la commande';
comment on column commande_historique.com_date_commande is 'Date et heure de la commande';
comment on column commande_historique.com_date_livraison is 'Date et heure de livraison';
comment on column commande_historique.com_montant_total is 'Montant total de la commande';
comment on column commande_historique.per_id is 'Client ayant passé la commande';
comment on column commande_historique.tab_id is 'Table associée à la commande';
comment on column commande_historique.rev_id is 'Réservation associée à la commande';
comment on column commande_historique.stc_code is 'Statut de la commande';
comment on column commande_historique.avi_id is 'Avis laissé par le client sur la commande.';
comment on column commande_historique.com_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table couvert
 **/
comment on table couvert is 'Couvert.';
comment on column couvert.vsl_id is 'Id de la vaisselle';
comment on column couvert.vsl_description is 'Description de la vaisselle.';

/**
  * Commentaires pour la table departement
 **/
comment on table departement is 'Département';
comment on column departement.dep_code is 'Code du département.';
comment on column departement.dep_libelle is 'Libellé du département.';
comment on column departement.reg_code is 'Région associée.';

/**
  * Commentaires pour la table employe
 **/
comment on table employe is 'Employé du restaurant';
comment on column employe.emp_telephone is 'Numéro de téléphone de l''employé.';
comment on column employe.emp_date_naissance is 'Date de naissance';
comment on column employe.emp_matricule is 'Matricule de l''employé';
comment on column employe.emp_date_embauche is 'Date d''embauche';
comment on column employe.emp_salaire is 'Salaire de l''employé';
comment on column employe.lie_id is 'Restaurant où travaille l''employé';
comment on column employe.per_id is 'Association vers la clé primaire de la classe parente';

/**
  * Commentaires pour la table lieu
 **/
comment on table lieu is 'Lieu';
comment on column lieu.lie_id is 'Identifiant du restaurant';
comment on column lieu.lie_nom is 'Nom du restaurant';
comment on column lieu.lie_adresse is 'Adresse du restaurant';
comment on column lieu.lie_discriminator is 'Discriminateur pour les instances de la hiérarchie de classe';
comment on column lieu.res_telephone is 'Numéro de téléphone';
comment on column lieu.res_date_creation is 'Date de création de l''enregistrement';
comment on column lieu.frn_telephone is 'Numéro de téléphone';
comment on column lieu.frn_bio is 'Si le fournisseur fait du bio.';

/**
  * Commentaires pour la table ligne_commande
 **/
comment on table ligne_commande is 'Ligne d''une commande';
comment on column ligne_commande.lig_id is 'Identifiant de la ligne';
comment on column ligne_commande.lig_quantite is 'Quantité commandée';
comment on column ligne_commande.lig_prix_unitaire is 'Prix unitaire au moment de la commande';
comment on column ligne_commande.lig_prix_total is 'Prix total de la ligne';
comment on column ligne_commande.com_id is 'Commande à laquelle appartient la ligne';
comment on column ligne_commande.pla_id is 'Plat commandé';
comment on column ligne_commande.lig_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table ligne_commande_historique
 **/
comment on table ligne_commande_historique is 'Ligne de commande pour historique avec préservation des clés primaires';
comment on column ligne_commande_historique.lig_id is 'Identifiant de la ligne';
comment on column ligne_commande_historique.lig_quantite is 'Quantité commandée';
comment on column ligne_commande_historique.lig_prix_unitaire is 'Prix unitaire au moment de la commande';
comment on column ligne_commande_historique.lig_prix_total is 'Prix total de la ligne';
comment on column ligne_commande_historique.pla_id is 'Plat commandé';
comment on column ligne_commande_historique.lig_date_creation is 'Date de création de l''enregistrement';
comment on column ligne_commande_historique.com_id is 'Commande à laquelle appartient la ligne';

/**
  * Commentaires pour la table menu
 **/
comment on table menu is 'Menu du restaurant';
comment on column menu.men_id is 'Identifiant du menu';
comment on column menu.men_nom is 'Nom du menu';
comment on column menu.men_description is 'Description du menu';
comment on column menu.men_prix is 'Prix du menu';
comment on column menu.men_disponible is 'Indique si le menu est disponible';
comment on column menu.men_date_debut is 'Date de début de validité du menu';
comment on column menu.men_date_fin is 'Date de fin de validité du menu';
comment on column menu.lie_id is 'Restaurant proposant ce menu';
comment on column menu.men_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table menu_plat
 **/
comment on table menu_plat is 'Plat dans un menu';
comment on column menu_plat.men_id is 'Menu contenant ce plat';
comment on column menu_plat.pla_id is 'Plat du menu';
comment on column menu_plat.mpl_ordre is 'Ordre d''affichage du plat dans le menu';
comment on column menu_plat.mpl_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table personne
 **/
comment on table personne is 'Classe de base représentant une personne';
comment on column personne.per_id is 'Identifiant de la personne';
comment on column personne.per_nom is 'Nom de la personne';
comment on column personne.per_prenom is 'Prénom de la personne';
comment on column personne.dep_code is 'Département de résidence de la personne.';
comment on column personne.per_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table plat
 **/
comment on table plat is 'Plat du menu';
comment on column plat.pla_id is 'Identifiant du plat';
comment on column plat.pla_nom is 'Nom du plat';
comment on column plat.pla_description is 'Description du plat';
comment on column plat.pla_prix is 'Prix du plat';
comment on column plat.pla_disponible is 'Indique si le plat est disponible';
comment on column plat.cat_code is 'Catégorie du plat';
comment on column plat.lie_id is 'Restaurant proposant ce plat';
comment on column plat.pla_date_creation is 'Date de création de l''enregistrement';
comment on column plat.pbo_volume is 'Volume de la boisson';
comment on column plat.ppr_vegetarien is 'Si le plat est végétarien.';

/**
  * Commentaires pour la table prestataire
 **/
comment on table prestataire is 'Prestaire du restaurant';
comment on column prestataire.pst_id is 'Identifiant de la personne';
comment on column prestataire.pst_nom is 'Nom de la personne';
comment on column prestataire.pst_prenom is 'Prénom de la personne';
comment on column prestataire.pst_telephone is 'Numéro de téléphone de l''employé.';

/**
  * Commentaires pour la table promotion
 **/
comment on table promotion is 'Promotion sur un plat';
comment on column promotion.pla_id is 'Plat concerné par la promotion.';
comment on column promotion.pro_libelle is 'Libellé de la promotion';
comment on column promotion.pro_pourcentage_reduction is 'Pourcentage de réduction (0-100)';
comment on column promotion.pro_date_debut is 'Date de début de la promotion';
comment on column promotion.pro_date_fin is 'Date de fin de la promotion';
comment on column promotion.pro_active is 'Indique si la promotion est active';
comment on column promotion.lie_id is 'Restaurant concerné par la promotion (null si globale)';
comment on column promotion.pro_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table region
 **/
comment on table region is 'Région';
comment on column region.reg_code is 'Code de la région.';
comment on column region.reg_libelle is 'Libellé de la région.';
comment on column region.reg_nom_responsable is 'Nom du responsable de la région.';

/**
  * Commentaires pour la table reservation
 **/
comment on table reservation is 'Réservation d''une table';
comment on column reservation.rev_id is 'Identifiant de la réservation';
comment on column reservation.rev_date_reservation is 'Date et heure de la réservation';
comment on column reservation.rev_nombre_personnes is 'Nombre de personnes';
comment on column reservation.rev_commentaire is 'Commentaire sur la réservation';
comment on column reservation.rev_confirmee is 'Indique si la réservation est confirmée';
comment on column reservation.per_id is 'Client ayant fait la réservation';
comment on column reservation.tab_id is 'Table réservée';
comment on column reservation.lie_id is 'Restaurant concerné par la réservation';
comment on column reservation.rev_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table table
 **/
comment on table "table" is 'Table du restaurant';
comment on column "table".tab_id is 'Identifiant de la table';
comment on column "table".tab_numero is 'Numéro de la table';
comment on column "table".tab_capacite is 'Capacité de la table (nombre de places)';
comment on column "table".tab_disponible is 'Indique si la table est disponible';
comment on column "table".lie_id is 'Restaurant auquel appartient la table';
comment on column "table".tab_date_creation is 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table translation
 **/
comment on table translation is 'Table pour stocker les traductions en SQL.';
comment on column translation.tra_resource_key is 'Clé de traduction.';
comment on column translation.tra_value is 'Valeur de la clé de traduction.';
comment on column translation.tra_lang is 'Langue de traduction';

/**
  * Commentaires pour la table verre
 **/
comment on table verre is 'Verre.';
comment on column verre.vsl_id is 'Id de la vaisselle';
comment on column verre.vsl_description is 'Description de la vaisselle.';
comment on column verre.vrr_a_pied is 'Si le verre est à pied ou non.';
