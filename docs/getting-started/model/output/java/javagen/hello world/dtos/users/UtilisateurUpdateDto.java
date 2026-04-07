////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.dtos.users;

import java.io.Serial;
import java.io.Serializable;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

/**
 * Objet de transfert pour la classe Utilisateur, dans le cas de la modification de celui-ci.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurUpdateDto implements Serializable {

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
	private String nom;

	/**
	 * Getter for nom.
	 *
	 * @return value of {@link #nom nom}.
	 */
	public String getNom() {
		return this.nom;
	}

	/**
	 * Set the value of {@link #nom nom}.
	 * @param nom value to set.
	 */
	public void setNom(String nom) {
		this.nom = nom;
	}
}
