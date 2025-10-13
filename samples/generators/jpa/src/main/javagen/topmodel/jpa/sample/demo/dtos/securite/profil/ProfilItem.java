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
	 * Id technique.
	 */
	Integer getId();

	/**
	 * Libellé du profil.
	 */
	String getLibelle();

	/**
	 * Nombre d'utilisateurs affectés au profil.
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
