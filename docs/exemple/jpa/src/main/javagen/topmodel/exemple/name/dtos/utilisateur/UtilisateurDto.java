////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dtos.utilisateur;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.constraints.NotNull;

import topmodel.exemple.name.dtos.utilisateur.interfaces.IUtilisateurDto;
import topmodel.exemple.name.entities.utilisateur.TypeUtilisateur;
import topmodel.exemple.name.entities.utilisateur.Utilisateur;

/**
 * Objet non persisté de communication avec le serveur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable, IUtilisateurDto {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
	 */
	@NotNull
	private long utilisateurId;

	/**
	 * Age en années de l'utilisateur.
	 * Alias of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#getAge() Utilisateur#getAge()} 
	 */
	private Long utilisateurAge;

	/**
	 * Profil de l'utilisateur.
	 * Alias of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#getProfilId() Utilisateur#getProfilId()} 
	 */
	private long utilisateurProfilId;

	/**
	 * Email de l'utilisateur.
	 * Alias of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#getEmail() Utilisateur#getEmail()} 
	 */
	@Email
	private String utilisateuremail;

	/**
	 * Type d'utilisateur en Many to one.
	 * Alias of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
	 */
	private TypeUtilisateur.Values utilisateurTypeUtilisateurCode;

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
	 * Copy constructor.
	 * @param utilisateurDto to copy
	 */
	public UtilisateurDto(UtilisateurDto utilisateurDto) {
		if(utilisateurDto == null) {
			return;
		}

		this.utilisateurId = utilisateurDto.getUtilisateurId();
		this.utilisateurAge = utilisateurDto.getUtilisateurAge();
		this.utilisateurProfilId = utilisateurDto.getUtilisateurProfilId();
		this.utilisateuremail = utilisateurDto.getUtilisateuremail();
		this.utilisateurTypeUtilisateurCode = utilisateurDto.getUtilisateurTypeUtilisateurCode();
		this.utilisateurParent = utilisateurDto.getUtilisateurParent();
	}

	/**
	 * All arg constructor.
	 * @param utilisateurId Id technique
	 * @param utilisateurAge Age en années de l'utilisateur
	 * @param utilisateurProfilId Profil de l'utilisateur
	 * @param utilisateuremail Email de l'utilisateur
	 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
	 * @param utilisateurParent UtilisateurParent
	 */
	public UtilisateurDto(long utilisateurId, Long utilisateurAge, long utilisateurProfilId, String utilisateuremail, TypeUtilisateur.Values utilisateurTypeUtilisateurCode, UtilisateurDto utilisateurParent) {
		this.utilisateurId = utilisateurId;
		this.utilisateurAge = utilisateurAge;
		this.utilisateurProfilId = utilisateurProfilId;
		this.utilisateuremail = utilisateuremail;
		this.utilisateurTypeUtilisateurCode = utilisateurTypeUtilisateurCode;
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Crée une nouvelle instance de 'UtilisateurDto'.
	 * @param utilisateur Instance de 'Utilisateur'.
	 *
	 * @return Une nouvelle instance de 'UtilisateurDto'.
	 */
	public UtilisateurDto(Utilisateur utilisateur) {
		this.from(utilisateur);
	}

	/**
	 * Map les champs des classes passées en paramètre dans l'instance courante.
	 * @param utilisateur Instance de 'Utilisateur'.
	 */
	protected void from(Utilisateur utilisateur) {
		if(utilisateur != null) {
			this.utilisateurId = utilisateur.getId();
			this.utilisateurAge = utilisateur.getAge();

			if(utilisateur.getProfil() != null) {
				this.utilisateurProfilId = utilisateur.getProfil().getId();
			}

			this.utilisateuremail = utilisateur.getEmail();

			if(utilisateur.getTypeUtilisateur() != null) {
				this.utilisateurTypeUtilisateurCode = utilisateur.getTypeUtilisateur().getCode();
			}

		}

	}

	/**
	 * Getter for utilisateurId.
	 *
	 * @return value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurId utilisateurId}.
	 */
	@Override
	public long getUtilisateurId() {
		return this.utilisateurId;
	}

	/**
	 * Getter for utilisateurAge.
	 *
	 * @return value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurAge utilisateurAge}.
	 */
	@Override
	public Long getUtilisateurAge() {
		return this.utilisateurAge;
	}

	/**
	 * Getter for utilisateurProfilId.
	 *
	 * @return value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurProfilId utilisateurProfilId}.
	 */
	@Override
	public long getUtilisateurProfilId() {
		return this.utilisateurProfilId;
	}

	/**
	 * Getter for utilisateuremail.
	 *
	 * @return value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateuremail utilisateuremail}.
	 */
	@Override
	public String getUtilisateuremail() {
		return this.utilisateuremail;
	}

	/**
	 * Getter for utilisateurTypeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurTypeUtilisateurCode utilisateurTypeUtilisateurCode}.
	 */
	@Override
	public TypeUtilisateur.Values getUtilisateurTypeUtilisateurCode() {
		return this.utilisateurTypeUtilisateurCode;
	}

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	@Override
	public UtilisateurDto getUtilisateurParent() {
		return this.utilisateurParent;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurId utilisateurId}.
	 * @param utilisateurId value to set
	 */
	public void setUtilisateurId(long utilisateurId) {
		this.utilisateurId = utilisateurId;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurAge utilisateurAge}.
	 * @param utilisateurAge value to set
	 */
	public void setUtilisateurAge(Long utilisateurAge) {
		this.utilisateurAge = utilisateurAge;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurProfilId utilisateurProfilId}.
	 * @param utilisateurProfilId value to set
	 */
	public void setUtilisateurProfilId(long utilisateurProfilId) {
		this.utilisateurProfilId = utilisateurProfilId;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateuremail utilisateuremail}.
	 * @param utilisateuremail value to set
	 */
	public void setUtilisateuremail(String utilisateuremail) {
		this.utilisateuremail = utilisateuremail;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurTypeUtilisateurCode utilisateurTypeUtilisateurCode}.
	 * @param utilisateurTypeUtilisateurCode value to set
	 */
	public void setUtilisateurTypeUtilisateurCode(TypeUtilisateur.Values utilisateurTypeUtilisateurCode) {
		this.utilisateurTypeUtilisateurCode = utilisateurTypeUtilisateurCode;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 * @param utilisateurParent value to set
	 */
	public void setUtilisateurParent(UtilisateurDto utilisateurParent) {
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Mappe 'UtilisateurDto' vers 'Utilisateur'.
	 * @param source Instance de 'UtilisateurDto'.
	 * @param dest Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Utilisateur'.
	 */
	public Utilisateur toUtilisateur(Utilisateur dest) {
		dest = dest == null ? new Utilisateur() : dest;

		dest.setId(this.getUtilisateurId());
		dest.setAge(this.getUtilisateurAge());
		dest.setEmail(this.getUtilisateuremail());

		return dest;
	}
}
