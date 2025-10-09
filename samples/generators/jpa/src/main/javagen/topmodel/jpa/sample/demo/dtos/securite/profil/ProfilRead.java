////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite.profil;

import java.io.Serial;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurItem;
import topmodel.jpa.sample.demo.entities.securite.profil.Profil;
import topmodel.jpa.sample.demo.entities.securite.profil.SecuriteProfilMappers;
import topmodel.jpa.sample.demo.enums.securite.profil.DroitCode;

/**
 * Détail d'un profil en lecture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ProfilRead implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Id technique.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.profil.Profil#getId() Profil#getId()}
	 */
	@NotNull
	private Integer id;

	/**
	 * Libellé du profil.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.profil.Profil#getLibelle() Profil#getLibelle()}
	 */
	@NotNull
	@Size(max = 100)
	private String libelle;

	/**
	 * Liste des droits du profil.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.profil.ProfilDroit#getDroit() ProfilDroit#getDroit()}
	 */
	@Size(max = 10)
	private List<DroitCode> droits;

	/**
	 * Date de création de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.profil.Profil#getDateCreation() Profil#getDateCreation()}
	 */
	@NotNull
	private LocalDateTime dateCreation;

	/**
	 * Date de modification de l'utilisateur.
	 * Alias of {@link topmodel.jpa.sample.demo.entities.securite.profil.Profil#getDateModification() Profil#getDateModification()}
	 */
	private LocalDateTime dateModification;

	/**
	 * Utilisateurs ayant ce profil.
	 */
	@Valid
	@NotNull
	private List<UtilisateurItem> utilisateurs;

	/**
	 * No arg constructor.
	 */
	public ProfilRead() {
		// No arg constructor
	}

	/**
	 * Crée une nouvelle instance de 'ProfilRead'.
	 * @param profil Instance de 'Profil'.
	 *
	 * @return Une nouvelle instance de 'ProfilRead'.
	 */
	public ProfilRead(Profil profil) {
		SecuriteProfilMappers.mapProfilRead(profil, this);
	}

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
	 * Getter for droits.
	 *
	 * @return value of {@link #droits droits}.
	 */
	public List<DroitCode> getDroits() {
		return this.droits;
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
	 * Getter for utilisateurs.
	 *
	 * @return value of {@link #utilisateurs utilisateurs}.
	 */
	public List<UtilisateurItem> getUtilisateurs() {
		return this.utilisateurs;
	}

	/**
	 * Setter for id.
	 */
	public void setId(Integer id) {
		this.id = id;
	}

	/**
	 * Setter for libelle.
	 */
	public void setLibelle(String libelle) {
		this.libelle = libelle;
	}

	/**
	 * Setter for droits.
	 */
	public void setDroits(List<DroitCode> droits) {
		this.droits = droits;
	}

	/**
	 * Setter for dateCreation.
	 */
	public void setDateCreation(LocalDateTime dateCreation) {
		this.dateCreation = dateCreation;
	}

	/**
	 * Setter for dateModification.
	 */
	public void setDateModification(LocalDateTime dateModification) {
		this.dateModification = dateModification;
	}

	/**
	 * Setter for utilisateurs.
	 */
	public void setUtilisateurs(List<UtilisateurItem> utilisateurs) {
		this.utilisateurs = utilisateurs;
	}


	/**
	 * Enumération des champs de la classe {@link topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead ProfilRead}.
	 */
	public enum Fields {
		ID(Integer.class),
		LIBELLE(String.class),
		DROITS(List.class),
		DATE_CREATION(LocalDateTime.class),
		DATE_MODIFICATION(LocalDateTime.class),
		UTILISATEURS(List.class);

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
