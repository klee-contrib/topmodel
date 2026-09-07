////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.entities.restaurant;

import java.util.Objects;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

/**
 * Paiement.
 */
@Entity
@Table(name = "paiement")
@IdClass(Paiement.PaiementId.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Paiement {

	/**
	 * Facture associée au paiement.
	 */
	@Id
	private Facture facture;

	/**
	 * Carte Swile utilisée pour le paiement.
	 */
	@Id
	private Integer swileCardId;

	/**
	 * Getter for facture.
	 *
	 * @return value of {@link #facture facture}.
	 */
	public Facture getFacture() {
		return this.facture;
	}

	/**
	 * Getter for swileCardId.
	 *
	 * @return value of {@link #swileCardId swileCardId}.
	 */
	public Integer getSwileCardId() {
		return this.swileCardId;
	}

	/**
	 * Set the value of {@link #facture facture}.
	 * @param facture value to set.
	 */
	public void setFacture(Facture facture) {
		this.facture = facture;
	}

	/**
	 * Set the value of {@link #swileCardId swileCardId}.
	 * @param swileCardId value to set.
	 */
	public void setSwileCardId(Integer swileCardId) {
		this.swileCardId = swileCardId;
	}

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_server.entities.restaurant.Paiement Paiement}.
	 */
	public enum Fields {
		FACTURE(Facture.class),
		SWILE_CARD_ID(Integer.class);

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

	public static class PaiementId {

		@JoinColumn(name = "id", referencedColumnName = "id")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Facture.class)
		private Facture facture;

		@Column(name = "swi_id", nullable = false, columnDefinition = "int")
		private Integer swileCardId;

		/**
		 * Getter for facture.
		 *
		 * @return value of {@link #facture facture}.
		 */
		public Facture getFacture() {
			return this.facture;
		}

		/**
		 * Set the value of {@link #facture facture}.
		 * @param facture value to set.
		 */
		public void setFacture(Facture facture) {
			this.facture = facture;
		}

		/**
		 * Getter for swileCardId.
		 *
		 * @return value of {@link #swileCardId swileCardId}.
		 */
		public Integer getSwileCardId() {
			return this.swileCardId;
		}

		/**
		 * Set the value of {@link #swileCardId swileCardId}.
		 * @param swileCardId value to set.
		 */
		public void setSwileCardId(Integer swileCardId) {
			this.swileCardId = swileCardId;
		}

		public boolean equals(Object o) {
			if (o == this) {
				return true;
			}

			if (o == null) {
				return false;
			}

			if (this.getClass() != o.getClass()) {
				return false;
			}

			PaiementId oId = (PaiementId) o;

			if (this.facture == null || oId.facture == null) {
				return false;
			}

			return Objects.equals(this.facture, oId.facture)
			 && Objects.equals(this.swileCardId, oId.swileCardId);
		}

		@Override
		public int hashCode() {
			return Objects.hash(facture == null ? null : facture, swileCardId == null ? null : swileCardId);
		}
	}
}
