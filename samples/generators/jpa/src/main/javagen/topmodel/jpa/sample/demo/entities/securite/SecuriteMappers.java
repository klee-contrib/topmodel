////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite;

import java.util.stream.Collectors;

import topmodel.jpa.sample.demo.dtos.securite.ProfilDto;

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
				target.setDroits(profil.getDroits().stream().filter(t -> t != null).map(droits -> droits.getCode()).collect(Collectors.toList()));
			}

			if (profil.getSecteurs() != null) {
				target.setSecteurs(profil.getSecteurs().stream().filter(t -> t != null).map(secteurs -> secteurs.getId()).collect(Collectors.toList()));
			}
		} else {
			throw new IllegalArgumentException("profil cannot be null");
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
		if (source.getTypeProfil() != null) {
			target.setTypeProfil(source.getTypeProfil());
		}
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
}
