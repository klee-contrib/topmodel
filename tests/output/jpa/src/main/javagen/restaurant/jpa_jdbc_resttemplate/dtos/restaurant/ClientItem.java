////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

/**
 * Détail d'un client en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ClientItem extends PersonneItem implements Serializable {

	/**
	 * ID de sérialisation.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom complet du client (calculé).
	 */
	@Size(max = 100)
	@Column("nom_complet")
	private String nomComplet;

	/**
	 * Accesseur de 'nomComplet'.
	 *
	 * @return Valeur de {@link #nomComplet nomComplet}.
	 */
	public String getNomComplet() {
		return this.nomComplet;
	}

	/**
	 * Définit la valeur de {@link #nomComplet nomComplet}.
	 * @param nomComplet Valeur à définir.
	 */
	public void setNomComplet(String nomComplet) {
		this.nomComplet = nomComplet;
	}
}
