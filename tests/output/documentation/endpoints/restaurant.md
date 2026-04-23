# Liste des endpoints du module Restaurant

| Nom                       | Méthode | Route                                           | Description                                    | Autorisation |
| ------------------------- | ------- | ----------------------------------------------- | ---------------------------------------------- | ------------ |
| AddClient                 | POST    | /api/restaurants/clients                        | Ajoute un client                               |              |
| AddCommande               | POST    | /api/restaurants/commandes                      | Crée une nouvelle commande                     |              |
| AddEmploye                | POST    | /api/restaurants/employes                       | Ajoute un employé (nécessite le rôle ADMIN)    | ADMIN        |
| AddPlat                   | POST    | /api/restaurants/plats                          | Ajoute un plat                                 |              |
| AddRestaurant             | POST    | /api/restaurants                                | Ajoute un restaurant                           |              |
| AddTable                  | POST    | /api/restaurants/tables                         | Ajoute une table                               |              |
| CreateMenu                | POST    | /api/restaurants/menus                          | Crée un menu avec ses plats                    | MANAGER      |
| CreateReservation         | POST    | /api/restaurants/reservations                   | Crée une réservation                           |              |
| DeleteClient              | DELETE  | /api/restaurants/clients/{perId}                | Supprime un client                             |              |
| DeleteCommande            | DELETE  | /api/restaurants/commandes/{comId}              | Supprime une commande                          |              |
| DeleteCommandeWithBody    | DELETE  | /api/restaurants/commandes                      | Supprime une commande                          |              |
| DeletePlat                | DELETE  | /api/restaurants/plats/{plaId}                  | Supprime un plat                               |              |
| DeleteRestaurant          | DELETE  | /api/restaurants/{resId}                        | Supprime un restaurant                         |              |
| DeleteTable               | DELETE  | /api/restaurants/tables/{tabId}                 | Supprime une table                             |              |
| ExportCommandes           | GET     | /api/restaurants/commandes/export               | Exporte les commandes au format CSV            | ADMIN        |
| GetAvisClients            | GET     | /api/restaurants/avis                           | Liste les avis clients avec filtres            |              |
| GetCategoriePlats         | GET     | /api/restaurants/categorie-plats                | Liste toutes les catégories de plats           |              |
| GetClient                 | GET     | /api/restaurants/clients/{perId}                | Charge le détail d'un client                   |              |
| GetClientAvecCommandes    | GET     | /api/restaurants/clients/{perId}/avec-commandes | Récupère un client avec toutes ses commandes   |              |
| GetClientCommandes        | GET     | /api/restaurants/clients/{perId}/commandes      | Liste les commandes d'un client                |              |
| GetClients                | GET     | /api/restaurants/clients                        | Liste tous les clients                         |              |
| GetCommande               | GET     | /api/restaurants/commandes/{comId}              | Charge le détail d'une commande                |              |
| GetCommandes              | GET     | /api/restaurants/commandes                      | Liste toutes les commandes                     |              |
| GetCommandesByDate        | GET     | /api/restaurants/commandes/by-date              | Récupère les commandes par date                |              |
| GetPlat                   | GET     | /api/restaurants/plats/{plaId}                  | Charge le détail d'un plat                     |              |
| GetPlats                  | GET     | /api/restaurants/plats                          | Liste tous les plats                           |              |
| GetRestaurant             | GET     | /api/restaurants/{resId}                        | Charge le détail d'un restaurant               |              |
| GetRestaurantMenu         | GET     | /api/restaurants/{resId}/menus/{menId}          | Récupère un menu spécifique d'un restaurant    |              |
| GetRestaurantPlats        | GET     | /api/restaurants/{resId}/plats                  | Liste les plats d'un restaurant                |              |
| GetRestaurantStatistiques | GET     | /api/restaurants/{resId}/statistiques           | Récupère les statistiques d'un restaurant      |              |
| GetRestaurantTables       | GET     | /api/restaurants/{resId}/tables                 | Liste les tables d'un restaurant               |              |
| GetRestaurants            | GET     | /api/restaurants                                | Liste tous les restaurants                     |              |
| GetStatutCommandes        | GET     | /api/restaurants/statuts-commande               | Liste tous les statuts de commande             |              |
| GetTable                  | GET     | /api/restaurants/tables/{tabId}                 | Charge le détail d'une table                   |              |
| GetTables                 | GET     | /api/restaurants/tables                         | Liste toutes les tables                        |              |
| PatchClient               | PATCH   | /api/restaurants/clients/{perId}                | Met à jour partiellement un client             |              |
| PatchCommande             | PATCH   | /api/restaurants/commandes/{comId}              | Met à jour partiellement une commande          |              |
| PatchPlat                 | PATCH   | /api/restaurants/plats/{plaId}                  | Met à jour partiellement un plat               |              |
| PatchPromotion            | PATCH   | /api/restaurants/plats/{plaId}/promotion        | Met à jour partiellement une promotion         |              |
| SearchPlats               | GET     | /api/restaurants/plats/search                   | Recherche de plats avec critères multiples     |              |
| SearchRestaurants         | GET     | /api/restaurants/search                         | Recherche avancée de restaurants               |              |
| UpdateClient              | PUT     | /api/restaurants/clients/{perId}                | Met à jour un client                           |              |
| UpdateCommande            | PUT     | /api/restaurants/commandes/{comId}              | Met à jour une commande                        |              |
| UpdateCommandeStatut      | PATCH   | /api/restaurants/commandes/{comId}/statut       | Met à jour uniquement le statut d'une commande |              |
| UpdatePlat                | PUT     | /api/restaurants/plats/{plaId}                  | Met à jour un plat                             |              |
| UpdateRestaurant          | PUT     | /api/restaurants/{resId}                        | Met à jour un restaurant                       |              |
| UpdateTable               | PUT     | /api/restaurants/tables/{tabId}                 | Met à jour une table                           |              |
