////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.api.server.restaurant;

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

import restaurant.jpa_server.dtos.restaurant.CommandeDeleteResult;
import restaurant.jpa_server.dtos.restaurant.CommandeItem;
import restaurant.jpa_server.dtos.restaurant.CommandeRead;
import restaurant.jpa_server.dtos.restaurant.CommandeWrite;
import restaurant.jpa_server.dtos.restaurant.ReservationRead;
import restaurant.jpa_server.dtos.restaurant.ReservationWrite;
import restaurant.jpa_server.enums.restaurant.StatutCommande;

@HttpExchange("api/restaurants")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface CommandeClient {


	/**
	 * Crée une nouvelle commande.
	 * @param commande Commande à créer.
	 *
	 * @return Commande créée.
	 */
	@PostExchange("/commandes")
	ResponseEntity<CommandeRead> addCommande(@RequestBody @Valid CommandeWrite commande);

	/**
	 * Crée une réservation.
	 * @param reservation Réservation à créer.
	 *
	 * @return Réservation créée.
	 */
	@PostExchange("/reservations")
	ResponseEntity<ReservationRead> createReservation(@RequestBody @Valid ReservationWrite reservation);

	/**
	 * Supprime une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Aucun retour.
	 */
	@DeleteExchange("/commandes/{comId}")
	ResponseEntity<Void> deleteCommande(@PathVariable("comId") Integer comId);

	/**
	 * Supprime une commande.
	 * @param commandeItem Commande item à supprimer dans le body.
	 *
	 * @return Détail de la suppression.
	 */
	@DeleteExchange("/commandes")
	ResponseEntity<CommandeDeleteResult> deleteCommandeWithBody(@RequestBody @Valid CommandeItem commandeItem);

	/**
	 * Exporte les commandes au format CSV.
	 * @param dateDebut Date et heure de la commande.
	 * @param dateFin Date et heure de la commande.
	 *
	 * @return Fichier CSV des commandes.
	 */
	@GetExchange(value = "/commandes/export", accept = { "application/octet-stream" })
	ResponseEntity<byte[]> exportCommandes(@RequestParam(value = "dateDebut", required = true) LocalDateTime dateDebut, @RequestParam(value = "dateFin", required = true) LocalDateTime dateFin);

	/**
	 * Charge le détail d'une commande.
	 * @param comId Identifiant de la commande.
	 *
	 * @return Détail de la commande.
	 */
	@GetExchange("/commandes/{comId}")
	ResponseEntity<CommandeRead> getCommande(@PathVariable("comId") Integer comId);

	/**
	 * Liste toutes les commandes.
	 * @param clientId Client ayant passé la commande.
	 * @param statutCommande Statut de la commande.
	 * @param tableId Table associée à la commande.
	 *
	 * @return Liste des commandes.
	 */
	@GetExchange("/commandes")
	ResponseEntity<List<CommandeItem>> getCommandes(@RequestParam(value = "clientId", required = true) Integer clientId, @RequestParam(value = "statutCommande", required = true) StatutCommande statutCommande, @RequestParam(value = "tableId", required = false) Integer tableId);

	/**
	 * Récupère les commandes par date.
	 * @param dateCommande Date et heure de la commande.
	 *
	 * @return Commandes pour la date spécifiée.
	 */
	@GetExchange("/commandes/by-date")
	ResponseEntity<List<CommandeItem>> getCommandesByDate(@RequestParam(value = "dateCommande", required = true) LocalDateTime dateCommande);

	/**
	 * Liste tous les statuts de commande.
	 *
	 * @return Liste des statuts de commande.
	 */
	@GetExchange("/statuts-commande")
	ResponseEntity<List<StatutCommande>> getStatutCommandes();

	/**
	 * Met à jour partiellement une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Données partielles de la commande.
	 *
	 * @return Commande mise à jour.
	 */
	@PatchExchange("/commandes/{comId}")
	ResponseEntity<CommandeRead> patchCommande(@PathVariable("comId") Integer comId, @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour une commande.
	 * @param comId Identifiant de la commande.
	 * @param commande Commande à mettre à jour.
	 *
	 * @return Commande mise à jour.
	 */
	@PutExchange("/commandes/{comId}")
	ResponseEntity<CommandeRead> updateCommande(@PathVariable("comId") Integer comId, @RequestBody @Valid CommandeWrite commande);

	/**
	 * Met à jour uniquement le statut d'une commande.
	 * @param comId Identifiant de la commande.
	 * @param statutCommande Statut de la commande.
	 *
	 * @return Commande avec le statut mis à jour.
	 */
	@PatchExchange("/commandes/{comId}/statut")
	ResponseEntity<CommandeRead> updateCommandeStatut(@PathVariable("comId") Integer comId, @RequestParam(value = "statutCommande", required = true) StatutCommande statutCommande);
}
