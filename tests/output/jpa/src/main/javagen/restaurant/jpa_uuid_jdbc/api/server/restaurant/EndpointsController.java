////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.api.server.restaurant;

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

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.TableClientItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.TableClientRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.TableClientWrite;
import restaurant.jpa_uuid_jdbc.entities.restaurant.CategoriePlat;
import restaurant.jpa_uuid_jdbc.entities.restaurant.StatutCommande;

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
	ClientRead addClient(@RequestBody @Valid ClientWrite client);

	/**
	 * Crée une nouvelle commande.
	 * @param commande Commande à créer.
	 *
	 * @return Commande créée.
	 */
	@PostMapping(path = "commandes")
	CommandeRead addCommande(@RequestBody @Valid CommandeWrite commande);

	/**
	 * Ajoute une ligne de commande.
	 * @param ligneCommande Ligne de commande à créer.
	 *
	 * @return Ligne de commande créée.
	 */
	@PostMapping(path = "ligne-commandes")
	LigneCommandeRead addLigneCommande(@RequestBody @Valid LigneCommandeWrite ligneCommande);

	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer.
	 *
	 * @return Plat créé.
	 */
	@PostMapping(path = "plats")
	PlatRead addPlat(@RequestBody @Valid PlatWrite plat);

	/**
	 * Ajoute un restaurant.
	 * @param restaurant Restaurant à créer.
	 *
	 * @return Restaurant créé.
	 */
	@PostMapping(path = "")
	RestaurantRead addRestaurant(@RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Ajoute une table.
	 * @param table Table à créer.
	 *
	 * @return Table créée.
	 */
	@PostMapping(path = "tables")
	TableClientRead addTable(@RequestBody @Valid TableClientWrite table);

	/**
	 * Supprime un client.
	 * @param cliId Identifiant du client.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "clients/{cliId}")
	void deleteClient(@PathVariable("cliId") Integer cliId);

	/**
	 * Supprime une commande.
	 * @param comId Identifiant de la commande.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "commandes/{comId}")
	void deleteCommande(@PathVariable("comId") Integer comId);

	/**
	 * Supprime une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "ligne-commandes/{ligId}")
	void deleteLigneCommande(@PathVariable("ligId") Integer ligId);

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat.
	 */
	@DeleteMapping(path = "plats/{plaId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	void deletePlat(@PathVariable("plaId") Integer plaId);

	/**
	 * Supprime un restaurant.
	 * @param resId Identifiant du restaurant.
	 */
	@DeleteMapping(path = "{resId}")
	@ResponseStatus(HttpStatus.NO_CONTENT)
	void deleteRestaurant(@PathVariable("resId") Integer resId);

	/**
	 * Supprime une table.
	 * @param tabId Identifiant de la table.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "tables/{tabId}")
	void deleteTable(@PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les catégories de plats.
	 *
	 * @return Liste des catégories de plats.
	 */
	@GetMapping(path = "categorie-plats")
	List<CategoriePlat> getCategoriePlats();

	/**
	 * Charge le détail d'un client.
	 * @param cliId Identifiant du client.
	 *
	 * @return Détail du client.
	 */
	@GetMapping(path = "clients/{cliId}")
	ClientRead getClient(@PathVariable("cliId") Integer cliId);

	/**
	 * Liste les commandes d'un client.
	 * @param cliId Identifiant du client.
	 *
	 * @return Liste des commandes du client.
	 */
	@GetMapping(path = "clients/{cliId}/commandes")
	List<CommandeItem> getClientCommandes(@PathVariable("cliId") Integer cliId);

	/**
	 * Liste tous les clients.
	 * @param nom Nom du client.
	 * @param email Adresse email du client.
	 *
	 * @return Liste des clients.
	 */
	@GetMapping(path = "clients")
	List<ClientItem> getClients(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "email", required = false) String email);

	/**
	 * Charge le détail d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail de la commande.
	 */
	@GetMapping(path = "commandes/{comId}")
	CommandeRead getCommande(@PathVariable("comId") Integer comId);

	/**
	 * Liste les lignes d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Liste des lignes de la commande.
	 */
	@GetMapping(path = "commandes/{comId}/lignes")
	List<LigneCommandeItem> getCommandeLignes(@PathVariable("comId") Integer comId);

	/**
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande.
	 * @param statutCommandeCode Statut de la commande.
	 * @param tableClientId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetMapping(path = "commandes")
	List<CommandeItem> getCommandes(@RequestParam(value = "clientId", required = true) Integer clientId, @RequestParam(value = "statutCommandeCode", required = true) String statutCommandeCode, @RequestParam(value = "tableClientId", required = false) Integer tableClientId);

	/**
	 * Récupère les commandes par date.
	 * @param dateCommande Date et heure de la commande.
	 *
	 * @return Commandes pour la date spécifiée.
	 */
	@GetMapping(path = "commandes/by-date")
	List<CommandeItem> getCommandesByDate(@RequestParam(value = "dateCommande", required = true) LocalDateTime dateCommande);

	/**
	 * Charge le détail d'une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 *
	 * @return Détail de la ligne de commande.
	 */
	@GetMapping(path = "ligne-commandes/{ligId}")
	LigneCommandeRead getLigneCommande(@PathVariable("ligId") Integer ligId);

	/**
	 * Liste toutes les lignes de commande.
	 * @param commandeId Commande à laquelle appartient la ligne.
	 * @param platId Plat commandé.
	 *
	 * @return Liste des lignes de commande.
	 */
	@GetMapping(path = "ligne-commandes")
	List<LigneCommandeItem> getLigneCommandes(@RequestParam(value = "commandeId", required = true) Integer commandeId, @RequestParam(value = "platId", required = true) Integer platId);

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat.
	 *
	 * @return Détail du plat.
	 */
	@GetMapping(path = "plats/{plaId}")
	PlatRead getPlat(@PathVariable("plaId") Integer plaId);

	/**
	 * Liste tous les plats.
	 * @param disponible Indique si le plat est disponible.
	 * @param restaurantIdRestaurant Restaurant proposant ce plat.
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat.
	 *
	 * @return Liste des plats.
	 */
	@GetMapping(path = "plats")
	List<PlatItem> getPlats(@RequestParam(value = "disponible", required = true) Boolean disponible, @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) String categoriePlatCodeCategoriePlat);

	/**
	 * Charge le détail d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 *
	 * @return Détail du restaurant.
	 */
	@GetMapping(path = "{resId}")
	RestaurantRead getRestaurant(@PathVariable("resId") Integer resId);

	/**
	 * Liste les plats d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si le plat est disponible.
	 * @param categoriePlatCodeCategoriePlat Catégorie du plat.
	 *
	 * @return Liste des plats du restaurant.
	 */
	@GetMapping(path = "{resId}/plats")
	List<PlatItem> getRestaurantPlats(@PathVariable("resId") Integer resId, @RequestParam(value = "disponible", required = true) Boolean disponible, @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) String categoriePlatCodeCategoriePlat);

	/**
	 * Liste les tables d'un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables du restaurant.
	 */
	@GetMapping(path = "{resId}/tables")
	List<TableClientItem> getRestaurantTables(@PathVariable("resId") Integer resId, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Liste tous les restaurants.
	 *
	 * @return Liste des restaurants.
	 */
	@GetMapping(path = "")
	List<RestaurantItem> getRestaurants();

	/**
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetMapping(path = "statuts-commande")
	List<StatutCommande> getStatutCommandes();

	/**
	 * Charge le détail d'une table.
	 * @param tabId Identifiant de la table.
	 *
	 * @return Détail de la table.
	 */
	@GetMapping(path = "tables/{tabId}")
	TableClientRead getTable(@PathVariable("tabId") Integer tabId);

	/**
	 * Liste toutes les tables.
	 * @param restaurantIdRestaurant Restaurant auquel appartient la table.
	 * @param disponible Indique si la table est disponible.
	 *
	 * @return Liste des tables.
	 */
	@GetMapping(path = "tables")
	List<TableClientItem> getTables(@RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour partiellement un client.
	 * @param cliId Identifiant du client.
	 * @param client Données partielles du client.
	 *
	 * @return Client mis à jour.
	 */
	@PatchMapping(path = "clients/{cliId}")
	ClientRead patchClient(@PathVariable("cliId") Integer cliId, @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour partiellement une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Données partielles de la commande.
	 *
	 * @return Commande mise à jour.
	 */
	@PatchMapping(path = "commandes/{comId}")
	CommandeRead patchCommande(@PathVariable("comId") Integer comId, @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Données partielles du plat.
	 *
	 * @return Plat mis à jour.
	 */
	@PatchMapping(path = "plats/{plaId}")
	PlatRead patchPlat(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PlatWrite plat);

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
	List<PlatItem> searchPlats(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "restaurantIdRestaurant", required = true) Integer restaurantIdRestaurant, @RequestParam(value = "categoriePlatCodeCategoriePlat", required = true) String categoriePlatCodeCategoriePlat, @RequestParam(value = "disponible", required = true) Boolean disponible);

	/**
	 * Met à jour un client.
	 * @param cliId Identifiant du client.
	 * @param client Client à mettre à jour.
	 *
	 * @return Client mis à jour.
	 */
	@PutMapping(path = "clients/{cliId}")
	ClientRead updateClient(@PathVariable("cliId") Integer cliId, @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Commande à mettre à jour.
	 *
	 * @return Commande mise à jour.
	 */
	@PutMapping(path = "commandes/{comId}")
	CommandeRead updateCommande(@PathVariable("comId") Integer comId, @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour uniquement le statut d'une commande.
	 * @param comId Identifiant de la commande.
	 * @param statutCommandeCode Statut de la commande.
	 *
	 * @return Commande avec le statut mis à jour.
	 */
	@PatchMapping(path = "commandes/{comId}/statut")
	CommandeRead updateCommandeStatut(@PathVariable("comId") Integer comId, @RequestParam(value = "statutCommandeCode", required = true) String statutCommandeCode);

	/**
	 * Met à jour une ligne de commande.
	 * @param ligId Identifiant de la ligne.
	 * @param ligneCommande Ligne de commande à mettre à jour.
	 *
	 * @return Ligne de commande mise à jour.
	 */
	@PutMapping(path = "ligne-commandes/{ligId}")
	LigneCommandeRead updateLigneCommande(@PathVariable("ligId") Integer ligId, @RequestBody @Valid LigneCommandeWrite ligneCommande);

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat.
	 * @param plat Plat à mettre à jour.
	 *
	 * @return Plat mis à jour.
	 */
	@PutMapping(path = "plats/{plaId}")
	PlatRead updatePlat(@PathVariable("plaId") Integer plaId, @RequestBody @Valid PlatWrite plat);

	/**
	 * Met à jour un restaurant.
	 * @param resId Identifiant du restaurant.
	 * @param restaurant Restaurant à mettre à jour.
	 *
	 * @return Restaurant mis à jour.
	 */
	@PutMapping(path = "{resId}")
	RestaurantRead updateRestaurant(@PathVariable("resId") Integer resId, @RequestBody @Valid RestaurantWrite restaurant);

	/**
	 * Met à jour une table.
	 * @param tabId Identifiant de la table.
	 * @param table Table à mettre à jour.
	 *
	 * @return Table mise à jour.
	 */
	@PutMapping(path = "tables/{tabId}")
	TableClientRead updateTable(@PathVariable("tabId") Integer tabId, @RequestBody @Valid TableClientWrite table);
}
