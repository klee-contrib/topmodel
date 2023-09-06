////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.utilisateur;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

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
import jakarta.persistence.OneToMany;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

import topmodel.jpa.sample.demo.entities.securite.Profil;

/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR", uniqueConstraints = {
    @UniqueConstraint(columnNames = {"UTI_EMAIL","UTI_ID_PARENT"})})
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur {

	/**
	 * Id technique.
	 */
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Id
	@Column(name = "UTI_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Age en années de l'utilisateur.
	 */
	@Column(name = "UTI_AGE", nullable = true, precision = 20, scale = 9, columnDefinition = "numeric")
	private BigDecimal age = 6;

	/**
	 * Profil de l'utilisateur.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Profil.class)
	@JoinColumn(name = "PRO_ID", referencedColumnName = "PRO_ID")
	private Profil profil;

	/**
	 * Email de l'utilisateur.
	 */
	@Column(name = "UTI_EMAIL", nullable = true, length = 50, columnDefinition = "varchar")
	private String email;

	/**
	 * Nom de l'utilisateur.
	 */
	@Column(name = "UTI_NOM", nullable = true, length = 3, columnDefinition = "varchar")
	private String nom = "Jabx";

	/**
	 * Si l'utilisateur est actif.
	 */
	@Column(name = "UTI_ACTIF", nullable = true, columnDefinition = "boolean")
	private Boolean actif;

	/**
	 * Type d'utilisateur en Many to one.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeUtilisateur.class)
	@JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
	private TypeUtilisateur typeUtilisateur = new TypeUtilisateur(TypeUtilisateurCode.ADM);

	/**
	 * Utilisateur parent.
	 */
	@OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, optional = true)
	@JoinColumn(name = "UTI_ID_PARENT", referencedColumnName = "UTI_ID", unique = true)
	private Utilisateur utilisateurParent;

	/**
	 * Utilisateur enfants.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "utilisateurEnfant")
	private List<Utilisateur> utilisateursEnfant;

	/**
	 * Date de création de l'utilisateur.
	 */
	@Column(name = "UTI_DATE_CREATION", nullable = true, columnDefinition = "date")
	@CreatedDate
	private LocalDate dateCreation;

	/**
	 * Date de modification de l'utilisateur.
	 */
	@Column(name = "UTI_DATE_MODIFICATION", nullable = true, columnDefinition = "date")
	@LastModifiedDate
	private LocalDateTime dateModification;

	/**
	 * Association réciproque de Utilisateur.UtilisateursEnfant.
	 */
	@ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = Utilisateur.class)
	@JoinColumn(name = "UTI_ID_ENFANT", referencedColumnName = "UTI_ID")
	private Utilisateur utilisateurEnfant;

	/**
	 * No arg constructor.
	 */
	public Utilisateur() {
	}

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#id id}.
	 */
	public Integer getId() {
		return this.id;
	}

	/**
	 * Getter for age.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#age age}.
	 */
	public BigDecimal getAge() {
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
	 * Getter for actif.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#actif actif}.
	 */
	public Boolean getActif() {
		return this.actif;
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
	 * Getter for utilisateursEnfant.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#utilisateursEnfant utilisateursEnfant}.
	 */
	public List<Utilisateur> getUtilisateursEnfant() {
		if(this.utilisateursEnfant == null)
			this.utilisateursEnfant = new ArrayList<>();
		return this.utilisateursEnfant;
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
	 * Getter for utilisateurEnfant.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#utilisateurEnfant utilisateurEnfant}.
	 */
	public Utilisateur getUtilisateurEnfant() {
		return this.utilisateurEnfant;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#id id}.
	 * @param id value to set
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#age age}.
	 * @param age value to set
	 */
	public void setAge(BigDecimal age) {
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
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#actif actif}.
	 * @param actif value to set
	 */
	public void setActif(Boolean actif) {
		this.actif = actif;
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
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#utilisateursEnfant utilisateursEnfant}.
	 * @param utilisateursEnfant value to set
	 */
	public void setUtilisateursEnfant(List<Utilisateur> utilisateursEnfant) {
		this.utilisateursEnfant = utilisateursEnfant;
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
	 * Set the value of {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur#utilisateurEnfant utilisateurEnfant}.
	 * @param utilisateurEnfant value to set
	 */
	public void setUtilisateurEnfant(Utilisateur utilisateurEnfant) {
		this.utilisateurEnfant = utilisateurEnfant;
	}

	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur Utilisateur}.
	 */
	public enum Fields  {
        ID(Integer.class), //
        AGE(BigDecimal.class), //
        PROFIL(Profil.class), //
        EMAIL(String.class), //
        NOM(String.class), //
        ACTIF(Boolean.class), //
        TYPE_UTILISATEUR(TypeUtilisateur.class), //
        UTILISATEUR_PARENT(Utilisateur.class), //
        UTILISATEURS_ENFANT(List.class), //
        DATE_CREATION(LocalDate.class), //
        DATE_MODIFICATION(LocalDateTime.class), //
        UTILISATEUR_ENFANT(Utilisateur.class);

		private Class<?> type;

		private Fields(Class<?> type) {
			this.type = type;
		}

		public Class<?> getType() {
			return this.type;
		}
	}
}
