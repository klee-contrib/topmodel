////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_sequence_server.entities.restaurant;

import java.math.BigDecimal;
import java.util.Objects;
import java.util.stream.Collectors;

import jakarta.annotation.Generated;

import restaurant.jpa_sequence_server.dtos.restaurant.AvisClientRead;
import restaurant.jpa_sequence_server.dtos.restaurant.AvisClientWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientItem;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientRead;
import restaurant.jpa_sequence_server.dtos.restaurant.ClientWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.CommandeRead;
import restaurant.jpa_sequence_server.dtos.restaurant.CommandeWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.EmployeRead;
import restaurant.jpa_sequence_server.dtos.restaurant.EmployeWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.LigneCommandeRead;
import restaurant.jpa_sequence_server.dtos.restaurant.LigneCommandeWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.MenuRead;
import restaurant.jpa_sequence_server.dtos.restaurant.MenuWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.PlatItem;
import restaurant.jpa_sequence_server.dtos.restaurant.PlatRead;
import restaurant.jpa_sequence_server.dtos.restaurant.PlatWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.PromotionRead;
import restaurant.jpa_sequence_server.dtos.restaurant.PromotionWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.ReservationRead;
import restaurant.jpa_sequence_server.dtos.restaurant.ReservationWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantAvecStatistiques;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantRead;
import restaurant.jpa_sequence_server.dtos.restaurant.RestaurantWrite;
import restaurant.jpa_sequence_server.dtos.restaurant.StatistiquesRestaurant;
import restaurant.jpa_sequence_server.dtos.restaurant.TableItem;
import restaurant.jpa_sequence_server.dtos.restaurant.TableRead;
import restaurant.jpa_sequence_server.dtos.restaurant.TableWrite;

