////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.util.Objects;
import java.util.stream.Collectors;

import jakarta.annotation.Generated;

import restaurant.jpa_server.entities.restaurant.CategoriePlat;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class RestaurantDTOMappers {

	private RestaurantDTOMappers() {
		// private constructor to hide implicite public one
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'PlatItemReadonly' passée en paramètre.
	 * @param platItem Instance de 'PlatItem' source.
	 * @param target Instance de 'PlatItemReadonly' cible.
	 *
	 * @return L'instance de 'PlatItemReadonly' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static PlatItemReadonly mapPlatItemReadonly(PlatItem platItem, PlatItemReadonly target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (platItem == null) {
			throw new IllegalArgumentException("platItem cannot be null");
		}

		target.setPrix(platItem.getPrix());
		target.setDisponible(platItem.getDisponible());
		return target;
	}

	/**
	 * Mappe 'MenuRead' vers une nouvelle instance de 'MenuWrite'.
	 * @param source Instance de 'MenuWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'MenuWrite' mappée depuis 'menuRead'.
	 */
	public static MenuRead toMenuRead(MenuWrite source) {
		return toMenuRead(source, new MenuRead());
	}

	/**
	 * Mappe 'MenuRead' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'MenuWrite' à mapper.
	 * @param target Instance de 'MenuRead' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'menuRead'.
	 */
	public static MenuRead toMenuRead(MenuWrite source, MenuRead target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNom(source.getNom());
		target.setDescription(source.getDescription());
		target.setPrix(source.getPrix());
		target.setDisponible(source.getDisponible());
		target.setDateDebut(source.getDateDebut());
		target.setDateFin(source.getDateFin());
		target.setRestaurantId(source.getRestaurantId());
		if (source.getCategoriesPlat() != null) {
			target.setCategoriesPlat(source.getCategoriesPlat().stream().filter(Objects::nonNull).map(CategoriePlat::getValue).collect(Collectors.toList()));
		} else {
			target.setCategoriesPlat(null);
		}

		return target;
	}

	/**
	 * Mappe 'PlatItem' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'PlatItemReadonly' à mapper.
	 * @param target Instance de 'PlatItem' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'platItem'.
	 */
	public static PlatItem toPlatItem(PlatItemReadonly source, PlatItem target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setId(source.getId());
		target.setNom(source.getNom());
		target.setCategoriePlatCode(source.getCategoriePlatCode());
		target.setPrix(source.getPrix());
		target.setDisponible(source.getDisponible());
		return target;
	}
}
