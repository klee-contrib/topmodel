////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite;

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

/**
 * Type d'utilisateur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_PROFIL")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class TypeProfil {

	/**
	 * Code du type d'utilisateur.
	 */
	@Id
	@Column(name = "TPR_CODE", nullable = false, length = 3)
	@Enumerated(EnumType.STRING)
	private TypeProfil.Values code;

	/**
	 * Libellé du type d'utilisateur.
	 */
	@Column(name = "TPR_LIBELLE", nullable = false, length = 3)
	private String libelle;

	/**
	 * No arg constructor.
	 */
	public TypeProfil() {
	}

	/**
	 * Copy constructor.
	 * @param typeProfil to copy
	 */
	public TypeProfil(TypeProfil typeProfil) {
		if(typeProfil == null) {
			return;
		}

		this.code = typeProfil.getCode();
		this.libelle = typeProfil.getLibelle();
	}

	/**
	 * All arg constructor.
	 * @param code Code du type d'utilisateur
	 * @param libelle Libellé du type d'utilisateur
	 */
	public TypeProfil(TypeProfil.Values code, String libelle) {
		this.code = code;
		this.libelle = libelle;
	}

	/**
	 * Getter for code.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#code code}.
	 */
	public TypeProfil.Values getCode() {
		return this.code;
	}

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#code code}.
	 * @param code value to set
	 */
	public void setCode(TypeProfil.Values code) {
		this.code = code;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil#libelle libelle}.
	 * @param libelle value to set
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil TypeProfil}.
	 */
	public enum Fields  {
        CODE(TypeProfil.Values.class), //
        LIBELLE(String.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}

	public enum Values {
		ADM("securite.typeProfil.values.ADM"), //
		GES("securite.typeProfil.values.GES"); 

		/**
		 * Libellé du type d'utilisateur.
		 */
		private final String libelle;

		/**
		 * All arg constructor.
		 */
		private Values(String libelle) {
			this.libelle = libelle;
		}

		/**
		 * Méthode permettant de récupérer l'entité correspondant au code.
		 *
		 * @return instance de {@link topmodel.jpa.sample.demo.entities.securite.TypeProfil} correspondant au code courant.
		 */
		public TypeProfil getEntity() {
			return new TypeProfil(this, libelle);
		}

		/**
		 * Libellé du type d'utilisateur.
		 */
		public String getLibelle(){
			return this.libelle;
		}
	}
}
