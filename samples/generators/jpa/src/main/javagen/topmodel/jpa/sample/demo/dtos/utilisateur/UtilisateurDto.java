////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur;

import java.io.Serializable;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Email;

import topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur;
import topmodel.jpa.sample.demo.entities.utilisateur.UtilisateurMappers;
import topmodel.jpa.sample.demo.enums.utilisateur.TypeUtilisateurCode;

/**
 * Objet non persisté de communication avec le serveur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
	 */
	private Integer id;

	/**
	 * Age en années de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getAge() Utilisateur#getAge()} 
	 */
	private BigDecimal age = 6;

	/**
	 * Profil de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getProfilId() Utilisateur#getProfilId()} 
	 */
	private Integer profilId;

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
	 * Si l'utilisateur est actif.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getActif() Utilisateur#getActif()} 
	 */
	private Boolean actif;

	/**
	 * Type d'utilisateur en Many to one.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
	 */
	private TypeUtilisateurCode typeUtilisateurCode = TypeUtilisateurCode.ADM;

	/**
	 * Utilisateur enfants.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#getUtilisateursEnfant() Utilisateur#getUtilisateursEnfant()} 
	 */
	private List<Integer> utilisateursEnfant;

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
	 * Crée une nouvelle instance de 'UtilisateurDto'.
	 * @param utilisateur Instance de 'Utilisateur'.
	 *
	 * @return Une nouvelle instance de 'UtilisateurDto'.
	 */
	public UtilisateurDto(Utilisateur utilisateur) {
		UtilisateurMappers.createUtilisateurDto(utilisateur, this);
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#age age}.
	 */
	public BigDecimal getAge() {
		return this.age;
	}

	/**
	 * Getter for profilId.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 */
	public Integer getProfilId() {
		return this.profilId;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for actif.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#actif actif}.
	 */
	public Boolean getActif() {
		return this.actif;
	}

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 */
	public TypeUtilisateurCode getTypeUtilisateurCode() {
		return this.typeUtilisateurCode;
	}

	/**
	 * Getter for utilisateursEnfant.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateursEnfant utilisateursEnfant}.
	 */
	public List<Integer> getUtilisateursEnfant() {
		return this.utilisateursEnfant;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateCreation dateCreation}.
	 */
	public LocalDate getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#dateModification dateModification}.
	 */
	public LocalDateTime getDateModification() {
		return this.dateModification;
	}

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
	 */
	public UtilisateurDto getUtilisateurParent() {
		return this.utilisateurParent;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#id id}.
	 * @param id value to set
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#age age}.
	 * @param age value to set
	 */
	public void setAge(BigDecimal age) {
		this.age = age;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#profilId profilId}.
	 * @param profilId value to set
	 */
	public void setProfilId(Integer profilId) {
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
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#actif actif}.
	 * @param actif value to set
	 */
	public void setActif(Boolean actif) {
		this.actif = actif;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
	 * @param typeUtilisateurCode value to set
	 */
	public void setTypeUtilisateurCode(TypeUtilisateurCode typeUtilisateurCode) {
		this.typeUtilisateurCode = typeUtilisateurCode;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto#utilisateursEnfant utilisateursEnfant}.
	 * @param utilisateursEnfant value to set
	 */
	public void setUtilisateursEnfant(List<Integer> utilisateursEnfant) {
		this.utilisateursEnfant = utilisateursEnfant;
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
	 * @param target Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Utilisateur'.
	 */
	public Utilisateur toUtilisateur(Utilisateur target) {
		return UtilisateurMappers.toUtilisateur(this, target);
	}


	/**
	 * Mappe 'UtilisateurDto' vers 'UtilisateurDto'.
	 * @param target Instance pré-existante de 'UtilisateurDto'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'UtilisateurDto'.
	 */
	public UtilisateurDto toUtilisateurDto(UtilisateurDto target) {
		return UtilisateurDTOMappers.toUtilisateurDto(this, target);
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto UtilisateurDto}.
	 */
	public enum Fields  {
        ID(Integer.class), //
        AGE(BigDecimal.class), //
        PROFIL_ID(Integer.class), //
        EMAIL(String.class), //
        NOM(String.class), //
        ACTIF(Boolean.class), //
        TYPE_UTILISATEUR_CODE(TypeUtilisateurCode.class), //
        UTILISATEURS_ENFANT(List.class), //
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
