////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.dtos.users;

import java.io.Serial;
import java.io.Serializable;

import org.springframework.data.domain.Page;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;
import jakarta.validation.Valid;

/**
 * Objet de transfert pour la classe Utilisateur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable {

	/**
	 * Serial ID.
	 */
	@Serial
	private static final long serialVersionUID = 1L;

	/**
	 * Nom de l'utilisateur.
	 * Alias of {@link hello world.entities.users.Utilisateur#getNom() Utilisateur#getNom()}
	 */
	@Size(max = 15)
	private String nomUtilisateur;

	/**
	 * Adresse de l'utilisateur.
	 */
	@Valid
	@NotNull
	private Page<AdresseDto> adresse;

	/**
	 * Getter for nomUtilisateur.
	 *
	 * @return value of {@link #nomUtilisateur nomUtilisateur}.
	 */
	public String getNomUtilisateur() {
		return this.nomUtilisateur;
	}

	/**
	 * Getter for adresse.
	 *
	 * @return value of {@link #adresse adresse}.
	 */
	public Page<AdresseDto> getAdresse() {
		return this.adresse;
	}

	/**
	 * Set the value of {@link #nomUtilisateur nomUtilisateur}.
	 * @param nomUtilisateur value to set.
	 */
	public void setNomUtilisateur(String nomUtilisateur) {
		this.nomUtilisateur = nomUtilisateur;
	}

	/**
	 * Set the value of {@link #adresse adresse}.
	 * @param adresse value to set.
	 */
	public void setAdresse(Page<AdresseDto> adresse) {
		this.adresse = adresse;
	}
}
