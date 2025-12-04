////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_enums.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_identity_enums.entities.restaurant.Client;
import restaurant.jpa_identity_enums.entities.restaurant.RestaurantMappers;

/**
 * Version minimale d'un client.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientMinimal implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getId() Client#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Nom du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getNom() Client#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Prénom du client.
	 * Alias of {@link restaurant.jpa_identity_enums.entities.restaurant.Client#getPrenom() Client#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Nom complet du client (calculé).
	 */
	@Size(max = 100)
	private String nomComplet;

	/**
	 * No arg constructor.
	 */
	public ClientMinimal() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'ClientMinimal'.
	 * @param client Instance de 'Client'.
	 *
	 * @return Une nouvelle instance de 'ClientMinimal'.
	 */
	public ClientMinimal(Client client) {
		RestaurantMappers.mapClientMinimal(client, this);
	}

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
	 * Getter for prenom.
	 *
	 * @return value of {@link #prenom prenom}.
	 */
	public String getPrenom() {
		return this.prenom;
	}

	/**
	 * Getter for nomComplet.
	 *
	 * @return value of {@link #nomComplet nomComplet}.
	 */
	public String getNomComplet() {
		return this.nomComplet;
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
	 * Set the value of {@link #prenom prenom}.
	 * @param prenom value to set.
	 */
	public void setPrenom(String prenom) {
		this.prenom = prenom;
	}

	/**
	 * Set the value of {@link #nomComplet nomComplet}.
	 * @param nomComplet value to set.
	 */
	public void setNomComplet(String nomComplet) {
		this.nomComplet = nomComplet;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_identity_enums.dtos.restaurant.ClientMinimal ClientMinimal}.
	 */
	public enum Fields {
		ID(Integer.class),
		NOM(String.class),
		PRENOM(String.class),
		NOM_COMPLET(String.class);

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
