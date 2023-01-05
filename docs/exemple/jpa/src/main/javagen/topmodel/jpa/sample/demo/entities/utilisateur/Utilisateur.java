////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;
import jakarta.persistence.UniqueConstraint;

import topmodel.jpa.sample.demo.entities.securite.Profil;

/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR", uniqueConstraints = {
    @UniqueConstraint(columnNames = {"EMAIL","ID_PARENT"})})
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur {

	/**
	 * Id technique.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "ID", nullable = false)
	private Long id;

	/**
	 * Age en années de l'utilisateur.
	 */
	@Column(name = "AGE", nullable = true, precision = 20, scale = 9)
	private Long age = 6;

	/**
	 * Profil de l'utilisateur.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Profil.class)
	@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
	private Profil profil;

	/**
	 * Email de l'utilisateur.
	 */
	@Column(name = "EMAIL", nullable = true, length = 50)
	private String email;

	/**
	 * Nom de l'utilisateur.
	 */
	@Column(name = "NOM", nullable = true, length = 3)
	private String nom = "Jabx";

	/**
	 * Type d'utilisateur en Many to one.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeUtilisateur.class)
	@JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
	private TypeUtilisateur typeUtilisateur = TypeUtilisateur.Values.ADM.getEntity();

	/**
	 * Utilisateur parent.
	 */
	@OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, optional = true)
	@JoinColumn(name = "ID_PARENT", referencedColumnName = "ID", unique = true)
	private Utilisateur utilisateurParent;

	/**
	 * Date de création de l'utilisateur.
	 */
	@Column(name = "DATE_CREATION", nullable = true)
	@CreatedDate
	private LocalDate dateCreation;

	/**
	 * Date de modification de l'utilisateur.
	 */
	@Column(name = "DATE_MODIFICATION", nullable = true)
	@LastModifiedDate
	private LocalDateTime dateModification;

	/**
	 * No arg constructor.
	 */
	public Utilisateur() {
	}

	/**
	 * Copy constructor.
	 * @param utilisateur to copy
	 */
	public Utilisateur(Utilisateur utilisateur) {
		if(utilisateur == null) {
			return;
		}

		this.id = utilisateur.getId();
		this.age = utilisateur.getAge();
		this.profil = utilisateur.getProfil();
		this.email = utilisateur.getEmail();
		this.nom = utilisateur.getNom();
		this.utilisateurParent = utilisateur.getUtilisateurParent();
		this.dateCreation = utilisateur.getDateCreation();
		this.dateModification = utilisateur.getDateModification();

		this.setTypeUtilisateurCode(utilisateur.getTypeUtilisateurCode());
	}

	/**
	 * All arg constructor.
	 * @param id Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profil Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param nom Nom de l'utilisateur
	 * @param typeUtilisateur Type d'utilisateur en Many to one
	 * @param utilisateurParent Utilisateur parent
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 */
	public Utilisateur(Long id, Long age, Profil profil, String email, String nom, TypeUtilisateur typeUtilisateur, Utilisateur utilisateurParent, LocalDate dateCreation, LocalDateTime dateModification) {
		this.id = id;
		this.age = age;
		this.profil = profil;
		this.email = email;
		this.nom = nom;
		this.typeUtilisateur = typeUtilisateur;
		this.utilisateurParent = utilisateurParent;
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
	}

	/**
	 * All arg constructor when Enum shortcut mode is set.
	 * @param id Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profil Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param nom Nom de l'utilisateur
	 * @param typeUtilisateur Type d'utilisateur en Many to one
	 * @param utilisateurParent Utilisateur parent
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 */
	public Utilisateur(Long id, Long age, Profil profil, String email, String nom, TypeUtilisateur.Values typeUtilisateurCode, Utilisateur utilisateurParent, LocalDate dateCreation, LocalDateTime dateModification) {
		this.id = id;
		this.age = age;
		this.profil = profil;
		this.email = email;
		this.nom = nom;
		this.setTypeUtilisateurCode(typeUtilisateurCode);
		this.utilisateurParent = utilisateurParent;
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#id id}.
	 */
	public Long getId() {
		return this.id;
	}

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#age age}.
	 */
	public Long getAge() {
		return this.age;
	}

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for typeUtilisateur.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
	 */
	public TypeUtilisateur getTypeUtilisateur() {
		return this.typeUtilisateur;
	}

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#utilisateurParent utilisateurParent}.
	 */
	public Utilisateur getUtilisateurParent() {
		return this.utilisateurParent;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#dateCreation dateCreation}.
	 */
	public LocalDate getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#dateModification dateModification}.
	 */
	public LocalDateTime getDateModification() {
		return this.dateModification;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#id id}.
	 * @param id value to set
	 */
	public void setId(Long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#age age}.
	 * @param age value to set
	 */
	public void setAge(Long age) {
		this.age = age;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#profil profil}.
	 * @param profil value to set
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#email email}.
	 * @param email value to set
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#nom nom}.
	 * @param nom value to set
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
	 * @param typeUtilisateur value to set
	 */
	public void setTypeUtilisateur(TypeUtilisateur typeUtilisateur) {
		this.typeUtilisateur = typeUtilisateur;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#utilisateurParent utilisateurParent}.
	 * @param utilisateurParent value to set
	 */
	public void setUtilisateurParent(Utilisateur utilisateurParent) {
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#dateCreation dateCreation}.
	 * @param dateCreation value to set
	 */
	public void setDateCreation(LocalDate dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#dateModification dateModification}.
	 * @param dateModification value to set
	 */
	public void setDateModification(LocalDateTime dateModification) {
		this.dateModification = dateModification;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#typeUtilisateurCode typeUtilisateurCode}.
	 * Cette méthode permet définir la valeur de la FK directement
	 * @param typeUtilisateurCode value to set
	 */
	public void setTypeUtilisateurCode(TypeUtilisateur.Values typeUtilisateurCode) {
		if (typeUtilisateurCode != null) {
			this.typeUtilisateur = typeUtilisateurCode.getEntity();
		} else {
			this.typeUtilisateur = null;
		}
	}

	/**
	 * Getter for typeUtilisateurCode.
	 * Cette méthode permet de manipuler directement la foreign key de la liste de référence
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
	 */
	@Transient
	public TypeUtilisateur.Values getTypeUtilisateurCode() {
		return this.typeUtilisateur != null ? this.typeUtilisateur.getCode() : null;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur Utilisateur}.
	 */
	public enum Fields  {
        ID(Long.class), //
        AGE(Long.class), //
        PROFIL(Profil.class), //
        EMAIL(String.class), //
        NOM(String.class), //
        TYPE_UTILISATEUR(TypeUtilisateur.class), //
        UTILISATEUR_PARENT(Utilisateur.class), //
        DATE_CREATION(LocalDate.class), //
        DATE_MODIFICATION(LocalDateTime.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}
