////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.utilisateur;

import java.io.Serializable;
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

import topmodel.exemple.name.dao.entities.securite.Profil;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur implements Serializable {
	/** Serial ID */
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 */
	@Id
	@SequenceGenerator(name = "SEQ_UTILISATEUR", sequenceName = "SEQ_UTILISATEUR",  initialValue = 1000, allocationSize = 1)
	@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_UTILISATEUR")
	@Column(name = "UTI_ID", nullable = false)
	private long id;

	/**
	 * Profil de l'utilisateur.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Profil.class)
	@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
	private Profil profil;

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
	 * Email de l'utilisateur.
	 */
	@Column(name = "UTI_EMAIL", nullable = true)
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
	 * All arg constructor.
	 * @param id Id technique
	 * @param profil Profil de l'utilisateur
	 * @param dateCreation Date de création de l'utilisateur
	 * @param dateModification Date de modification de l'utilisateur
	 * @param email Email de l'utilisateur
	 * @param typeUtilisateur Type d'utilisateur en Many to one
	 */
	public Utilisateur(long id, Profil profil, DateTime dateCreation, DateTime dateModification, String email, TypeUtilisateur typeUtilisateur) {
		this.id = id;
		this.profil = profil;
		this.dateCreation = dateCreation;
		this.dateModification = dateModification;
		this.email = email;
		this.typeUtilisateur = typeUtilisateur;
	}

	/**
	 * Alias constructor.
	 * Ce constructeur permet d'initialiser un objet Utilisateur avec comme paramètres les classes dont les propriétés sont référencées Utilisateur.
	 * A ne pas utiliser pour construire un Dto en plusieurs requêtes.
	 * Voir la <a href="https://klee-contrib.github.io/topmodel/#/generator/jpa?id=constructeurs-par-alias">documentation</a>

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#id id}.
	 */
	public long getId() {
		return this.id;
	}

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#dateCreation dateCreation}.
	 */
	public DateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#dateModification dateModification}.
	 */
	public DateTime getDateModification() {
		return this.dateModification;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for typeUtilisateur.
	 *
	 * @return value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
	 */
	protected TypeUtilisateur getTypeUtilisateur() {
		return this.typeUtilisateur;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#id id}.
	 * @param id value to set
	 */
	public void setId(long id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#profil profil}.
	 * @param profil value to set
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#dateCreation dateCreation}.
	 * @param dateCreation value to set
	 */
	public void setDateCreation(DateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#dateModification dateModification}.
	 * @param dateModification value to set
	 */
	public void setDateModification(DateTime dateModification) {
		this.dateModification = dateModification;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#email email}.
	 * @param email value to set
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#typeUtilisateur typeUtilisateur}.
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
	 * Enumération des champs de la classe {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur Utilisateur}.
	 */
	public enum Fields implements IFieldEnum<Utilisateur> {
        ID, //
        PROFIL, //
        DATE_CREATION, //
        DATE_MODIFICATION, //
        EMAIL, //
        TYPE_UTILISATEUR
	}
}
