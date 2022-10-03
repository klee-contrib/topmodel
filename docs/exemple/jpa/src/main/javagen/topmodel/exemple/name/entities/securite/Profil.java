////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.entities.securite;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import javax.annotation.Generated;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.JoinTable;
import javax.persistence.ManyToMany;
import javax.persistence.ManyToOne;
import javax.persistence.OneToMany;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;

import topmodel.exemple.name.entities.securite.Droits;
import topmodel.exemple.name.entities.securite.TypeProfil;
import topmodel.exemple.utils.IFieldEnum;

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
	private long id;

	/**
	 * Type de profil.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
	@JoinColumn(name = "TPR_CODE", referencedColumnName = "TPR_CODE")
	private TypeProfil typeProfil;

	/**
	 * Liste des droits de l'utilisateur.
	 */
	@ManyToMany(fetch = FetchType.LAZY, cascade = { CascadeType.PERSIST, CascadeType.MERGE })
	@JoinTable(name = "PROFIL_DROITS", joinColumns = @JoinColumn(name = "PRO_ID"), inverseJoinColumns = @JoinColumn(name = "DRO_CODE"))
	private List<Droits> droits;

	/**
	 * Liste des secteurs de l'utilisateur.
	 */
	@OneToMany(cascade = CascadeType.ALL, orphanRemoval = true, fetch = FetchType.LAZY, mappedBy = "profil")
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
	public Profil(long id, TypeProfil typeProfil, List<Droits> droits, List<Secteur> secteurs) {
		this.id = id;
		this.typeProfil = typeProfil;
		this.droits = droits;
		this.secteurs = secteurs;
	}

	/**
	 * Crée une nouvelle instance de 'Profil'.
	 * @param profil Instance de 'Profil'.
	 *
	 * @return Une nouvelle instance de 'Profil'.
	 */
	public Profil(Profil profil) {
		this.from(profil);
	}

	/**
	 * Map les champs des classes passées en paramètre dans l'instance courante.
	 * @param profil Instance de 'Profil'.
	 */
	protected void from(Profil profil) {
		if(profil != null) {
			this.id = profil.getId();
			this.typeProfil = profil.getTypeProfil();
			this.droits = profil.getDroits();
			this.secteurs = profil.getSecteurs();
		}

	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Profil#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for typeProfil.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Profil#typeProfil typeProfil}.
	 */
	public TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Getter for droits.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Profil#droits droits}.
	 */
	public List<Droits> getDroits() {
		if(this.droits == null)
			this.droits = new ArrayList<>();
		return this.droits;
	}

	/**
	 * Getter for secteurs.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Profil#secteurs secteurs}.
	 */
	public List<Secteur> getSecteurs() {
		if(this.secteurs == null)
			this.secteurs = new ArrayList<>();
		return this.secteurs;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Profil#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Profil#typeProfil typeProfil}.
	 * @param typeProfil value to set
	 */
	public void setTypeProfil(TypeProfil typeProfil) {
		this.typeProfil = typeProfil;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Profil#droits droits}.
	 * @param droits value to set
	 */
	public void setDroits(List<Droits> droits) {
		this.droits = droits;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Profil#secteurs secteurs}.
	 * @param secteurs value to set
	 */
	public void setSecteurs(List<Secteur> secteurs) {
		this.secteurs = secteurs;
	}

	/**
	 * Equal function comparing Id.
	 */
	public boolean equals(Object o) {
		if(o instanceof Profil) {
			Profil profil = (Profil) o;
			if(this == profil)
				return true;

			if(profil == null || this.getId() == null)
				return false;

			return this.getId().equals(profil.getId());
		}

		return false;
	}

	/**
	 * Mappe 'Profil' vers 'Profil'.
	 * @param source Instance de 'Profil'.
	 * @param dest Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Profil'.
	 */
	public Profil toProfil(Profil dest) {
		dest = dest == null ? new Profil() : dest;

		dest.setId(this.getId());
		dest.setTypeProfil(this.getTypeProfil());
		dest.setDroits(this.getDroits());
		dest.setSecteurs(this.getSecteurs());

		return dest;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.entities.securite.Profil Profil}.
	 */
	public enum Fields implements IFieldEnum<Profil> {
        ID(long.class), //
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
