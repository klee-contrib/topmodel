----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	03_values.sql
--   Description		:	Script d'insertion des valeurs initiales.
-- ===========================================================================================

/**		Initialisation de la table assiette		**/
INSERT INTO assiette(vsl_id, vsl_description, ast_taille) VALUES(1, 'Grande assiette', 29);

/**		Initialisation de la table categorie_plat		**/
INSERT INTO categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) VALUES('AUTRE', 'restaurant.categoriePlat.values.Autre', 5, NULL);
INSERT INTO categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) VALUES('ENTREE', 'restaurant.categoriePlat.values.Entree', 2, NULL);
INSERT INTO categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) VALUES('PRINCIPAL', 'restaurant.categoriePlat.values.Principal', 3, 10);
INSERT INTO categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) VALUES('DESSERT', 'restaurant.categoriePlat.values.Dessert', 4, NULL);
INSERT INTO categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) VALUES('BOISSON', 'restaurant.categoriePlat.values.Boisson', 1, 2);

/**		Initialisation de la table region		**/
INSERT INTO region(reg_code, reg_libelle, reg_nom_responsable) VALUES('IDF', 'restaurant.region.values.Idf', NULL);

/**		Initialisation de la table categorie_plat_region		**/
INSERT INTO categorie_plat_region(reg_code, cat_code) VALUES('IDF', 'ENTREE');
INSERT INTO categorie_plat_region(reg_code, cat_code) VALUES('IDF', 'DESSERT');

/**		Initialisation de la table couvert		**/
INSERT INTO couvert(vsl_id, vsl_description) VALUES(2, 'Fourchette');
INSERT INTO couvert(vsl_id, vsl_description) VALUES(3, 'Couteau');

/**		Initialisation de la table departement		**/
INSERT INTO departement(dep_code, dep_libelle, reg_code) VALUES('75', 'restaurant.departement.values.Paris', 'IDF');
INSERT INTO departement(dep_code, dep_libelle, reg_code) VALUES('92', 'restaurant.departement.values.HautsDeSeine', 'IDF');
INSERT INTO departement(dep_code, dep_libelle, reg_code) VALUES('93', 'restaurant.departement.values.SeineSaintDenis', 'IDF');
INSERT INTO departement(dep_code, dep_libelle, reg_code) VALUES('94', 'restaurant.departement.values.SeineEtMarne', 'IDF');

/**		Initialisation de la table lieu		**/
INSERT INTO lieu(lie_id, lie_nom, lie_adresse, lie_discriminator, res_telephone, res_date_creation, frn_telephone, frn_bio) VALUES(1, 'Burger King', NULL, 'RESTAURANT', NULL, '1954-01-01T00:00:00Z', NULL, NULL);
INSERT INTO lieu(lie_id, lie_nom, lie_adresse, lie_discriminator, res_telephone, res_date_creation, frn_telephone, frn_bio) VALUES(2, 'Pomona', NULL, 'FOURNISSEUR', NULL, NULL, NULL, true);

/**		Initialisation de la table personne		**/
INSERT INTO personne(per_id, per_nom, per_prenom, dep_code, per_date_creation) VALUES(1, 'Michel', 'Jean', NULL, '2026-01-01T00:00:00Z');
INSERT INTO personne(per_id, per_nom, per_prenom, dep_code, per_date_creation) VALUES(2, 'Christophe', 'Michel', NULL, '2026-01-01T00:00:00Z');

/**		Initialisation de la table employe		**/
INSERT INTO employe(emp_telephone, emp_date_naissance, emp_matricule, emp_date_embauche, emp_salaire, lie_id, per_id) VALUES(NULL, NULL, '123456', '2026-01-01T00:00:00Z', NULL, 1, 2);
