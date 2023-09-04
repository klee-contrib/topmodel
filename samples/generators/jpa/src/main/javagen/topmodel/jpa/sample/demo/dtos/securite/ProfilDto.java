////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite;

import java.io.Serializable;
import java.util.List;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.securite.Profil;
import topmodel.jpa.sample.demo.entities.securite.SecuriteMappers;
import topmodel.jpa.sample.demo.enums.securite.DroitCode;
import topmodel.jpa.sample.demo.enums.securite.TypeProfilCode;

/**
 * Objet métier non persisté représentant Profil.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ProfilDto implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.Profil#getId() Profil#getId()} 
	 */
	private Long id;

	/**
	 * Type de profil.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
	 */
	private TypeProfilCode typeProfilCode;

	/**
	 * Liste des droits de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.Profil#getDroits() Profil#getDroits()} 
	 */
	private List<DroitCode> droits;

	/**
	 * Liste paginée des utilisateurs de ce profil.
	 */
	private List<UtilisateurDto> utilisateurs;

	/**
	 * Liste des secteurs du profil.
	 */
	private List<SecteurDto> secteurs;

	/**
	 * No arg constructor.
	 */
	public ProfilDto() {
	}

	/**
	 * Crée une nouvelle instance de 'ProfilDto'.
	 * @param profil Instance de 'Profil'.
	 *
	 * @return Une nouvelle instance de 'ProfilDto'.
	 */
	public ProfilDto(Profil profil) {
		SecuriteMappers.createProfilDto(profil, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#id id}.
	 */
	public Long getId() {
		return this.id;
	}

	/**
	 * Getter for typeProfilCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#typeProfilCode typeProfilCode}.
	 */
	public TypeProfilCode getTypeProfilCode() {
		return this.typeProfilCode;
	}

	/**
	 * Getter for droits.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#droits droits}.
	 */
	public List<DroitCode> getDroits() {
		return this.droits;
	}

	/**
	 * Getter for utilisateurs.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#utilisateurs utilisateurs}.
	 */
	public List<UtilisateurDto> getUtilisateurs() {
		return this.utilisateurs;
	}

	/**
	 * Getter for secteurs.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#secteurs secteurs}.
	 */
	public List<SecteurDto> getSecteurs() {
		return this.secteurs;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#id id}.
	 * @param id value to set
	 */
	public void setId(Long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#typeProfilCode typeProfilCode}.
	 * @param typeProfilCode value to set
	 */
	public void setTypeProfilCode(TypeProfilCode typeProfilCode) {
		this.typeProfilCode = typeProfilCode;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#droits droits}.
	 * @param droits value to set
	 */
	public void setDroits(List<DroitCode> droits) {
		this.droits = droits;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#utilisateurs utilisateurs}.
	 * @param utilisateurs value to set
	 */
	public void setUtilisateurs(List<UtilisateurDto> utilisateurs) {
		this.utilisateurs = utilisateurs;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto#secteurs secteurs}.
	 * @param secteurs value to set
	 */
	public void setSecteurs(List<SecteurDto> secteurs) {
		this.secteurs = secteurs;
	}

	/**
	 * Mappe 'ProfilDto' vers 'Profil'.
	 * @param target Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Profil'.
	 */
	public Profil toProfil(Profil target) {
		return SecuriteMappers.toProfil(this, target);
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.dtos.securite.ProfilDto ProfilDto}.
	 */
	public enum Fields  {
        ID(Long.class), //
        TYPE_PROFIL_CODE(TypeProfilCode.class), //
        DROITS(List.class), //
        UTILISATEURS(List.class), //
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
