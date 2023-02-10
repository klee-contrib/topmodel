////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.mappers;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;
import topmodel.jpa.sample.demo.entities.utilisateur.Utilisateur;

public static class UtilisateurMappers {

	/**
	 * Map les champs des classes passées en paramètre dans l'objet target'.
	 * @param target Instance de 'UtilisateurDto'.
	 * @param utilisateur Instance de 'Utilisateur'.
	 */
	public static void map(UtilisateurDto target, Utilisateur utilisateur) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (utilisateur != null) {
			target.utilisateurParent = utilisateur.getUtilisateurParent() == null ? null : new UtilisateurDto(utilisateur.getUtilisateurParent());
			target.id = utilisateur.getId();
			target.age = utilisateur.getAge();
			if (utilisateur.getProfil() != null) {
				target.setProfilId(utilisateur.getProfil().getId());
			}

			target.email = utilisateur.getEmail();
			target.nom = utilisateur.getNom();
			if (utilisateur.getTypeUtilisateur() != null) {
				target.setTypeUtilisateurCode(utilisateur.getTypeUtilisateur().getCode());
			}

			target.dateCreation = utilisateur.getDateCreation();
			target.dateModification = utilisateur.getDateModification();
		} else {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}
	}

	/**
	 * Mappe 'UtilisateurDto' vers 'Utilisateur'.
	 * @param source Instance de 'UtilisateurDto'.
	 * @param target Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 */
	public void toUtilisateur(UtilisateurDto source, Utilisateur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (source.getUtilisateurParent() != null) {
			target.setUtilisateurParent(source.getUtilisateurParent().toUtilisateur(target.getUtilisateurParent()));
		}

		target.setId(source.getId());
		target.setAge(source.getAge());
		target.setEmail(source.getEmail());
		target.setNom(source.getNom());
		if (source.getTypeUtilisateurCode() != null) {
			target.setTypeUtilisateur(source.getTypeUtilisateurCode().getEntity());
		}
		target.setDateCreation(source.getDateCreation());
		target.setDateModification(source.getDateModification());

		return target;
	}
}
