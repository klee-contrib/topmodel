////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_server.dtos.restaurant;

import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

import restaurant.jpa_server.enums.restaurant.TypeFactureCode;

/**
 * Type de facture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record TypeFacture(
	TypeFactureCode code,
	@Size(max = 100) String libelle
) {

	public static final TypeFacture ELE = new TypeFacture(TypeFactureCode.ELE, "restaurant.typeFacture.values.ELE");

	public static final TypeFacture PHY = new TypeFacture(TypeFactureCode.PHY, "restaurant.typeFacture.values.PHY");

	/**
	 * Liste de toutes les valeurs de l'énumération TypeFacture.
	 */
	public static final List<TypeFacture> VALUES = List.of(ELE, PHY);

	/**
	 * Retourne la valeur de l'énumération pour la clé spécifiée.
	 * @param code La clé de l'énumération pour laquelle obtenir la valeur.
	 *
	 * @return La valeur de l'énumération correspondant à la clé 'Code'.
	 */
	public static TypeFacture getValue(TypeFactureCode code) {
		return switch (code) {
			case ELE -> ELE;
			case PHY -> PHY;
		};
	}
}
