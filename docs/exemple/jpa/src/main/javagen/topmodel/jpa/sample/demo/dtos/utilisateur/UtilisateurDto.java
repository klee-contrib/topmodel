////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur;

import java.io.Serializable;
import java.time.LocalDate;
import java.time.LocalDateTime;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Email;

import topmodel.jpa.sample.demo.dtos.utilisateur.interfaces.IUtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.TypeUtilisateur;
import topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur;

/**
 * Objet non persisté de communication avec le serveur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable, IUtilisateurDto {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Date de création de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getDateCreation() Utilisateur#getDateCreation()} 
	 */
	private LocalDate dateCreation;

	/**
	 * Date de modification de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getDateModification() Utilisateur#getDateModification()} 
	 */
	private LocalDateTime dateModification;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
	 */
	private Long id;

	/**
	 * Age en années de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getAge() Utilisateur#getAge()} 
	 */
	private Long age;

	/**
	 * Profil de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getProfilId() Utilisateur#getProfilId()} 
	 */
	private Long profilId;

	/**
	 * Email de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getEmail() Utilisateur#getEmail()} 
	 */
	@Email
	private String email;

	/**
	 * Type d'utilisateur en Many to one.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
	 */
	private TypeUtilisateur.Values typeUtilisateurCode;

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

		this.dateCreation = utilisateurDto.getDateCreation();
		this.dateModification = utilisateurDto.getDateModification();
		this.id = utilisateurDto.getId();
		this.age = utilisateurDto.getAge();
		this.profilId = utilisateurDto.getProfilId();
		this.email = utilisateurDto.getEmail();
		this.typeUtilisateurCode = utilisateurDto.getTypeUtilisateurCode();
		this.utilisateurParent = utilisateurDto.getUtilisateurParent();
	}

	/**
	 * All arg constructor.
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @param id Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profilId Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param utilisateurParent UtilisateurParent
	 */
	public UtilisateurDto(LocalDate dateCreation, LocalDateTime dateModification, Long id, Long age, Long profilId, String email, TypeUtilisateur.Values typeUtilisateurCode, UtilisateurDto utilisateurParent) {
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
		this.id = id;
		this.age = age;
		this.profilId = profilId;
		this.email = email;
		this.typeUtilisateurCode = typeUtilisateurCode;
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
		if (utilisateur != null) {
			this.utilisateurParent = utilisateur.getUtilisateurParent() != null ? null : new UtilisateurDto(utilisateur.getUtilisateurParent());
			this.dateCreation = utilisateur.getDateCreation();
			this.dateModification = utilisateur.getDateModification();
			this.id = utilisateur.getId();
			this.age = utilisateur.getAge();
			if (utilisateur.getProfil() != null) {
				this.profilId = utilisateur.getProfil().getId();
			}

			this.email = utilisateur.getEmail();
			if (utilisateur.getTypeUtilisateur() != null) {
				this.typeUtilisateurCode = utilisateur.getTypeUtilisateur().getCode();
			}
		} else {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateCreation dateCreation}.
	 */
	@Override
	public LocalDate getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateModification dateModification}.
	 */
	@Override
	public LocalDateTime getDateModification() {
		return this.dateModification;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#id id}.
	 */
	@Override
	public Long getId() {
		return this.id;
	}

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#age age}.
	 */
	@Override
	public Long getAge() {
		return this.age;
	}

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 */
	@Override
	public Long getProfilId() {
		return this.profilId;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#email email}.
	 */
	@Override
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 */
	@Override
	public TypeUtilisateur.Values getTypeUtilisateurCode() {
		return this.typeUtilisateurCode;
	}

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	@Override
	public UtilisateurDto getUtilisateurParent() {
		return this.utilisateurParent;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateCreation dateCreation}.
	 * @param dateCreation value to set
	 */
	public void setDateCreation(LocalDate dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateModification dateModification}.
	 * @param dateModification value to set
	 */
	public void setDateModification(LocalDateTime dateModification) {
		this.dateModification = dateModification;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#id id}.
	 * @param id value to set
	 */
	public void setId(Long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#age age}.
	 * @param age value to set
	 */
	public void setAge(Long age) {
		this.age = age;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 * @param profilId value to set
	 */
	public void setProfilId(Long profilId) {
		this.profilId = profilId;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#email email}.
	 * @param email value to set
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 * @param typeUtilisateurCode value to set
	 */
	public void setTypeUtilisateurCode(TypeUtilisateur.Values typeUtilisateurCode) {
		this.typeUtilisateurCode = typeUtilisateurCode;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
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

		if (this.getUtilisateurParent() != null) {
			dest.setUtilisateurParent(this.getUtilisateurParent().toUtilisateur(dest.getUtilisateurParent()));
		}

		dest.setDateCreation(this.getDateCreation());
		dest.setDateModification(this.getDateModification());
		dest.setId(this.getId());
		dest.setAge(this.getAge());
		dest.setEmail(this.getEmail());

		return dest;
	}
}
