////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.profil;

import java.util.Objects;
import java.util.stream.Collectors;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilWrite;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class SecuriteProfilMappers {

	private SecuriteProfilMappers() {
		// private constructor to hide implicite public one
	}

	/**
	 * Mapper les champs sources sur une nouvelle instance de la classe.
	 * @param profil Instance de 'Profil'.
	 *
	 * @return Une nouvelle instance de 'ProfilRead' sur laquelle les champs sources ont été mappée.
	 */
	public static ProfilRead createProfilRead(Profil profil) {
		return createProfilRead(profil, new ProfilRead());
	}

	/**
	 * Mapper les champs sources sur une nouvelle instance de la classe ou bien sur l'instance passée en paramètres.
	 * @param profil Instance de 'Profil'.
	 *
	 * @return Une nouvelle instance de 'ProfilRead' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée.
	 */
	public static ProfilRead createProfilRead(Profil profil, ProfilRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (profil == null) {
			throw new IllegalArgumentException("profil cannot be null");
		}

		target.setId(profil.getId());
		target.setLibelle(profil.getLibelle());
		if (profil.getDroits() != null) {
			target.setDroits(profil.getDroits().stream().filter(Objects::nonNull).map(Droit::getCode).collect(Collectors.toList()));
		}

		target.setDateCreation(profil.getDateCreation());
		target.setDateModification(profil.getDateModification());
		return target;
	}

	/**
	 * Mappe 'Profil' vers une nouvelle instance de 'Profil'.
	 * @param source Instance de 'Profil' à mapper.
	 *
	 * @return Nouvelle instance de 'Profil' mappée depuis 'profil'.
	 */
	public static Profil toProfil(Profil source) {
			return toProfil(source, new Profil());
	}

	/**
	 * Mappe 'Profil' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'Profil' à mapper.
	 * @param target Instance de 'Profil' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'profil'.
	 */
	public static Profil toProfil(Profil source, Profil target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setLibelle(source.getLibelle());
		target.setDroits(source.getDroits());
		return target;
	}

	/**
	 * Mappe 'Profil' vers une nouvelle instance de 'ProfilWrite'.
	 * @param source Instance de 'ProfilWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'ProfilWrite' mappée depuis 'profil'.
	 */
	public static Profil toProfil(ProfilWrite source) {
			return toProfil(source, new Profil());
	}

	/**
	 * Mappe 'Profil' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'ProfilWrite' à mapper.
	 * @param target Instance de 'Profil' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'profil'.
	 */
	public static Profil toProfil(ProfilWrite source, Profil target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setLibelle(source.getLibelle());
		if (source.getDroits() != null) {
			target.setDroits(source.getDroits().stream().map(Droit::new).collect(Collectors.toList()));
		}

		return target;
	}
}
