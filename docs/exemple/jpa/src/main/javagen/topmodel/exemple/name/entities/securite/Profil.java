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
	@SequenceGenerator(name = "SEQ_PROFIL", sequenceName = "SEQ_PROFIL", initialValue = 1000)
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PROFIL")
	@Column(name = "PRO_ID", nullable = false)
	private long id;

	/**
	 * Type de profil.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
	@JoinColumn(name = "CODE", referencedColumnName = "CODE")
	private TypeProfil typeProfil;

	/**
	 * Liste des droits de l'utilisateur.
	 */
	@ManyToMany(fetch = FetchType.LAZY, cascade = { CascadeType.PERSIST, CascadeType.MERGE })
	private List<Droits> droitsAppli;

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

		this.droitsAppli = profil.getDroitsAppli().stream().collect(Collectors.toList());
		this.secteurs = profil.getSecteurs().stream().collect(Collectors.toList());
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param typeProfil Type de profil
	 * @param droitsAppli Liste des droits de l'utilisateur
	 * @param secteurs Liste des secteurs de l'utilisateur
	 */
	public Profil(long id, TypeProfil typeProfil, List<Droits> droitsAppli, List<Secteur> secteurs) {
		this.id = id;
		this.typeProfil = typeProfil;
		this.droitsAppli = droitsAppli;
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
			this.droitsAppli = profil.getDroitsAppli();
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
	protected TypeProfil getTypeProfil() {
		return this.typeProfil;
	}

	/**
	 * Getter for droitsAppli.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.securite.Profil#droitsAppli droitsAppli}.
	 */
	protected List<Droits> getDroitsAppli() {
		if(this.droitsAppli == null)
			this.droitsAppli = new ArrayList<>();
		return this.droitsAppli;
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
	 * Set the value of {@link topmodel.exemple.name.entities.securite.Profil#droitsAppli droitsAppli}.
	 * @param droitsAppli value to set
	 */
	public void setDroitsAppli(List<Droits> droitsAppli) {
		this.droitsAppli = droitsAppli;
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
		dest.setDroitsAppli(this.getDroitsAppli());
		dest.setSecteurs(this.getSecteurs());

		return dest;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.entities.securite.Profil Profil}.
	 */
	public enum Fields implements IFieldEnum<Profil> {
        ID, //
        TYPE_PROFIL, //
        DROITS_APPLI, //
        SECTEURS
	}
}
