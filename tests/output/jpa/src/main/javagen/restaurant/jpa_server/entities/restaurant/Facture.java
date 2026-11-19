////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

import restaurant.jpa_server.enums.restaurant.TypeFactureCode;

/**
 * Facture.
 */
@Entity
@Table(name = "facture")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Facture {

	/**
	 * Identifiant de la facture.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "id", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Commande associée à la facture.
	 */
	@JoinColumn(name = "com_id", referencedColumnName = "com_id")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Commande.class)
	private Commande commande;

	/**
	 * type de facture.
	 */
	@Enumerated(EnumType.STRING)
	@Column(name = "tfa_code", length = 10, columnDefinition = "varchar")
	private TypeFactureCode typeFactureCode;

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
	public Commande getCommande() {
		return this.commande;
	}

	/**
	 * Getter for typeFactureCode.
	 *
	 * @return value of {@link #typeFactureCode typeFactureCode}.
	 */
	public TypeFactureCode getTypeFactureCode() {
		return this.typeFactureCode;
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
	public void setCommande(Commande commande) {
		this.commande = commande;
	}

	/**
	 * Set the value of {@link #typeFactureCode typeFactureCode}.
	 * @param typeFactureCode value to set.
	 */
	public void setTypeFactureCode(TypeFactureCode typeFactureCode) {
		this.typeFactureCode = typeFactureCode;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Facture Facture}.
	 */
	public enum Fields {
		ID(Integer.class),
		COMMANDE(Commande.class),
		TYPE_FACTURE_CODE(TypeFactureCode.class);

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
