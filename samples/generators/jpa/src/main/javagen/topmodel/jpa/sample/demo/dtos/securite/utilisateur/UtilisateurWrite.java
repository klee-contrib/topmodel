////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite.utilisateur;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDate;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

import topmodel.jpa.sample.demo.entities.securite.utilisateur.SecuriteUtilisateurMappers;
import topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur;
import topmodel.jpa.sample.demo.enums.securite.utilisateur.TypeUtilisateurCode;

/**
 * Détail d'un utilisateur en écriture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurWrite implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getNom() Utilisateur#getNom()}
	 */
	@NotNull
	@Size(max = 100)
	private String nom;

	/**
	 * Nom de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getPrenom() Utilisateur#getPrenom()}
	 */
	@NotNull
	@Size(max = 100)
	private String prenom;

	/**
	 * Email de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getEmail() Utilisateur#getEmail()}
	 */
	@Email
	@NotNull
	@Size(max = 50)
	private String email;

	/**
	 * Age de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getDateNaissance() Utilisateur#getDateNaissance()}
	 */
	private LocalDate dateNaissance;

	/**
	 * Adresse de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getAdresse() Utilisateur#getAdresse()}
	 */
	@Size(max = 100)
	private String adresse;

	/**
	 * Si l'utilisateur est actif.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getActif() Utilisateur#getActif()}
	 */
	@NotNull
	private Boolean actif = true;

	/**
	 * Profil de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getProfil() Utilisateur#getProfil()}
	 */
	@NotNull
	private Integer profilId;

	/**
	 * Type d'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur#getTypeUtilisateur() Utilisateur#getTypeUtilisateur()}
	 */
	@NotNull
	private TypeUtilisateurCode typeUtilisateurCode = TypeUtilisateurCode.GEST;

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
	 * Getter for profilId.
	 *
	 * @return value of {@link #profilId profilId}.
	 */
	public Integer getProfilId() {
		return this.profilId;
	}

	/**
	 * Getter for typeUtilisateurCode.
	 *
	 * @return value of {@link #typeUtilisateurCode typeUtilisateurCode}.
	 */
	public TypeUtilisateurCode getTypeUtilisateurCode() {
		return this.typeUtilisateurCode;
	}

	/**
	 * Setter for nom.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}

	/**
	 * Setter for prenom.
	 */
	public void setPrenom(String prenom) {
		this.prenom = prenom;
	}

	/**
	 * Setter for email.
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Setter for dateNaissance.
	 */
	public void setDateNaissance(LocalDate dateNaissance) {
		this.dateNaissance = dateNaissance;
	}

	/**
	 * Setter for adresse.
	 */
	public void setAdresse(String adresse) {
		this.adresse = adresse;
	}

	/**
	 * Setter for actif.
	 */
	public void setActif(Boolean actif) {
		this.actif = actif;
	}

	/**
	 * Setter for profilId.
	 */
	public void setProfilId(Integer profilId) {
		this.profilId = profilId;
	}

	/**
	 * Setter for typeUtilisateurCode.
	 */
	public void setTypeUtilisateurCode(TypeUtilisateurCode typeUtilisateurCode) {
		this.typeUtilisateurCode = typeUtilisateurCode;
	}

	/**
	 * Mappe 'UtilisateurWrite' vers 'Utilisateur'.
	 * @param target Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Utilisateur'.
	 */
	public Utilisateur toUtilisateur(Utilisateur target) {
		return SecuriteUtilisateurMappers.toUtilisateur(this, target);
	}


	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurWrite UtilisateurWrite}.
	 */
	public enum Fields {
		NOM(String.class),
		PRENOM(String.class),
		EMAIL(String.class),
		DATE_NAISSANCE(LocalDate.class),
		ADRESSE(String.class),
		ACTIF(Boolean.class),
		PROFIL_ID(Integer.class),
		TYPE_UTILISATEUR_CODE(TypeUtilisateurCode.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}
