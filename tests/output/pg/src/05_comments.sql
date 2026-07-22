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
COMMENT ON TABLE assiette IS 'Assiette.';
COMMENT ON COLUMN assiette.vsl_id IS 'Id de la vaisselle';
COMMENT ON COLUMN assiette.vsl_description IS 'Description de la vaisselle.';
COMMENT ON COLUMN assiette.ast_taille IS 'Taille de l''assiette.';

/**
  * Commentaires pour la table avis_client
 **/
COMMENT ON TABLE avis_client IS 'Avis d''un client sur un restaurant';
COMMENT ON COLUMN avis_client.avi_id IS 'Identifiant de l''avis';
COMMENT ON COLUMN avis_client.avi_note IS 'Note sur 5';
COMMENT ON COLUMN avis_client.avi_commentaire IS 'Commentaire de l''avis';
COMMENT ON COLUMN avis_client.avi_date_avis IS 'Date de l''avis';
COMMENT ON COLUMN avis_client.avi_approuve IS 'Indique si l''avis est approuvé par le restaurant';
COMMENT ON COLUMN avis_client.per_id IS 'Client ayant donné l''avis';
COMMENT ON COLUMN avis_client.lie_id IS 'Restaurant concerné par l''avis';
COMMENT ON COLUMN avis_client.avi_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table categorie_plat
 **/
COMMENT ON TABLE categorie_plat IS 'Catégorie de plat';
COMMENT ON COLUMN categorie_plat.cat_code IS 'Code de la catégorie';
COMMENT ON COLUMN categorie_plat.cat_libelle IS 'Libellé de la catégorie';
COMMENT ON COLUMN categorie_plat.cat_ordre IS 'Ordre d''affichage dans le menu.';
COMMENT ON COLUMN categorie_plat.cat_prix_moyen IS 'Prix moyen de la catégorie, à titre indicatif.';

/**
  * Commentaires pour la table categorie_plat_region
 **/
COMMENT ON TABLE categorie_plat_region IS 'Catégories de plats disponibles par région';
COMMENT ON COLUMN categorie_plat_region.reg_code IS 'Région';
COMMENT ON COLUMN categorie_plat_region.cat_code IS 'Catégorie de plat';

/**
  * Commentaires pour la table client
 **/
COMMENT ON TABLE client IS 'Client du restaurant';
COMMENT ON COLUMN client.cli_email IS 'Adresse email du client';
COMMENT ON COLUMN client.per_id IS 'Association vers la clé primaire de la classe parente';

/**
  * Commentaires pour la table commande
 **/
COMMENT ON TABLE commande IS 'Commande d''un client';
COMMENT ON COLUMN commande.com_id IS 'Identifiant de la commande';
COMMENT ON COLUMN commande.com_date_commande IS 'Date et heure de la commande';
COMMENT ON COLUMN commande.com_date_livraison IS 'Date et heure de livraison';
COMMENT ON COLUMN commande.com_montant_total IS 'Montant total de la commande';
COMMENT ON COLUMN commande.per_id IS 'Client ayant passé la commande';
COMMENT ON COLUMN commande.tab_id IS 'Table associée à la commande';
COMMENT ON COLUMN commande.rev_id IS 'Réservation associée à la commande';
COMMENT ON COLUMN commande.stc_code IS 'Statut de la commande';
COMMENT ON COLUMN commande.avi_id IS 'Avis laissé par le client sur la commande.';
COMMENT ON COLUMN commande.com_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table commande_historique
 **/
COMMENT ON TABLE commande_historique IS 'Commande pour historique avec préservation des clés primaires';
COMMENT ON COLUMN commande_historique.com_id IS 'Identifiant de la commande';
COMMENT ON COLUMN commande_historique.com_date_commande IS 'Date et heure de la commande';
COMMENT ON COLUMN commande_historique.com_date_livraison IS 'Date et heure de livraison';
COMMENT ON COLUMN commande_historique.com_montant_total IS 'Montant total de la commande';
COMMENT ON COLUMN commande_historique.per_id IS 'Client ayant passé la commande';
COMMENT ON COLUMN commande_historique.tab_id IS 'Table associée à la commande';
COMMENT ON COLUMN commande_historique.rev_id IS 'Réservation associée à la commande';
COMMENT ON COLUMN commande_historique.stc_code IS 'Statut de la commande';
COMMENT ON COLUMN commande_historique.avi_id IS 'Avis laissé par le client sur la commande.';
COMMENT ON COLUMN commande_historique.com_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table couvert
 **/
COMMENT ON TABLE couvert IS 'Couvert.';
COMMENT ON COLUMN couvert.vsl_id IS 'Id de la vaisselle';
COMMENT ON COLUMN couvert.vsl_description IS 'Description de la vaisselle.';

/**
  * Commentaires pour la table departement
 **/
COMMENT ON TABLE departement IS 'Département';
COMMENT ON COLUMN departement.dep_code IS 'Code du département.';
COMMENT ON COLUMN departement.dep_libelle IS 'Libellé du département.';
COMMENT ON COLUMN departement.reg_code IS 'Région associée.';

