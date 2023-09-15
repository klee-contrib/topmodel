----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- =========================================================================================== 
--   Application Name	:	pg 
--   Script Name		:	05_comments.sql
--   Description		:	Script de création des commentaires. 
-- =========================================================================================== 

/**
  * Commentaires pour la table DROIT
 **/
COMMENT ON TABLE DROIT IS 'Droits de l''application';
COMMENT ON COLUMN DROIT.DRO_CODE IS 'Code du droit';
COMMENT ON COLUMN DROIT.DRO_LIBELLE IS 'Libellé du droit';
COMMENT ON COLUMN DROIT.TDR_CODE IS 'Type de profil pouvant faire l''action';

/**
  * Commentaires pour la table PROFIL
 **/
COMMENT ON TABLE PROFIL IS 'Profil des utilisateurs';
COMMENT ON COLUMN PROFIL.PRO_ID IS 'Id technique';
COMMENT ON COLUMN PROFIL.PRO_LIBELLE IS 'Libellé du profil.';
COMMENT ON COLUMN PROFIL.PRO_DATE_CREATION IS 'Date de création de l''utilisateur.';
COMMENT ON COLUMN PROFIL.PRO_DATE_MODIFICATION IS 'Date de modification de l''utilisateur.';

/**
  * Commentaires pour la table PROFIL_DROIT
 **/
COMMENT ON TABLE PROFIL_DROIT IS 'Liste des droits du profil';
COMMENT ON COLUMN PROFIL_DROIT.PRO_ID IS 'Liste des droits du profil';
COMMENT ON COLUMN PROFIL_DROIT.DRO_CODE IS 'Liste des droits du profil';

/**
  * Commentaires pour la table TYPE_DROIT
 **/
COMMENT ON TABLE TYPE_DROIT IS 'Type de droit';
COMMENT ON COLUMN TYPE_DROIT.TDR_CODE IS 'Code du type de droit';
COMMENT ON COLUMN TYPE_DROIT.TDR_LIBELLE IS 'Libellé du type de droit';

/**
  * Commentaires pour la table TYPE_UTILISATEUR
 **/
COMMENT ON TABLE TYPE_UTILISATEUR IS 'Type d''utilisateur';
COMMENT ON COLUMN TYPE_UTILISATEUR.TUT_CODE IS 'Code du type d''utilisateur';
COMMENT ON COLUMN TYPE_UTILISATEUR.TUT_LIBELLE IS 'Libellé du type d''utilisateur';

/**
  * Commentaires pour la table UTILISATEUR
 **/
COMMENT ON TABLE UTILISATEUR IS 'Utilisateur de l''application';
COMMENT ON COLUMN UTILISATEUR.UTI_ID IS 'Id de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_NOM IS 'Nom de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_PRENOM IS 'Nom de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_EMAIL IS 'Email de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_DATE_NAISSANCE IS 'Age de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_ACTIF IS 'Si l''utilisateur est actif';
COMMENT ON COLUMN UTILISATEUR.PRO_ID IS 'Profil de l''utilisateur';
COMMENT ON COLUMN UTILISATEUR.TUT_CODE IS 'Type d''utilisateur';
COMMENT ON COLUMN UTILISATEUR.UTI_DATE_CREATION IS 'Date de création de l''utilisateur.';
COMMENT ON COLUMN UTILISATEUR.UTI_DATE_MODIFICATION IS 'Date de modification de l''utilisateur.';
