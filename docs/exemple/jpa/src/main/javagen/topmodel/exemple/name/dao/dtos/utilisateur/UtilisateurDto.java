////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.utilisateur;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.constraints.NotNull;

import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;

/**
 * Objet non persisté de communication avec le serveur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
	 */
	private long id;

	/**
	 * Email de l'utilisateur.
	 * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getEmail() Utilisateur#getEmail()} 
	 */
	@Email
	private String email;

	/**
	 * Type d'utilisateur en Many to one.
	 * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
	 */
	private TypeUtilisateur.Values typeUtilisateurCode;

	/**
	 * Type d'utilisateur en one to one.
	 * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCodeOrigin() Utilisateur#getTypeUtilisateurCodeOrigin()} 
	 */
	private TypeUtilisateur.Values typeUtilisateurCodeOrigin;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
	 */
	@NotNull
	private long profilId;

	/**
	 * Type de profil.
	 * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
	 */
	private TypeProfil.Values profilTypeProfilCode;

	/**
	 * UtilisateurParent.
	 */
	private UtilisateurDto utilisateurParent;

	/**
	 * No arg constructor.
	 */
	public UtilisateurDto() {
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param typeUtilisateurCodeOrigin Type d'utilisateur en one to one
	 * @param profilId Id technique
	 * @param profilTypeProfilCode Type de profil
	 * @param utilisateurParent UtilisateurParent
	 */
	public UtilisateurDto(long id, String email, TypeUtilisateur.Values typeUtilisateurCode, TypeUtilisateur.Values typeUtilisateurCodeOrigin, long profilId, TypeProfil.Values profilTypeProfilCode, UtilisateurDto utilisateurParent) {
		this.id = id;
		this.email = email;
		this.typeUtilisateurCode = typeUtilisateurCode;
		this.typeUtilisateurCodeOrigin = typeUtilisateurCodeOrigin;
		this.profilId = profilId;
		this.profilTypeProfilCode = profilTypeProfilCode;
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 */
	public TypeUtilisateur.Values getTypeUtilisateurCode() {
		return this.typeUtilisateurCode;
	}

	/**
	 * Getter for typeUtilisateurCodeOrigin.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#typeUtilisateurCodeOrigin typeUtilisateurCodeOrigin}.
	 */
	public TypeUtilisateur.Values getTypeUtilisateurCodeOrigin() {
		return this.typeUtilisateurCodeOrigin;
	}

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 */
	public long getProfilId() {
		return this.profilId;
	}

	/**
	 * Getter for profilTypeProfilCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilTypeProfilCode profilTypeProfilCode}.
	 */
	public TypeProfil.Values getProfilTypeProfilCode() {
		return this.profilTypeProfilCode;
	}

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	public UtilisateurDto getUtilisateurParent() {
		return this.utilisateurParent;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#email email}.
	 * @param email value to set
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 * @param typeUtilisateurCode value to set
	 */
	public void setTypeUtilisateurCode(TypeUtilisateur.Values typeUtilisateurCode) {
		this.typeUtilisateurCode = typeUtilisateurCode;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#typeUtilisateurCodeOrigin typeUtilisateurCodeOrigin}.
	 * @param typeUtilisateurCodeOrigin value to set
	 */
	public void setTypeUtilisateurCodeOrigin(TypeUtilisateur.Values typeUtilisateurCodeOrigin) {
		this.typeUtilisateurCodeOrigin = typeUtilisateurCodeOrigin;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 * @param profilId value to set
	 */
	public void setProfilId(long profilId) {
		this.profilId = profilId;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilTypeProfilCode profilTypeProfilCode}.
	 * @param profilTypeProfilCode value to set
	 */
	public void setProfilTypeProfilCode(TypeProfil.Values profilTypeProfilCode) {
		this.profilTypeProfilCode = profilTypeProfilCode;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 * @param utilisateurParent value to set
	 */
	public void setUtilisateurParent(UtilisateurDto utilisateurParent) {
		this.utilisateurParent = utilisateurParent;
	}
}
