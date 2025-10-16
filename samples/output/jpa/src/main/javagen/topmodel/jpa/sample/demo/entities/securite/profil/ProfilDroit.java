////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.profil;

import java.util.Objects;

import jakarta.annotation.Generated;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

/**
 * Association N-N Profils <> Droits.
 */
@Entity
@Table(name = "PROFIL_DROIT")
@IdClass(ProfilDroit.ProfilDroitId.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ProfilDroit {

	/**
	 * Profil.
	 */
	@Id
	private Profil profil;

	/**
	 * Droit.
	 */
	@Id
	private Droit droit;

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link #profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Getter for droit.
	 *
	 * @return value of {@link #droit droit}.
	 */
	public Droit getDroit() {
		return this.droit;
	}

	/**
	 * Set the value of {@link #profil profil}.
	 * @param profil value to set.
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Set the value of {@link #droit droit}.
	 * @param droit value to set.
	 */
	public void setDroit(Droit droit) {
		this.droit = droit;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.profil.ProfilDroit ProfilDroit}.
	 */
	public enum Fields {
		PROFIL(Profil.class),
		DROIT(Droit.class);

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

	public static class ProfilDroitId {

		@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Profil.class)
		private Profil profil;

		@JoinColumn(name = "DRO_CODE", referencedColumnName = "DRO_CODE")
		@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Droit.class)
		private Droit droit;

		/**
		 * Getter for profil.
		 *
		 * @return value of {@link #profil profil}.
		 */
		public Profil getProfil() {
			return this.profil;
		}

		/**
		 * Set the value of {@link #profil profil}.
		 * @param profil value to set.
		 */
		public void setProfil(Profil profil) {
			this.profil = profil;
		}

		/**
		 * Getter for droit.
		 *
		 * @return value of {@link #droit droit}.
		 */
		public Droit getDroit() {
			return this.droit;
		}

		/**
		 * Set the value of {@link #droit droit}.
		 * @param droit value to set.
		 */
		public void setDroit(Droit droit) {
			this.droit = droit;
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

			ProfilDroitId oId = (ProfilDroitId) o;

			if (this.profil == null || oId.profil == null || this.droit == null || oId.droit == null) {
				return false;
			}

			return Objects.equals(this.profil.getId(), oId.profil.getId())
			 && Objects.equals(this.droit.getCode(), oId.droit.getCode());
		}

		@Override
		public int hashCode() {
			return Objects.hash(profil == null ? null : profil.getId(), droit == null ? null : droit.getCode());
		}
	}
}
