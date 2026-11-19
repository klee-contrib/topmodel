////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

/**
 * Détail d'un restaurant en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class RestaurantRead implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Lieu#getId() Lieu#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Lieu#getNom() Lieu#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Adresse du restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Lieu#getAdresse() Lieu#getAdresse()}
	 */
	@Size(max = 100)
	private String adresse;

	/**
	 * Numéro de téléphone.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getTelephone() Restaurant#getTelephone()}
	 */
	@Size(max = 20)
	private String telephone;

	/**
	 * Association réciproque de Menu.Restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getMenus() Restaurant#getMenus()}
	 */
	@NotNull
	private List<Integer> menus;

	/**
	 * Association réciproque de Plat.Restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getPlats() Restaurant#getPlats()}
	 */
	@NotNull
	private List<Integer> plats;

	/**
	 * Association réciproque de Promotion.Restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getPromotions() Restaurant#getPromotions()}
	 */
	private List<Integer> promotions;

	/**
	 * Association réciproque de AvisClient.Restaurant.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getAvisClients() Restaurant#getAvisClients()}
	 */
	@NotNull
	private List<Integer> avisClients;

	/**
	 * Date de création de l'enregistrement.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getDateCreation() Restaurant#getDateCreation()}
	 */
	@NotNull
	@PastOrPresent
	private LocalDateTime dateCreation;

	/**
	 * Association réciproque de Table.RestaurantId.
	 * Alias de {@link restaurant.jpa_feign.entities.restaurant.Restaurant#getTableIds() Restaurant#getTableIds()}
	 */
	@Valid
	@NotNull
	private List<TableItem> tables;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'nom'.
	 *
	 * @return Valeur de {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Accesseur de 'adresse'.
	 *
	 * @return Valeur de {@link #adresse adresse}.
	 */
	public String getAdresse() {
		return this.adresse;
	}

	/**
	 * Accesseur de 'telephone'.
	 *
	 * @return Valeur de {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
	}

	/**
	 * Accesseur de 'menus'.
	 *
	 * @return Valeur de {@link #menus menus}.
	 */
	public List<Integer> getMenus() {
		return this.menus;
	}

	/**
	 * Accesseur de 'plats'.
	 *
	 * @return Valeur de {@link #plats plats}.
	 */
	public List<Integer> getPlats() {
		return this.plats;
	}

	/**
	 * Accesseur de 'promotions'.
	 *
	 * @return Valeur de {@link #promotions promotions}.
	 */
	public List<Integer> getPromotions() {
		return this.promotions;
	}

	/**
	 * Accesseur de 'avisClients'.
	 *
	 * @return Valeur de {@link #avisClients avisClients}.
	 */
	public List<Integer> getAvisClients() {
		return this.avisClients;
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
	 * Accesseur de 'tables'.
	 *
	 * @return Valeur de {@link #tables tables}.
	 */
	public List<TableItem> getTables() {
		return this.tables;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #nom nom}.
	 * @param nom Valeur à définir.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Définit la valeur de {@link #adresse adresse}.
	 * @param adresse Valeur à définir.
	 */
	public void setAdresse(String adresse) {
		this.adresse = adresse;
	}

	/**
	 * Définit la valeur de {@link #telephone telephone}.
	 * @param telephone Valeur à définir.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}

	/**
	 * Définit la valeur de {@link #menus menus}.
	 * @param menus Valeur à définir.
	 */
	public void setMenus(List<Integer> menus) {
		this.menus = menus;
	}

	/**
	 * Définit la valeur de {@link #plats plats}.
	 * @param plats Valeur à définir.
	 */
	public void setPlats(List<Integer> plats) {
		this.plats = plats;
	}

	/**
	 * Définit la valeur de {@link #promotions promotions}.
	 * @param promotions Valeur à définir.
	 */
	public void setPromotions(List<Integer> promotions) {
		this.promotions = promotions;
	}

	/**
	 * Définit la valeur de {@link #avisClients avisClients}.
	 * @param avisClients Valeur à définir.
	 */
	public void setAvisClients(List<Integer> avisClients) {
		this.avisClients = avisClients;
	}

	/**
	 * Définit la valeur de {@link #dateCreation dateCreation}.
	 * @param dateCreation Valeur à définir.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Définit la valeur de {@link #tables tables}.
	 * @param tables Valeur à définir.
	 */
	public void setTables(List<TableItem> tables) {
		this.tables = tables;
	}

	/**
	 * Énumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.RestaurantRead RestaurantRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		ADRESSE(String.class),
		TELEPHONE(String.class),
		MENUS(List.class),
		PLATS(List.class),
		PROMOTIONS(List.class),
		AVIS_CLIENTS(List.class),
		DATE_CREATION(LocalDateTime.class),
		TABLES(List.class);

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
