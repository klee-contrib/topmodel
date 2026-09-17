////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.dtos.restaurant;

import java.util.List;

import jakarta.annotation.Generated;
import jakarta.validation.constraints.Size;

import restaurant.jpa_feign.enums.restaurant.TypeFactureCode;

/**
 * Type de facture.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public record TypeFacture(
	TypeFactureCode code,
	@Size(max = 100) String libelle
) {

	public static final TypeFacture ELE = new TypeFacture(TypeFactureCode.ELE, "Facture électronique");

	public static final TypeFacture PHY = new TypeFacture(TypeFactureCode.PHY, "Facture physique");

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

	/**
	 * Enumération des champs de la classe {@link restaurant.jpa_feign.dtos.restaurant.TypeFacture TypeFacture}.
	 */
	public enum Fields {
		CODE(TypeFactureCode.class),
		LIBELLE(String.class);

		private final Class<?> type;

		Fields(Class<?> type) {
			this.type = type;
		}

		/**
		 * Getter for type.
		 *
		 * @return value of {@link #type type}.
		 */
		public Class<?> getType() {
			return this.type;
		}
	}
}
