////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.JoinTable;
import jakarta.persistence.ManyToMany;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.OrderBy;
import jakarta.persistence.Table;

/**
 * Profil des utilisateurs.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "PROFIL")
public class Profil {

	/**
	 * Id technique.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "PRO_ID", nullable = false)
	private Long id;

	/**
	 * Type de profil.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
	@JoinColumn(name = "TPR_CODE", referencedColumnName = "TPR_CODE")
	private TypeProfil typeProfil;

	/**
	 * Liste des droits de l'utilisateur.
	 */
	@ManyToMany(fetch = FetchType.LAZY)
	@JoinTable(name = "PROFIL_DROIT", joinColumns = @JoinColumn(name = "PRO_ID"), inverseJoinColumns = @JoinColumn(name = "DRO_CODE"))
	@OrderBy("code ASC")
	private List<Droit> droits;

	/**
	 * Liste des secteurs de l'utilisateur.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "profil")
	private List<Secteur> secteurs;

	/**
	 * No arg constructor.
	 */
	public Profil() {
	}

	/**
	 * Copy constructor.
	 * @param profil to copy
	 */
	public Profil(Profil profil) {
		if(profil == null) {
			return;
		}

		this.id = profil.getId();
		this.typeProfil = profil.getTypeProfil();

		this.droits = profil.getDroits().stream().collect(Collectors.toList());
		this.secteurs = profil.getSecteurs().stream().collect(Collectors.toList());
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param typeProfil Type de profil
	 * @param droits Liste des droits de l'utilisateur
	 * @param secteurs Liste des secteurs de l'utilisateur
	 */
	public Profil(Long id, TypeProfil typeProfil, List<Droit> droits, List<Secteur> secteurs) {
		this.id = id;
		this.typeProfil = typeProfil;
		this.droits = droits;
		this.secteurs = secteurs;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#id id}.
	 */
	public Long getId() {
		return this.id;
	}

	/**
	 * Getter for typeProfil.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#typeProfil typeProfil}.
	 */
	public TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Getter for droits.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#droits droits}.
	 */
	public List<Droit> getDroits() {
		if(this.droits == null)
			this.droits = new ArrayList<>();
		return this.droits;
	}

	/**
	 * Getter for secteurs.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#secteurs secteurs}.
	 */
	public List<Secteur> getSecteurs() {
		if(this.secteurs == null)
			this.secteurs = new ArrayList<>();
		return this.secteurs;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#id id}.
	 * @param id value to set
	 */
	public void setId(Long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#typeProfil typeProfil}.
	 * @param typeProfil value to set
	 */
	public void setTypeProfil(TypeProfil typeProfil) {
		this.typeProfil = typeProfil;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#droits droits}.
	 * @param droits value to set
	 */
	public void setDroits(List<Droit> droits) {
		this.droits = droits;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#secteurs secteurs}.
	 * @param secteurs value to set
	 */
	public void setSecteurs(List<Secteur> secteurs) {
		this.secteurs = secteurs;
	}

	/**
	 * Mappe 'Profil' vers 'Profil'.
	 * @param target Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Profil'.
	 */
	public Profil toProfil(Profil target) {
		target = target == null ? new Profil() : target;
		SecuriteMappers.toProfil(this, target);
		return target;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.Profil Profil}.
	 */
	public enum Fields  {
        ID(Long.class), //
        TYPE_PROFIL(TypeProfil.class), //
        DROITS(List.class), //
        SECTEURS(List.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}
