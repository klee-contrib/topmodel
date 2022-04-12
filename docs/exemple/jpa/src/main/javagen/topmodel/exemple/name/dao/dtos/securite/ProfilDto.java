////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.securite;

import java.io.Serializable;
import java.util.List;

import javax.annotation.Generated;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.entities.securite.TypeProfil;

/**
 * Objet métier non persisté représentant Profil.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ProfilDto implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
	 */
	private long id;

	/**
	 * Type de profil.
	 * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
	 */
	private TypeProfil.Values typeProfilCode;

	/**
	 * Liste paginée des utilisateurs de ce profil.
	 */
	private List<UtilisateurDto> utilisateurs;

	/**
	 * No arg constructor.
	 */
	public ProfilDto() {
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param typeProfilCode Type de profil
	 * @param utilisateurs Liste paginée des utilisateurs de ce profil
	 */
	public ProfilDto(long id, TypeProfil.Values typeProfilCode, List<UtilisateurDto> utilisateurs) {
		this.id = id;
		this.typeProfilCode = typeProfilCode;
		this.utilisateurs = utilisateurs;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.securite.ProfilDto#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for typeProfilCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.securite.ProfilDto#typeProfilCode typeProfilCode}.
	 */
	public TypeProfil.Values getTypeProfilCode() {
		return this.typeProfilCode;
	}

	/**
	 * Getter for utilisateurs.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.securite.ProfilDto#utilisateurs utilisateurs}.
	 */
	public List<UtilisateurDto> getUtilisateurs() {
		return this.utilisateurs;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.securite.ProfilDto#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.securite.ProfilDto#typeProfilCode typeProfilCode}.
	 * @param typeProfilCode value to set
	 */
	public void setTypeProfilCode(TypeProfil.Values typeProfilCode) {
		this.typeProfilCode = typeProfilCode;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.securite.ProfilDto#utilisateurs utilisateurs}.
	 * @param utilisateurs value to set
	 */
	public void setUtilisateurs(List<UtilisateurDto> utilisateurs) {
		this.utilisateurs = utilisateurs;
	}
}
