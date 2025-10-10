////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite.profil;

import jakarta.annotation.Generated;

/**
 * Détail d'un profil en liste.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface ProfilItem {

	/**
	 * Getter for id.
	 */
	Integer getId();

	/**
	 * Getter for libelle.
	 */
	String getLibelle();

	/**
	 * Getter for nombreUtilisateurs.
	 */
	Long getNombreUtilisateurs();

	/**
	 * Hydrate values of instance.
	 * @param id value to set.
	 * @param libelle value to set.
	 * @param nombreUtilisateurs value to set.
	 */
	void hydrate(Integer id, String libelle, Long nombreUtilisateurs);
}