@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class RestaurantMappers {

	private RestaurantMappers() {
		// private constructor to hide implicite public one
	}

	/**
	 * Crée une nouvelle instance de la classe 'AvisClientRead' en mappant les champs sources.
	 * @param avisClient Instance de 'AvisClient' source.
	 *
	 * @return Une nouvelle instance de 'AvisClientRead' sur laquelle les champs sources ont été mappés.
	 */
	public static AvisClientRead createAvisClientRead(AvisClient avisClient) {
		return mapAvisClientRead(avisClient, new AvisClientRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'AvisClientRead' passée en paramètre.
	 * @param avisClient Instance de 'AvisClient' source.
	 * @param target Instance de 'AvisClientRead' cible.
	 *
	 * @return L'instance de 'AvisClientRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static AvisClientRead mapAvisClientRead(AvisClient avisClient, AvisClientRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (avisClient == null) {
			throw new IllegalArgumentException("avisClient cannot be null");
		}

		target.setId(avisClient.getId());
		target.setNote(avisClient.getNote());
		target.setCommentaire(avisClient.getCommentaire());
		target.setDateAvis(avisClient.getDateAvis());
		target.setApprouve(avisClient.getApprouve());
		if (avisClient.getClient() != null) {
			target.setClientId(avisClient.getClient().getId());
		} else {
			target.setClientId(null);
		}

		if (avisClient.getRestaurant() != null) {
			target.setRestaurantId(avisClient.getRestaurant().getId());
		} else {
			target.setRestaurantId(null);
		}

		target.setNombreVues(avisClient.getNombreVues());
		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'ClientItem' en mappant les champs sources.
	 * @param client Instance de 'Client' source.
	 *
	 * @return Une nouvelle instance de 'ClientItem' sur laquelle les champs sources ont été mappés.
	 */
	public static ClientItem createClientItem(Client client) {
		return mapClientItem(client, new ClientItem());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'ClientItem' passée en paramètre.
	 * @param client Instance de 'Client' source.
	 * @param target Instance de 'ClientItem' cible.
	 *
	 * @return L'instance de 'ClientItem' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static ClientItem mapClientItem(Client client, ClientItem target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (client == null) {
			throw new IllegalArgumentException("client cannot be null");
		}

		target.setId(client.getId());
		target.setNom(client.getNom());
		target.setPrenom(client.getPrenom());
		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'ClientRead' en mappant les champs sources.
	 * @param client Instance de 'Client' source.
	 *
	 * @return Une nouvelle instance de 'ClientRead' sur laquelle les champs sources ont été mappés.
	 */
	public static ClientRead createClientRead(Client client) {
		return mapClientRead(client, new ClientRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'ClientRead' passée en paramètre.
	 * @param client Instance de 'Client' source.
	 * @param target Instance de 'ClientRead' cible.
	 *
	 * @return L'instance de 'ClientRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static ClientRead mapClientRead(Client client, ClientRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (client == null) {
			throw new IllegalArgumentException("client cannot be null");
		}

		target.setId(client.getId());
		target.setNom(client.getNom());
		target.setPrenom(client.getPrenom());
		target.setDepartementCode(client.getDepartementCode());
		target.setEmail(client.getEmail());
		if (client.getAvisClients() != null) {
			target.setAvisClients(client.getAvisClients().stream().filter(Objects::nonNull).map(AvisClient::getId).collect(Collectors.toList()));
		} else {
			target.setAvisClients(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'CommandeRead' en mappant les champs sources.
	 * @param commande Instance de 'Commande' source.
	 *
	 * @return Une nouvelle instance de 'CommandeRead' sur laquelle les champs sources ont été mappés.
	 */
	public static CommandeRead createCommandeRead(Commande commande) {
		return mapCommandeRead(commande, new CommandeRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'CommandeRead' passée en paramètre.
	 * @param commande Instance de 'Commande' source.
	 * @param target Instance de 'CommandeRead' cible.
	 *
	 * @return L'instance de 'CommandeRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static CommandeRead mapCommandeRead(Commande commande, CommandeRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (commande == null) {
			throw new IllegalArgumentException("commande cannot be null");
		}

		target.setId(commande.getId());
		target.setDateCommande(commande.getDateCommande());
		target.setDateLivraison(commande.getDateLivraison());
		target.setMontantTotal(commande.getMontantTotal());
		target.setTableId(commande.getTableId());
		target.setStatutCommande(commande.getStatutCommande());
		if (commande.getAvisClient() != null) {
			target.setAvisClientId(commande.getAvisClient().getId());
		} else {
			target.setAvisClientId(null);
		}

		if (commande.getClient() != null) {
			target.setClient(target.getClient() != null ? RestaurantMappers.mapClientRead(commande.getClient(), target.getClient()) : RestaurantMappers.createClientRead(commande.getClient()));
		} else {
			target.setClient(null);
		}

		if (commande.getLignes() != null) {
			target.setLignes(commande.getLignes().stream().filter(Objects::nonNull).map(RestaurantMappers::createLigneCommandeRead).collect(Collectors.toList()));
		} else {
			target.setLignes(null);
		}

		if (commande.getReservation() != null) {
			target.setReservation(target.getReservation() != null ? RestaurantMappers.mapReservationRead(commande.getReservation(), target.getReservation()) : RestaurantMappers.createReservationRead(commande.getReservation()));
		} else {
			target.setReservation(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'EmployeRead' en mappant les champs sources.
	 * @param employe Instance de 'Employe' source.
	 *
	 * @return Une nouvelle instance de 'EmployeRead' sur laquelle les champs sources ont été mappés.
	 */
	public static EmployeRead createEmployeRead(Employe employe) {
		return mapEmployeRead(employe, new EmployeRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'EmployeRead' passée en paramètre.
	 * @param employe Instance de 'Employe' source.
	 * @param target Instance de 'EmployeRead' cible.
	 *
	 * @return L'instance de 'EmployeRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static EmployeRead mapEmployeRead(Employe employe, EmployeRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (employe == null) {
			throw new IllegalArgumentException("employe cannot be null");
		}

		target.setId(employe.getId());
		target.setNom(employe.getNom());
		target.setPrenom(employe.getPrenom());
		target.setDepartementCode(employe.getDepartementCode());
		target.setTelephone(employe.getTelephone());
		target.setDateNaissance(employe.getDateNaissance());
		target.setMatricule(employe.getMatricule());
		target.setDateEmbauche(employe.getDateEmbauche());
		target.setSalaire(employe.getSalaire());
		if (employe.getRestaurant() != null) {
			target.setRestaurantId(employe.getRestaurant().getId());
		} else {
			target.setRestaurantId(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'LigneCommandeRead' en mappant les champs sources.
	 * @param ligneCommande Instance de 'LigneCommande' source.
	 *
	 * @return Une nouvelle instance de 'LigneCommandeRead' sur laquelle les champs sources ont été mappés.
	 */
	public static LigneCommandeRead createLigneCommandeRead(LigneCommande ligneCommande) {
		return mapLigneCommandeRead(ligneCommande, new LigneCommandeRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'LigneCommandeRead' passée en paramètre.
	 * @param ligneCommande Instance de 'LigneCommande' source.
	 * @param target Instance de 'LigneCommandeRead' cible.
	 *
	 * @return L'instance de 'LigneCommandeRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static LigneCommandeRead mapLigneCommandeRead(LigneCommande ligneCommande, LigneCommandeRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (ligneCommande == null) {
			throw new IllegalArgumentException("ligneCommande cannot be null");
		}

		target.setId(ligneCommande.getId());
		target.setQuantite(ligneCommande.getQuantite());
		target.setPrixUnitaire(ligneCommande.getPrixUnitaire());
		target.setPrixTotal(ligneCommande.getPrixTotal());
		if (ligneCommande.getCommande() != null) {
			target.setCommandeId(ligneCommande.getCommande().getId());
		} else {
			target.setCommandeId(null);
		}

		if (ligneCommande.getPlat() != null) {
			target.setPlatId(ligneCommande.getPlat().getId());
		} else {
			target.setPlatId(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'MenuRead' en mappant les champs sources.
	 * @param menu Instance de 'Menu' source.
	 *
	 * @return Une nouvelle instance de 'MenuRead' sur laquelle les champs sources ont été mappés.
	 */
	public static MenuRead createMenuRead(Menu menu) {
		return mapMenuRead(menu, new MenuRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'MenuRead' passée en paramètre.
	 * @param menu Instance de 'Menu' source.
	 * @param target Instance de 'MenuRead' cible.
	 *
	 * @return L'instance de 'MenuRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static MenuRead mapMenuRead(Menu menu, MenuRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (menu == null) {
			throw new IllegalArgumentException("menu cannot be null");
		}

		target.setId(menu.getId());
		target.setNom(menu.getNom());
		target.setDescription(menu.getDescription());
		target.setPrix(menu.getPrix());
		target.setDisponible(menu.getDisponible());
		target.setDateDebut(menu.getDateDebut());
		target.setDateFin(menu.getDateFin());
		if (menu.getRestaurant() != null) {
			target.setRestaurantId(menu.getRestaurant().getId());
		} else {
			target.setRestaurantId(null);
		}

		return target;
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'PlatItem' passée en paramètre.
	 * @param plat Instance de 'Plat' source.
	 * @param target Instance de 'PlatItem' cible.
	 *
	 * @return L'instance de 'PlatItem' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static PlatItem mapPlatItem(Plat plat, PlatItem target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (plat == null) {
			throw new IllegalArgumentException("plat cannot be null");
		}

		target.setId(plat.getId());
		target.setNom(plat.getNom());
		target.setPrix(plat.getPrix());
		target.setDisponible(plat.getDisponible());
		if (plat.getCategoriePlat() != null) {
			target.setCategoriePlatCode(plat.getCategoriePlat().getCode());
		} else {
			target.setCategoriePlatCode(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'PlatRead' en mappant les champs sources.
	 * @param plat Instance de 'Plat' source.
	 *
	 * @return Une nouvelle instance de 'PlatRead' sur laquelle les champs sources ont été mappés.
	 */
	public static PlatRead createPlatRead(Plat plat) {
		return mapPlatRead(plat, new PlatRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'PlatRead' passée en paramètre.
	 * @param plat Instance de 'Plat' source.
	 * @param target Instance de 'PlatRead' cible.
	 *
	 * @return L'instance de 'PlatRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static PlatRead mapPlatRead(Plat plat, PlatRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (plat == null) {
			throw new IllegalArgumentException("plat cannot be null");
		}

		target.setId(plat.getId());
		target.setNom(plat.getNom());
		target.setDescription(plat.getDescription());
		target.setPrix(plat.getPrix());
		target.setDisponible(plat.getDisponible());
		if (plat.getCategoriePlat() != null) {
			target.setCategoriePlatCode(plat.getCategoriePlat().getCode());
		} else {
			target.setCategoriePlatCode(null);
		}

		if (plat.getRestaurant() != null) {
			target.setRestaurantId(plat.getRestaurant().getId());
		} else {
			target.setRestaurantId(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'PromotionRead' en mappant les champs sources.
	 * @param promotion Instance de 'Promotion' source.
	 *
	 * @return Une nouvelle instance de 'PromotionRead' sur laquelle les champs sources ont été mappés.
	 */
	public static PromotionRead createPromotionRead(Promotion promotion) {
		return mapPromotionRead(promotion, new PromotionRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'PromotionRead' passée en paramètre.
	 * @param promotion Instance de 'Promotion' source.
	 * @param target Instance de 'PromotionRead' cible.
	 *
	 * @return L'instance de 'PromotionRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static PromotionRead mapPromotionRead(Promotion promotion, PromotionRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (promotion == null) {
			throw new IllegalArgumentException("promotion cannot be null");
		}

		if (promotion.getPlat() != null) {
			target.setPlatId(promotion.getPlat().getId());
		} else {
			target.setPlatId(null);
		}

		target.setLibelle(promotion.getLibelle());
		target.setPourcentageReduction(promotion.getPourcentageReduction());
		target.setDateDebut(promotion.getDateDebut());
		target.setDateFin(promotion.getDateFin());
		target.setActive(promotion.getActive());
		if (promotion.getRestaurant() != null) {
			target.setRestaurantId(promotion.getRestaurant().getId());
		} else {
			target.setRestaurantId(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'ReservationRead' en mappant les champs sources.
	 * @param reservation Instance de 'Reservation' source.
	 *
	 * @return Une nouvelle instance de 'ReservationRead' sur laquelle les champs sources ont été mappés.
	 */
	public static ReservationRead createReservationRead(Reservation reservation) {
		return mapReservationRead(reservation, new ReservationRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'ReservationRead' passée en paramètre.
	 * @param reservation Instance de 'Reservation' source.
	 * @param target Instance de 'ReservationRead' cible.
	 *
	 * @return L'instance de 'ReservationRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static ReservationRead mapReservationRead(Reservation reservation, ReservationRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (reservation == null) {
			throw new IllegalArgumentException("reservation cannot be null");
		}

		target.setId(reservation.getId());
		target.setDateReservation(reservation.getDateReservation());
		target.setNombrePersonnes(reservation.getNombrePersonnes());
		target.setCommentaire(reservation.getCommentaire());
		target.setConfirmee(reservation.getConfirmee());
		if (reservation.getClient() != null) {
			target.setClientId(reservation.getClient().getId());
		} else {
			target.setClientId(null);
		}

		target.setTableId(reservation.getTableId());
		if (reservation.getRestaurant() != null) {
			target.setRestaurantId(reservation.getRestaurant().getId());
		} else {
			target.setRestaurantId(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'RestaurantAvecStatistiques' en mappant les champs sources.
	 * @param restaurant Instance de 'Restaurant' source.
	 * @param nombrePlats Nombre de plats.
	 * @param nombreTables Nombre de tables.
	 * @param noteMoyenne Note moyenne.
	 *
	 * @return Une nouvelle instance de 'RestaurantAvecStatistiques' sur laquelle les champs sources ont été mappés.
	 */
	public static RestaurantAvecStatistiques createRestaurantAvecStatistiques(Restaurant restaurant, Integer nombrePlats, Integer nombreTables, BigDecimal noteMoyenne) {
		return mapRestaurantAvecStatistiques(restaurant, nombrePlats, nombreTables, noteMoyenne, new RestaurantAvecStatistiques());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'RestaurantAvecStatistiques' passée en paramètre.
	 * @param restaurant Instance de 'Restaurant' source.
	 * @param nombrePlats Nombre de plats.
	 * @param nombreTables Nombre de tables.
	 * @param noteMoyenne Note moyenne.
	 * @param target Instance de 'RestaurantAvecStatistiques' cible.
	 *
	 * @return L'instance de 'RestaurantAvecStatistiques' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static RestaurantAvecStatistiques mapRestaurantAvecStatistiques(Restaurant restaurant, Integer nombrePlats, Integer nombreTables, BigDecimal noteMoyenne, RestaurantAvecStatistiques target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (restaurant == null) {
			throw new IllegalArgumentException("restaurant cannot be null");
		}

		target.setId(restaurant.getId());
		target.setNom(restaurant.getNom());
		target.setAdresse(restaurant.getAdresse());
		target.setTelephone(restaurant.getTelephone());
		if (restaurant.getMenus() != null) {
			target.setMenus(restaurant.getMenus().stream().filter(Objects::nonNull).map(Menu::getId).collect(Collectors.toList()));
		} else {
			target.setMenus(null);
		}

		if (restaurant.getPlats() != null) {
			target.setPlats(restaurant.getPlats().stream().filter(Objects::nonNull).map(Plat::getId).collect(Collectors.toList()));
		} else {
			target.setPlats(null);
		}

		if (restaurant.getPromotions() != null) {
			target.setPromotions(restaurant.getPromotions().stream().filter(Objects::nonNull).map(p -> p.getPlat().getId()).collect(Collectors.toList()));
		} else {
			target.setPromotions(null);
		}

		if (restaurant.getAvisClients() != null) {
			target.setAvisClients(restaurant.getAvisClients().stream().filter(Objects::nonNull).map(AvisClient::getId).collect(Collectors.toList()));
		} else {
			target.setAvisClients(null);
		}

		target.setTableIds(restaurant.getTableIds());
		target.setNombrePlats(nombrePlats);
		target.setNombreTables(nombreTables);
		target.setNoteMoyenne(noteMoyenne);
		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'RestaurantRead' en mappant les champs sources.
	 * @param restaurant Instance de 'Restaurant' source.
	 *
	 * @return Une nouvelle instance de 'RestaurantRead' sur laquelle les champs sources ont été mappés.
	 */
	public static RestaurantRead createRestaurantRead(Restaurant restaurant) {
		return mapRestaurantRead(restaurant, new RestaurantRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'RestaurantRead' passée en paramètre.
	 * @param restaurant Instance de 'Restaurant' source.
	 * @param target Instance de 'RestaurantRead' cible.
	 *
	 * @return L'instance de 'RestaurantRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static RestaurantRead mapRestaurantRead(Restaurant restaurant, RestaurantRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (restaurant == null) {
			throw new IllegalArgumentException("restaurant cannot be null");
		}

		target.setId(restaurant.getId());
		target.setNom(restaurant.getNom());
		target.setAdresse(restaurant.getAdresse());
		target.setTelephone(restaurant.getTelephone());
		if (restaurant.getMenus() != null) {
			target.setMenus(restaurant.getMenus().stream().filter(Objects::nonNull).map(Menu::getId).collect(Collectors.toList()));
		} else {
			target.setMenus(null);
		}

		if (restaurant.getPlats() != null) {
			target.setPlats(restaurant.getPlats().stream().filter(Objects::nonNull).map(Plat::getId).collect(Collectors.toList()));
		} else {
			target.setPlats(null);
		}

		if (restaurant.getPromotions() != null) {
			target.setPromotions(restaurant.getPromotions().stream().filter(Objects::nonNull).map(p -> p.getPlat().getId()).collect(Collectors.toList()));
		} else {
			target.setPromotions(null);
		}

		if (restaurant.getAvisClients() != null) {
			target.setAvisClients(restaurant.getAvisClients().stream().filter(Objects::nonNull).map(AvisClient::getId).collect(Collectors.toList()));
		} else {
			target.setAvisClients(null);
		}

		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'StatistiquesRestaurant' en mappant les champs sources.
	 * @param restaurant Instance de 'Restaurant' source.
	 * @param nombreCommandes Nombre de commandes.
	 * @param chiffreAffaires Chiffre d'affaires.
	 * @param nombreClients Nombre de clients.
	 * @param noteMoyenne Note moyenne.
	 *
	 * @return Une nouvelle instance de 'StatistiquesRestaurant' sur laquelle les champs sources ont été mappés.
	 */
	public static StatistiquesRestaurant createStatistiquesRestaurant(Restaurant restaurant, Integer nombreCommandes, BigDecimal chiffreAffaires, Integer nombreClients, BigDecimal noteMoyenne) {
		return mapStatistiquesRestaurant(restaurant, nombreCommandes, chiffreAffaires, nombreClients, noteMoyenne, new StatistiquesRestaurant());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'StatistiquesRestaurant' passée en paramètre.
	 * @param restaurant Instance de 'Restaurant' source.
	 * @param nombreCommandes Nombre de commandes.
	 * @param chiffreAffaires Chiffre d'affaires.
	 * @param nombreClients Nombre de clients.
	 * @param noteMoyenne Note moyenne.
	 * @param target Instance de 'StatistiquesRestaurant' cible.
	 *
	 * @return L'instance de 'StatistiquesRestaurant' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static StatistiquesRestaurant mapStatistiquesRestaurant(Restaurant restaurant, Integer nombreCommandes, BigDecimal chiffreAffaires, Integer nombreClients, BigDecimal noteMoyenne, StatistiquesRestaurant target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNombreCommandes(nombreCommandes);
		target.setChiffreAffaires(chiffreAffaires);
		target.setNombreClients(nombreClients);
		target.setNoteMoyenne(noteMoyenne);
		return target;
	}

	/**
	 * Crée une nouvelle instance de la classe 'TableRead' en mappant les champs sources.
	 * @param table Instance de 'TableRestaurant' source.
	 *
	 * @return Une nouvelle instance de 'TableRead' sur laquelle les champs sources ont été mappés.
	 */
	public static TableRead createTableRead(TableRestaurant table) {
		return mapTableRead(table, new TableRead());
	}

	/**
	 * Mappe les champs sources sur l'instance de la classe 'TableRead' passée en paramètre.
	 * @param table Instance de 'TableRestaurant' source.
	 * @param target Instance de 'TableRead' cible.
	 *
	 * @return L'instance de 'TableRead' passée en paramètres sur lesquels les champs sources ont été mappés.
	 */
	public static TableRead mapTableRead(TableRestaurant table, TableRead target) {
		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		if (table == null) {
			throw new IllegalArgumentException("table cannot be null");
		}

		target.setId(table.getId());
		target.setNumero(table.getNumero());
		target.setCapacite(table.getCapacite());
		target.setDisponible(table.getDisponible());
		target.setRestaurantId(table.getRestaurantId());
		return target;
	}

	/**
	 * Mappe 'AvisClient' vers une nouvelle instance de 'AvisClientWrite'.
	 * @param source Instance de 'AvisClientWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'AvisClientWrite' mappée depuis 'avisClient'.
	 */
	public static AvisClient toAvisClient(AvisClientWrite source) {
		return toAvisClient(source, new AvisClient());
	}

	/**
	 * Mappe 'AvisClient' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'AvisClientWrite' à mapper.
	 * @param target Instance de 'AvisClient' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'avisClient'.
	 */
	public static AvisClient toAvisClient(AvisClientWrite source, AvisClient target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNote(source.getNote());
		target.setCommentaire(source.getCommentaire());
		target.setApprouve(source.getApprouve());
		return target;
	}

	/**
	 * Mappe 'Client' vers une nouvelle instance de 'ClientWrite'.
	 * @param source Instance de 'ClientWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'ClientWrite' mappée depuis 'client'.
	 */
	public static Client toClient(ClientWrite source) {
		return toClient(source, new Client());
	}

	/**
	 * Mappe 'Client' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'ClientWrite' à mapper.
	 * @param target Instance de 'Client' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'client'.
	 */
	public static Client toClient(ClientWrite source, Client target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNom(source.getNom());
		target.setPrenom(source.getPrenom());
		target.setDepartementCode(source.getDepartementCode());
		target.setEmail(source.getEmail());
		return target;
	}

	/**
	 * Mappe 'Commande' vers une nouvelle instance de 'CommandeWrite'.
	 * @param source Instance de 'CommandeWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'CommandeWrite' mappée depuis 'commande'.
	 */
	public static Commande toCommande(CommandeWrite source) {
		return toCommande(source, new Commande());
	}

	/**
	 * Mappe 'Commande' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'CommandeWrite' à mapper.
	 * @param target Instance de 'Commande' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'commande'.
	 */
	public static Commande toCommande(CommandeWrite source, Commande target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setDateCommande(source.getDateCommande());
		target.setDateLivraison(source.getDateLivraison());
		target.setMontantTotal(source.getMontantTotal());
		target.setTableId(source.getTableId());
		target.setStatutCommande(source.getStatutCommande());
		if (source.getClient() != null) {
			target.setClient(target.getClient() != null ? RestaurantMappers.toClient(source.getClient(), target.getClient()) : RestaurantMappers.toClient(source.getClient()));
		} else {
			target.setClient(null);
		}

		if (source.getReservation() != null) {
			target.setReservation(target.getReservation() != null ? RestaurantMappers.toReservation(source.getReservation(), target.getReservation()) : RestaurantMappers.toReservation(source.getReservation()));
		} else {
			target.setReservation(null);
		}

		if (source.getLignes() != null) {
			target.setLignes(source.getLignes().stream().filter(Objects::nonNull).map(RestaurantMappers::toLigneCommande).collect(Collectors.toList()));
		} else {
			target.setLignes(null);
		}

		return target;
	}

	/**
	 * Mappe 'Employe' vers une nouvelle instance de 'EmployeWrite'.
	 * @param source Instance de 'EmployeWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'EmployeWrite' mappée depuis 'employe'.
	 */
	public static Employe toEmploye(EmployeWrite source) {
		return toEmploye(source, new Employe());
	}

	/**
	 * Mappe 'Employe' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'EmployeWrite' à mapper.
	 * @param target Instance de 'Employe' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'employe'.
	 */
	public static Employe toEmploye(EmployeWrite source, Employe target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNom(source.getNom());
		target.setPrenom(source.getPrenom());
		target.setDepartementCode(source.getDepartementCode());
		target.setTelephone(source.getTelephone());
		target.setDateNaissance(source.getDateNaissance());
		target.setMatricule(source.getMatricule());
		target.setDateEmbauche(source.getDateEmbauche());
		target.setSalaire(source.getSalaire());
		return target;
	}

	/**
	 * Mappe 'LigneCommande' vers une nouvelle instance de 'LigneCommandeWrite'.
	 * @param source Instance de 'LigneCommandeWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'LigneCommandeWrite' mappée depuis 'ligneCommande'.
	 */
	public static LigneCommande toLigneCommande(LigneCommandeWrite source) {
		return toLigneCommande(source, new LigneCommande());
	}

	/**
	 * Mappe 'LigneCommande' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'LigneCommandeWrite' à mapper.
	 * @param target Instance de 'LigneCommande' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'ligneCommande'.
	 */
	public static LigneCommande toLigneCommande(LigneCommandeWrite source, LigneCommande target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setQuantite(source.getQuantite());
		target.setPrixUnitaire(source.getPrixUnitaire());
		target.setPrixTotal(source.getPrixTotal());
		return target;
	}

	/**
	 * Mappe 'Menu' vers une nouvelle instance de 'MenuWrite'.
	 * @param source Instance de 'MenuWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'MenuWrite' mappée depuis 'menu'.
	 */
	public static Menu toMenu(MenuWrite source) {
		return toMenu(source, new Menu());
	}

	/**
	 * Mappe 'Menu' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'MenuWrite' à mapper.
	 * @param target Instance de 'Menu' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'menu'.
	 */
	public static Menu toMenu(MenuWrite source, Menu target) {
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
		return target;
	}

	/**
	 * Mappe 'Plat' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'PlatItem' à mapper.
	 * @param target Instance de 'Plat' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'plat'.
	 */
	public static Plat toPlat(PlatItem source, Plat target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setId(source.getId());
		target.setNom(source.getNom());
		target.setPrix(source.getPrix());
		target.setDisponible(source.getDisponible());
		return target;
	}

	/**
	 * Mappe 'Plat' vers une nouvelle instance de 'PlatWrite'.
	 * @param source Instance de 'PlatWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'PlatWrite' mappée depuis 'plat'.
	 */
	public static Plat toPlat(PlatWrite source) {
		return toPlat(source, new Plat());
	}

	/**
	 * Mappe 'Plat' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'PlatWrite' à mapper.
	 * @param target Instance de 'Plat' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'plat'.
	 */
	public static Plat toPlat(PlatWrite source, Plat target) {
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
		return target;
	}

	/**
	 * Mappe 'Promotion' vers une nouvelle instance de 'PromotionWrite'.
	 * @param source Instance de 'PromotionWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'PromotionWrite' mappée depuis 'promotion'.
	 */
	public static Promotion toPromotion(PromotionWrite source) {
		return toPromotion(source, new Promotion());
	}

	/**
	 * Mappe 'Promotion' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'PromotionWrite' à mapper.
	 * @param target Instance de 'Promotion' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'promotion'.
	 */
	public static Promotion toPromotion(PromotionWrite source, Promotion target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setLibelle(source.getLibelle());
		target.setPourcentageReduction(source.getPourcentageReduction());
		target.setDateDebut(source.getDateDebut());
		target.setDateFin(source.getDateFin());
		target.setActive(source.getActive());
		return target;
	}

	/**
	 * Mappe 'Reservation' vers une nouvelle instance de 'ReservationWrite'.
	 * @param source Instance de 'ReservationWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'ReservationWrite' mappée depuis 'reservation'.
	 */
	public static Reservation toReservation(ReservationWrite source) {
		return toReservation(source, new Reservation());
	}

	/**
	 * Mappe 'Reservation' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'ReservationWrite' à mapper.
	 * @param target Instance de 'Reservation' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'reservation'.
	 */
	public static Reservation toReservation(ReservationWrite source, Reservation target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setDateReservation(source.getDateReservation());
		target.setNombrePersonnes(source.getNombrePersonnes());
		target.setCommentaire(source.getCommentaire());
		target.setConfirmee(source.getConfirmee());
		target.setTableId(source.getTableId());
		return target;
	}

	/**
	 * Mappe 'Restaurant' vers une nouvelle instance de 'RestaurantWrite'.
	 * @param source Instance de 'RestaurantWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'RestaurantWrite' mappée depuis 'restaurant'.
	 */
	public static Restaurant toRestaurant(RestaurantWrite source) {
		return toRestaurant(source, new Restaurant());
	}

	/**
	 * Mappe 'Restaurant' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'RestaurantWrite' à mapper.
	 * @param target Instance de 'Restaurant' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'restaurant'.
	 */
	public static Restaurant toRestaurant(RestaurantWrite source, Restaurant target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNom(source.getNom());
		target.setAdresse(source.getAdresse());
		target.setTelephone(source.getTelephone());
		if (source.getTables() != null) {
			target.setTableIds(source.getTables().stream().filter(Objects::nonNull).map(TableItem::getId).collect(Collectors.toList()));
		} else {
			target.setTableIds(null);
		}

		return target;
	}

	/**
	 * Mappe 'TableRestaurant' vers une nouvelle instance de 'TableWrite'.
	 * @param source Instance de 'TableWrite' à mapper.
	 *
	 * @return Nouvelle instance de 'TableWrite' mappée depuis 'tableRestaurant'.
	 */
	public static TableRestaurant toTableRestaurant(TableWrite source) {
		return toTableRestaurant(source, new TableRestaurant());
	}

	/**
	 * Mappe 'TableRestaurant' vers une nouvelle instance ou bien sur l'instance passée en paramètres.
	 * @param source Instance de 'TableWrite' à mapper.
	 * @param target Instance de 'TableRestaurant' sur laquelle mapper.
	 *
	 * @return Nouvelle instance ou bien l'instance passée en paramètres mappée depuis 'tableRestaurant'.
	 */
	public static TableRestaurant toTableRestaurant(TableWrite source, TableRestaurant target) {
		if (source == null) {
			throw new IllegalArgumentException("source cannot be null");
		}

		if (target == null) {
			throw new IllegalArgumentException("target cannot be null");
		}

		target.setNumero(source.getNumero());
		target.setCapacite(source.getCapacite());
		target.setDisponible(source.getDisponible());
		target.setRestaurantId(source.getRestaurantId());
		return target;
	}
}
