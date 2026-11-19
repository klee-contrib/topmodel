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
 * Prestaire du restaurant.
 */
@Table(name = "prestataire")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Prestataire implements EmployeBase {

	/**
	 * Identifiant de la personne.
	 */
	@Id
	@Column("pst_id")
	private Integer id;

	/**
	 * Nom de la personne.
	 */
	@NotNull
	@Column("pst_nom")
	private String nom;

	/**
	 * Prénom de la personne.
	 */
	@NotNull
	@Column("pst_prenom")
	private String prenom;

	/**
	 * Numéro de téléphone de l'employé.
	 */
	@Column("pst_telephone")
	private String telephone;

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
	 * Getter for telephone.
	 *
	 * @return value of {@link #telephone telephone}.
	 */
	public String getTelephone() {
		return this.telephone;
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
	 * Set the value of {@link #telephone telephone}.
	 * @param telephone value to set.
	 */
	public void setTelephone(String telephone) {
		this.telephone = telephone;
	}
}
