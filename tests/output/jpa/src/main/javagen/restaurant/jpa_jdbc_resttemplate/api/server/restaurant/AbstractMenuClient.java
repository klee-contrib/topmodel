////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_jdbc_resttemplate.api.server.restaurant;

import java.net.URI;
import java.util.Collection;
import java.util.List;

import org.springframework.core.io.Resource;
import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.util.LinkedMultiValueMap;
import org.springframework.util.MultiValueMap;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.util.UriComponentsBuilder;

import jakarta.annotation.Generated;

import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.MenuRead;
import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.MenuWrite;
import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.PlatItem;
import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.PlatRead;
import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.PlatWrite;
import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.PromotionRead;
import restaurant.jpa_jdbc_resttemplate.dtos.restaurant.PromotionWrite;
import restaurant.jpa_jdbc_resttemplate.entities.restaurant.CategoriePlat;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public abstract class AbstractMenuClient {

	protected RestTemplate restTemplate;
	protected String host;

	/**
	 * Constructeur par paramètres.
	 * @param restTemplate
	 * @param host
	 */
	protected AbstractMenuClient(RestTemplate restTemplate, String host) {
		this.restTemplate = restTemplate;
		this.host = host;
	}

	/**
	 * Méthode de récupération des headers.
	 * @return Headers à ajouter à la requête
	 */
	protected abstract HttpHeaders getHeaders();

	/**
	 * Ajoute une valeur au corps multipart de la requête.
	 * Les valeurs nulles sont ignorées, les collections sont ajoutées élément par élément.
	 * @param body Corps multipart de la requête
	 * @param name Nom du champ
	 * @param value Valeur du champ
	 */
	protected void addPart(MultiValueMap<String, Object> body, String name, Object value) {
		if (value instanceof Collection<?> values) {
			values.forEach(v -> addPart(body, name, v));
		} else if (value instanceof MultipartFile file) {
			body.add(name, file.getResource());
		} else if (value instanceof Resource resource) {
			body.add(name, resource);
		} else if (value != null) {
			body.add(name, String.valueOf(value));
		}
	}

	/**
	 * UriComponentsBuilder pour la méthode 'addPlat'.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder addPlatUriComponentsBuilder() {
		String uri = host + "/api/restaurants/plats";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Ajoute un plat.
	 * @param plat Plat à créer
	 * @return Plat créé
	 */
	public ResponseEntity<PlatRead> addPlat(PlatWrite plat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.addPlatUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(plat, headers), PlatRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode 'createMenu'.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder createMenuUriComponentsBuilder() {
		String uri = host + "/api/restaurants/menus";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Crée un menu avec ses plats.
	 * @param menu Menu à créer
	 * @param regCodeOrigine Code de la région.
	 * @return Menu créé avec ses plats
	 */
	public ResponseEntity<MenuRead> createMenu(MenuWrite menu, String regCodeOrigine){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.createMenuUriComponentsBuilder();
		headers.setContentType(MediaType.MULTIPART_FORM_DATA);
		MultiValueMap<String, Object> body = new LinkedMultiValueMap<>();
		if (menu != null) {
			addPart(body, "nom", menu.getNom());
			addPart(body, "description", menu.getDescription());
			addPart(body, "prix", menu.getPrix());
			addPart(body, "disponible", menu.getDisponible());
			addPart(body, "dateDebut", menu.getDateDebut());
			addPart(body, "dateFin", menu.getDateFin());
			addPart(body, "restaurantId", menu.getRestaurantId());
			addPart(body, "categoriesPlat", menu.getCategoriesPlat());
		}
		addPart(body, "regCodeOrigine", regCodeOrigine);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.POST, new HttpEntity<>(body, headers), MenuRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode 'deletePlat'.
	 * @param plaId Identifiant du plat
	 */
	protected UriComponentsBuilder deletePlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Supprime un plat.
	 * @param plaId Identifiant du plat
	 */
	public ResponseEntity deletePlat(Integer plaId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.deletePlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.DELETE, new HttpEntity<>(headers), (Class<?>) null);
	}

	/**
	 * UriComponentsBuilder pour la méthode 'getCategoriePlats'.
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getCategoriePlatsUriComponentsBuilder() {
		String uri = host + "/api/restaurants/categorie-plats";
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Liste toutes les catégories de plats.
	 * @return Liste des catégories de plats
	 */
	public ResponseEntity<List<CategoriePlat>> getCategoriePlats(){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getCategoriePlatsUriComponentsBuilder();
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<CategoriePlat>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode 'getPlat'.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getPlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Charge le détail d'un plat.
	 * @param plaId Identifiant du plat
	 * @return Détail du plat
	 */
	public ResponseEntity<PlatRead> getPlat(Integer plaId){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getPlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), PlatRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode 'getPlats'.
	 * @param restaurantId Restaurant proposant ce plat
	 * @param categoriePlatCode Catégorie du plat
	 * @param disponible Indique si le plat est disponible
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder getPlatsUriComponentsBuilder(Integer restaurantId, String categoriePlatCode, Boolean disponible) {
		String uri = host + "/api/restaurants/plats";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("restaurantId", restaurantId);
		uriBuilder.queryParam("categoriePlatCode", categoriePlatCode);
		uriBuilder.queryParam("disponible", disponible);
		return uriBuilder;
	}

	/**
	 * Liste tous les plats.
	 * @param restaurantId Restaurant proposant ce plat
	 * @param categoriePlatCode Catégorie du plat
	 * @param disponible Indique si le plat est disponible
	 * @return Liste des plats
	 */
	public ResponseEntity<List<PlatItem>> getPlats(Integer restaurantId, String categoriePlatCode, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.getPlatsUriComponentsBuilder(restaurantId, categoriePlatCode, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<PlatItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode 'patchPlat'.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchPlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement un plat.
	 * @param plaId Identifiant du plat
	 * @param plat Données partielles du plat
	 * @return Plat mis à jour
	 */
	public ResponseEntity<PlatRead> patchPlat(Integer plaId, PlatWrite plat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchPlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(plat, headers), PlatRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode 'patchPromotion'.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder patchPromotionUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s/promotion".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour partiellement une promotion.
	 * @param plaId Identifiant du plat
	 * @param promotion Données partielles de la promotion
	 * @return Promotion mise à jour
	 */
	public ResponseEntity<PromotionRead> patchPromotion(Integer plaId, PromotionWrite promotion){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.patchPromotionUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PATCH, new HttpEntity<>(promotion, headers), PromotionRead.class);
	}

	/**
	 * UriComponentsBuilder pour la méthode 'searchPlats'.
	 * @param nom Nom du plat
	 * @param restaurantId Restaurant proposant ce plat
	 * @param categoriePlatCode Catégorie du plat
	 * @param disponible Indique si le plat est disponible
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder searchPlatsUriComponentsBuilder(String nom, Integer restaurantId, String categoriePlatCode, Boolean disponible) {
		String uri = host + "/api/restaurants/plats/search";
		UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));
		uriBuilder.queryParam("nom", nom);
		uriBuilder.queryParam("restaurantId", restaurantId);
		uriBuilder.queryParam("categoriePlatCode", categoriePlatCode);
		uriBuilder.queryParam("disponible", disponible);
		return uriBuilder;
	}

	/**
	 * Recherche de plats avec critères multiples.
	 * @param nom Nom du plat
	 * @param restaurantId Restaurant proposant ce plat
	 * @param categoriePlatCode Catégorie du plat
	 * @param disponible Indique si le plat est disponible
	 * @return Plats correspondant aux critères de recherche
	 */
	public ResponseEntity<List<PlatItem>> searchPlats(String nom, Integer restaurantId, String categoriePlatCode, Boolean disponible){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.searchPlatsUriComponentsBuilder(nom, restaurantId, categoriePlatCode, disponible);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.GET, new HttpEntity<>(headers), new ParameterizedTypeReference<List<PlatItem>>() {});
	}

	/**
	 * UriComponentsBuilder pour la méthode 'updatePlat'.
	 * @param plaId Identifiant du plat
	 * @return uriBuilder avec les query params remplis
	 */
	protected UriComponentsBuilder updatePlatUriComponentsBuilder(Integer plaId) {
		String uri = host + "/api/restaurants/plats/%s".formatted(plaId);
		return UriComponentsBuilder.fromUri(URI.create(uri));
	}

	/**
	 * Met à jour un plat.
	 * @param plaId Identifiant du plat
	 * @param plat Plat à mettre à jour
	 * @return Plat mis à jour
	 */
	public ResponseEntity<PlatRead> updatePlat(Integer plaId, PlatWrite plat){
		HttpHeaders headers = this.getHeaders();
		UriComponentsBuilder uri = this.updatePlatUriComponentsBuilder(plaId);
		return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.PUT, new HttpEntity<>(plat, headers), PlatRead.class);
	}
}
