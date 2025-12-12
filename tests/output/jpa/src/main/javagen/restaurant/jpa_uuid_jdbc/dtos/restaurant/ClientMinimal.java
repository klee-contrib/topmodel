////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_uuid_jdbc.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

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
	 * Identifiant de la personne.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	@Column("per_id")
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_nom")
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_uuid_jdbc.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_prenom")
	private String prenom;

	/**
	 * Nom complet du client (calculé).
	 */
	@Size(max = 100)
	@Column("nom_complet")
	private String nomComplet;

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
	 * Enumération des champs de la classe {@link restaurant.jpa_uuid_jdbc.dtos.restaurant.ClientMinimal ClientMinimal}.
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
