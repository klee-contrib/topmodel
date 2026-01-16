////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_uuid_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Client;
import restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.RestaurantMappers;

/**
 * Détail d'un client en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientItem implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la personne.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Personne#getId() Personne#getId()}
	 */
	@NotNull
	@Column("per_id")
	private Integer id;

	/**
	 * Nom de la personne.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Personne#getNom() Personne#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	@Column("per_nom")
	private String nom;

	/**
	 * Prénom de la personne.
	 * Alias of {@link restaurant.jpa_jdbc_uuid_resttemplate.entities.restaurant.Personne#getPrenom() Personne#getPrenom()}
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
	 * No arg constructor.
	 */
	public ClientItem() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'ClientItem'.
	 * @param client Instance de 'Client'.
	 *
	 * @return Une nouvelle instance de 'ClientItem'.
	 */
	public ClientItem(Client client) {
		RestaurantMappers.mapClientItem(client, this);
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
}
