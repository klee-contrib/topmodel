////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_associations.entities.restaurant;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.OrderBy;
import jakarta.persistence.Table;

/**
 * Plat du menu.
 */
@Entity
@Table(name = "PLAT")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Plat {

	/**
	 * Identifiant du plat.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "PLA_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Nom du plat.
	 */
	@Column(name = "PLA_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Description du plat.
	 */
	@Column(name = "PLA_DESCRIPTION", length = 100, columnDefinition = "varchar")
	private String description;

	/**
	 * Prix du plat.
	 */
	@Column(name = "PLA_PRIX", nullable = false, scale = 2, columnDefinition = "decimal")
	private BigDecimal prix;

	/**
	 * Indique si le plat est disponible.
	 */
	@Column(name = "PLA_DISPONIBLE", nullable = false, columnDefinition = "boolean")
	private Boolean disponible = true;

	/**
	 * Catégorie du plat.
	 */
	@JoinColumn(name = "CAT_CODE_CATEGORIE_PLAT", referencedColumnName = "CAT_CODE")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = CategoriePlat.class)
	private CategoriePlat categoriePlatCategoriePlat;

	/**
	 * Restaurant proposant ce plat.
	 */
	@JoinColumn(name = "RES_ID_RESTAURANT", referencedColumnName = "RES_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Restaurant.class)
	private Restaurant restaurantRestaurant;

	/**
	 * Association réciproque de LigneCommande.PlatId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "plat")
	private List<LigneCommande> ligneCommandes;

	/**
	 * Association réciproque de MenuPlat.PlatIdPlat.
	 */
	@OrderBy("ordre ASC")
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "platPlat")
	private List<MenuPlat> menuPlatsPlat;

	/**
	 * Association réciproque de PromotionPlat.PlatIdPlat.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "platPlat")
	private List<PromotionPlat> promotionPlatsPlat;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

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
	 * Getter for categoriePlatCategoriePlat.
	 *
	 * @return value of {@link #categoriePlatCategoriePlat categoriePlatCategoriePlat}.
	 */
	public CategoriePlat getCategoriePlatCategoriePlat() {
		return this.categoriePlatCategoriePlat;
	}

	/**
	 * Getter for restaurantRestaurant.
	 *
	 * @return value of {@link #restaurantRestaurant restaurantRestaurant}.
	 */
	public Restaurant getRestaurantRestaurant() {
		return this.restaurantRestaurant;
	}

	/**
	 * Getter for ligneCommandes.
	 *
	 * @return value of {@link #ligneCommandes ligneCommandes}.
	 */
	public List<LigneCommande> getLigneCommandes() {
		if (this.ligneCommandes == null) {
			this.ligneCommandes = new ArrayList<>();
		}
		return this.ligneCommandes;
	}

	/**
	 * Getter for menuPlatsPlat.
	 *
	 * @return value of {@link #menuPlatsPlat menuPlatsPlat}.
	 */
	public List<MenuPlat> getMenuPlatsPlat() {
		if (this.menuPlatsPlat == null) {
			this.menuPlatsPlat = new ArrayList<>();
		}
		return this.menuPlatsPlat;
	}

	/**
	 * Getter for promotionPlatsPlat.
	 *
	 * @return value of {@link #promotionPlatsPlat promotionPlatsPlat}.
	 */
	public List<PromotionPlat> getPromotionPlatsPlat() {
		if (this.promotionPlatsPlat == null) {
			this.promotionPlatsPlat = new ArrayList<>();
		}
		return this.promotionPlatsPlat;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
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
	 * Set the value of {@link #categoriePlatCategoriePlat categoriePlatCategoriePlat}.
	 * @param categoriePlatCategoriePlat value to set.
	 */
	public void setCategoriePlatCategoriePlat(CategoriePlat categoriePlatCategoriePlat) {
		this.categoriePlatCategoriePlat = categoriePlatCategoriePlat;
	}

	/**
	 * Set the value of {@link #restaurantRestaurant restaurantRestaurant}.
	 * @param restaurantRestaurant value to set.
	 */
	public void setRestaurantRestaurant(Restaurant restaurantRestaurant) {
		this.restaurantRestaurant = restaurantRestaurant;
	}

	/**
	 * Set the value of {@link #ligneCommandes ligneCommandes}.
	 * @param ligneCommandes value to set.
	 */
	public void setLigneCommandes(List<LigneCommande> ligneCommandes) {
		this.ligneCommandes = ligneCommandes;
	}

	/**
	 * Set the value of {@link #menuPlatsPlat menuPlatsPlat}.
	 * @param menuPlatsPlat value to set.
	 */
	public void setMenuPlatsPlat(List<MenuPlat> menuPlatsPlat) {
		this.menuPlatsPlat = menuPlatsPlat;
	}

	/**
	 * Set the value of {@link #promotionPlatsPlat promotionPlatsPlat}.
	 * @param promotionPlatsPlat value to set.
	 */
	public void setPromotionPlatsPlat(List<PromotionPlat> promotionPlatsPlat) {
		this.promotionPlatsPlat = promotionPlatsPlat;
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_associations.entities.restaurant.Plat#ligneCommandes ligneCommandes}.
	 * @param ligneCommande value to add to plat.
	 */
	void addLigneCommande(LigneCommande ligneCommande) {
		this.ligneCommandes.add(ligneCommande);
		ligneCommande.setPlat(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_associations.entities.restaurant.Plat#menuPlatsPlat menuPlatsPlat}.
	 * @param menuPlat value to add to platPlat.
	 */
	void addMenuPlatPlat(MenuPlat menuPlat) {
		this.menuPlatsPlat.add(menuPlat);
		menuPlat.setPlatPlat(this);
	}

	/**
	 * Add a value to {@link restaurant.jpa_identity_associations.entities.restaurant.Plat#promotionPlatsPlat promotionPlatsPlat}.
	 * @param promotionPlat value to add to platPlat.
	 */
	void addPromotionPlatPlat(PromotionPlat promotionPlat) {
		this.promotionPlatsPlat.add(promotionPlat);
		promotionPlat.setPlatPlat(this);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_associations.entities.restaurant.Plat#ligneCommandes ligneCommandes}.
	 * @param ligneCommande ligneCommande value to remove.
	 */
	void removeLigneCommande(LigneCommande ligneCommande) {
		this.ligneCommandes.remove(ligneCommande);
		ligneCommande.setPlat(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_associations.entities.restaurant.Plat#menuPlatsPlat menuPlatsPlat}.
	 * @param menuPlat menuPlat value to remove.
	 */
	void removeMenuPlatPlat(MenuPlat menuPlat) {
		this.menuPlatsPlat.remove(menuPlat);
		menuPlat.setPlatPlat(null);
	}

	/**
	 * Remove a value from {@link restaurant.jpa_identity_associations.entities.restaurant.Plat#promotionPlatsPlat promotionPlatsPlat}.
	 * @param promotionPlat promotionPlat value to remove.
	 */
	void removePromotionPlatPlat(PromotionPlat promotionPlat) {
		this.promotionPlatsPlat.remove(promotionPlat);
		promotionPlat.setPlatPlat(null);
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_associations.entities.restaurant.Plat Plat}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		DESCRIPTION(String.class),
		PRIX(BigDecimal.class),
		DISPONIBLE(Boolean.class),
		CATEGORIE_PLAT_CATEGORIE_PLAT(CategoriePlat.class),
		RESTAURANT_RESTAURANT(Restaurant.class),
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
