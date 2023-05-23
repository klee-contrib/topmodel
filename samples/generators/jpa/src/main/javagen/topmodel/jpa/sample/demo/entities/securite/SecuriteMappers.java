////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite;

import java.util.Objects;
import java.util.stream.Collectors;

import topmodel.jpa.sample.demo.dtos.securite.ProfilDto;
import topmodel.jpa.sample.demo.dtos.securite.SecteurDto;

public class SecuriteMappers {

	/**
	 * Map les champs des classes passées en paramètre dans l'objet target'.
	 * @param target Instance de 'ProfilDto' (ou null pour créer une nouvelle instance).
	 * @param profil Instance de 'Profil'.
	 *
	 * @return Une nouvelle instance de 'ProfilDto' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée.
	 */
	public static ProfilDto createProfilDto(Profil profil, ProfilDto target) {
		if (target == null) {
			target = new ProfilDto();
		}

		if (profil != null) {
			target.setId(profil.getId());
			if (profil.getTypeProfil() != null) {
				target.setTypeProfilCode(profil.getTypeProfil().getCode());
			}

			if (profil.getDroits() != null) {
				target.setDroits(profil.getDroits().stream().filter(Objects::nonNull).map(Droit::getCode).collect(Collectors.toList()));
			}

		} else {
			throw new IllegalArgumentException("profil cannot be null");
		}
		return target;
	}

	/**
	 * Map les champs des classes passées en paramètre dans l'objet target'.
	 * @param target Instance de 'SecteurDto' (ou null pour créer une nouvelle instance).
	 * @param secteur Instance de 'Secteur'.
	 *
	 * @return Une nouvelle instance de 'SecteurDto' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée.
	 */
	public static SecteurDto createSecteurDto(Secteur secteur, SecteurDto target) {
		if (target == null) {
			target = new SecteurDto();
		}

		if (secteur != null) {
			target.setId(secteur.getId());
		} else {
			throw new IllegalArgumentException("secteur cannot be null");
		}
		return target;
	}

	/**
	 * Mappe 'Profil' vers 'Profil'.
	 * @param source Instance de 'Profil'.
	 * @param target Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une nouvelle instance de 'Profil' ou bien l'instance passée en paramètre dont les champs ont été surchargés.
	 */
	public static Profil toProfil(Profil source, Profil target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			target = new Profil();
		}

		target.setId(source.getId());
		target.setTypeProfil(source.getTypeProfil());
		target.setDroits(source.getDroits());
		target.setSecteurs(source.getSecteurs());
		return target;
	}

	/**
	 * Mappe 'ProfilDto' vers 'Profil'.
	 * @param source Instance de 'ProfilDto'.
	 * @param target Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une nouvelle instance de 'Profil' ou bien l'instance passée en paramètre dont les champs ont été surchargés.
	 */
	public static Profil toProfil(ProfilDto source, Profil target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			target = new Profil();
		}

		target.setId(source.getId());
		if (source.getTypeProfilCode() != null) {
			target.setTypeProfil(source.getTypeProfilCode().getEntity());
		}

		target.setDroits(source.getDroits().stream().map(Droit.Values::getEntity).collect(Collectors.toList()));
		return target;
	}

	/**
	 * Mappe 'SecteurDto' vers 'Secteur'.
	 * @param source Instance de 'SecteurDto'.
	 * @param target Instance pré-existante de 'Secteur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une nouvelle instance de 'Secteur' ou bien l'instance passée en paramètre dont les champs ont été surchargés.
	 */
	public static Secteur toSecteur(SecteurDto source, Secteur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			target = new Secteur();
		}

		target.setId(source.getId());
		return target;
	}
}
