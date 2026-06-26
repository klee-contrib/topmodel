----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	05_comments.sql
--   Description		:	Script de création de commentaires sur les tables et les colonnes.
-- ===========================================================================================

/**
  * Commentaires pour la table ASSIETTE
 **/
COMMENT ON TABLE ASSIETTE IS 'Assiette.';
COMMENT ON COLUMN ASSIETTE.VSL_ID IS 'Id de la vaisselle';
COMMENT ON COLUMN ASSIETTE.VSL_DESCRIPTION IS 'Description de la vaisselle.';
COMMENT ON COLUMN ASSIETTE.AST_TAILLE IS 'Taille de l''assiette.';

/**
  * Commentaires pour la table AVIS_CLIENT
 **/
COMMENT ON TABLE AVIS_CLIENT IS 'Avis d''un client sur un restaurant';
COMMENT ON COLUMN AVIS_CLIENT.AVI_ID IS 'Identifiant de l''avis';
COMMENT ON COLUMN AVIS_CLIENT.AVI_NOTE IS 'Note sur 5';
COMMENT ON COLUMN AVIS_CLIENT.AVI_COMMENTAIRE IS 'Commentaire de l''avis';
COMMENT ON COLUMN AVIS_CLIENT.AVI_DATE_AVIS IS 'Date de l''avis';
COMMENT ON COLUMN AVIS_CLIENT.AVI_APPROUVE IS 'Indique si l''avis est approuvé par le restaurant';
COMMENT ON COLUMN AVIS_CLIENT.PER_ID IS 'Client ayant donné l''avis';
COMMENT ON COLUMN AVIS_CLIENT.LIE_ID IS 'Restaurant concerné par l''avis';
COMMENT ON COLUMN AVIS_CLIENT.AVI_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table CATEGORIE_PLAT
 **/
COMMENT ON TABLE CATEGORIE_PLAT IS 'Catégorie de plat';
COMMENT ON COLUMN CATEGORIE_PLAT.CAT_CODE IS 'Code de la catégorie';
COMMENT ON COLUMN CATEGORIE_PLAT.CAT_LIBELLE IS 'Libellé de la catégorie';
COMMENT ON COLUMN CATEGORIE_PLAT.CAT_ORDRE IS 'Ordre d''affichage dans le menu.';
COMMENT ON COLUMN CATEGORIE_PLAT.CAT_PRIX_MOYEN IS 'Prix moyen de la catégorie, à titre indicatif.';

/**
  * Commentaires pour la table CATEGORIE_PLAT_REGION
 **/
COMMENT ON TABLE CATEGORIE_PLAT_REGION IS 'Catégories de plats disponibles par région';
COMMENT ON COLUMN CATEGORIE_PLAT_REGION.REG_CODE IS 'Région';
COMMENT ON COLUMN CATEGORIE_PLAT_REGION.CAT_CODE IS 'Catégorie de plat';

/**
  * Commentaires pour la table CLIENT
 **/
COMMENT ON TABLE CLIENT IS 'Client du restaurant';
COMMENT ON COLUMN CLIENT.CLI_EMAIL IS 'Adresse email du client';
COMMENT ON COLUMN CLIENT.PER_ID IS 'Association vers la clé primaire de la classe parente';

/**
  * Commentaires pour la table COMMANDE
 **/
COMMENT ON TABLE COMMANDE IS 'Commande d''un client';
COMMENT ON COLUMN COMMANDE.COM_ID IS 'Identifiant de la commande';
COMMENT ON COLUMN COMMANDE.COM_DATE_COMMANDE IS 'Date et heure de la commande';
COMMENT ON COLUMN COMMANDE.COM_DATE_LIVRAISON IS 'Date et heure de livraison';
COMMENT ON COLUMN COMMANDE.COM_MONTANT_TOTAL IS 'Montant total de la commande';
COMMENT ON COLUMN COMMANDE.PER_ID IS 'Client ayant passé la commande';
COMMENT ON COLUMN COMMANDE.TAB_ID IS 'Table associée à la commande';
COMMENT ON COLUMN COMMANDE.REV_ID IS 'Réservation associée à la commande';
COMMENT ON COLUMN COMMANDE.STC_CODE IS 'Statut de la commande';
COMMENT ON COLUMN COMMANDE.AVI_ID IS 'Avis laissé par le client sur la commande.';
COMMENT ON COLUMN COMMANDE.COM_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table COMMANDE_HISTORIQUE
 **/
