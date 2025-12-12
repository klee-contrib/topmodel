////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
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

import restaurant.jpa_uuid_jdbc.dtos.restaurant.AvisClientRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.EmployeRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.EmployeWrite;

@RequestMapping("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface PersonneController {

	/**
	 * Ajoute un client.
	 * @param client Client à créer.
	 *
	 * @return Client créé.
	 */
	@PostMapping(path = "clients")
	ClientRead addClient(@RequestBody @Valid ClientWrite client);

	/**
	 * Ajoute un employé (nécessite le rôle ADMIN).
	 * @param employe Employé à créer.
	 *
	 * @return Employé créé.
	 */
	@PostMapping(path = "employes")
	@PreAuthorize("isAuthenticated()")
	EmployeRead addEmploye(@RequestBody @Valid EmployeWrite employe);

	/**
	 * Supprime un client.
	 * @param perId Identifiant de la personne.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "clients/{perId}")
	void deleteClient(@PathVariable("perId") Integer perId);

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
	@GetMapping(path = "avis")
	List<AvisClientRead> getAvisClients(@RequestParam(value = "resRestaurantId", required = true) Integer resRestaurantId, @RequestParam(value = "noteMin", required = true) Integer noteMin, @RequestParam(value = "approuve", required = true) Boolean approuve, @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Charge le détail d'un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Détail du client.
	 */
	@GetMapping(path = "clients/{perId}")
	ClientRead getClient(@PathVariable("perId") Integer perId);

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Client avec ses commandes.
	 */
	@GetMapping(path = "clients/{perId}/avec-commandes")
	ClientAvecCommandes getClientAvecCommandes(@PathVariable("perId") Integer perId);

	/**
	 * Liste les commandes d'un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Liste des commandes du client.
	 */
	@GetMapping(path = "clients/{perId}/commandes")
	List<CommandeItem> getClientCommandes(@PathVariable("perId") Integer perId);

	/**
	 * Liste tous les clients.
	 * @param nom Nom de la personne.
	 * @param email Adresse email du client.
	 *
	 * @return Liste des clients.
	 */
	@GetMapping(path = "clients")
	List<ClientItem> getClients(@RequestParam(value = "nom", required = true) String nom, @RequestParam(value = "email", required = false) String email);

	/**
	 * Met à jour partiellement un client.
	 * @param perId Identifiant de la personne.
	 * @param client Données partielles du client.
	 *
	 * @return Client mis à jour.
	 */
	@PatchMapping(path = "clients/{perId}")
	ClientRead patchClient(@PathVariable("perId") Integer perId, @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour un client.
	 * @param perId Identifiant de la personne.
	 * @param client Client à mettre à jour.
	 *
	 * @return Client mis à jour.
	 */
	@PutMapping(path = "clients/{perId}")
	ClientRead updateClient(@PathVariable("perId") Integer perId, @RequestBody @Valid ClientWrite client);
}
