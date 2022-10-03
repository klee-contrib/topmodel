////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.entities.utilisateur;

import java.time.LocalDate;
import java.util.DateTime;

import javax.annotation.Generated;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.EntityListeners;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;
import javax.persistence.UniqueConstraint;
import javax.validation.constraints.Email;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import oorg.springframework.data.jpa.domain.support.AuditingEntityListener;

import topmodel.exemple.name.entities.securite.Profil;
import topmodel.exemple.name.entities.utilisateur.TypeUtilisateur;
import topmodel.exemple.utils.IFieldEnum;

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
	 * Date de création de l'utilisateur.
	 */
	@Column(name = "DATE_CREATION", nullable = true)
	@CreatedDate
	private DateTime dateCreation;

	/**
	 * Date de modification de l'utilisateur.
	 */
	@Column(name = "DATE_MODIFICATION", nullable = true)
	@LastModifiedDate
	private DateTime dateModification;

	/**
	 * Id technique.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "ID", nullable = false)
	private long id;

	/**
	 * Age en années de l'utilisateur.
	 */
	@Column(name = "AGE", nullable = true, precision = 20, scale = 9)
	private Long age;

	/**
	 * Profil de l'utilisateur.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Profil.class)
	@JoinColumn(name = "ID", referencedColumnName = "PRO_ID")
	private Profil profil;

	/**
	 * Email de l'utilisateur.
	 */
	@Column(name = "EMAIL", nullable = true, length = 50)
	@Email
	private String email;

	/**
	 * Type d'utilisateur en Many to one.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeUtilisateur.class)
	@JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
	private TypeUtilisateur typeUtilisateur;

	/**
	 * Utilisateur parent.
	 */
	@OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = true)
	@JoinColumn(name = "ID_PARENT", referencedColumnName = "ID", unique = true)
	private Utilisateur utilisateurParent;

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

		this.dateCreation = utilisateur.getDateCreation();
		this.dateModification = utilisateur.getDateModification();
		this.id = utilisateur.getId();
		this.age = utilisateur.getAge();
		this.profil = utilisateur.getProfil();
		this.email = utilisateur.getEmail();
		this.typeUtilisateur = utilisateur.getTypeUtilisateur();
		this.utilisateurParent = utilisateur.getUtilisateurParent();
	}

	/**
	 * All arg constructor.
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @param id Id technique
	 * @param age Age en années de l'utilisateur
	 * @param profil Profil de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateur Type d'utilisateur en Many to one
	 * @param utilisateurParent Utilisateur parent
	 */
	public Utilisateur(DateTime dateCreation, DateTime dateModification, long id, Long age, Profil profil, String email, TypeUtilisateur typeUtilisateur, Utilisateur utilisateurParent) {
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
		this.id = id;
		this.age = age;
		this.profil = profil;
		this.email = email;
		this.typeUtilisateur = typeUtilisateur;
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#dateCreation dateCreation}.
	 */
	public DateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#dateModification dateModification}.
	 */
	public DateTime getDateModification() {
		return this.dateModification;
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#age age}.
	 */
	public Long getAge() {
		return this.age;
	}

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for typeUtilisateur.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
	 */
	public TypeUtilisateur getTypeUtilisateur() {
		return this.typeUtilisateur;
	}

	/**
	 * Getter for utilisateurParent.
	 *
	 * @return value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#utilisateurParent utilisateurParent}.
	 */
	public Utilisateur getUtilisateurParent() {
		return this.utilisateurParent;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#dateCreation dateCreation}.
	 * @param dateCreation value to set
	 */
	public void setDateCreation(DateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#dateModification dateModification}.
	 * @param dateModification value to set
	 */
	public void setDateModification(DateTime dateModification) {
		this.dateModification = dateModification;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#age age}.
	 * @param age value to set
	 */
	public void setAge(Long age) {
		this.age = age;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#profil profil}.
	 * @param profil value to set
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#email email}.
	 * @param email value to set
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
	 * @param typeUtilisateur value to set
	 */
	public void setTypeUtilisateur(TypeUtilisateur typeUtilisateur) {
		this.typeUtilisateur = typeUtilisateur;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.entities.utilisateur.Utilisateur#utilisateurParent utilisateurParent}.
	 * @param utilisateurParent value to set
	 */
	public void setUtilisateurParent(Utilisateur utilisateurParent) {
		this.utilisateurParent = utilisateurParent;
	}

	/**
	 * Equal function comparing Id.
	 */
	public boolean equals(Object o) {
		if(o instanceof Utilisateur) {
			Utilisateur utilisateur = (Utilisateur) o;
			if(this == utilisateur)
				return true;

			if(utilisateur == null || this.getId() == null)
				return false;

			return this.getId().equals(utilisateur.getId());
		}

		return false;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.exemple.name.entities.utilisateur.Utilisateur Utilisateur}.
	 */
	public enum Fields implements IFieldEnum<Utilisateur> {
        DATE_CREATION(DateTime.class), //
        DATE_MODIFICATION(DateTime.class), //
        ID(long.class), //
        AGE(Long.class), //
        PROFIL(Profil.class), //
        EMAIL(String.class), //
        TYPE_UTILISATEUR(TypeUtilisateur.class), //
        UTILISATEUR_PARENT(Utilisateur.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}
