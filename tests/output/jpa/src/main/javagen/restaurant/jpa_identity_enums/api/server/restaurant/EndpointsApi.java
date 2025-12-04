////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_identity_enums.dtos.restaurant.ClientItem;
import restaurant.jpa_identity_enums.dtos.restaurant.ClientRead;
import restaurant.jpa_identity_enums.dtos.restaurant.ClientWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.CommandeItem;
import restaurant.jpa_identity_enums.dtos.restaurant.CommandeRead;
import restaurant.jpa_identity_enums.dtos.restaurant.CommandeWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_identity_enums.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_identity_enums.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.PlatItem;
import restaurant.jpa_identity_enums.dtos.restaurant.PlatRead;
import restaurant.jpa_identity_enums.dtos.restaurant.PlatWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.RestaurantItem;
import restaurant.jpa_identity_enums.dtos.restaurant.RestaurantRead;
import restaurant.jpa_identity_enums.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.TableClientItem;
import restaurant.jpa_identity_enums.dtos.restaurant.TableClientRead;
import restaurant.jpa_identity_enums.dtos.restaurant.TableClientWrite;
import restaurant.jpa_identity_enums.enums.restaurant.CategoriePlat;
import restaurant.jpa_identity_enums.enums.restaurant.StatutCommande;

@FeignClient(name = "Restaurant", contextId = "EndpointsApi")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface EndpointsApi {

	/**
	 * Ajoute un client.
	 * @param client Client à créer.
	 *
	 * @return Client créé.
	 */
	@Operation(description = "Ajoute un client")
	@PostMapping(path = "api/restaurants/clients")
	ClientRead addClient(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Client à créer") @RequestBody @Valid ClientWrite client);

	/**
	 * Crée une nouvelle commande.
	 * @param commande Commande à créer.
	 *
	 * @return Commande créée.
	 */
	@PostMapping(path = "api/restaurants/commandes")
	@Operation(description = "Crée une nouvelle commande")
	CommandeRead addCommande(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Commande à créer") @RequestBody @Valid CommandeWrite commande);

	/**
	 * Ajoute une ligne de commande.
	 * @param ligneCommande Ligne de commande à créer.
	 *
	 * @return Ligne de commande créée.
	 */
	@PostMapping(path = "api/restaurants/ligne-commandes")
	@Operation(description = "Ajoute une ligne de commande")
	LigneCommandeRead addLigneCommande(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Ligne de commande à créer") @RequestBody @Valid LigneCommandeWrite ligneCommande);

	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer.
	 *
	 * @return Plat créé.
	 */
	@Operation(description = "Ajoute un plat")
	@PostMapping(path = "api/restaurants/plats")
	PlatRead addPlat(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Plat à créer") @RequestBody @Valid PlatWrite plat);

	/**
	 * Ajoute un restaurant.
	 * @param restaurant Restaurant à créer.
	 *
	 * @return Restaurant créé.
	 */
	@PostMapping(path = "api/restaurants")
	@Operation(description = "Ajoute un restaurant")
	RestaurantRead addRestaurant(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Restaurant à créer") @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Ajoute une table.
	 * @param table Table à créer.
	 *
	 * @return Table créée.
	 */
	@Operation(description = "Ajoute une table")
	@PostMapping(path = "api/restaurants/tables")
	TableClientRead addTable(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Table à créer") @RequestBody @Valid TableClientWrite table);

	/**
	 * Supprime un client.
	 * @param cliId Identifiant du client.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un client")
	@DeleteMapping(path = "api/restaurants/clients/{cliId}")
	void deleteClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Supprime une commande.
	 * @param comId Identifiant de la commande.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime une commande")
	@DeleteMapping(path = "api/restaurants/commandes/{comId}")
	void deleteCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Supprime une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime une ligne de commande")
	@DeleteMapping(path = "api/restaurants/ligne-commandes/{ligId}")
	void deleteLigneCommande(@Parameter(description = "Identifiant de la ligne") @PathVariable("ligId") Integer ligId);

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un plat")
	@DeleteMapping(path = "api/restaurants/plats/{plaId}")
	void deletePlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId);

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "api/restaurants/{resId}")
	@Operation(description = "Supprime un restaurant")
	void deleteRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId);

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime une table")
	@DeleteMapping(path = "api/restaurants/tables/{tabId}")
	void deleteTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les catégories de plats.
	 *
	 * @return Liste des catégories de plats.
	 */
	@GetMapping(path = "api/restaurants/categorie-plats")
	@Operation(description = "Liste toutes les catégories de plats")
	List<CategoriePlat> getCategoriePlats();

	/**
	 * Charge le détail d'un client.
	 * @param cliId Identifiant du client.
	 *
	 * @return Détail du client.
	 */
	@GetMapping(path = "api/restaurants/clients/{cliId}")
	@Operation(description = "Charge le détail d'un client")
	ClientRead getClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Liste les commandes d'un client.
	 * @param cliId Identifiant du client.
	 *
	 * @return Liste des commandes du client.
	 */
	@Operation(description = "Liste les commandes d'un client")
	@GetMapping(path = "api/restaurants/clients/{cliId}/commandes")
	List<CommandeItem> getClientCommandes(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Liste tous les clients.
	 * @param nom Nom du client.
	 * @param email Adresse email du client.
	 *
	 * @return Liste des clients.
	 */
	@GetMapping(path = "api/restaurants/clients")
	@Operation(description = "Liste tous les clients")
	List<ClientItem> getClients(@Parameter(description = "Nom du client") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Adresse email du client") @RequestParam(value = "email", required = false) String email);

	/**
	 * Charge le détail d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail de la commande.
	 */
	@GetMapping(path = "api/restaurants/commandes/{comId}")
	@Operation(description = "Charge le détail d'une commande")
	CommandeRead getCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Liste les lignes d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Liste des lignes de la commande.
	 */
	@Operation(description = "Liste les lignes d'une commande")
	@GetMapping(path = "api/restaurants/commandes/{comId}/lignes")
	List<LigneCommandeItem> getCommandeLignes(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande.
	 * @param statutCommandeCode Statut de la commande.
	 * @param tableClientId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetMapping(path = "api/restaurants/commandes")
	@Operation(description = "Liste toutes les commandes")
	List<CommandeItem> getCommandes(@Parameter(description = "Client ayant passé la commande") @RequestParam(value = "clientId", required = true) Integer clientId, @Parameter(description = "Statut de la commande") @RequestParam(value = "statutCommandeCode", required = true) StatutCommande statutCommandeCode, @Parameter(description = "Table associée à la commande") @RequestParam(value = "tableClientId", required = false) Integer tableClientId);

	/**
	 * Récupère les commandes par date.
	 * @param dateCommande Date et heure de la commande.
	 *
	 * @return Commandes pour la date spécifiée.
	 */
	@GetMapping(path = "api/restaurants/commandes/by-date")
	@Operation(description = "Récupère les commandes par date")
	List<CommandeItem> getCommandesByDate(@Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateCommande", required = true) LocalDateTime dateCommande);

	/**
	 * Charge le détail d'une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 *
	 * @return Détail de la ligne de commande.
	 */
	@GetMapping(path = "api/restaurants/ligne-commandes/{ligId}")
	@Operation(description = "Charge le détail d'une ligne de commande")
	LigneCommandeRead getLigneCommande(@Parameter(description = "Identifiant de la ligne") @PathVariable("ligId") Integer ligId);

	/**
	 * Liste toutes les lignes de commande.
	 * @param commandeId Commande à laquelle appartient la ligne.
	 * @param platId Plat commandé.
	 *
	 * @return Liste des lignes de commande.
	 */
	@GetMapping(path = "api/restaurants/ligne-commandes")
	@Operation(description = "Liste toutes les lignes de commande")
	List<LigneCommandeItem> getLigneCommandes(@Parameter(description = "Commande à laquelle appartient la ligne") @RequestParam(value = "commandeId", required = true) Integer commandeId, @Parameter(description = "Plat commandé") @RequestParam(value = "platId", required = true) Integer platId);

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Détail du plat.
	 */
	@GetMapping(path = "api/restaurants/plats/{plaId}")
	@Operation(description = "Charge le détail d'un plat")
	PlatRead getPlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId);

	/**
	 * Liste tous les plats.
	 * @param disponible Indique si le plat est disponible.
	 * @param restaurantIdRestaurant Restaurant proposant ce plat.
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat.
	 *
	 * @return Liste des plats.
	 */
	@GetMapping(path = "api/restaurants/plats")
	@Operation(description = "Liste tous les plats")
	List<PlatItem> getPlats(@Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible, @Parameter(description = "Restaurant proposant ce plat") @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) CategoriePlat categoriePlatCodeCategoriePlat);

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Détail du restaurant.
	 */
	@GetMapping(path = "api/restaurants/{resId}")
	@Operation(description = "Charge le détail d'un restaurant")
	RestaurantRead getRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId);

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si le plat est disponible.
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat.
	 *
	 * @return Liste des plats du restaurant.
	 */
	@GetMapping(path = "api/restaurants/{resId}/plats")
	@Operation(description = "Liste les plats d'un restaurant")
	List<PlatItem> getRestaurantPlats(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) CategoriePlat categoriePlatCodeCategoriePlat);

	/**
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables du restaurant.
	 */
	@GetMapping(path = "api/restaurants/{resId}/tables")
	@Operation(description = "Liste les tables d'un restaurant")
	List<TableClientItem> getRestaurantTables(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Indique si la table est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Liste tous les restaurants.
	 *
	 * @return Liste des restaurants.
	 */
	@GetMapping(path = "api/restaurants")
	@Operation(description = "Liste tous les restaurants")
	List<RestaurantItem> getRestaurants();

	/**
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetMapping(path = "api/restaurants/statuts-commande")
	@Operation(description = "Liste tous les statuts de commande")
	List<StatutCommande> getStatutCommandes();

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Détail de la table.
	 */
	@GetMapping(path = "api/restaurants/tables/{tabId}")
	@Operation(description = "Charge le détail d'une table")
	TableClientRead getTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les tables.
	 * @param restaurantIdRestaurant Restaurant auquel appartient la table.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables.
	 */
	@GetMapping(path = "api/restaurants/tables")
	@Operation(description = "Liste toutes les tables")
	List<TableClientItem> getTables(@Parameter(description = "Restaurant auquel appartient la table") @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @Parameter(description = "Indique si la table est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour partiellement un client.
	 * @param cliId Identifiant du client.
	 * @param client Données partielles du client.
	 *
	 * @return Client mis à jour.
	 */
	@PatchMapping(path = "api/restaurants/clients/{cliId}")
	@Operation(description = "Met à jour partiellement un client")
	ClientRead patchClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles du client") @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour partiellement une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Données partielles de la commande.
	 *
	 * @return Commande mise à jour.
	 */
	@PatchMapping(path = "api/restaurants/commandes/{comId}")
	@Operation(description = "Met à jour partiellement une commande")
	CommandeRead patchCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles de la commande") @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Données partielles du plat.
	 *
	 * @return Plat mis à jour.
	 */
	@PatchMapping(path = "api/restaurants/plats/{plaId}")
	@Operation(description = "Met à jour partiellement un plat")
	PlatRead patchPlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles du plat") @RequestBody @Valid PlatWrite plat);

	/**
	 * Recherche de plats avec critères multiples.
	 * @param nom Nom du plat.
	 * @param restaurantIdRestaurant Restaurant proposant ce plat.
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat.
	 * @param disponible Indique si le plat est disponible.
	 *
	 * @return Plats correspondant aux critères de recherche.
	 */
	@GetMapping(path = "api/restaurants/plats/search")
	@Operation(description = "Recherche de plats avec critères multiples")
	List<PlatItem> searchPlats(@Parameter(description = "Nom du plat") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Restaurant proposant ce plat") @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) CategoriePlat categoriePlatCodeCategoriePlat, @Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour un client.
	 * @param cliId Identifiant du client.
	 * @param client Client à mettre à jour.
	 *
	 * @return Client mis à jour.
	 */
	@Operation(description = "Met à jour un client")
	@PutMapping(path = "api/restaurants/clients/{cliId}")
	ClientRead updateClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Client à mettre à jour") @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Commande à mettre à jour.
	 *
	 * @return Commande mise à jour.
	 */
	@Operation(description = "Met à jour une commande")
	@PutMapping(path = "api/restaurants/commandes/{comId}")
	CommandeRead updateCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Commande à mettre à jour") @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour uniquement le statut d'une commande.
	 * @param comId Identifiant de la commande.
	 * @param statutCommandeCode Statut de la commande.
	 *
	 * @return Commande avec le statut mis à jour.
	 */
	@PatchMapping(path = "api/restaurants/commandes/{comId}/statut")
	@Operation(description = "Met à jour uniquement le statut d'une commande")
	CommandeRead updateCommandeStatut(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId, @Parameter(description = "Statut de la commande") @RequestParam(value = "statutCommandeCode", required = true) StatutCommande statutCommandeCode);

	/**
	 * Met à jour une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 * @param ligneCommande Ligne de commande à mettre à jour.
	 *
	 * @return Ligne de commande mise à jour.
	 */
	@Operation(description = "Met à jour une ligne de commande")
	@PutMapping(path = "api/restaurants/ligne-commandes/{ligId}")
	LigneCommandeRead updateLigneCommande(@Parameter(description = "Identifiant de la ligne") @PathVariable("ligId") Integer ligId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Ligne de commande à mettre à jour") @RequestBody @Valid LigneCommandeWrite ligneCommande);

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Plat à mettre à jour.
	 *
	 * @return Plat mis à jour.
	 */
	@Operation(description = "Met à jour un plat")
	@PutMapping(path = "api/restaurants/plats/{plaId}")
	PlatRead updatePlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Plat à mettre à jour") @RequestBody @Valid PlatWrite plat);

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param restaurant Restaurant à mettre à jour.
	 *
	 * @return Restaurant mis à jour.
	 */
	@PutMapping(path = "api/restaurants/{resId}")
	@Operation(description = "Met à jour un restaurant")
	RestaurantRead updateRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Restaurant à mettre à jour") @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table.
	 * @param table Table à mettre à jour.
	 *
	 * @return Table mise à jour.
	 */
	@Operation(description = "Met à jour une table")
	@PutMapping(path = "api/restaurants/tables/{tabId}")
	TableClientRead updateTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Table à mettre à jour") @RequestBody @Valid TableClientWrite table);
}
