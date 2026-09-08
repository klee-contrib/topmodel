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
 * Facture.
 */
@Table(name = "facture")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Facture {

	/**
	 * Identifiant de la facture.
	 */
	@Id
	@Column("id")
	private Integer id;

	/**
	 * Commande associée à la facture.
	 */
	@NotNull
	@Column("com_id")
	private Integer commande;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for commande.
	 *
	 * @return value of {@link #commande commande}.
	 */
	public Integer getCommande() {
		return this.commande;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #commande commande}.
	 * @param commande value to set.
	 */
	public void setCommande(Integer commande) {
		this.commande = commande;
	}
}
