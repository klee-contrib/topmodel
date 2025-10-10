////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.profil;

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
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

import topmodel.jpa.sample.demo.entities.securite.utilisateur.Utilisateur;

/**
 * Profil des utilisateurs.
 */
@Entity
@Table(name = "PROFIL")
@EntityListeners(AuditingEntityListener.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Profil {

	/**
	 * Id technique.
	 */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "PRO_ID", nullable = false, columnDefinition = "int")
	private Integer id;

	/**
	 * Libellé du profil.
	 */
	@Column(name = "PRO_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
	private String libelle;

	/**
	 * Liste des droits du profil.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "profil")
	private List<ProfilDroit> profilDroits;

	/**
	 * Association réciproque de Utilisateur.ProfilId.
	 */
	@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "profil")
	private List<Utilisateur> utilisateurs;

	/**
	 * Date de création de l'utilisateur.
	 */
	@CreatedDate
	@Column(name = "PRO_DATE_CREATION", nullable = false, columnDefinition = "date")
	private LocalDateTime dateCreation = LocalDateTime.now();

	/**
	 * Date de modification de l'utilisateur.
	 */
	@LastModifiedDate
	@Column(name = "PRO_DATE_MODIFICATION", columnDefinition = "date")
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
	 * Getter for libelle.
	 *
	 * @return value of {@link #libelle libelle}.
	 */
	public String getLibelle() {
		return this.libelle;
	}

	/**
	 * Getter for profilDroits.
	 *
	 * @return value of {@link #profilDroits profilDroits}.
	 */
	public List<ProfilDroit> getProfilDroits() {
		if (this.profilDroits == null) {
			this.profilDroits = new ArrayList<>();
		}
		return this.profilDroits;
	}

	/**
	 * Getter for utilisateurs.
	 *
	 * @return value of {@link #utilisateurs utilisateurs}.
	 */
	public List<Utilisateur> getUtilisateurs() {
		if (this.utilisateurs == null) {
			this.utilisateurs = new ArrayList<>();
		}
		return this.utilisateurs;
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
	 * Set the value of {@link #libelle libelle}.
	 * @param libelle value to set.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Set the value of {@link #profilDroits profilDroits}.
	 * @param profilDroits value to set.
	 */
	public void setProfilDroits(List<ProfilDroit> profilDroits) {
		this.profilDroits = profilDroits;
	}

	/**
	 * Set the value of {@link #utilisateurs utilisateurs}.
	 * @param utilisateurs value to set.
	 */
	public void setUtilisateurs(List<Utilisateur> utilisateurs) {
		this.utilisateurs = utilisateurs;
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
	 * Mappe 'Profil' vers 'Profil'.
	 * @param target Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une instance de 'Profil'.
	 */
	public Profil toProfil(Profil target) {
		return SecuriteProfilMappers.toProfil(this, target);
	}


	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.entities.securite.profil.Profil Profil}.
	 */
	public enum Fields {
		ID(Integer.class),
		LIBELLE(String.class),
		PROFIL_DROITS(List.class),
		UTILISATEURS(List.class),
		DATE_CREATION(LocalDateTime.class),
		DATE_MODIFICATION(LocalDateTime.class);

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