COMMENT ON TABLE COMMANDE_HISTORIQUE IS 'Commande pour historique avec préservation des clés primaires';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.COM_ID IS 'Identifiant de la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.COM_DATE_COMMANDE IS 'Date et heure de la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.COM_DATE_LIVRAISON IS 'Date et heure de livraison';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.COM_MONTANT_TOTAL IS 'Montant total de la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.PER_ID IS 'Client ayant passé la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.TAB_ID IS 'Table associée à la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.REV_ID IS 'Réservation associée à la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.STC_CODE IS 'Statut de la commande';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.AVI_ID IS 'Avis laissé par le client sur la commande.';
COMMENT ON COLUMN COMMANDE_HISTORIQUE.COM_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table COUVERT
 **/
COMMENT ON TABLE COUVERT IS 'Couvert.';
COMMENT ON COLUMN COUVERT.VSL_ID IS 'Id de la vaisselle';
COMMENT ON COLUMN COUVERT.VSL_DESCRIPTION IS 'Description de la vaisselle.';

/**
  * Commentaires pour la table DEPARTEMENT
 **/
COMMENT ON TABLE DEPARTEMENT IS 'Département';
COMMENT ON COLUMN DEPARTEMENT.DEP_CODE IS 'Code du département.';
COMMENT ON COLUMN DEPARTEMENT.DEP_LIBELLE IS 'Libellé du département.';
COMMENT ON COLUMN DEPARTEMENT.REG_CODE IS 'Région associée.';

/**
  * Commentaires pour la table EMPLOYE
 **/
COMMENT ON TABLE EMPLOYE IS 'Employé du restaurant';
COMMENT ON COLUMN EMPLOYE.EMP_TELEPHONE IS 'Numéro de téléphone de l''employé.';
COMMENT ON COLUMN EMPLOYE.EMP_DATE_NAISSANCE IS 'Date de naissance';
COMMENT ON COLUMN EMPLOYE.EMP_MATRICULE IS 'Matricule de l''employé';
COMMENT ON COLUMN EMPLOYE.EMP_DATE_EMBAUCHE IS 'Date d''embauche';
COMMENT ON COLUMN EMPLOYE.EMP_SALAIRE IS 'Salaire de l''employé';
COMMENT ON COLUMN EMPLOYE.LIE_ID IS 'Restaurant où travaille l''employé';
COMMENT ON COLUMN EMPLOYE.PER_ID IS 'Association vers la clé primaire de la classe parente';

/**
  * Commentaires pour la table LIEU
 **/
COMMENT ON TABLE LIEU IS 'Lieu';
COMMENT ON COLUMN LIEU.LIE_ID IS 'Identifiant du restaurant';
COMMENT ON COLUMN LIEU.LIE_NOM IS 'Nom du restaurant';
COMMENT ON COLUMN LIEU.LIE_ADRESSE IS 'Adresse du restaurant';
COMMENT ON COLUMN LIEU.LIE_DISCRIMINATOR IS 'Discriminateur pour les instances de la hiérarchie de classe';
COMMENT ON COLUMN LIEU.RES_TELEPHONE IS 'Numéro de téléphone';
COMMENT ON COLUMN LIEU.RES_DATE_CREATION IS 'Date de création de l''enregistrement';
COMMENT ON COLUMN LIEU.FRN_TELEPHONE IS 'Numéro de téléphone';
COMMENT ON COLUMN LIEU.FRN_BIO IS 'Si le fournisseur fait du bio.';

/**
  * Commentaires pour la table LIGNE_COMMANDE
 **/
COMMENT ON TABLE LIGNE_COMMANDE IS 'Ligne d''une commande';
COMMENT ON COLUMN LIGNE_COMMANDE.LIG_ID IS 'Identifiant de la ligne';
COMMENT ON COLUMN LIGNE_COMMANDE.LIG_QUANTITE IS 'Quantité commandée';
COMMENT ON COLUMN LIGNE_COMMANDE.LIG_PRIX_UNITAIRE IS 'Prix unitaire au moment de la commande';
COMMENT ON COLUMN LIGNE_COMMANDE.LIG_PRIX_TOTAL IS 'Prix total de la ligne';
COMMENT ON COLUMN LIGNE_COMMANDE.COM_ID IS 'Commande à laquelle appartient la ligne';
COMMENT ON COLUMN LIGNE_COMMANDE.PLA_ID IS 'Plat commandé';
COMMENT ON COLUMN LIGNE_COMMANDE.LIG_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table LIGNE_COMMANDE_HISTORIQUE
 **/
