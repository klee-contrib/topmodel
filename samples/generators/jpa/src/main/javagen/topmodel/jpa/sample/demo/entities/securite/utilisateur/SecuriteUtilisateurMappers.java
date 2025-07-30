////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.entities.securite.utilisateur;

import jakarta.annotation.Generated;

import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurRead;
import topmodel.jpa.sample.demo.dtos.securite.utilisateur.UtilisateurWrite;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class SecuriteUtilisateurMappers {

	private SecuriteUtilisateurMappers() {
		// private constructor to hide implicite public one
	}

	/**
	 * Crée une nouvelle instance de la classe 'UtilisateurRead' en mappant les champs sources.
	 * @param utilisateur Instance de 'Utilisateur' source.
	 *
	 * @return Une nouvelle instance de 'UtilisateurRead' sur laquelle les champs sources ont été mappés.
	 */
	public static UtilisateurRead createUtilisateurRead(Utilisateur utilisateur) {
		return mapUtilisateurRead(utilisateur, new UtilisateurRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'UtilisateurRead' passée en paramètre.
	 * @param utilisateur Instance de 'Utilisateur' source.
	 * @param target Instance de 'UtilisateurRead' cible.
	 *
	 * @return L'instance de 'UtilisateurRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static UtilisateurRead mapUtilisateurRead(Utilisateur utilisateur, UtilisateurRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (utilisateur == null) {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}

		target.setId(utilisateur.getId());
		target.setNom(utilisateur.getNom());
		target.setPrenom(utilisateur.getPrenom());
		target.setEmail(utilisateur.getEmail());
		target.setDateNaissance(utilisateur.getDateNaissance());
		target.setAdresse(utilisateur.getAdresse());
		target.setActif(utilisateur.getActif());
		if (utilisateur.getProfil() != null) {
			target.setProfilId(utilisateur.getProfil().getId());
		} else {
			target.setProfilId(null);
		}

		if (utilisateur.getTypeUtilisateur() != null) {
			target.setTypeUtilisateurCode(utilisateur.getTypeUtilisateur().getCode());
		} else {
			target.setTypeUtilisateurCode(null);
		}

		target.setDateCreation(utilisateur.getDateCreation());
		target.setDateModification(utilisateur.getDateModification());
		return target;
	}

	/**
	 * Mappe 'Utilisateur' vers une nouvelle instance de 'UtilisateurWrite'.
	 * @param source Instance de 'UtilisateurWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'UtilisateurWrite' mappée depuis 'utilisateur'.
	 */
	public static Utilisateur toUtilisateur(UtilisateurWrite source) {
			return toUtilisateur(source, new Utilisateur());
	}

	/**
	 * Mappe 'Utilisateur' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'UtilisateurWrite' à mapper.
	 * @param target Instance de 'Utilisateur' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'utilisateur'.
	 */
	public static Utilisateur toUtilisateur(UtilisateurWrite source, Utilisateur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNom(source.getNom());
		target.setPrenom(source.getPrenom());
		target.setEmail(source.getEmail());
		target.setDateNaissance(source.getDateNaissance());
		target.setAdresse(source.getAdresse());
		target.setActif(source.getActif());
		if (source.getTypeUtilisateurCode() != null) {
			target.setTypeUtilisateur(new TypeUtilisateur(source.getTypeUtilisateurCode()));
		} else {
			target.setTypeUtilisateur(null);
		}

		return target;
	}
}
