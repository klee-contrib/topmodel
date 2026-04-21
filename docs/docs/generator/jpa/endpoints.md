# Génération des endpoints

Les générateurs d'endpoints créent des interfaces ou classes permettant de définir des APIs serveur ou client.

| Nom                     | Condition d'activation                                         | Objets ciblés | Fichiers générés                                                                                                                                  |
| ----------------------- | -------------------------------------------------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| `SpringApiServerGen`    | `apiGeneration: server`                                        | Endpoints     | Interface définissant les méthodes annotées permettant de définir une API serveur. L'implémentation est à la charge du développeur                |
| `SpringApiClientGen`    | `apiGeneration: client` && `clientApiGeneration: restClient`   | Endpoints     | Interface contenant les annotations `@XXXExchange` pour la génération d'un client API via `HttpServiceProxyFactory`                               |
| `SpringRestTemplateGen` | `apiGeneration: client` && `clientApiGeneration: restTemplate` | Endpoints     | Classe abstraite définissant les méthodes permettant d'appeler une API externe à l'aide d'un `RestTemplate` Spring                                |
| `FeignClientApiGen`     | `apiGeneration: client` && `clientApiGeneration: feignClient`  | Endpoints     | Interface contenant les annotations nécessaires à la construction par Feign d'une API cliente                                                     |

## Détermination du nom de fichier

Le nom de la classe générée est déterminé par la configuration `apisName` (si définie) ou par la valeur par défaut du mode choisi. Dans tous les cas, le template `{fileName}` est remplacé par le nom du fichier d'endpoints défini dans le modèle, converti en PascalCase.

**Valeurs par défaut selon le mode :**

| Mode             | Nom de classe par défaut   | Type généré      |
| ---------------- | -------------------------- | ---------------- |
| **Server**       | `{fileName}Controller`     | Interface        |
| **RestClient**   | `{fileName}Client`         | Interface        |
| **RestTemplate** | `Abstract{fileName}Client` | Classe abstraite |
| **FeignClient**  | `{fileName}Api`            | Interface        |

**Exemple :** Pour un fichier d'endpoints nommé `utilisateur`, les noms générés seront :

- Mode Server : `UtilisateurController`
- Mode RestClient : `UtilisateurClient`
- Mode RestTemplate : `AbstractUtilisateurClient`
- Mode FeignClient : `UtilisateurApi`

### Personnalisation du nom

Le nom peut être personnalisé via la configuration `apisName`, qui remplace la valeur par défaut du mode. Le template `{fileName}` sera toujours remplacé par le nom du fichier d'endpoints en PascalCase.

**Exemple de personnalisation :**

```yaml
jpa:
  - tags:
      - api
    apiGeneration: Server
    apisName: "{fileName}Service" # Génère UtilisateurService au lieu de UtilisateurController
```

## Emplacement des fichiers

Le chemin du fichier est déterminé par la configuration `apiPath`, qui peut utiliser les variables suivantes :

- `{app}` : Nom de l'application
- `{module}` : Module du fichier d'endpoints
- Variables personnalisées définies dans la configuration

La valeur par défaut de `apiPath` est `"javagen:{app:path}/api/{module:path}"`.

Le chemin complet du fichier sera : `{outputDirectory}/{apiPath}/{nomClasse}.java`.

## Génération de l'Api Server (Spring)

Le générateur crée des interfaces contenant, pour chaque `endpoint` paramétré, la méthode abstraite correspondant au nom de l'endpoint, à implémenter dans votre controller. Cette méthode aura déjà l'annotation `XXXMapping` correspondant au verbe HTTP défini dans l'endpoint (par exemple `@GetMapping`, `@PostMapping`, etc.).

Pour créer votre API, il suffit donc de créer un nouveau controller qui implémente l'interface générée. L'annotation `@RestController` reste nécessaire.

**Comportements automatiques :**

- Si le domain du body de l'endpoint définit un `mediaType`, alors il sera valorisé dans l'annotation avec l'attribut `consumes`
- De la même manière pour le domain du paramètre de retour, avec l'attribut `produces`
- Si la méthode retourne `void` ou `Void`, l'annotation `@ResponseStatus(HttpStatus.NO_CONTENT)` (code HTTP 204) est automatiquement ajoutée à la méthode

**Exemple :**

```java
@RestController
public class UtilisateurControllerImpl implements UtilisateurController {

  @Override
  public UtilisateurDto getUtilisateur(Long id) {
    // Implémentation
  }
}
```

## Api Client (Spring)

### RestClient (Spring Web 6+)

Il s'agit du mode par défaut, soit lorsque la variable `clientApiGeneration` vaut `RestClient`.

Le générateur crée alors des interfaces contenant des annotations `XXXExchange`, dont il faudra configurer un bean d'implémentation.

