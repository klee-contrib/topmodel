////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'une promotion en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PromotionWrite implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Libellé de la promotion.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Promotion#getLibelle() Promotion#getLibelle()}
	 */
	@NotNull
	@Size(max = 100)
	private String libelle;

	/**
	 * Pourcentage de réduction (0-100).
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Promotion#getPourcentageReduction() Promotion#getPourcentageReduction()}
	 */
	@NotNull
	private Integer pourcentageReduction;

	/**
	 * Date de début de la promotion.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Promotion#getDateDebut() Promotion#getDateDebut()}
	 */
	@NotNull
	private LocalDateTime dateDebut;

	/**
	 * Date de fin de la promotion.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Promotion#getDateFin() Promotion#getDateFin()}
	 */
	@NotNull
	private LocalDateTime dateFin;

	/**
	 * Indique si la promotion est active.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Promotion#getActive() Promotion#getActive()}
	 */
	@NotNull
	private Boolean active = true;

	/**
	 * Restaurant concerné par la promotion (null si globale).
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Promotion#getRestaurant() Promotion#getRestaurant()}
	 */
	private Integer restaurantId;

	/**
	 * Accesseur de 'libelle'.
	 *
	 * @return Valeur de {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Accesseur de 'pourcentageReduction'.
	 *
	 * @return Valeur de {@link #pourcentageReduction pourcentageReduction}.
	 */
	public Integer getPourcentageReduction() {
		return this.pourcentageReduction;
	}

	/**
	 * Accesseur de 'dateDebut'.
	 *
	 * @return Valeur de {@link #dateDebut dateDebut}.
	 */
	public LocalDateTime getDateDebut() {
		return this.dateDebut;
	}

	/**
	 * Accesseur de 'dateFin'.
	 *
	 * @return Valeur de {@link #dateFin dateFin}.
	 */
	public LocalDateTime getDateFin() {
		return this.dateFin;
	}

	/**
	 * Accesseur de 'active'.
	 *
	 * @return Valeur de {@link #active active}.
	 */
	public Boolean getActive() {
		return this.active;
	}

	/**
	 * Accesseur de 'restaurantId'.
	 *
	 * @return Valeur de {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
	}

	/**
	 * Définit la valeur de {@link #libelle libelle}.
	 * @param libelle Valeur à définir.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Définit la valeur de {@link #pourcentageReduction pourcentageReduction}.
	 * @param pourcentageReduction Valeur à définir.
	 */
	public void setPourcentageReduction(Integer pourcentageReduction) {
		this.pourcentageReduction = pourcentageReduction;
	}

	/**
	 * Définit la valeur de {@link #dateDebut dateDebut}.
	 * @param dateDebut Valeur à définir.
	 */
	public void setDateDebut(LocalDateTime dateDebut) {
		this.dateDebut = dateDebut;
	}

	/**
	 * Définit la valeur de {@link #dateFin dateFin}.
	 * @param dateFin Valeur à définir.
	 */
	public void setDateFin(LocalDateTime dateFin) {
		this.dateFin = dateFin;
	}

	/**
	 * Définit la valeur de {@link #active active}.
	 * @param active Valeur à définir.
	 */
	public void setActive(Boolean active) {
		this.active = active;
	}

	/**
	 * Définit la valeur de {@link #restaurantId restaurantId}.
	 * @param restaurantId Valeur à définir.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.PromotionWrite PromotionWrite}.
	 */
	public enum Fields {
		LIBELLE(String.class),
		POURCENTAGE_REDUCTION(Integer.class),
		DATE_DEBUT(LocalDateTime.class),
		DATE_FIN(LocalDateTime.class),
		ACTIVE(Boolean.class),
		RESTAURANT_ID(Integer.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Accesseur de 'type'.
		 *
		 * @return Valeur de {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}
