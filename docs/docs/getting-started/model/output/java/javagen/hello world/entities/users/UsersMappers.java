////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package hello world.entities.users;

import hello world.dtos.users.UtilisateurCreateDto;
import hello world.dtos.users.UtilisateurDetailDto;
import hello world.dtos.users.UtilisateurSearchResultDto;
import hello world.dtos.users.UtilisateurUpdateDto;
import hello world.entities.refs.TypeUtilisateur;

import jakarta.annotation.Generated;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UsersMappers {

	private UsersMappers() {
		// private constructor to hide implicite public one
	}

	/**
	 * Crée une nouvelle instance de la classe 'UtilisateurDetailDto' en mappant les champs sources.
	 * @param utilisateur Instance de 'Utilisateur' source.
	 * @param typeUtilisateur Instance de 'TypeUtilisateur' source.
	 *
	 * @return Une nouvelle instance de 'UtilisateurDetailDto' sur laquelle les champs sources ont été mappés.
	 */
	public static UtilisateurDetailDto createUtilisateurDetailDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur) {
		return mapUtilisateurDetailDto(utilisateur, typeUtilisateur, new UtilisateurDetailDto());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'UtilisateurDetailDto' passée en paramètre.
	 * @param utilisateur Instance de 'Utilisateur' source.
	 * @param typeUtilisateur Instance de 'TypeUtilisateur' source.
	 * @param target Instance de 'UtilisateurDetailDto' cible.
	 *
	 * @return L'instance de 'UtilisateurDetailDto' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static UtilisateurDetailDto mapUtilisateurDetailDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur, UtilisateurDetailDto target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (utilisateur == null) {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}

		if (typeUtilisateur == null) {
			throw new IllegalArgumentException("typeUtilisateur cannot be null");
		}

		target.setEmail(utilisateur.getEmail());
		target.setNom(utilisateur.getNom());
		target.setDateInscription(utilisateur.getDateInscription());
		target.setTypeUtilisateurCode(utilisateur.getTypeUtilisateurCode());
		target.setLibelleTypeUtilisateur(typeUtilisateur.getLibelle());
		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'UtilisateurSearchResultDto' en mappant les champs sources.
	 * @param utilisateur Instance de 'Utilisateur' source.
	 * @param typeUtilisateur Instance de 'TypeUtilisateur' source.
	 *
	 * @return Une nouvelle instance de 'UtilisateurSearchResultDto' sur laquelle les champs sources ont été mappés.
	 */
	public static UtilisateurSearchResultDto createUtilisateurSearchResultDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur) {
		return mapUtilisateurSearchResultDto(utilisateur, typeUtilisateur, new UtilisateurSearchResultDto());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'UtilisateurSearchResultDto' passée en paramètre.
	 * @param utilisateur Instance de 'Utilisateur' source.
	 * @param typeUtilisateur Instance de 'TypeUtilisateur' source.
	 * @param target Instance de 'UtilisateurSearchResultDto' cible.
	 *
	 * @return L'instance de 'UtilisateurSearchResultDto' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static UtilisateurSearchResultDto mapUtilisateurSearchResultDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur, UtilisateurSearchResultDto target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (utilisateur == null) {
			throw new IllegalArgumentException("utilisateur cannot be null");
		}

		if (typeUtilisateur == null) {
			throw new IllegalArgumentException("typeUtilisateur cannot be null");
		}

		target.setEmail(utilisateur.getEmail());
		target.setNom(utilisateur.getNom());
		target.setDateInscription(utilisateur.getDateInscription());
		target.setTypeUtilisateurCode(utilisateur.getTypeUtilisateurCode());
		target.setLibelleTypeUtilisateur(typeUtilisateur.getLibelle());
		return target;
	}

	/**
	 * Mappe 'Utilisateur' vers une nouvelle instance de 'UtilisateurCreateDto'.
	 * @param source Instance de 'UtilisateurCreateDto' à mapper.
	 *
	 * @return Nouvelle instance de 'UtilisateurCreateDto' mappée depuis 'utilisateur'.
	 */
	public static Utilisateur toUtilisateur(UtilisateurCreateDto source) {
		return toUtilisateur(source, new Utilisateur());
	}

	/**
	 * Mappe 'Utilisateur' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'UtilisateurCreateDto' à mapper.
	 * @param target Instance de 'Utilisateur' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'utilisateur'.
	 */
	public static Utilisateur toUtilisateur(UtilisateurCreateDto source, Utilisateur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setEmail(source.getUtilisateurEmail());
		target.setNom(source.getUtilisateurNom());
		target.setDateInscription(source.getUtilisateurDateInscription());
		target.setTypeUtilisateurCode(source.getUtilisateurTypeUtilisateurCode());
		return target;
	}

	/**
	 * Mappe 'Utilisateur' vers une nouvelle instance de 'UtilisateurUpdateDto'.
	 * @param source Instance de 'UtilisateurUpdateDto' à mapper.
	 *
	 * @return Nouvelle instance de 'UtilisateurUpdateDto' mappée depuis 'utilisateur'.
	 */
	public static Utilisateur toUtilisateur(UtilisateurUpdateDto source) {
		return toUtilisateur(source, new Utilisateur());
	}

	/**
	 * Mappe 'Utilisateur' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'UtilisateurUpdateDto' à mapper.
	 * @param target Instance de 'Utilisateur' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'utilisateur'.
	 */
	public static Utilisateur toUtilisateur(UtilisateurUpdateDto source, Utilisateur target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNom(source.getNom());
		return target;
	}
}
