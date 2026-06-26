////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.dtos.restaurant;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;

import org.springframework.data.relational.core.mapping.Column;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;

/**
 * Résultat de suppression d'une commande.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class CommandeDeleteResult implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Identifiant de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getId() Commande#getId()}
	 */
	@NotNull
	@Column("com_id")
	private Integer id;

	/**
	 * Date et heure de la commande.
	 * Alias of {@link restaurant.jpa_jdbc_resttemplate.entities.restaurant.Commande#getDateCommande() Commande#getDateCommande()}
	 */
	@NotNull
	@Column("com_date_commande")
	private LocalDateTime dateCommande;

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for dateCommande.
	 *
	 * @return value of {@link #dateCommande dateCommande}.
	 */
	public LocalDateTime getDateCommande() {
		return this.dateCommande;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #dateCommande dateCommande}.
	 * @param dateCommande value to set.
	 */
	public void setDateCommande(LocalDateTime dateCommande) {
		this.dateCommande = dateCommande;
	}
}
