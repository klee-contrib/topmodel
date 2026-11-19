////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;

/**
 * Détail d'une ligne de commande en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class LigneCommandeRead implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la ligne.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getId() LigneCommande#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Quantité commandée.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getQuantite() LigneCommande#getQuantite()}
	 */
	@NotNull
	private Integer quantite;

	/**
	 * Prix unitaire au moment de la commande.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getPrixUnitaire() LigneCommande#getPrixUnitaire()}
	 */
	@NotNull
	private BigDecimal prixUnitaire;

	/**
	 * Prix total de la ligne.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getPrixTotal() LigneCommande#getPrixTotal()}
	 */
	@NotNull
	private BigDecimal prixTotal;

	/**
	 * Commande à laquelle appartient la ligne.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getCommande() LigneCommande#getCommande()}
	 */
	@NotNull
	private Integer commandeId;

	/**
	 * Plat commandé.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getPlat() LigneCommande#getPlat()}
	 */
	@NotNull
	private Integer platId;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_server.entities.restaurant.LigneCommande#getDateCreation() LigneCommande#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'quantite'.
	 *
	 * @return Valeur de {@link #quantite quantite}.
	 */
	public Integer getQuantite() {
		return this.quantite;
	}

	/**
	 * Accesseur de 'prixUnitaire'.
	 *
	 * @return Valeur de {@link #prixUnitaire prixUnitaire}.
	 */
	public BigDecimal getPrixUnitaire() {
		return this.prixUnitaire;
	}

	/**
	 * Accesseur de 'prixTotal'.
	 *
	 * @return Valeur de {@link #prixTotal prixTotal}.
	 */
	public BigDecimal getPrixTotal() {
		return this.prixTotal;
	}

	/**
	 * Accesseur de 'commandeId'.
	 *
	 * @return Valeur de {@link #commandeId commandeId}.
	 */
	public Integer getCommandeId() {
		return this.commandeId;
	}

	/**
	 * Accesseur de 'platId'.
	 *
	 * @return Valeur de {@link #platId platId}.
	 */
	public Integer getPlatId() {
		return this.platId;
	}

	/**
	 * Accesseur de 'dateCreation'.
	 *
	 * @return Valeur de {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #quantite quantite}.
	 * @param quantite Valeur à définir.
	 */
	public void setQuantite(Integer quantite) {
		this.quantite = quantite;
	}

	/**
	 * Définit la valeur de {@link #prixUnitaire prixUnitaire}.
	 * @param prixUnitaire Valeur à définir.
	 */
	public void setPrixUnitaire(BigDecimal prixUnitaire) {
		this.prixUnitaire = prixUnitaire;
	}

	/**
	 * Définit la valeur de {@link #prixTotal prixTotal}.
	 * @param prixTotal Valeur à définir.
	 */
	public void setPrixTotal(BigDecimal prixTotal) {
		this.prixTotal = prixTotal;
	}

	/**
	 * Définit la valeur de {@link #commandeId commandeId}.
	 * @param commandeId Valeur à définir.
	 */
	public void setCommandeId(Integer commandeId) {
		this.commandeId = commandeId;
	}

	/**
	 * Définit la valeur de {@link #platId platId}.
	 * @param platId Valeur à définir.
	 */
	public void setPlatId(Integer platId) {
		this.platId = platId;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}
}
