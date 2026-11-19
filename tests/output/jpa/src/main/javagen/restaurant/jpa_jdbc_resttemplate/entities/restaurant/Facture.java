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
	 * type de facture.
	 */
	@Column("tfa_code")
	private String typeFactureCode;

	/**
	 * Accesseur de 'id'.
	 *
	 * @return Valeur de {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Accesseur de 'commande'.
	 *
	 * @return Valeur de {@link #commande commande}.
	 */
	public Integer getCommande() {
		return this.commande;
	}

	/**
	 * Accesseur de 'typeFactureCode'.
	 *
	 * @return Valeur de {@link #typeFactureCode typeFactureCode}.
	 */
	public String getTypeFactureCode() {
		return this.typeFactureCode;
	}

	/**
	 * Définit la valeur de {@link #id id}.
	 * @param id Valeur à définir.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Définit la valeur de {@link #commande commande}.
	 * @param commande Valeur à définir.
	 */
	public void setCommande(Integer commande) {
		this.commande = commande;
	}

	/**
	 * Définit la valeur de {@link #typeFactureCode typeFactureCode}.
	 * @param typeFactureCode Valeur à définir.
	 */
	public void setTypeFactureCode(String typeFactureCode) {
		this.typeFactureCode = typeFactureCode;
	}
}
