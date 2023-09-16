////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.securite.profil;

import jakarta.annotation.Generated;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface ProfilItem {

	/**
	 * Getter for id.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.profil.ProfilItem#id id}.
	 */
	Integer getId();

	/**
	 * Getter for libelle.
	 *
	 * @return value of {@link topmodel.jpa.sample.demo.dtos.securite.profil.ProfilItem#libelle libelle}.
	 */
	String getLibelle();

	/**
	 * hydrate values of instance.
	 * @param id value to set
	 * @param libelle value to set
	 */
	void hydrate(Integer id, String libelle);
}
