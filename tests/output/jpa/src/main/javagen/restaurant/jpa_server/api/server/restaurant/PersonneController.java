////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.api.server.restaurant;

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

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

import jakarta.annotation.Generated;
import jakarta.validation.Valid;

import restaurant.jpa_server.dtos.restaurant.AvisClientRead;
import restaurant.jpa_server.dtos.restaurant.ClientAvecCommandes;
import restaurant.jpa_server.dtos.restaurant.ClientItem;
import restaurant.jpa_server.dtos.restaurant.ClientRead;
import restaurant.jpa_server.dtos.restaurant.ClientWrite;
import restaurant.jpa_server.dtos.restaurant.CommandeItem;
import restaurant.jpa_server.dtos.restaurant.EmployeRead;
import restaurant.jpa_server.dtos.restaurant.EmployeWrite;

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
	@Operation(description = "Ajoute un client")
	ClientRead addClient(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Client à créer") @RequestBody @Valid ClientWrite client);

	/**
	 * Ajoute un employé (nécessite le rôle ADMIN).
	 * @param employe Employé à créer.
	 * @param token Token.
	 *
	 * @return Employé créé.
	 */
	@PostMapping(path = "employes")
	@PreAuthorize("isAuthenticated()")
	@Operation(description = "Ajoute un employé (nécessite le rôle ADMIN)")
	EmployeRead addEmploye(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Employé à créer") @RequestBody @Valid EmployeWrite employe, @Parameter(description = "Token") @RequestParam(value = "token", required = false) String token);

	/**
	 * Supprime un client.
	 * @param perId Identifiant de la personne.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@DeleteMapping(path = "clients/{perId}")
	@Operation(description = "Supprime un client")
	void deleteClient(@Parameter(description = "Identifiant de la personne") @PathVariable("perId") Integer perId);

	/**
	 * Liste les avis clients avec filtres.
	 * @param resId Identifiant du restaurant.
	 * @param noteMin Note sur 5.
	 * @param approuve Indique si l'avis est approuvé par le restaurant.
	 * @param dateDebut Date de l'avis.
	 * @param dateFin Date de l'avis.
	 *
	 * @return Liste des avis correspondant aux critères.
	 */
	@GetMapping(path = "avis")
	@Operation(description = "Liste les avis clients avec filtres")
	List<AvisClientRead> getAvisClients(@Parameter(description = "Identifiant du restaurant") @RequestParam(value = "resId", required = true) Integer resId, @Parameter(description = "Note sur 5") @RequestParam(value = "noteMin", required = true) Integer noteMin, @Parameter(description = "Indique si l'avis est approuvé par le restaurant") @RequestParam(value = "approuve", required = true) Boolean approuve, @Parameter(description = "Date de l'avis") @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @Parameter(description = "Date de l'avis") @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Charge le détail d'un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Détail du client.
	 */
	@GetMapping(path = "clients/{perId}")
	@Operation(description = "Charge le détail d'un client")
	ClientRead getClient(@Parameter(description = "Identifiant de la personne") @PathVariable("perId") Integer perId);

	/**
	 * Récupère un client avec toutes ses commandes.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Client avec ses commandes.
	 */
	@GetMapping(path = "clients/{perId}/avec-commandes")
	@Operation(description = "Récupère un client avec toutes ses commandes")
	ClientAvecCommandes getClientAvecCommandes(@Parameter(description = "Identifiant de la personne") @PathVariable("perId") Integer perId);

	/**
	 * Liste les commandes d'un client.
	 * @param perId Identifiant de la personne.
	 *
	 * @return Liste des commandes du client.
	 */
	@GetMapping(path = "clients/{perId}/commandes")
	@Operation(description = "Liste les commandes d'un client")
	List<CommandeItem> getClientCommandes(@Parameter(description = "Identifiant de la personne") @PathVariable("perId") Integer perId);

	/**
	 * Liste tous les clients.
	 * @param nom Nom de la personne.
	 * @param email Adresse email du client.
	 *
	 * @return Liste des clients.
	 */
	@GetMapping(path = "clients")
	@Operation(description = "Liste tous les clients")
	List<ClientItem> getClients(@Parameter(description = "Nom de la personne") @RequestParam(value = "nom", required = true) String nom, @Parameter(description = "Adresse email du client") @RequestParam(value = "email", required = false) String email);

	/**
	 * Met à jour partiellement un client.
	 * @param perId Identifiant de la personne.
	 * @param client Données partielles du client.
	 *
	 * @return Client mis à jour.
	 */
	@PatchMapping(path = "clients/{perId}")
	@Operation(description = "Met à jour partiellement un client")
	ClientRead patchClient(@Parameter(description = "Identifiant de la personne") @PathVariable("perId") Integer perId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Données partielles du client") @RequestBody @Valid ClientWrite client);

	/**
	 * Met à jour un client.
	 * @param perId Identifiant de la personne.
	 * @param client Client à mettre à jour.
	 *
	 * @return Client mis à jour.
	 */
	@PutMapping(path = "clients/{perId}")
	@Operation(description = "Met à jour un client")
	ClientRead updateClient(@Parameter(description = "Identifiant de la personne") @PathVariable("perId") Integer perId, @io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Client à mettre à jour") @RequestBody @Valid ClientWrite client);
}
