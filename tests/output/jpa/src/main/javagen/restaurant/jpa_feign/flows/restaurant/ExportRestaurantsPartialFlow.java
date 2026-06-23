////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.flows.restaurant;

import org.springframework.batch.infrastructure.item.ItemProcessor;

import restaurant.jpa_feign.dtos.restaurant.RestaurantRead;
import restaurant.jpa_feign.entities.restaurant.Restaurant;

public interface ExportRestaurantsPartialFlow {

	/**
	 * Processus de transformation de la donnée dans le flow. Les étapes sont ordonnées comme suit :.
	 * Read - AfterSource - Map - BeforeWrite - Write
	 * Le map remplace le mapping par défaut de TopModel
	 *
	 * @return ItemProcessor étape au sens de spring-batch.
	 */
	ItemProcessor<Restaurant, Restaurant> afterSource();

	/**
	 * Processus de transformation de la donnée dans le flow. Les étapes sont ordonnées comme suit :.
	 * Read - AfterSource - Map - BeforeWrite - Write
	 * Le map remplace le mapping par défaut de TopModel
	 *
	 * @return ItemProcessor étape au sens de spring-batch.
	 */
	ItemProcessor<Restaurant, RestaurantRead> map();

	/**
	 * Processus de transformation de la donnée dans le flow. Les étapes sont ordonnées comme suit :.
	 * Read - AfterSource - Map - BeforeWrite - Write
	 * Le map remplace le mapping par défaut de TopModel
	 *
	 * @return ItemProcessor étape au sens de spring-batch.
	 */
	ItemProcessor<RestaurantRead, RestaurantRead> beforeTarget();
}
