----
---- ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
----

-- ===========================================================================================
--   Application Name	:	Restaurant 
--   Script Name		:	04_resources.sql
--   Description		:	Scripts d'insertion des ressources (libellés traduits).
-- ===========================================================================================

/**		Initialisation des traductions des propriétés de la table vaisselle		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.vaisselle.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.vaisselle.description', 'fr', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.vaisselle.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.vaisselle.description', 'de', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.vaisselle.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.vaisselle.description', 'en', 'Description');

/**		Initialisation des traductions des propriétés de la table assiette		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.assiette.taille', 'fr', 'Taille');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.assiette.taille', 'de', 'Taille');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.assiette.taille', 'en', 'Taille');

/**		Initialisation des traductions des propriétés de la table avis_client		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.note', 'fr', 'Note');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.commentaire', 'fr', 'Commentaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.dateAvis', 'fr', 'DateAvis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.approuve', 'fr', 'Approuve');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.clientId', 'fr', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.restaurantId', 'fr', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.dateCreation.dateCreation', 'fr', 'Date de création');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.note', 'de', 'Note');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.commentaire', 'de', 'Commentaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.dateAvis', 'de', 'DateAvis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.approuve', 'de', 'Approuve');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.clientId', 'de', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.restaurantId', 'de', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.dateCreation.dateCreation', 'de', 'Date de création');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.note', 'en', 'Note');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.commentaire', 'en', 'Commentaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.dateAvis', 'en', 'DateAvis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.approuve', 'en', 'Approuve');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.clientId', 'en', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.avisClient.restaurantId', 'en', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.dateCreation.dateCreation', 'en', 'Creation date');

/**		Initialisation des traductions des propriétés de la table categorie_plat		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.code', 'fr', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.libelle', 'fr', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.ordre', 'fr', 'Ordre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.prixMoyen', 'fr', 'PrixMoyen');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.code', 'de', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.libelle', 'de', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.ordre', 'de', 'Ordre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.prixMoyen', 'de', 'PrixMoyen');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.code', 'en', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.libelle', 'en', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.ordre', 'en', 'Ordre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.prixMoyen', 'en', 'PrixMoyen');

/**		Initialisation des traductions des propriétés de la table categorie_plat_region		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlatRegion.regionCode', 'fr', 'RegionCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlatRegion.categoriePlatCode', 'fr', 'CategoriePlat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlatRegion.regionCode', 'de', 'RegionCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlatRegion.categoriePlatCode', 'de', 'CategoriePlat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlatRegion.regionCode', 'en', 'RegionCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlatRegion.categoriePlatCode', 'en', 'CategoriePlat');

/**		Initialisation des traductions des propriétés de la table client		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.email', 'fr', 'Courriel');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.swileCardId', 'fr', 'SwileCard');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.personneId', 'fr', 'PersonneId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.email', 'de', 'Courriel');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.swileCardId', 'de', 'SwileCard');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.personneId', 'de', 'PersonneId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.email', 'en', 'Email');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.swileCardId', 'en', 'SwileCard');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.client.personneId', 'en', 'PersonneId');

/**		Initialisation des traductions des propriétés de la table commande		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.dateCommande', 'fr', 'DateCommande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.dateLivraison', 'fr', 'DateLivraison');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.montantTotal', 'fr', 'Montant total');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.clientId', 'fr', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.tableId', 'fr', 'TableId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.reservationId', 'fr', 'Reservation');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.statutCommande', 'fr', 'StatutCommande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.avisClientId', 'fr', 'AvisClient');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.dateCommande', 'de', 'DateCommande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.dateLivraison', 'de', 'DateLivraison');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.montantTotal', 'de', 'Montant total');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.clientId', 'de', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.tableId', 'de', 'TableId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.reservationId', 'de', 'Reservation');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.statutCommande', 'de', 'StatutCommande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.avisClientId', 'de', 'AvisClient');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.dateCommande', 'en', 'DateCommande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.dateLivraison', 'en', 'DateLivraison');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.montantTotal', 'en', 'Total amount');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.clientId', 'en', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.tableId', 'en', 'TableId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.reservationId', 'en', 'Reservation');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.statutCommande', 'en', 'StatutCommande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.commande.avisClientId', 'en', 'AvisClient');

/**		Initialisation des traductions des propriétés de la table departement		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.code', 'fr', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.libelle', 'fr', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.regionCode', 'fr', 'RegionCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.code', 'de', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.libelle', 'de', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.regionCode', 'de', 'RegionCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.code', 'en', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.libelle', 'en', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.regionCode', 'en', 'RegionCode');

/**		Initialisation des traductions des propriétés de la table employe		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employeBase.telephone', 'fr', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.dateNaissance', 'fr', 'DateNaissance');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.matricule', 'fr', 'Matricule');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.dateEmbauche', 'fr', 'DateEmbauche');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.salaire', 'fr', 'Salaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.restaurantId', 'fr', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.personneId', 'fr', 'PersonneId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employeBase.telephone', 'de', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.dateNaissance', 'de', 'DateNaissance');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.matricule', 'de', 'Matricule');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.dateEmbauche', 'de', 'DateEmbauche');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.salaire', 'de', 'Salaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.restaurantId', 'de', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.personneId', 'de', 'PersonneId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employeBase.telephone', 'en', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.dateNaissance', 'en', 'DateNaissance');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.matricule', 'en', 'Matricule');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.dateEmbauche', 'en', 'DateEmbauche');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.salaire', 'en', 'Salaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.restaurantId', 'en', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.employe.personneId', 'en', 'PersonneId');

/**		Initialisation des traductions des propriétés de la table facture		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.facture.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.facture.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.facture.id', 'en', 'Id');

/**		Initialisation des traductions des propriétés de la table lieu		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.nom', 'fr', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.adresse', 'fr', 'Adresse');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.discriminator', 'fr', 'Discriminator');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.nom', 'de', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.adresse', 'de', 'Adresse');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.discriminator', 'de', 'Discriminator');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.nom', 'en', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.adresse', 'en', 'Adresse');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.lieu.discriminator', 'en', 'Discriminator');

/**		Initialisation des traductions des propriétés de la table restaurant		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.restaurant.telephone', 'fr', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.restaurant.telephone', 'de', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.restaurant.telephone', 'en', 'Telephone');

/**		Initialisation des traductions des propriétés de la table fournisseur		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.fournisseur.telephone', 'fr', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.fournisseur.bio', 'fr', 'Bio');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.fournisseur.telephone', 'de', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.fournisseur.bio', 'de', 'Bio');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.fournisseur.telephone', 'en', 'Telephone');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.fournisseur.bio', 'en', 'Bio');

/**		Initialisation des traductions des propriétés de la table ligne_commande		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.quantite', 'fr', 'Quantite');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.prixUnitaire', 'fr', 'PrixUnitaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.prixTotal', 'fr', 'PrixTotal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.commandeId', 'fr', 'Commande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.platId', 'fr', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.quantite', 'de', 'Quantite');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.prixUnitaire', 'de', 'PrixUnitaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.prixTotal', 'de', 'PrixTotal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.commandeId', 'de', 'Commande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.platId', 'de', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.quantite', 'en', 'Quantite');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.prixUnitaire', 'en', 'PrixUnitaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.prixTotal', 'en', 'PrixTotal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.commandeId', 'en', 'Commande');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommande.platId', 'en', 'Plat');

/**		Initialisation des traductions des propriétés de la table ligne_commande_historique		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommandeHistorique.commandeHistoriqueId', 'fr', 'CommandeHistoriqueId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommandeHistorique.commandeHistoriqueId', 'de', 'CommandeHistoriqueId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.ligneCommandeHistorique.commandeHistoriqueId', 'en', 'CommandeHistoriqueId');

/**		Initialisation des traductions des propriétés de la table menu		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.nom', 'fr', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.description', 'fr', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.prix', 'fr', 'Prix');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.disponible', 'fr', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.dateDebut', 'fr', 'DateDebut');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.dateFin', 'fr', 'DateFin');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.restaurantId', 'fr', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.nom', 'de', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.description', 'de', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.prix', 'de', 'Prix');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.disponible', 'de', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.dateDebut', 'de', 'DateDebut');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.dateFin', 'de', 'DateFin');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.restaurantId', 'de', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.nom', 'en', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.description', 'en', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.prix', 'en', 'Prix');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.disponible', 'en', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.dateDebut', 'en', 'DateDebut');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.dateFin', 'en', 'DateFin');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menu.restaurantId', 'en', 'Restaurant');

/**		Initialisation des traductions des propriétés de la table menu_plat		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.menuId', 'fr', 'Menu');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.platId', 'fr', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.ordre', 'fr', 'Ordre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.menuId', 'de', 'Menu');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.platId', 'de', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.ordre', 'de', 'Ordre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.menuId', 'en', 'Menu');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.platId', 'en', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.menuPlat.ordre', 'en', 'Ordre');

/**		Initialisation des traductions des propriétés de la table paiement		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.paiement.factureId', 'fr', 'Facture');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.paiement.swileCardId', 'fr', 'SwileCardId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.paiement.factureId', 'de', 'Facture');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.paiement.swileCardId', 'de', 'SwileCardId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.paiement.factureId', 'en', 'Facture');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.paiement.swileCardId', 'en', 'SwileCardId');

/**		Initialisation des traductions des propriétés de la table personne		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.nom', 'fr', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.prenom', 'fr', 'Prénom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personne.departementCode', 'fr', 'DepartementCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.nom', 'de', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.prenom', 'de', 'Prénom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personne.departementCode', 'de', 'DepartementCode');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.nom', 'en', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personneBase.prenom', 'en', 'Prénom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.personne.departementCode', 'en', 'DepartementCode');

/**		Initialisation des traductions des propriétés de la table plat		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.nom', 'fr', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.description', 'fr', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.prix', 'fr', 'Prix');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.disponible', 'fr', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.categoriePlatCode', 'fr', 'CategoriePlat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.restaurantId', 'fr', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.nom', 'de', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.description', 'de', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.prix', 'de', 'Prix');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.disponible', 'de', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.categoriePlatCode', 'de', 'CategoriePlat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.restaurantId', 'de', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.nom', 'en', 'Nom');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.description', 'en', 'Description');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.prix', 'en', 'Prix');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.disponible', 'en', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.categoriePlatCode', 'en', 'CategoriePlat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.plat.restaurantId', 'en', 'Restaurant');

/**		Initialisation des traductions des propriétés de la table plat_boisson		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.platBoisson.volume', 'fr', 'Volume');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.platBoisson.volume', 'de', 'Volume');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.platBoisson.volume', 'en', 'Volume');

/**		Initialisation des traductions des propriétés de la table plat_principal		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.platPrincipal.vegetarien', 'fr', 'Vegetarien');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.platPrincipal.vegetarien', 'de', 'Vegetarien');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.platPrincipal.vegetarien', 'en', 'Vegetarien');

/**		Initialisation des traductions des propriétés de la table promotion		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.platId', 'fr', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.libelle', 'fr', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.pourcentageReduction', 'fr', 'PourcentageReduction');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.dateDebut', 'fr', 'DateDebut');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.dateFin', 'fr', 'DateFin');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.active', 'fr', 'Active');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.restaurantId', 'fr', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.platId', 'de', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.libelle', 'de', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.pourcentageReduction', 'de', 'PourcentageReduction');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.dateDebut', 'de', 'DateDebut');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.dateFin', 'de', 'DateFin');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.active', 'de', 'Active');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.restaurantId', 'de', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.platId', 'en', 'Plat');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.libelle', 'en', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.pourcentageReduction', 'en', 'PourcentageReduction');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.dateDebut', 'en', 'DateDebut');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.dateFin', 'en', 'DateFin');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.active', 'en', 'Active');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.promotion.restaurantId', 'en', 'Restaurant');

/**		Initialisation des traductions des propriétés de la table region		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.code', 'fr', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.libelle', 'fr', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.nomResponsable', 'fr', 'NomResponsable');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.code', 'de', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.libelle', 'de', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.nomResponsable', 'de', 'NomResponsable');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.code', 'en', 'Code');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.libelle', 'en', 'Libelle');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.nomResponsable', 'en', 'NomResponsable');

/**		Initialisation des traductions des propriétés de la table reservation		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.dateReservation', 'fr', 'DateReservation');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.nombrePersonnes', 'fr', 'NombrePersonnes');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.commentaire', 'fr', 'Commentaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.confirmee', 'fr', 'Confirmee');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.clientId', 'fr', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.tableId', 'fr', 'TableId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.restaurantId', 'fr', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.dateReservation', 'de', 'DateReservation');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.nombrePersonnes', 'de', 'NombrePersonnes');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.commentaire', 'de', 'Commentaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.confirmee', 'de', 'Confirmee');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.clientId', 'de', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.tableId', 'de', 'TableId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.restaurantId', 'de', 'Restaurant');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.dateReservation', 'en', 'DateReservation');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.nombrePersonnes', 'en', 'NombrePersonnes');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.commentaire', 'en', 'Commentaire');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.confirmee', 'en', 'Confirmee');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.clientId', 'en', 'Client');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.tableId', 'en', 'TableId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.reservation.restaurantId', 'en', 'Restaurant');

/**		Initialisation des traductions des propriétés de la table table		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.id', 'fr', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.numero', 'fr', 'Numero');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.capacite', 'fr', 'Capacite');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.disponible', 'fr', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.restaurantId', 'fr', 'RestaurantId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.id', 'de', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.numero', 'de', 'Numero');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.capacite', 'de', 'Capacite');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.disponible', 'de', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.restaurantId', 'de', 'RestaurantId');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.id', 'en', 'Id');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.numero', 'en', 'Numero');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.capacite', 'en', 'Capacite');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.disponible', 'en', 'Disponible');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.table.restaurantId', 'en', 'RestaurantId');

/**		Initialisation des traductions des propriétés de la table translation		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.resourceKey', 'fr', 'ResourceKey');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.value', 'fr', 'Value');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.lang', 'fr', 'Lang');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.resourceKey', 'de', 'ResourceKey');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.value', 'de', 'Value');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.lang', 'de', 'Lang');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.resourceKey', 'en', 'ResourceKey');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.value', 'en', 'Value');
insert into translation(tra_resource_key, tra_lang, tra_value) values('common.translation.lang', 'en', 'Lang');

/**		Initialisation des traductions des propriétés de la table verre		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.verre.aPied', 'fr', 'APied');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.verre.aPied', 'de', 'APied');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.verre.aPied', 'en', 'APied');

/**		Initialisation des traductions des valeurs de la table categorie_plat		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Autre', 'fr', 'Autre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Entree', 'fr', 'Entrée');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Principal', 'fr', 'Plat principal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Dessert', 'fr', 'Dessert');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Boisson', 'fr', 'Boisson');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Autre', 'de', 'Autre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Entree', 'de', 'Entrée');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Principal', 'de', 'Plat principal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Dessert', 'de', 'Dessert');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Boisson', 'de', 'Boisson');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Autre', 'en', 'Autre');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Entree', 'en', 'Starter');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Principal', 'en', 'Plat principal');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Dessert', 'en', 'Dessert');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.categoriePlat.values.Boisson', 'en', 'Drink');

/**		Initialisation des traductions des valeurs de la table departement		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.Paris', 'fr', 'Paris');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.HautsDeSeine', 'fr', 'Hauts de Seine');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineSaintDenis', 'fr', 'Seine Saint Denis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineEtMarne', 'fr', 'Seine et Marne');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.Paris', 'de', 'Paris');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.HautsDeSeine', 'de', 'Hauts de Seine');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineSaintDenis', 'de', 'Seine Saint Denis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineEtMarne', 'de', 'Seine et Marne');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.Paris', 'en', 'Paris');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.HautsDeSeine', 'en', 'Hauts de Seine');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineSaintDenis', 'en', 'Seine Saint Denis');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.departement.values.SeineEtMarne', 'en', 'Seine et Marne');

/**		Initialisation des traductions des valeurs de la table region		**/
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.values.Idf', 'fr', 'Île de France');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.values.Idf', 'de', 'Île de France');
insert into translation(tra_resource_key, tra_lang, tra_value) values('restaurant.region.values.Idf', 'en', 'Île de France');
