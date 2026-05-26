////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.service.annotation.DeleteExchange;
import org.springframework.web.service.annotation.GetExchange;
import org.springframework.web.service.annotation.HttpExchange;
import org.springframework.web.service.annotation.PatchExchange;
import org.springframework.web.service.annotation.PostExchange;
import org.springframework.web.service.annotation.PutExchange;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_sequence_server.dtos.restaurant.AvisClientRead;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientItem;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientRead;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.CommandeItem;
import restaurant.jpa_sequence_server.dtos.restaurant.EmployeRead;
import restaurant.jpa_sequence_server.dtos.restaurant.EmployeWrite;

@HttpExchange("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface PersonneClient {


	/**
	 * Ajoute un client.
	 * @param client Client à créer.
	 *
	 * @return Client créé.
	 */
	@PostExchange("/clients")
	ResponseEntity<ClientRead> addClient(@RequestBody @Valid ClientWrite client);

	/**
	 * Ajoute un employé (nécessite le rôle ADMIN).
	 * @param employe Employé à créer.
	 *
	 * @return Employé créé.
	 */
	@PostExchange("/employes")
	ResponseEntity<EmployeRead> addEmploye(@RequestBody @Valid EmployeWrite employe);

	/**
	 * Supprime un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Aucun retour.
	 */
	@DeleteExchange("/clients/{perId}")
	ResponseEntity<Void> deleteClient(@PathVariable("perId") Integer perId);

	/**
	 * Liste les avis clients avec filtres.
	 * @param resRestaurantId Identifiant du restaurant.
	 * @param noteMin Note sur 5.
	 * @param approuve Indique si l'avis est approuvé par le restaurant.
	 * @param dateDebut Date de l'avis.
	 * @param dateFin Date de l'avis.
	 *
	 * @return Liste des avis correspondant aux critères.
	 */
	@GetExchange("/avis")
	ResponseEntity<List<AvisClientRead>> getAvisClients(@RequestParam(value = "resRestaurantId", required = true) Integer resRestaurantId, @RequestParam(value = "noteMin", required = true) Integer noteMin, @RequestParam(value = "approuve", required = true) Boolean approuve, @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Charge le détail d'un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Détail du client.
	 */
	@GetExchange("/clients/{perId}")
	ResponseEntity<ClientRead> getClient(@PathVariable("perId") Integer perId);

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Client avec ses commandes.
	 */
	@GetExchange("/clients/{perId}/avec-commandes")
	ResponseEntity<ClientAvecCommandes> getClientAvecCommandes(@PathVariable("perId") Integer perId);

	/**
	 * Liste les commandes d'un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Liste des commandes du client.
	 */
	@GetExchange("/clients/{perId}/commandes")
	ResponseEntity<List<CommandeItem>> getClientCommandes(@PathVariable("perId") Integer perId);

	/**
	 * Liste tous les clients.
	 * @param nom Nom de la personne.
	 * @param email Adresse email du client.
	 *
	 * @return Liste des clients.
	 */
	@GetExchange("/clients")
	ResponseEntity<List<ClientItem>> getClients(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "email", required = false) String email);

	/**
	 * Met à jour partiellement un client.
	 * @param perId Identifiant de la personne.
	 * @param client Données partielles du client.
	 *
	 * @return Client mis à jour.
	 */
	@PatchExchange("/clients/{perId}")
	ResponseEntity<ClientRead> patchClient(@PathVariable("perId") Integer perId, @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour un client.
	 * @param perId Identifiant de la personne.
	 * @param client Client à mettre à jour.
	 *
	 * @return Client mis à jour.
	 */
	@PutExchange("/clients/{perId}")
	ResponseEntity<ClientRead> updateClient(@PathVariable("perId") Integer perId, @RequestBody @Valid ClientWrite client);
}
