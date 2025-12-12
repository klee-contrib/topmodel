////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.util.List;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un plat en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class PlatWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom du plat.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getNom() Plat#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("pla_nom")
	private String nom;

	/**
	 * Description du plat.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getDescription() Plat#getDescription()}
	 */
	@Size(max = 100)
	@Column("pla_description")
	private String description;

	/**
	 * Prix du plat.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getPrix() Plat#getPrix()}
	 */
	@NotNull
	@Column("pla_prix")
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getDisponible() Plat#getDisponible()}
	 */
	@NotNull
	@Column("pla_disponible")
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getCategoriePlatCode() Plat#getCategoriePlatCode()}
	 */
	@NotNull
	@Size(max = 10)
	@Column("cat_code")
	private String categoriePlatCode;

	/**
	 * Restaurant proposant ce plat.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getRestaurantId() Plat#getRestaurantId()}
	 */
	@NotNull
	@Column("res_id")
	private Integer restaurantId;

	/**
	 * Association réciproque de LigneCommande.PlatId.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Plat#getLigneCommandes() Plat#getLigneCommandes()}
	 */
	@NotNull
	@Column("lig_id")
	private List<Integer> ligneCommandes;

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for description.
	 *
	 * @return value of {@link #description description}.
	 */
	public String getDescription() {
		return this.description;
	}

	/**
	 * Getter for prix.
	 *
	 * @return value of {@link #prix prix}.
	 */
	public BigDecimal getPrix() {
		return this.prix;
	}

	/**
	 * Getter for disponible.
	 *
	 * @return value of {@link #disponible disponible}.
	 */
	public Boolean getDisponible() {
		return this.disponible;
	}

	/**
	 * Getter for categoriePlatCode.
	 *
	 * @return value of {@link #categoriePlatCode categoriePlatCode}.
	 */
	public String getCategoriePlatCode() {
		return this.categoriePlatCode;
	}

	/**
	 * Getter for restaurantId.
	 *
	 * @return value of {@link #restaurantId restaurantId}.
	 */
	public Integer getRestaurantId() {
		return this.restaurantId;
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
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link #description description}.
	 * @param description value to set.
	 */
	public void setDescription(String description) {
		this.description = description;
	}

	/**
	 * Set the value of {@link #prix prix}.
	 * @param prix value to set.
	 */
	public void setPrix(BigDecimal prix) {
		this.prix = prix;
	}

	/**
	 * Set the value of {@link #disponible disponible}.
	 * @param disponible value to set.
	 */
	public void setDisponible(Boolean disponible) {
		this.disponible = disponible;
	}

	/**
	 * Set the value of {@link #categoriePlatCode categoriePlatCode}.
	 * @param categoriePlatCode value to set.
	 */
	public void setCategoriePlatCode(String categoriePlatCode) {
		this.categoriePlatCode = categoriePlatCode;
	}

	/**
	 * Set the value of {@link #restaurantId restaurantId}.
	 * @param restaurantId value to set.
	 */
	public void setRestaurantId(Integer restaurantId) {
		this.restaurantId = restaurantId;
	}

	/**
	 * Set the value of {@link #ligneCommandes ligneCommandes}.
	 * @param ligneCommandes value to set.
	 */
	public void setLigneCommandes(List<Integer> ligneCommandes) {
		this.ligneCommandes = ligneCommandes;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.PlatWrite PlatWrite}.
	 */
	public enum Fields {
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		CATEGORIE_PLAT_CODE(String.class),
		RESTAURANT_ID(Integer.class),
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
