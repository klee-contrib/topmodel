////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.api.server.restaurant;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
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

import restaurant.jpa_identity_enums.dtos.restaurant.CommandeItem;
import restaurant.jpa_identity_enums.dtos.restaurant.CommandeRead;
import restaurant.jpa_identity_enums.dtos.restaurant.CommandeWrite;
import restaurant.jpa_identity_enums.dtos.restaurant.ReservationRead;
import restaurant.jpa_identity_enums.dtos.restaurant.ReservationWrite;
import restaurant.jpa_identity_enums.enums.restaurant.StatutCommande;

@FeignClient(name = "Restaurant", contextId = "CommandeApi")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface CommandeApi {

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
	 * Crée une réservation.
	 * @param reservation Réservation à créer.
	 *
	 * @return Réservation créée.
	 */
	@Operation(description = "Crée une réservation")
	@PostMapping(path = "api/restaurants/reservations")
	ReservationRead createReservation(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Réservation à créer") @RequestBody @Valid ReservationWrite reservation);

	/**
	 * Supprime une commande.
	 * @param comId Identifiant de la commande.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime une commande")
	@DeleteMapping(path = "api/restaurants/commandes/{comId}")
	void deleteCommande(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

	/**
	 * Supprime une commande.
	 * @param commandeItem Commande item à supprimer dans le body.
	 */
	@ResponseStatus(HttpStatus.NO_CONTENT)
	@Operation(description = "Supprime une commande")
	@DeleteMapping(path = "api/restaurants/commandes")
	void deleteCommandeWithBody(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Commande item à supprimer dans le body") @RequestBody @Valid CommandeItem commandeItem);

	/**
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande.
	 * @param dateFin Date et heure de la commande.
	 *
	 * @return Fichier CSV des commandes.
	 */
	@PreAuthorize("isAuthenticated()")
	@Operation(description = "Exporte les commandes au format CSV")
	@GetMapping(path = "api/restaurants/commandes/export", produces = "application/octet-stream")
	byte[] exportCommandes(@Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

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
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande.
	 * @param statutCommandeCode Statut de la commande.
	 * @param tableId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetMapping(path = "api/restaurants/commandes")
	@Operation(description = "Liste toutes les commandes")
	List<CommandeItem> getCommandes(@Parameter(description = "Client ayant passé la commande") @RequestParam(value = "clientId", required = true) Integer clientId, @Parameter(description = "Statut de la commande") @RequestParam(value = "statutCommandeCode", required = true) StatutCommande statutCommandeCode, @Parameter(description = "Table associée à la commande") @RequestParam(value = "tableId", required = false) Integer tableId);

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
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetMapping(path = "api/restaurants/statuts-commande")
	@Operation(description = "Liste tous les statuts de commande")
	List<StatutCommande> getStatutCommandes();

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
}