COMMENT ON TABLE LIGNE_COMMANDE_HISTORIQUE IS 'Ligne de commande pour historique avec préservation des clés primaires';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.LIG_ID IS 'Identifiant de la ligne';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.LIG_QUANTITE IS 'Quantité commandée';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.LIG_PRIX_UNITAIRE IS 'Prix unitaire au moment de la commande';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.LIG_PRIX_TOTAL IS 'Prix total de la ligne';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.PLA_ID IS 'Plat commandé';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.LIG_DATE_CREATION IS 'Date de création de l''enregistrement';
COMMENT ON COLUMN LIGNE_COMMANDE_HISTORIQUE.COM_ID IS 'Commande à laquelle appartient la ligne';

/**
  * Commentaires pour la table MENU
 **/
COMMENT ON TABLE MENU IS 'Menu du restaurant';
COMMENT ON COLUMN MENU.MEN_ID IS 'Identifiant du menu';
COMMENT ON COLUMN MENU.MEN_NOM IS 'Nom du menu';
COMMENT ON COLUMN MENU.MEN_DESCRIPTION IS 'Description du menu';
COMMENT ON COLUMN MENU.MEN_PRIX IS 'Prix du menu';
COMMENT ON COLUMN MENU.MEN_DISPONIBLE IS 'Indique si le menu est disponible';
COMMENT ON COLUMN MENU.MEN_DATE_DEBUT IS 'Date de début de validité du menu';
COMMENT ON COLUMN MENU.MEN_DATE_FIN IS 'Date de fin de validité du menu';
COMMENT ON COLUMN MENU.LIE_ID IS 'Restaurant proposant ce menu';
COMMENT ON COLUMN MENU.MEN_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table MENU_PLAT
 **/
COMMENT ON TABLE MENU_PLAT IS 'Plat dans un menu';
COMMENT ON COLUMN MENU_PLAT.MEN_ID IS 'Menu contenant ce plat';
COMMENT ON COLUMN MENU_PLAT.PLA_ID IS 'Plat du menu';
COMMENT ON COLUMN MENU_PLAT.MPL_ORDRE IS 'Ordre d''affichage du plat dans le menu';
COMMENT ON COLUMN MENU_PLAT.MPL_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table PERSONNE
 **/
COMMENT ON TABLE PERSONNE IS 'Classe de base représentant une personne';
COMMENT ON COLUMN PERSONNE.PER_ID IS 'Identifiant de la personne';
COMMENT ON COLUMN PERSONNE.PER_NOM IS 'Nom de la personne';
COMMENT ON COLUMN PERSONNE.PER_PRENOM IS 'Prénom de la personne';
COMMENT ON COLUMN PERSONNE.DEP_CODE IS 'Département de résidence de la personne.';
COMMENT ON COLUMN PERSONNE.PER_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table PLAT
 **/
COMMENT ON TABLE PLAT IS 'Plat du menu';
COMMENT ON COLUMN PLAT.PLA_ID IS 'Identifiant du plat';
COMMENT ON COLUMN PLAT.PLA_NOM IS 'Nom du plat';
COMMENT ON COLUMN PLAT.PLA_DESCRIPTION IS 'Description du plat';
COMMENT ON COLUMN PLAT.PLA_PRIX IS 'Prix du plat';
COMMENT ON COLUMN PLAT.PLA_DISPONIBLE IS 'Indique si le plat est disponible';
COMMENT ON COLUMN PLAT.CAT_CODE IS 'Catégorie du plat';
COMMENT ON COLUMN PLAT.LIE_ID IS 'Restaurant proposant ce plat';
COMMENT ON COLUMN PLAT.PLA_DATE_CREATION IS 'Date de création de l''enregistrement';
COMMENT ON COLUMN PLAT.PBO_VOLUME IS 'Volume de la boisson';
COMMENT ON COLUMN PLAT.PPR_VEGETARIEN IS 'Si le plat est végétarien.';

/**
  * Commentaires pour la table PRESTATAIRE
 **/
COMMENT ON TABLE PRESTATAIRE IS 'Prestaire du restaurant';
COMMENT ON COLUMN PRESTATAIRE.PST_ID IS 'Identifiant de la personne';
COMMENT ON COLUMN PRESTATAIRE.PST_NOM IS 'Nom de la personne';
COMMENT ON COLUMN PRESTATAIRE.PST_PRENOM IS 'Prénom de la personne';
COMMENT ON COLUMN PRESTATAIRE.PST_TELEPHONE IS 'Numéro de téléphone de l''employé.';