/**
  * Commentaires pour la table employe
 **/
COMMENT ON TABLE employe IS 'Employé du restaurant';
COMMENT ON COLUMN employe.emp_telephone IS 'Numéro de téléphone de l''employé.';
COMMENT ON COLUMN employe.emp_date_naissance IS 'Date de naissance';
COMMENT ON COLUMN employe.emp_matricule IS 'Matricule de l''employé';
COMMENT ON COLUMN employe.emp_date_embauche IS 'Date d''embauche';
COMMENT ON COLUMN employe.emp_salaire IS 'Salaire de l''employé';
COMMENT ON COLUMN employe.lie_id IS 'Restaurant où travaille l''employé';
COMMENT ON COLUMN employe.per_id IS 'Association vers la clé primaire de la classe parente';

/**
  * Commentaires pour la table lieu
 **/
COMMENT ON TABLE lieu IS 'Lieu';
COMMENT ON COLUMN lieu.lie_id IS 'Identifiant du restaurant';
COMMENT ON COLUMN lieu.lie_nom IS 'Nom du restaurant';
COMMENT ON COLUMN lieu.lie_adresse IS 'Adresse du restaurant';
COMMENT ON COLUMN lieu.lie_discriminator IS 'Discriminateur pour les instances de la hiérarchie de classe';
COMMENT ON COLUMN lieu.res_telephone IS 'Numéro de téléphone';
COMMENT ON COLUMN lieu.res_date_creation IS 'Date de création de l''enregistrement';
COMMENT ON COLUMN lieu.frn_telephone IS 'Numéro de téléphone';
COMMENT ON COLUMN lieu.frn_bio IS 'Si le fournisseur fait du bio.';

/**
  * Commentaires pour la table ligne_commande
 **/
COMMENT ON TABLE ligne_commande IS 'Ligne d''une commande';
COMMENT ON COLUMN ligne_commande.lig_id IS 'Identifiant de la ligne';
COMMENT ON COLUMN ligne_commande.lig_quantite IS 'Quantité commandée';
COMMENT ON COLUMN ligne_commande.lig_prix_unitaire IS 'Prix unitaire au moment de la commande';
COMMENT ON COLUMN ligne_commande.lig_prix_total IS 'Prix total de la ligne';
COMMENT ON COLUMN ligne_commande.com_id IS 'Commande à laquelle appartient la ligne';
COMMENT ON COLUMN ligne_commande.pla_id IS 'Plat commandé';
COMMENT ON COLUMN ligne_commande.lig_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table ligne_commande_historique
 **/
COMMENT ON TABLE ligne_commande_historique IS 'Ligne de commande pour historique avec préservation des clés primaires';
COMMENT ON COLUMN ligne_commande_historique.lig_id IS 'Identifiant de la ligne';
COMMENT ON COLUMN ligne_commande_historique.lig_quantite IS 'Quantité commandée';
COMMENT ON COLUMN ligne_commande_historique.lig_prix_unitaire IS 'Prix unitaire au moment de la commande';
COMMENT ON COLUMN ligne_commande_historique.lig_prix_total IS 'Prix total de la ligne';
COMMENT ON COLUMN ligne_commande_historique.pla_id IS 'Plat commandé';
COMMENT ON COLUMN ligne_commande_historique.lig_date_creation IS 'Date de création de l''enregistrement';
COMMENT ON COLUMN ligne_commande_historique.com_id IS 'Commande à laquelle appartient la ligne';

/**
  * Commentaires pour la table menu
 **/
COMMENT ON TABLE menu IS 'Menu du restaurant';
COMMENT ON COLUMN menu.men_id IS 'Identifiant du menu';
COMMENT ON COLUMN menu.men_nom IS 'Nom du menu';
COMMENT ON COLUMN menu.men_description IS 'Description du menu';
COMMENT ON COLUMN menu.men_prix IS 'Prix du menu';
COMMENT ON COLUMN menu.men_disponible IS 'Indique si le menu est disponible';
COMMENT ON COLUMN menu.men_date_debut IS 'Date de début de validité du menu';
COMMENT ON COLUMN menu.men_date_fin IS 'Date de fin de validité du menu';
COMMENT ON COLUMN menu.lie_id IS 'Restaurant proposant ce menu';
COMMENT ON COLUMN menu.men_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table menu_plat
 **/
COMMENT ON TABLE menu_plat IS 'Plat dans un menu';
COMMENT ON COLUMN menu_plat.men_id IS 'Menu contenant ce plat';
COMMENT ON COLUMN menu_plat.pla_id IS 'Plat du menu';
COMMENT ON COLUMN menu_plat.mpl_ordre IS 'Ordre d''affichage du plat dans le menu';
COMMENT ON COLUMN menu_plat.mpl_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table personne
 **/
COMMENT ON TABLE personne IS 'Classe de base représentant une personne';
COMMENT ON COLUMN personne.per_id IS 'Identifiant de la personne';
COMMENT ON COLUMN personne.per_nom IS 'Nom de la personne';
COMMENT ON COLUMN personne.per_prenom IS 'Prénom de la personne';
COMMENT ON COLUMN personne.dep_code IS 'Département de résidence de la personne.';
COMMENT ON COLUMN personne.per_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table plat
 **/
