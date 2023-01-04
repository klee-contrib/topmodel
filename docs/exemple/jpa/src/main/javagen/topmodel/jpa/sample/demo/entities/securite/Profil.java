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
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;

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
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "profil")
	private List<TypeProfil> typeProfils;

	/**
	 * Liste des droits de l'utilisateur.
	 */
	@ManyToMany(fetch = FetchType.LAZY)
	@JoinTable(name = "PROFIL_DROITS", joinColumns = @JoinColumn(name = "PRO_ID"), inverseJoinColumns = @JoinColumn(name = "DRO_CODE"))
	private List<Droits> droits;

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

		this.secteurs = profil.getSecteurs().stream().collect(Collectors.toList());

		this.setTypeProfils(profil.getTypeProfils());
		this.setDroits(profil.getDroits());
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param typeProfils Type de profil
	 * @param droits Liste des droits de l'utilisateur
	 * @param secteurs Liste des secteurs de l'utilisateur
	 */
	public Profil(Long id, List<TypeProfil> typeProfils, List<Droits> droits, List<Secteur> secteurs) {
		this.id = id;
		this.typeProfils = typeProfils;
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
	 * Getter for typeProfils.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#typeProfils typeProfils}.
	 */
	public List<TypeProfil> getTypeProfils() {
		if(this.typeProfils == null)
			this.typeProfils = new ArrayList<>();
		return this.typeProfils;
	}

	/**
	 * Getter for droits.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#droits droits}.
	 */
	public List<Droits> getDroits() {
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
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#secteurs secteurs}.
	 * @param secteurs value to set
	 */
	public void setSecteurs(List<Secteur> secteurs) {
		this.secteurs = secteurs;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#typeProfils typeProfils}.
	 * Cette méthode permet définir la valeur de la FK directement
	 * @param typeProfils value to set
	 */
	public void setTypeProfilsCode(List<TypeProfil.Values> typeProfils) {
		if (typeProfils != null) {
			if (this.typeProfils != null) {
				this.typeProfils.clear();
			} else {
				this.typeProfils = new ArrayList<>();
			}
			this.typeProfils.addAll(typeProfils.stream().map(p -> new TypeProfil(p, p.getLibelle())).collect(Collectors.toList()));
		} else {
			this.typeProfils = null;
		}
	}

	/**
	 * Getter for typeProfils.
	 * Cette méthode permet de manipuler directement la foreign key de la liste de référence
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#typeProfils typeProfils}.
	 */
	@Transient
	public List<TypeProfil.Values> getTypeProfilsCode() {
		return this.typeProfils != null ? this.typeProfils.stream().map(TypeProfil::getCode).collect(Collectors.toList()) : null;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#droits droits}.
	 * Cette méthode permet définir la valeur de la FK directement
	 * @param droits value to set
	 */
	public void setDroitsCode(List<Droits.Values> droits) {
		if (droits != null) {
			if (this.droits != null) {
				this.droits.clear();
			} else {
				this.droits = new ArrayList<>();
			}
			this.droits.addAll(droits.stream().map(p -> new Droits(p, p.getLibelle(), p.getTypeProfilCode())).collect(Collectors.toList()));
		} else {
			this.droits = null;
		}
	}

	/**
	 * Getter for droits.
	 * Cette méthode permet de manipuler directement la foreign key de la liste de référence
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.securite.Profil#droits droits}.
	 */
	@Transient
	public List<Droits.Values> getDroitsCode() {
		return this.droits != null ? this.droits.stream().map(Droits::getCode).collect(Collectors.toList()) : null;
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
		dest.setTypeProfils(this.getTypeProfils());
		dest.setDroits(this.getDroits());
		dest.setSecteurs(this.getSecteurs());

		return dest;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.Profil Profil}.
	 */
	public enum Fields  {
        ID(Long.class), //
        TYPE_PROFILS(List.class), //
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
