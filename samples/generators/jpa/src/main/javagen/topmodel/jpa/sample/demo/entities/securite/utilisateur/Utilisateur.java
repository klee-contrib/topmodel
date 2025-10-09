////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import jakarta.annotation.Generated;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityListeners;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

import topmodel.jpa.sample.demo.entities.securite.profil.Profil;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

/**
 * Utilisateur de l'application.
 */
@Entity
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(name = "UTILISATEUR", uniqueConstraints = {@UniqueConstraint(columnNames = {"UTI_EMAIL"})})
public class Utilisateur {

	/**
	 * Id de l'utilisateur.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "UTI_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Nom de l'utilisateur.
	 */
	@Column(name = "UTI_NOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String nom;

	/**
	 * Nom de l'utilisateur.
	 */
	@Column(name = "UTI_PRENOM", nullable = false, length = 100, columnDefinition = "varchar")
	private String prenom;

	/**
	 * Email de l'utilisateur.
	 */
	@Column(name = "UTI_EMAIL", nullable = false, length = 50, columnDefinition = "varchar")
	private String email;

	/**
	 * Age de l'utilisateur.
	 */
	@Column(name = "UTI_DATE_NAISSANCE", columnDefinition = "date")
	private LocalDate dateNaissance;

	/**
	 * Adresse de l'utilisateur.
	 */
	@Column(name = "UTI_ADRESSE", length = 100, columnDefinition = "varchar")
	private String adresse;

	/**
	 * Si l'utilisateur est actif.
	 */
	@Column(name = "UTI_ACTIF", nullable = false, columnDefinition = "boolean")
	private Boolean actif = true;

	/**
	 * Profil de l'utilisateur.
	 */
	@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Profil.class)
	private Profil profil;

	/**
	 * Type d'utilisateur.
	 */
	@JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
	@ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = TypeUtilisateur.class)
	private TypeUtilisateur typeUtilisateur = new TypeUtilisateur(TypeUtilisateurCode.GEST);

	/**
	 * Date de création de l'utilisateur.
	 */
	@CreatedDate
	@Column(name = "UTI_DATE_CREATION", nullable = false, columnDefinition = "date")
	private LocalDateTime dateCreation = LocalDateTime.now();

	/**
	 * Date de modification de l'utilisateur.
	 */
	@LastModifiedDate
	@Column(name = "UTI_DATE_MODIFICATION", columnDefinition = "date")
	private LocalDateTime dateModification = LocalDateTime.now();

	/**
	 * Getter for id.
	 *
	 * @return value of {@link #id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Getter for prenom.
	 *
	 * @return value of {@link #prenom prenom}.
	 */
	public String getPrenom() {
		return this.prenom;
	}

	/**
	 * Getter for email.
	 *
	 * @return value of {@link #email email}.
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Getter for dateNaissance.
	 *
	 * @return value of {@link #dateNaissance dateNaissance}.
	 */
	public LocalDate getDateNaissance() {
		return this.dateNaissance;
	}

	/**
	 * Getter for adresse.
	 *
	 * @return value of {@link #adresse adresse}.
	 */
	public String getAdresse() {
		return this.adresse;
	}

	/**
	 * Getter for actif.
	 *
	 * @return value of {@link #actif actif}.
	 */
	public Boolean getActif() {
		return this.actif;
	}

	/**
	 * Getter for profil.
	 *
	 * @return value of {@link #profil profil}.
	 */
	public Profil getProfil() {
		return this.profil;
	}

	/**
	 * Getter for typeUtilisateur.
	 *
	 * @return value of {@link #typeUtilisateur typeUtilisateur}.
	 */
	public TypeUtilisateur getTypeUtilisateur() {
		return this.typeUtilisateur;
	}

	/**
	 * Getter for dateCreation.
	 *
	 * @return value of {@link #dateCreation dateCreation}.
	 */
	public LocalDateTime getDateCreation() {
		return this.dateCreation;
	}

	/**
	 * Getter for dateModification.
	 *
	 * @return value of {@link #dateModification dateModification}.
	 */
	public LocalDateTime getDateModification() {
		return this.dateModification;
	}

	/**
	 * Set the value of {@link #id id}.
	 * @param id value to set.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Set the value of {@link #prenom prenom}.
	 * @param prenom value to set.
	 */
	public void setPrenom(String prenom) {
		this.prenom = prenom;
	}

	/**
	 * Set the value of {@link #email email}.
	 * @param email value to set.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Set the value of {@link #dateNaissance dateNaissance}.
	 * @param dateNaissance value to set.
	 */
	public void setDateNaissance(LocalDate dateNaissance) {
		this.dateNaissance = dateNaissance;
	}

	/**
	 * Set the value of {@link #adresse adresse}.
	 * @param adresse value to set.
	 */
	public void setAdresse(String adresse) {
		this.adresse = adresse;
	}

	/**
	 * Set the value of {@link #actif actif}.
	 * @param actif value to set.
	 */
	public void setActif(Boolean actif) {
		this.actif = actif;
	}

	/**
	 * Set the value of {@link #profil profil}.
	 * @param profil value to set.
	 */
	public void setProfil(Profil profil) {
		this.profil = profil;
	}

	/**
	 * Set the value of {@link #typeUtilisateur typeUtilisateur}.
	 * @param typeUtilisateur value to set.
	 */
	public void setTypeUtilisateur(TypeUtilisateur typeUtilisateur) {
		this.typeUtilisateur = typeUtilisateur;
	}

	/**
	 * Set the value of {@link #dateCreation dateCreation}.
	 * @param dateCreation value to set.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Set the value of {@link #dateModification dateModification}.
	 * @param dateModification value to set.
	 */
	public void setDateModification(LocalDateTime dateModification) {
		this.dateModification = dateModification;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur Utilisateur}.
	 */
	public enum Fields {
        ID(Integer.class), //
        NOM(String.class), //
        PRENOM(String.class), //
        EMAIL(String.class), //
        DATE_NAISSANCE(LocalDate.class), //
        ADRESSE(String.class), //
        ACTIF(Boolean.class), //
        PROFIL(Profil.class), //
        TYPE_UTILISATEUR(TypeUtilisateur.class), //
        DATE_CREATION(LocalDateTime.class), //
        DATE_MODIFICATION(LocalDateTime.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}
