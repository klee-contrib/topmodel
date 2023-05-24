////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.dtos.utilisateur;

import jakarta.annotation.Generated;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDTOMappers {

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
		target.setActif(source.getActif());
		target.setTypeUtilisateurCode(source.getTypeUtilisateurCode());
		target.setUtilisateursEnfant(source.getUtilisateursEnfant());
		target.setDateCreation(source.getDateCreation());
		target.setDateModification(source.getDateModification());
		return target;
	}
}
