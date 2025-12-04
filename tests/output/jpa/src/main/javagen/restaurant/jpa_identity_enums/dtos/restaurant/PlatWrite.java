////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.math.BigDecimal;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_enums.entities.restaurant.Plat;
import restaurant.jpa_identity_enums.entities.restaurant.RestaurantMappers;
import restaurant.jpa_identity_enums.enums.restaurant.CategoriePlat;

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
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getNom() Plat#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Description du plat.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getDescription() Plat#getDescription()}
	 */
	@Size(max = 100)
	private String description;

	/**
	 * Prix du plat.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getPrix() Plat#getPrix()}
	 */
	@NotNull
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getDisponible() Plat#getDisponible()}
	 */
	@NotNull
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getCategoriePlatCategoriePlat() Plat#getCategoriePlatCategoriePlat()}
	 */
	@NotNull
	private CategoriePlat categoriePlatCodeCategoriePlat;

	/**
	 * Restaurant proposant ce plat.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getRestaurantRestaurant() Plat#getRestaurantRestaurant()}
	 */
	@NotNull
	private Integer restaurantIdRestaurant;

	/**
	 * Association réciproque de LigneCommande.PlatId.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getLigneCommandes() Plat#getLigneCommandes()}
	 */
	@NotNull
	private List<Integer> ligneCommandes;

	/**
	 * Association réciproque de MenuPlat.PlatIdPlat.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getMenuPlatsPlat() Plat#getMenuPlatsPlat()}
	 */
	@NotNull
	private List<Integer> menuPlatsPlat;

	/**
	 * Association réciproque de PromotionPlat.PlatIdPlat.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Plat#getPromotionPlatsPlat() Plat#getPromotionPlatsPlat()}
	 */
	@NotNull
	private List<Integer> promotionPlatsPlat;

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
	 * Getter for categoriePlatCodeCategoriePlat.
	 *
	 * @return value of {@link #categoriePlatCodeCategoriePlat categoriePlatCodeCategoriePlat}.
	 */
	public CategoriePlat getCategoriePlatCodeCategoriePlat() {
		return this.categoriePlatCodeCategoriePlat;
	}

	/**
	 * Getter for restaurantIdRestaurant.
	 *
	 * @return value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 */
	public Integer getRestaurantIdRestaurant() {
		return this.restaurantIdRestaurant;
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
	 * Getter for menuPlatsPlat.
	 *
	 * @return value of {@link #menuPlatsPlat menuPlatsPlat}.
	 */
	public List<Integer> getMenuPlatsPlat() {
		return this.menuPlatsPlat;
	}

	/**
	 * Getter for promotionPlatsPlat.
	 *
	 * @return value of {@link #promotionPlatsPlat promotionPlatsPlat}.
	 */
	public List<Integer> getPromotionPlatsPlat() {
		return this.promotionPlatsPlat;
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
	 * Set the value of {@link #categoriePlatCodeCategoriePlat categoriePlatCodeCategoriePlat}.
	 * @param categoriePlatCodeCategoriePlat value to set.
	 */
	public void setCategoriePlatCodeCategoriePlat(CategoriePlat categoriePlatCodeCategoriePlat) {
		this.categoriePlatCodeCategoriePlat = categoriePlatCodeCategoriePlat;
	}

	/**
	 * Set the value of {@link #restaurantIdRestaurant restaurantIdRestaurant}.
	 * @param restaurantIdRestaurant value to set.
	 */
	public void setRestaurantIdRestaurant(Integer restaurantIdRestaurant) {
		this.restaurantIdRestaurant = restaurantIdRestaurant;
	}

	/**
	 * Set the value of {@link #ligneCommandes ligneCommandes}.
	 * @param ligneCommandes value to set.
	 */
	public void setLigneCommandes(List<Integer> ligneCommandes) {
		this.ligneCommandes = ligneCommandes;
	}

	/**
	 * Set the value of {@link #menuPlatsPlat menuPlatsPlat}.
	 * @param menuPlatsPlat value to set.
	 */
	public void setMenuPlatsPlat(List<Integer> menuPlatsPlat) {
		this.menuPlatsPlat = menuPlatsPlat;
	}

	/**
	 * Set the value of {@link #promotionPlatsPlat promotionPlatsPlat}.
	 * @param promotionPlatsPlat value to set.
	 */
	public void setPromotionPlatsPlat(List<Integer> promotionPlatsPlat) {
		this.promotionPlatsPlat = promotionPlatsPlat;
	}

	/**
	 * Mappe 'PlatWrite' vers 'Plat'.
	 * @param target Instance pré-existante de 'Plat'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Plat'.
	 */
	public Plat toPlat(Plat target) {
		return RestaurantMappers.toPlat(this, target);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.PlatWrite PlatWrite}.
	 */
	public enum Fields {
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		CATEGORIE_PLAT_CODE_CATEGORIE_PLAT(CategoriePlat.class),
		RESTAURANT_ID_RESTAURANT(Integer.class),
		LIGNE_COMMANDES(List.class),
		MENU_PLATS_PLAT(List.class),
		PROMOTION_PLATS_PLAT(List.class);

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
