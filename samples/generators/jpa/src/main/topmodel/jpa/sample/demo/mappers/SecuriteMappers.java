////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.mappers;

import java.util.stream.Collectors;

import topmodel.jpa.sample.demo.dtos.securite.ProfilDto;
import topmodel.jpa.sample.demo.entities.securite.Profil;

public static class SecuriteMappers {

	/**
	 * Map les champs des classes passées en paramètre dans l'objet target'.
	 * @param target Instance de 'ProfilDto'.
	 * @param profil Instance de 'Profil'.
	 */
	public static void map(ProfilDto target, Profil profil) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (profil != null) {
			target.id = profil.getId();
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
	}

	/**
	 * Mappe 'Profil' vers 'Profil'.
	 * @param source Instance de 'Profil'.
	 * @param target Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.
	 */
	public void toProfil(Profil source, Profil target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
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
	 */
	public void toProfil(ProfilDto source, Profil target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setId(source.getId());
		if (source.getTypeProfilCode() != null) {
			target.setTypeProfil(source.getTypeProfilCode().getEntity());
		}
		target.setDroits(source.getDroits().stream().map(Droit.Values::getEntity).collect(Collectors.toList()));

		return target;
	}
}