**Note importante :** Les méthodes générées retournent toujours un `ResponseEntity<T>` (où `T` est le type de retour défini dans l'endpoint), permettant de gérer les différents codes HTTP de réponse.

**Exemple de configuration :**

```java
@Bean
protected UtilisateurApiClient utilisateurApiClient() {
  var restClient = RestClient.builder()
    .baseUrl("http://localhost:8080/my-app/api/")
    .build();
  var adapter = RestClientAdapter.create(restClient);
  var factory = HttpServiceProxyFactory.builderFor(adapter).build();
  return factory.createClient(UtilisateurApiClient.class);
}
```

### RestTemplate

Pour activer ce mode de génération, positionner la variable `clientApiGeneration` à `RestTemplate`.

Le générateur crée alors des classes abstraites contenant toutes les méthodes permettant d'accéder aux endpoints paramétrés.

**Note importante :** Les méthodes générées retournent toujours un `ResponseEntity<T>` (où `T` est le type de retour défini dans l'endpoint), permettant de gérer les différents codes HTTP de réponse.

Pour créer votre client d'API, il suffit de créer une classe qui hérite de cette classe abstraite. Pour fonctionner, elle devra appeler le constructeur de la classe abstraite, en renseignant :

- Le host de l'API
- Une instance de `RestTemplate`

**Exemple d'implémentation :**

```java
@Service
public class UtilisateurApiClient extends AbstractUtilisateurApiClient {

  private static final String HOST = "http://localhost:8080/my-app/api/";

  @Autowired
  public UtilisateurApiClient(RestTemplate restTemplate) {
    super(restTemplate, HOST);
  }
}
```

**Exemple d'utilisation :**

```java
@Service
public class UtilisateurService {

  private final UtilisateurApiClient utilisateurApiClient;

  @Autowired
  public UtilisateurService(UtilisateurApiClient utilisateurApiClient) {
    this.utilisateurApiClient = utilisateurApiClient;
  }

  public UtilisateurDto getUtilisateur(Long id) {
    var headers = new HttpHeaders();
    headers.add("token-securise", "MON_TOKEN_SECURISE");
    return utilisateurApiClient.getUtilisateur(id, headers);
  }
}
```

### FeignClient

Pour activer ce mode de génération, positionner la variable `clientApiGeneration` à `FeignClient`.

Le générateur crée des interfaces similaires au mode `Server`, à la différence près que le suffixe est `Api` au lieu de `Controller`, et que l'annotation `@FeignClient` est ajoutée à l'interface.

**Note :** Ce mode nécessite la dépendance Spring Cloud OpenFeign.

```xml
<dependency>
    <groupId>org.springframework.cloud</groupId>
    <artifactId>spring-cloud-starter-openfeign</artifactId>
</dependency>
```

## Configuration

### `apiPath`

Localisation de l'API générée (client ou serveur), relative au répertoire de génération.

Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

_Templating_: `{app}`, `{module}`

_Valeur par défaut_: `"javagen:{app:path}/api/{module:path}"`

_Variables par tag_: **oui** (plusieurs clients/serveurs pourraient être générés si un fichier a plusieurs tags)

### `apiGeneration`

Mode de génération de l'API (`"Client"` ou `"Server"`).

_Variables par tag_: **oui** (la valeur de la variable doit être `"Client"` ou `"Server"`. Le client et le serveur pourraient être générés si un fichier a plusieurs tags)

### `clientApiGeneration`

Mode de génération de l'API Client. Les valeurs possibles sont :

- `RestClient` : Génération d'un client en mode RestClient (interface Exchange) - valeur par défaut
- `RestTemplate` : Génération d'un client en mode RestTemplate (classe abstraite à initialiser)
- `FeignClient` : Génération d'un client en mode Feign (interface spring controller avec l'annotation Feign)

Cette propriété n'est utilisée que lorsque `apiGeneration` est défini à `"Client"` ou contient une variable qui peut être résolue à `"Client"`.

_Valeur par défaut_: `RestClient`

_Variables par tag_: **non**

### `apisName`

Nom des classes d'API générées. Permet de personnaliser le nom des interfaces/classes d'API.

_Templating_: `{fileName}` (remplacé par le nom du fichier en PascalCase)

_Valeur par défaut_: Dépend du type d'API générée (par exemple, `{fileName}Api` pour les clients, `{fileName}Controller` pour les serveurs)

**Exemple :**

```yaml
jpa:
  - tags:
      - api
    apiGeneration: Server
    apisName: "{fileName}Service"  # Génère UtilisateurService au lieu de UtilisateurController
```

### `openApiAnnotations`

Si les annotations `swagger-annotation-jakarta` doivent être ajoutées aux interfaces. Nécessite à minima la dépendance :

```xml
<!-- https://mvnrepository.com/artifact/io.swagger.core.v3/swagger-annotations-jakarta -->
<dependency>
    <groupId>io.swagger.core.v3</groupId>
    <artifactId>swagger-annotations-jakarta</artifactId>
</dependency>
```

_Templating_: `{module}`

_Valeur par défaut_: `false`

_Variables par tag_: **oui** (plusieurs clients/serveurs pourraient être générés si un fichier a plusieurs tags)