COMMENT ON TABLE plat IS 'Plat du menu';
COMMENT ON COLUMN plat.pla_id IS 'Identifiant du plat';
COMMENT ON COLUMN plat.pla_nom IS 'Nom du plat';
COMMENT ON COLUMN plat.pla_description IS 'Description du plat';
COMMENT ON COLUMN plat.pla_prix IS 'Prix du plat';
COMMENT ON COLUMN plat.pla_disponible IS 'Indique si le plat est disponible';
COMMENT ON COLUMN plat.cat_code IS 'Catégorie du plat';
COMMENT ON COLUMN plat.lie_id IS 'Restaurant proposant ce plat';
COMMENT ON COLUMN plat.pla_date_creation IS 'Date de création de l''enregistrement';
COMMENT ON COLUMN plat.pbo_volume IS 'Volume de la boisson';
COMMENT ON COLUMN plat.ppr_vegetarien IS 'Si le plat est végétarien.';

/**
  * Commentaires pour la table prestataire
 **/
COMMENT ON TABLE prestataire IS 'Prestaire du restaurant';
COMMENT ON COLUMN prestataire.pst_id IS 'Identifiant de la personne';
COMMENT ON COLUMN prestataire.pst_nom IS 'Nom de la personne';
COMMENT ON COLUMN prestataire.pst_prenom IS 'Prénom de la personne';
COMMENT ON COLUMN prestataire.pst_telephone IS 'Numéro de téléphone de l''employé.';

/**
  * Commentaires pour la table promotion
 **/
COMMENT ON TABLE promotion IS 'Promotion sur un plat';
COMMENT ON COLUMN promotion.pla_id IS 'Plat concerné par la promotion.';
COMMENT ON COLUMN promotion.pro_libelle IS 'Libellé de la promotion';
COMMENT ON COLUMN promotion.pro_pourcentage_reduction IS 'Pourcentage de réduction (0-100)';
COMMENT ON COLUMN promotion.pro_date_debut IS 'Date de début de la promotion';
COMMENT ON COLUMN promotion.pro_date_fin IS 'Date de fin de la promotion';
COMMENT ON COLUMN promotion.pro_active IS 'Indique si la promotion est active';
COMMENT ON COLUMN promotion.lie_id IS 'Restaurant concerné par la promotion (null si globale)';
COMMENT ON COLUMN promotion.pro_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table region
 **/
COMMENT ON TABLE region IS 'Région';
COMMENT ON COLUMN region.reg_code IS 'Code de la région.';
COMMENT ON COLUMN region.reg_libelle IS 'Libellé de la région.';
COMMENT ON COLUMN region.reg_nom_responsable IS 'Nom du responsable de la région.';

/**
  * Commentaires pour la table reservation
 **/
COMMENT ON TABLE reservation IS 'Réservation d''une table';
COMMENT ON COLUMN reservation.rev_id IS 'Identifiant de la réservation';
COMMENT ON COLUMN reservation.rev_date_reservation IS 'Date et heure de la réservation';
COMMENT ON COLUMN reservation.rev_nombre_personnes IS 'Nombre de personnes';
COMMENT ON COLUMN reservation.rev_commentaire IS 'Commentaire sur la réservation';
COMMENT ON COLUMN reservation.rev_confirmee IS 'Indique si la réservation est confirmée';
COMMENT ON COLUMN reservation.per_id IS 'Client ayant fait la réservation';
COMMENT ON COLUMN reservation.tab_id IS 'Table réservée';
COMMENT ON COLUMN reservation.lie_id IS 'Restaurant concerné par la réservation';
COMMENT ON COLUMN reservation.rev_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table table
 **/
COMMENT ON TABLE "table" IS 'Table du restaurant';
COMMENT ON COLUMN "table".tab_id IS 'Identifiant de la table';
COMMENT ON COLUMN "table".tab_numero IS 'Numéro de la table';
COMMENT ON COLUMN "table".tab_capacite IS 'Capacité de la table (nombre de places)';
COMMENT ON COLUMN "table".tab_disponible IS 'Indique si la table est disponible';
COMMENT ON COLUMN "table".lie_id IS 'Restaurant auquel appartient la table';
COMMENT ON COLUMN "table".tab_date_creation IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table translation
 **/
COMMENT ON TABLE translation IS 'Table pour stocker les traductions en SQL.';
COMMENT ON COLUMN translation.tra_resource_key IS 'Clé de traduction.';
COMMENT ON COLUMN translation.tra_value IS 'Valeur de la clé de traduction.';
COMMENT ON COLUMN translation.tra_lang IS 'Langue de traduction';

/**
  * Commentaires pour la table verre
 **/
COMMENT ON TABLE verre IS 'Verre.';
COMMENT ON COLUMN verre.vsl_id IS 'Id de la vaisselle';
COMMENT ON COLUMN verre.vsl_description IS 'Description de la vaisselle.';
COMMENT ON COLUMN verre.vrr_a_pied IS 'Si le verre est à pied ou non.';
