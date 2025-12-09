////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

import restaurant.jpa_identity_associations.entities.restaurant.Commande;
import restaurant.jpa_identity_associations.entities.restaurant.RestaurantMappers;
import restaurant.jpa_identity_associations.enums.restaurant.StatutCommandeCode;

/**
 * Détail d'une commande en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Date et heure de livraison.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Commande#getDateLivraison() Commande#getDateLivraison()}
	 */
	private LocalDateTime dateLivraison;

	/**
	 * Client ayant passé la commande.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Commande#getClient() Commande#getClient()}
	 */
	@NotNull
	private Integer clientId;

	/**
	 * Table associée à la commande.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Commande#getTableClient() Commande#getTableClient()}
	 */
	private Integer tableClientId;

	/**
	 * Statut de la commande.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Commande#getStatutCommande() Commande#getStatutCommande()}
	 */
	@NotNull
	private StatutCommandeCode statutCommandeCode = StatutCommandeCode.EN_ATT;

	/**
	 * Association réciproque de LigneCommande.CommandeId.
	 * Alias of {@link restaurant.jpa_identity_associations.entities.restaurant.Commande#getLigneCommandes() Commande#getLigneCommandes()}
	 */
	@NotNull
	private List<Integer> ligneCommandes;

	/**
	 * Getter for dateLivraison.
	 *
	 * @return value of {@link #dateLivraison dateLivraison}.
	 */
	public LocalDateTime getDateLivraison() {
		return this.dateLivraison;
	}

	/**
	 * Getter for clientId.
	 *
	 * @return value of {@link #clientId clientId}.
	 */
	public Integer getClientId() {
		return this.clientId;
	}

	/**
	 * Getter for tableClientId.
	 *
	 * @return value of {@link #tableClientId tableClientId}.
	 */
	public Integer getTableClientId() {
		return this.tableClientId;
	}

	/**
	 * Getter for statutCommandeCode.
	 *
	 * @return value of {@link #statutCommandeCode statutCommandeCode}.
	 */
	public StatutCommandeCode getStatutCommandeCode() {
		return this.statutCommandeCode;
	}

	/**
	 * Getter for ligneCommandes.
	 *
	 * @return value of {@link #ligneCommandes ligneCommandes}.
	 */
	public List<Integer> getLigneCommandes() {
		return this.ligneCommandes;
	}

	/**
	 * Set the value of {@link #dateLivraison dateLivraison}.
	 * @param dateLivraison value to set.
	 */
	public void setDateLivraison(LocalDateTime dateLivraison) {
		this.dateLivraison = dateLivraison;
	}

	/**
	 * Set the value of {@link #clientId clientId}.
	 * @param clientId value to set.
	 */
	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	/**
	 * Set the value of {@link #tableClientId tableClientId}.
	 * @param tableClientId value to set.
	 */
	public void setTableClientId(Integer tableClientId) {
		this.tableClientId = tableClientId;
	}

	/**
	 * Set the value of {@link #statutCommandeCode statutCommandeCode}.
	 * @param statutCommandeCode value to set.
	 */
	public void setStatutCommandeCode(StatutCommandeCode statutCommandeCode) {
		this.statutCommandeCode = statutCommandeCode;
	}

	/**
	 * Set the value of {@link #ligneCommandes ligneCommandes}.
	 * @param ligneCommandes value to set.
	 */
	public void setLigneCommandes(List<Integer> ligneCommandes) {
		this.ligneCommandes = ligneCommandes;
	}

	/**
	 * Mappe 'CommandeWrite' vers 'Commande'.
	 * @param target Instance pré-existante de 'Commande'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Commande'.
	 */
	public Commande toCommande(Commande target) {
		return RestaurantMappers.toCommande(this, target);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.dtos.restaurant.CommandeWrite CommandeWrite}.
	 */
	public enum Fields {
		DATE_LIVRAISON(LocalDateTime.class),
		CLIENT_ID(Integer.class),
		TABLE_CLIENT_ID(Integer.class),
		STATUT_COMMANDE_CODE(StatutCommandeCode.class),
		LIGNE_COMMANDES(List.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}
