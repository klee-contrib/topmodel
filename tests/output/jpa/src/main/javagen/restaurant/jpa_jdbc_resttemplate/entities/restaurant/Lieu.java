////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.entities.restaurant;

import org.springframework.data.annotation.Id;
import org.springframework.data.relational.core.mapping.Column;
import org.springframework.data.relational.core.mapping.Table;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Lieu.
 */
@Table(name = "lieu")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class Lieu {

	/**
	 * Identifiant du restaurant.
	 */
	@Id
	@Column("lie_id")
	private Integer id;

	/**
	 * Nom du restaurant.
	 */
	@NotNull
	@Column("lie_nom")
	private String nom;

	/**
	 * Adresse du restaurant.
	 */
	@Column("lie_adresse")
	private String adresse;

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
}
