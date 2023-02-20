////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur;

public class UtilisateurDTOMappers {

	/**
	 * Mappe 'UtilisateurDto' vers 'IUtilisateur'.
	 * @param source Instance de 'UtilisateurDto'.
	 * @param target Instance pré-existante de 'IUtilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une nouvelle instance de 'IUtilisateur' ou bien l'instance passée en paramètre dont les champs ont été surchargés.
	 */
	public static IUtilisateur toIUtilisateur(UtilisateurDto source, IUtilisateur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.hydrate(source.getId(), source.getAge(), source.getProfilId(), source.getEmail(), source.getNom(), source.getTypeUtilisateurCode(), source.getDateCreation(), source.getDateModification());
		return target;
	}

	/**
	 * Mappe 'UtilisateurDto' vers 'UtilisateurDto'.
	 * @param source Instance de 'UtilisateurDto'.
	 * @param target Instance pré-existante de 'UtilisateurDto'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une nouvelle instance de 'UtilisateurDto' ou bien l'instance passée en paramètre dont les champs ont été surchargés.
	 */
	public static UtilisateurDto toUtilisateurDto(UtilisateurDto source, UtilisateurDto target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			target = new UtilisateurDto();
		}

		target.setId(source.getId());
		target.setAge(source.getAge());
		target.setProfilId(source.getProfilId());
		target.setEmail(source.getEmail());
		target.setNom(source.getNom());
		target.setTypeUtilisateurCode(source.getTypeUtilisateurCode());
		target.setDateCreation(source.getDateCreation());
		target.setDateModification(source.getDateModification());
		return target;
	}
}
