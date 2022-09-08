////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.entities.utilisateur;

import java.time.LocalDate;
import java.util.DateTime;

import javax.annotation.Generated;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.EntityListeners;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;
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
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur {

	/**
	 * Date de création de l'utilisateur.
	 */
	@Column(name = "UTI_DATE_CREATION", nullable = true)
	@CreatedDate
	private DateTime dateCreation;

	/**
	 * Date de modification de l'utilisateur.
	 */
	@Column(name = "UTI_DATE_MODIFICATION", nullable = true)
	@LastModifiedDate
	private DateTime dateModification;

	/**
	 * Id technique.
	 */
	@Id
	@SequenceGenerator(name = "SEQ_UTILISATEUR", sequenceName = "SEQ_UTILISATEUR", initialValue = 1000)
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_UTILISATEUR")
	@Column(name = "UTI_ID", nullable = false)
	private long id;

	/**
	 * Age en années de l'utilisateur.
	 */
	@Column(name = "UTI_AGE", nullable = true, precision = 20, scale = 9)
	private Long age;

	/**
	 * Profil de l'utilisateur.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Profil.class)
	@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
	private Profil profil;

	/**
	 * Email de l'utilisateur.
	 */
	@Column(name = "UTI_EMAIL", nullable = true, length = 50)
	@Email
	private String email;

	/**
	 * Type d'utilisateur en Many to one.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeUtilisateur.class)
	@JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
	private TypeUtilisateur typeUtilisateur;

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
	 */
	public Utilisateur(DateTime dateCreation, DateTime dateModification, long id, Long age, Profil profil, String email, TypeUtilisateur typeUtilisateur) {
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
		this.id = id;
		this.age = age;
		this.profil = profil;
		this.email = email;
		this.typeUtilisateur = typeUtilisateur;
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
	protected TypeUtilisateur getTypeUtilisateur() {
		return this.typeUtilisateur;
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
	 * Equal function comparing Id.
	 */
	public boolean equals(Object o) {
		if(o instanceof Utilisateur utilisateur) {
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
        DATE_CREATION, //
        DATE_MODIFICATION, //
        ID, //
        AGE, //
        PROFIL, //
        EMAIL, //
        TYPE_UTILISATEUR
	}
}