/**
  * Commentaires pour la table PROMOTION
 **/
COMMENT ON TABLE PROMOTION IS 'Promotion sur un plat';
COMMENT ON COLUMN PROMOTION.PLA_ID IS 'Plat concerné par la promotion.';
COMMENT ON COLUMN PROMOTION.PRO_LIBELLE IS 'Libellé de la promotion';
COMMENT ON COLUMN PROMOTION.PRO_POURCENTAGE_REDUCTION IS 'Pourcentage de réduction (0-100)';
COMMENT ON COLUMN PROMOTION.PRO_DATE_DEBUT IS 'Date de début de la promotion';
COMMENT ON COLUMN PROMOTION.PRO_DATE_FIN IS 'Date de fin de la promotion';
COMMENT ON COLUMN PROMOTION.PRO_ACTIVE IS 'Indique si la promotion est active';
COMMENT ON COLUMN PROMOTION.LIE_ID IS 'Restaurant concerné par la promotion (null si globale)';
COMMENT ON COLUMN PROMOTION.PRO_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table REGION
 **/
COMMENT ON TABLE REGION IS 'Région';
COMMENT ON COLUMN REGION.REG_CODE IS 'Code de la région.';
COMMENT ON COLUMN REGION.REG_LIBELLE IS 'Libellé de la région.';
COMMENT ON COLUMN REGION.REG_NOM_RESPONSABLE IS 'Nom du responsable de la région.';

/**
  * Commentaires pour la table RESERVATION
 **/
COMMENT ON TABLE RESERVATION IS 'Réservation d''une table';
COMMENT ON COLUMN RESERVATION.REV_ID IS 'Identifiant de la réservation';
COMMENT ON COLUMN RESERVATION.REV_DATE_RESERVATION IS 'Date et heure de la réservation';
COMMENT ON COLUMN RESERVATION.REV_NOMBRE_PERSONNES IS 'Nombre de personnes';
COMMENT ON COLUMN RESERVATION.REV_COMMENTAIRE IS 'Commentaire sur la réservation';
COMMENT ON COLUMN RESERVATION.REV_CONFIRMEE IS 'Indique si la réservation est confirmée';
COMMENT ON COLUMN RESERVATION.PER_ID IS 'Client ayant fait la réservation';
COMMENT ON COLUMN RESERVATION.TAB_ID IS 'Table réservée';
COMMENT ON COLUMN RESERVATION.LIE_ID IS 'Restaurant concerné par la réservation';
COMMENT ON COLUMN RESERVATION.REV_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table TABLE_RESTAURANT
 **/
COMMENT ON TABLE TABLE_RESTAURANT IS 'Table du restaurant';
COMMENT ON COLUMN TABLE_RESTAURANT.TAB_ID IS 'Identifiant de la table';
COMMENT ON COLUMN TABLE_RESTAURANT.TAB_NUMERO IS 'Numéro de la table';
COMMENT ON COLUMN TABLE_RESTAURANT.TAB_CAPACITE IS 'Capacité de la table (nombre de places)';
COMMENT ON COLUMN TABLE_RESTAURANT.TAB_DISPONIBLE IS 'Indique si la table est disponible';
COMMENT ON COLUMN TABLE_RESTAURANT.LIE_ID IS 'Restaurant auquel appartient la table';
COMMENT ON COLUMN TABLE_RESTAURANT.TAB_DATE_CREATION IS 'Date de création de l''enregistrement';

/**
  * Commentaires pour la table TRANSLATION
 **/
COMMENT ON TABLE TRANSLATION IS 'Table pour stocker les traductions en SQL.';
COMMENT ON COLUMN TRANSLATION.TRA_RESOURCE_KEY IS 'Clé de traduction.';
COMMENT ON COLUMN TRANSLATION.TRA_VALUE IS 'Valeur de la clé de traduction.';
COMMENT ON COLUMN TRANSLATION.TRA_LANG IS 'Langue de traduction';

/**
  * Commentaires pour la table VERRE
 **/
COMMENT ON TABLE VERRE IS 'Verre.';
COMMENT ON COLUMN VERRE.VSL_ID IS 'Id de la vaisselle';
COMMENT ON COLUMN VERRE.VSL_DESCRIPTION IS 'Description de la vaisselle.';
COMMENT ON COLUMN VERRE.VRR_A_PIED IS 'Si le verre est à pied ou non.';
