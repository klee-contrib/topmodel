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

import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeDetailRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.CommandeWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.LigneCommandeItem;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ReservationAvecDetails;
import restaurant.jpa_uuid_jdbc.dtos.restaurant.ReservationWrite;
import restaurant.jpa_uuid_jdbc.entities.restaurant.StatutCommande;

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
	 * Crée une réservation.
	 * @param reservation Réservation à créer.
	 *
	 * @return Réservation créée.
	 */
	@PostMapping(path = "reservations")
	ReservationAvecDetails createReservation(@RequestBody @Valid ReservationWrite reservation);

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
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande.
	 * @param dateFin Date et heure de la commande.
	 *
	 * @return Fichier CSV des commandes.
	 */
	@PreAuthorize("isAuthenticated()")
	@GetMapping(path = "commandes/export", produces = "application/octet-stream")
	byte[] exportCommandes(@RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Charge le détail d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail de la commande.
	 */
	@GetMapping(path = "commandes/{comId}")
	CommandeRead getCommande(@PathVariable("comId") Integer comId);

	/**
	 * Récupère le détail complet d'une commande avec ses lignes.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail complet de la commande.
	 */
	@GetMapping(path = "commandes/{comId}/detail")
	CommandeDetailRead getCommandeDetail(@PathVariable("comId") Integer comId);

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
	 * @param tableId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetMapping(path = "commandes")
	List<CommandeItem> getCommandes(@RequestParam(value = "clientId", required = true) Integer clientId, @RequestParam(value = "statutCommandeCode", required = true) String statutCommandeCode, @RequestParam(value = "tableId", required = false) Integer tableId);

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
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetMapping(path = "statuts-commande")
	List<StatutCommande> getStatutCommandes();

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
}
