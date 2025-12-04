////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_sequence_metamodel.dtos.restaurant.ClientItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.ClientRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.ClientWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.PlatItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.PlatRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.PlatWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.RestaurantItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.RestaurantRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.TableClientItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.TableClientRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.TableClientWrite;
import restaurant.jpa_sequence_metamodel.entities.restaurant.CategoriePlat;
import restaurant.jpa_sequence_metamodel.entities.restaurant.StatutCommande;
import restaurant.jpa_sequence_metamodel.enums.restaurant.CategoriePlatCode;
import restaurant.jpa_sequence_metamodel.enums.restaurant.StatutCommandeCode;

@RequestMapping("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface EndpointsController {

	/**
	 * Ajoute un client.
	 * @param client Client à créer.
	 *
	 * @return Client créé.
	 */
	@PostMapping(path = "clients")
	@Operation(description = "Ajoute un client")
	ClientRead addClient(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Client à créer") @RequestBody @Valid ClientWrite client);

	/**
	 * Crée une nouvelle commande.
	 * @param commande Commande à créer.
	 *
	 * @return Commande créée.
	 */
	@PostMapping(path = "commandes")
	@Operation(description = "Crée une nouvelle commande")
	CommandeRead addCommande(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Commande à créer") @RequestBody @Valid CommandeWrite commande);

	/**
	 * Ajoute une ligne de commande.
	 * @param ligneCommande Ligne de commande à créer.
	 *
	 * @return Ligne de commande créée.
	 */
	@PostMapping(path = "ligne-commandes")
	@Operation(description = "Ajoute une ligne de commande")
	LigneCommandeRead addLigneCommande(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Ligne de commande à créer") @RequestBody @Valid LigneCommandeWrite ligneCommande);

	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer.
	 *
	 * @return Plat créé.
	 */
	@PostMapping(path = "plats")
	@Operation(description = "Ajoute un plat")
	PlatRead addPlat(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Plat à créer") @RequestBody @Valid PlatWrite plat);

	/**
	 * Ajoute un restaurant.
	 * @param restaurant Restaurant à créer.
	 *
	 * @return Restaurant créé.
	 */
	@PostMapping(path = "")
	@Operation(description = "Ajoute un restaurant")
	RestaurantRead addRestaurant(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Restaurant à créer") @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Ajoute une table.
	 * @param table Table à créer.
	 *
	 * @return Table créée.
	 */
	@PostMapping(path = "tables")
	@Operation(description = "Ajoute une table")
	TableClientRead addTable(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Table à créer") @RequestBody @Valid TableClientWrite table);

	/**
	 * Supprime un client.
	 * @param cliId Identifiant du client.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "clients/{cliId}")
	@Operation(description = "Supprime un client")
	void deleteClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Supprime une commande.
	 * @param comId Identifiant de la commande.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "commandes/{comId}")
	@Operation(description = "Supprime une commande")
	void deleteCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Supprime une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "ligne-commandes/{ligId}")
	@Operation(description = "Supprime une ligne de commande")
	void deleteLigneCommande(@Parameter(description = "Identifiant de la ligne") @PathVariable("ligId") Integer ligId);

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat.
	 */
	@DeleteMapping(path = "plats/{plaId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un plat")
	void deletePlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId);

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant.
	 */
	@DeleteMapping(path = "{resId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime un restaurant")
	void deleteRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId);

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "tables/{tabId}")
	@Operation(description = "Supprime une table")
	void deleteTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les catégories de plats.
	 *
	 * @return Liste des catégories de plats.
	 */
	@GetMapping(path = "categorie-plats")
	@Operation(description = "Liste toutes les catégories de plats")
	List<CategoriePlat> getCategoriePlats();

	/**
	 * Charge le détail d'un client.
	 * @param cliId Identifiant du client.
	 *
	 * @return Détail du client.
	 */
	@GetMapping(path = "clients/{cliId}")
	@Operation(description = "Charge le détail d'un client")
	ClientRead getClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Liste les commandes d'un client.
	 * @param cliId Identifiant du client.
	 *
	 * @return Liste des commandes du client.
	 */
	@GetMapping(path = "clients/{cliId}/commandes")
	@Operation(description = "Liste les commandes d'un client")
	List<CommandeItem> getClientCommandes(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId);

	/**
	 * Liste tous les clients.
	 * @param nom Nom du client.
	 * @param email Adresse email du client.
	 *
	 * @return Liste des clients.
	 */
	@GetMapping(path = "clients")
	@Operation(description = "Liste tous les clients")
	List<ClientItem> getClients(@Parameter(description = "Nom du client") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Adresse email du client") @RequestParam(value = "email", required = false) String email);

	/**
	 * Charge le détail d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail de la commande.
	 */
	@GetMapping(path = "commandes/{comId}")
	@Operation(description = "Charge le détail d'une commande")
	CommandeRead getCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Liste les lignes d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Liste des lignes de la commande.
	 */
	@GetMapping(path = "commandes/{comId}/lignes")
	@Operation(description = "Liste les lignes d'une commande")
	List<LigneCommandeItem> getCommandeLignes(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande.
	 * @param statutCommandeCode Statut de la commande.
	 * @param tableClientId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetMapping(path = "commandes")
	@Operation(description = "Liste toutes les commandes")
	List<CommandeItem> getCommandes(@Parameter(description = "Client ayant passé la commande") @RequestParam(value = "clientId", required = true) Integer clientId, @Parameter(description = "Statut de la commande") @RequestParam(value = "statutCommandeCode", required = true) StatutCommandeCode statutCommandeCode, @Parameter(description = "Table associée à la commande") @RequestParam(value = "tableClientId", required = false) Integer tableClientId);

	/**
	 * Récupère les commandes par date.
	 * @param dateCommande Date et heure de la commande.
	 *
	 * @return Commandes pour la date spécifiée.
	 */
	@GetMapping(path = "commandes/by-date")
	@Operation(description = "Récupère les commandes par date")
	List<CommandeItem> getCommandesByDate(@Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateCommande", required = true) LocalDateTime dateCommande);

	/**
	 * Charge le détail d'une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 *
	 * @return Détail de la ligne de commande.
	 */
	@GetMapping(path = "ligne-commandes/{ligId}")
	@Operation(description = "Charge le détail d'une ligne de commande")
	LigneCommandeRead getLigneCommande(@Parameter(description = "Identifiant de la ligne") @PathVariable("ligId") Integer ligId);

	/**
	 * Liste toutes les lignes de commande.
	 * @param commandeId Commande à laquelle appartient la ligne.
	 * @param platId Plat commandé.
	 *
	 * @return Liste des lignes de commande.
	 */
	@GetMapping(path = "ligne-commandes")
	@Operation(description = "Liste toutes les lignes de commande")
	List<LigneCommandeItem> getLigneCommandes(@Parameter(description = "Commande à laquelle appartient la ligne") @RequestParam(value = "commandeId", required = true) Integer commandeId, @Parameter(description = "Plat commandé") @RequestParam(value = "platId", required = true) Integer platId);

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Détail du plat.
	 */
	@GetMapping(path = "plats/{plaId}")
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
	@GetMapping(path = "plats")
	@Operation(description = "Liste tous les plats")
	List<PlatItem> getPlats(@Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible, @Parameter(description = "Restaurant proposant ce plat") @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) CategoriePlatCode categoriePlatCodeCategoriePlat);

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Détail du restaurant.
	 */
	@GetMapping(path = "{resId}")
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
	@GetMapping(path = "{resId}/plats")
	@Operation(description = "Liste les plats d'un restaurant")
	List<PlatItem> getRestaurantPlats(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) CategoriePlatCode categoriePlatCodeCategoriePlat);

	/**
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables du restaurant.
	 */
	@GetMapping(path = "{resId}/tables")
	@Operation(description = "Liste les tables d'un restaurant")
	List<TableClientItem> getRestaurantTables(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @Parameter(description = "Indique si la table est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Liste tous les restaurants.
	 *
	 * @return Liste des restaurants.
	 */
	@GetMapping(path = "")
	@Operation(description = "Liste tous les restaurants")
	List<RestaurantItem> getRestaurants();

	/**
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetMapping(path = "statuts-commande")
	@Operation(description = "Liste tous les statuts de commande")
	List<StatutCommande> getStatutCommandes();

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Détail de la table.
	 */
	@GetMapping(path = "tables/{tabId}")
	@Operation(description = "Charge le détail d'une table")
	TableClientRead getTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les tables.
	 * @param restaurantIdRestaurant Restaurant auquel appartient la table.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables.
	 */
	@GetMapping(path = "tables")
	@Operation(description = "Liste toutes les tables")
	List<TableClientItem> getTables(@Parameter(description = "Restaurant auquel appartient la table") @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @Parameter(description = "Indique si la table est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour partiellement un client.
	 * @param cliId Identifiant du client.
	 * @param client Données partielles du client.
	 *
	 * @return Client mis à jour.
	 */
	@PatchMapping(path = "clients/{cliId}")
	@Operation(description = "Met à jour partiellement un client")
	ClientRead patchClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles du client") @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour partiellement une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Données partielles de la commande.
	 *
	 * @return Commande mise à jour.
	 */
	@PatchMapping(path = "commandes/{comId}")
	@Operation(description = "Met à jour partiellement une commande")
	CommandeRead patchCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles de la commande") @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Données partielles du plat.
	 *
	 * @return Plat mis à jour.
	 */
	@PatchMapping(path = "plats/{plaId}")
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
	@GetMapping(path = "plats/search")
	@Operation(description = "Recherche de plats avec critères multiples")
	List<PlatItem> searchPlats(@Parameter(description = "Nom du plat") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Restaurant proposant ce plat") @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @Parameter(description = "Catégorie du plat") @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) CategoriePlatCode categoriePlatCodeCategoriePlat, @Parameter(description = "Indique si le plat est disponible") @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour un client.
	 * @param cliId Identifiant du client.
	 * @param client Client à mettre à jour.
	 *
	 * @return Client mis à jour.
	 */
	@PutMapping(path = "clients/{cliId}")
	@Operation(description = "Met à jour un client")
	ClientRead updateClient(@Parameter(description = "Identifiant du client") @PathVariable("cliId") Integer cliId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Client à mettre à jour") @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Commande à mettre à jour.
	 *
	 * @return Commande mise à jour.
	 */
	@PutMapping(path = "commandes/{comId}")
	@Operation(description = "Met à jour une commande")
	CommandeRead updateCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Commande à mettre à jour") @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour uniquement le statut d'une commande.
	 * @param comId Identifiant de la commande.
	 * @param statutCommandeCode Statut de la commande.
	 *
	 * @return Commande avec le statut mis à jour.
	 */
	@PatchMapping(path = "commandes/{comId}/statut")
	@Operation(description = "Met à jour uniquement le statut d'une commande")
	CommandeRead updateCommandeStatut(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId, @Parameter(description = "Statut de la commande") @RequestParam(value = "statutCommandeCode", required = true) StatutCommandeCode statutCommandeCode);

	/**
	 * Met à jour une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 * @param ligneCommande Ligne de commande à mettre à jour.
	 *
	 * @return Ligne de commande mise à jour.
	 */
	@PutMapping(path = "ligne-commandes/{ligId}")
	@Operation(description = "Met à jour une ligne de commande")
	LigneCommandeRead updateLigneCommande(@Parameter(description = "Identifiant de la ligne") @PathVariable("ligId") Integer ligId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Ligne de commande à mettre à jour") @RequestBody @Valid LigneCommandeWrite ligneCommande);

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Plat à mettre à jour.
	 *
	 * @return Plat mis à jour.
	 */
	@PutMapping(path = "plats/{plaId}")
	@Operation(description = "Met à jour un plat")
	PlatRead updatePlat(@Parameter(description = "Identifiant du plat") @PathVariable("plaId") Integer plaId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Plat à mettre à jour") @RequestBody @Valid PlatWrite plat);

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param restaurant Restaurant à mettre à jour.
	 *
	 * @return Restaurant mis à jour.
	 */
	@PutMapping(path = "{resId}")
	@Operation(description = "Met à jour un restaurant")
	RestaurantRead updateRestaurant(@Parameter(description = "Identifiant du restaurant") @PathVariable("resId") Integer resId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Restaurant à mettre à jour") @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table.
	 * @param table Table à mettre à jour.
	 *
	 * @return Table mise à jour.
	 */
	@PutMapping(path = "tables/{tabId}")
	@Operation(description = "Met à jour une table")
	TableClientRead updateTable(@Parameter(description = "Identifiant de la table") @PathVariable("tabId") Integer tabId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Table à mettre à jour") @RequestBody @Valid TableClientWrite table);
}
