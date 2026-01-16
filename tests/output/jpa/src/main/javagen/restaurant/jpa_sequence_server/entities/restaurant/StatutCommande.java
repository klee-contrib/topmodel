////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Enumerated;
import jakarta.persistence.EnumType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;

import restaurant.jpa_sequence_server.enums.restaurant.StatutCommandeCode;

/**
 * Statut d'une commande.
 */
@Entity
@Immutable
@Table(name = "STATUT_COMMANDE")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class StatutCommande {

	@Transient
	public static final StatutCommande ANNULE = new StatutCommande(StatutCommandeCode.ANNULE);

	@Transient
	public static final StatutCommande EN_ATT = new StatutCommande(StatutCommandeCode.EN_ATT);

	@Transient
	public static final StatutCommande EN_PREP = new StatutCommande(StatutCommandeCode.EN_PREP);

	@Transient
	public static final StatutCommande PRETE = new StatutCommande(StatutCommandeCode.PRETE);

	@Transient
	public static final StatutCommande SERVIE = new StatutCommande(StatutCommandeCode.SERVIE);

	/**
	 * Code du statut.
	 */
	@Id
	@Enumerated(EnumType.STRING)
	@Column(name = "STC_CODE", nullable = false, length = 10, columnDefinition = "varchar")
	private StatutCommandeCode code;

	/**
	 * Libellé du statut.
	 */
	@Column(name = "STC_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public StatutCommande() {
		// No arg constructor
	}

	/**
	 * Enum constructor.
	 * @param code Code dont on veut obtenir l'instance.
	 */
	public StatutCommande(StatutCommandeCode code) {
		this.code = code;
		switch(code) {
			case ANNULE:
				this.libelle = "restaurant.statutCommande.values.Annulee";
				break;
			case EN_ATT:
				this.libelle = "restaurant.statutCommande.values.EnAttente";
				break;
			case EN_PREP:
				this.libelle = "restaurant.statutCommande.values.EnPreparation";
				break;
			case PRETE:
				this.libelle = "restaurant.statutCommande.values.Prete";
				break;
			case SERVIE:
				this.libelle = "restaurant.statutCommande.values.Servie";
				break;
		}
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link #code code}.
	 */
	public StatutCommandeCode getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_sequence_server.entities.restaurant.StatutCommande StatutCommande}.
	 */
	public enum Fields {
		CODE(StatutCommandeCode.class),
		LIBELLE(String.class);

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
