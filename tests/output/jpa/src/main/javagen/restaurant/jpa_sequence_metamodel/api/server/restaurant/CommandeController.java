////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_metamodel.api.server.restaurant;

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

import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeDetailRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.CommandeWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.ReservationAvecDetails;
import restaurant.jpa_sequence_metamodel.dtos.restaurant.ReservationWrite;
import restaurant.jpa_sequence_metamodel.entities.restaurant.StatutCommande;
import restaurant.jpa_sequence_metamodel.enums.restaurant.StatutCommandeCode;

@RequestMapping("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface CommandeController {

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
	 * Crée une réservation.
	 * @param reservation Réservation à créer.
	 *
	 * @return Réservation créée.
	 */
	@PostMapping(path = "reservations")
	@Operation(description = "Crée une réservation")
	ReservationAvecDetails createReservation(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Réservation à créer") @RequestBody @Valid ReservationWrite reservation);

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
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande.
	 * @param dateFin Date et heure de la commande.
	 *
	 * @return Fichier CSV des commandes.
	 */
	@PreAuthorize("isAuthenticated()")
	@Operation(description = "Exporte les commandes au format CSV")
	@GetMapping(path = "commandes/export", produces = "application/octet-stream")
	byte[] exportCommandes(@Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @Parameter(description = "Date et heure de la commande") @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

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
	 * Récupère le détail complet d'une commande avec ses lignes.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail complet de la commande.
	 */
	@GetMapping(path = "commandes/{comId}/detail")
	@Operation(description = "Récupère le détail complet d'une commande avec ses lignes")
	CommandeDetailRead getCommandeDetail(@Parameter(description = "Identifiant de la commande") @PathVariable("comId") Integer comId);

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
	 * @param tableId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetMapping(path = "commandes")
	@Operation(description = "Liste toutes les commandes")
	List<CommandeItem> getCommandes(@Parameter(description = "Client ayant passé la commande") @RequestParam(value = "clientId", required = true) Integer clientId, @Parameter(description = "Statut de la commande") @RequestParam(value = "statutCommandeCode", required = true) StatutCommandeCode statutCommandeCode, @Parameter(description = "Table associée à la commande") @RequestParam(value = "tableId", required = false) Integer tableId);

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
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetMapping(path = "statuts-commande")
	@Operation(description = "Liste tous les statuts de commande")
	List<StatutCommande> getStatutCommandes();

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
}
