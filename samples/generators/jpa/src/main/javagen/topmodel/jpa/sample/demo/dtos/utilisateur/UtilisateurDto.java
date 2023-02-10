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
import topmodel.jpa.sample.demo.mappers.UtilisateurDTOMappers;

/**
 * Objet non persisté de communication avec le serveur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable, IUtilisateurDto {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
	 */
	private Long id;

	/**
	 * Age en années de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getAge() Utilisateur#getAge()} 
	 */
	private Long age = 6l;

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
	 * Nom de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getNom() Utilisateur#getNom()} 
	 */
	private String nom = "Jabx";

	/**
	 * Type d'utilisateur en Many to one.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
	 */
	private TypeUtilisateur.Values typeUtilisateurCode = TypeUtilisateur.Values.ADM;

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

		this.id = utilisateurDto.getId();
		this.age = utilisateurDto.getAge();
		this.profilId = utilisateurDto.getProfilId();
		this.email = utilisateurDto.getEmail();
		this.nom = utilisateurDto.getNom();
		this.typeUtilisateurCode = utilisateurDto.getTypeUtilisateurCode();
		this.dateCreation = utilisateurDto.getDateCreation();
		this.dateModification = utilisateurDto.getDateModification();
		this.utilisateurParent = utilisateurDto.getUtilisateurParent();
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profilId Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param nom Nom de l'utilisateur
	 * @param typeUtilisateurCode Type d'utilisateur en Many to one
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @param utilisateurParent UtilisateurParent
	 */
	public UtilisateurDto(Long id, Long age, Long profilId, String email, String nom, TypeUtilisateur.Values typeUtilisateurCode, LocalDate dateCreation, LocalDateTime dateModification, UtilisateurDto utilisateurParent) {
		this.id = id;
		this.age = age;
		this.profilId = profilId;
		this.email = email;
		this.nom = nom;
		this.typeUtilisateurCode = typeUtilisateurCode;
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Crée une nouvelle instance de 'UtilisateurDto'.
	 * @param utilisateur Instance de 'Utilisateur'.
	 *
	 * @return Une nouvelle instance de 'UtilisateurDto'.
	 */
	public UtilisateurDto(Utilisateur utilisateur) {
		UtilisateurDto.map(this, utilisateur);
	}

	/**
	 * Map les champs des classes passées en paramètre dans l'instance courante.
	 * @param utilisateur Instance de 'Utilisateur'.
	 */
	public static void map(UtilisateurDto target, Utilisateur utilisateur) {
		if (utilisateur != null) {
			target.utilisateurParent = utilisateur.getUtilisateurParent() == null ? null : new UtilisateurDto(utilisateur.getUtilisateurParent());
			target.id = utilisateur.getId();
			target.age = utilisateur.getAge();
			if (utilisateur.getProfil() != null) {
				target.profilId = utilisateur.getProfil().getId();
			}

			target.email = utilisateur.getEmail();
			target.nom = utilisateur.getNom();
			if (utilisateur.getTypeUtilisateur() != null) {
				target.typeUtilisateurCode = utilisateur.getTypeUtilisateur().getCode();
			}

			target.dateCreation = utilisateur.getDateCreation();
			target.dateModification = utilisateur.getDateModification();
		} else {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}
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
	 * Getter for nom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#nom nom}.
	 */
	@Override
	public String getNom() {
		return this.nom;
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
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	@Override
	public UtilisateurDto getUtilisateurParent() {
		return this.utilisateurParent;
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
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#nom nom}.
	 * @param nom value to set
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 * @param typeUtilisateurCode value to set
	 */
	public void setTypeUtilisateurCode(TypeUtilisateur.Values typeUtilisateurCode) {
		this.typeUtilisateurCode = typeUtilisateurCode;
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
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 * @param utilisateurParent value to set
	 */
	public void setUtilisateurParent(UtilisateurDto utilisateurParent) {
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Mappe 'UtilisateurDto' vers 'Utilisateur'.
	 * @param source Instance de 'UtilisateurDto'.
	 * @param target Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Utilisateur'.
	 */
	public Utilisateur toUtilisateur(Utilisateur target) {
		target = target == null ? new Utilisateur() : dest;
		UtilisateurDTOMappers.ToUtilisateur(source, target);
		return target;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto UtilisateurDto}.
	 */
	public enum Fields  {
        ID(Long.class), //
        AGE(Long.class), //
        PROFIL_ID(Long.class), //
        EMAIL(String.class), //
        NOM(String.class), //
        TYPE_UTILISATEUR_CODE(TypeUtilisateur.Values.class), //
        DATE_CREATION(LocalDate.class), //
        DATE_MODIFICATION(LocalDateTime.class), //
        UTILISATEUR_PARENT(UtilisateurDto.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}
