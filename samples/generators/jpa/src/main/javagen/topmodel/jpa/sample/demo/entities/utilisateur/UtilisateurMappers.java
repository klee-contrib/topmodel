////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.utilisateur;

import java.util.Objects;
import java.util.stream.Collectors;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.utilisateur.UtilisateurDto;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurMappers {

	/**
	 * Map les champs des classes passées en paramètre dans l'objet target'.
	 * @param target Instance de 'UtilisateurDto' (ou null pour créer une nouvelle instance).
	 * @param utilisateur Instance de 'Utilisateur'.
	 *
	 * @return Une nouvelle instance de 'UtilisateurDto' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée.
	 */
	public static UtilisateurDto createUtilisateurDto(Utilisateur utilisateur, UtilisateurDto target) {
		if (target == null) {
			target = new UtilisateurDto();
		}

		if (utilisateur != null) {
			if (utilisateur.getUtilisateurParent() != null) {
				target.setUtilisateurParent(UtilisateurMappers.createUtilisateurDto(utilisateur.getUtilisateurParent(), target.getUtilisateurParent()));
			}

			target.setId(utilisateur.getId());
			target.setAge(utilisateur.getAge());
			if (utilisateur.getProfil() != null) {
				target.setProfilId(utilisateur.getProfil().getId());
			}

			target.setEmail(utilisateur.getEmail());
			target.setNom(utilisateur.getNom());
			target.setActif(utilisateur.getActif());
			if (utilisateur.getTypeUtilisateur() != null) {
				target.setTypeUtilisateurCode(utilisateur.getTypeUtilisateur().getCode());
			}

			if (utilisateur.getUtilisateursEnfant() != null) {
				target.setUtilisateursEnfant(utilisateur.getUtilisateursEnfant().stream().filter(Objects::nonNull).map(Utilisateur::getId).collect(Collectors.toList()));
			}

			target.setDateCreation(utilisateur.getDateCreation());
			target.setDateModification(utilisateur.getDateModification());
		} else {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}
		return target;
	}

	/**
	 * Mappe 'UtilisateurDto' vers 'Utilisateur'.
	 * @param source Instance de 'UtilisateurDto'.
	 * @param target Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.
	 *
	 * @return Une nouvelle instance de 'Utilisateur' ou bien l'instance passée en paramètre dont les champs ont été surchargés.
	 */
	public static Utilisateur toUtilisateur(UtilisateurDto source, Utilisateur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			target = new Utilisateur();
		}

		target.setId(source.getId());
		target.setAge(source.getAge());
		target.setEmail(source.getEmail());
		target.setNom(source.getNom());
		target.setActif(source.getActif());
		if (source.getTypeUtilisateurCode() != null) {
			target.setTypeUtilisateur(source.getTypeUtilisateurCode().getEntity());
		}

		target.setDateCreation(source.getDateCreation());
		target.setDateModification(source.getDateModification());
		if (source.getUtilisateurParent() != null) {
			target.setUtilisateurParent(UtilisateurMappers.toUtilisateur(source.getUtilisateurParent(), target.getUtilisateurParent()));
		}

		return target;
	}
}
