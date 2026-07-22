----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	03_values.sql
--   Description		:	Script d'insertion des valeurs initiales.
-- ===========================================================================================

/**		Initialisation de la table assiette		**/
insert into assiette(vsl_id, vsl_description, ast_taille) values(1, 'Grande assiette', 29);

/**		Initialisation de la table categorie_plat		**/
insert into categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) values('AUTRE', 'restaurant.categoriePlat.values.Autre', 5, null);
insert into categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) values('ENTREE', 'restaurant.categoriePlat.values.Entree', 2, null);
insert into categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) values('PRINCIPAL', 'restaurant.categoriePlat.values.Principal', 3, 10);
insert into categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) values('DESSERT', 'restaurant.categoriePlat.values.Dessert', 4, null);
insert into categorie_plat(cat_code, cat_libelle, cat_ordre, cat_prix_moyen) values('BOISSON', 'restaurant.categoriePlat.values.Boisson', 1, 2);

/**		Initialisation de la table region		**/
insert into region(reg_code, reg_libelle, reg_nom_responsable) values('IDF', 'restaurant.region.values.Idf', null);

/**		Initialisation de la table categorie_plat_region		**/
insert into categorie_plat_region(reg_code, cat_code) values('IDF', 'ENTREE');
insert into categorie_plat_region(reg_code, cat_code) values('IDF', 'DESSERT');

/**		Initialisation de la table couvert		**/
insert into couvert(vsl_id, vsl_description) values(2, 'Fourchette');
insert into couvert(vsl_id, vsl_description) values(3, 'Couteau');

/**		Initialisation de la table departement		**/
insert into departement(dep_code, dep_libelle, reg_code) values('75', 'restaurant.departement.values.Paris', 'IDF');
insert into departement(dep_code, dep_libelle, reg_code) values('92', 'restaurant.departement.values.HautsDeSeine', 'IDF');
insert into departement(dep_code, dep_libelle, reg_code) values('93', 'restaurant.departement.values.SeineSaintDenis', 'IDF');
insert into departement(dep_code, dep_libelle, reg_code) values('94', 'restaurant.departement.values.SeineEtMarne', 'IDF');

/**		Initialisation de la table lieu		**/
insert into lieu(lie_id, lie_nom, lie_adresse, lie_discriminator, res_telephone, res_date_creation, frn_telephone, frn_bio) values(1, 'Burger King', null, 'RESTAURANT', null, '1954-01-01T00:00:00Z', null, null);
insert into lieu(lie_id, lie_nom, lie_adresse, lie_discriminator, res_telephone, res_date_creation, frn_telephone, frn_bio) values(2, 'Pomona', null, 'FOURNISSEUR', null, null, null, true);

/**		Initialisation de la table personne		**/
insert into personne(per_id, per_nom, per_prenom, dep_code, per_date_creation) values(1, 'Michel', 'Jean', null, '2026-01-01T00:00:00Z');
insert into personne(per_id, per_nom, per_prenom, dep_code, per_date_creation) values(2, 'Christophe', 'Michel', null, '2026-01-01T00:00:00Z');

/**		Initialisation de la table employe		**/
insert into employe(emp_telephone, emp_date_naissance, emp_matricule, emp_date_embauche, emp_salaire, lie_id, per_id) values(null, null, '123456', '2026-01-01T00:00:00Z', null, 1, 2);
