# Jpa Generator

## Présentation

Le module JPA contient les générateurs pour écrire les fichiers suivants :

- Un fichier de définition de classe pour chaque classe dans le modèle.
- Un fichier de définition de classe pour chaque enum.
- Un fichier d'interface DAO `JpaRepository` pour chacune des classes persistées du modèle.
- Un (ou deux) fichier(s) par module avec les mappers des classes du module.
- Un fichier de contrôleur pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode serveur.
- Un fichier de client d'API pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode client.
- Des fichiers de resources contenant les traductions (`label`) du modèle.
- Les interfaces Hibernate Meta Models.

Sur toutes les classes et interfaces générées est ajoutée l'annotation `@Generated("TopModel : https://github.com/klee-contrib/topmodel")` pour permettre de retrouver la doc au cas où. Cette annotation peut être masquée avec le paramètre `generatedHint`.

## Générateurs

La documentation est organisée par générateur logique. Chacune des pages ci-dessous décrit le comportement du ou des générateurs concernés ainsi que les options de configuration qui leur sont propres :

| Nom                                                                                                              | Condition d'activation                                         | Objets ciblés                                                                | Fichiers générés                                                                                                                                                  |
| ---------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------- | ---------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [`JavaDtoGen`](/generator/jpa/classes)                                                                           | Toujours                                                       | Classes non abstraites, non persistées, qui ne sont pas des enums            | Pojo contenant les propriétés définies dans le modèle avec les annotations de validation                                                                          |
| [`JdbcEntityGen`](/generator/jpa/classes)                                                                        | `useJdbc: true`                                                | Classes non abstraites, persistées, qui ne sont pas en `enum: true`          | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance Jdbc                                                       |
| [`JpaDaoGen`](/generator/jpa/daos)                                                                               | `daosPath` défini                                              | Classes persistées qui ne sont pas des enums (hors `enum: class` modifiable) | Interface Repository permettant de requêter la classe en question                                                                                                 |
| [`JpaEntityGen`](/generator/jpa/classes)                                                                         | `useJdbc: false`                                               | Classes non abstraites, persistées, non `readonly`                           | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance JPA                                                        |
| [`JpaEnumEntityGen`](/generator/jpa/classes)                                                                     | `useJdbc: false`                                               | Classes non abstraites, persistées, avec `enum: class` et `readonly`         | Pojo persisté avec des membres statiques représentant les entités décrites dans les values                                                                        |
| [`JavaEnumClassPropGen`](/generator/jpa/classes)                                                                 | `uniqueValueGeneration` autorise les enums                     | Classes non abstraites avec `enum: class`                                    | Enumération des valeurs possibles de la clé primaire de la classe                                                                                                 |
| [`JavaEnumDtoGen`](/generator/jpa/classes)                                                                       | Toujours                                                       | Classes non abstraites, non persistées, avec `enum: class`                   | Pojo non persisté avec des membres statiques représentant les instances décrites dans les values                                                                  |
| [`JavaEnumEnumGen`](/generator/jpa/classes)                                                                      | Toujours                                                       | Classes non abstraites avec `enum: true`                                     | Enum contenant toutes les valeurs définies dans les values, dont la clé est la primaryKey ou la première propriété de la classe                                    |
| [`JavaUniqValPropGen`](/generator/jpa/classes)                                                                   | `uniqueValueGeneration` autorise les constantes                | Classes non abstraites qui ne sont pas en `enum: true`                       | Classe utilitaire de constantes `public static final` exposant les valeurs connues d'une propriété à clé d'unicité                                                 |
| [`JpaInterfaceGen`](/generator/jpa/classes)                                                                      | Toujours                                                       | Classes avec `abstract: true`                                                | Interface ne contenant que des `getters` des propriétés définies dans le modèle                                                                                   |
| [`SpringDataFlowGen`](/generator/jpa/dataflows)                                                                  | `dataFlowsPath` défini                                         | Dataflows                                                                    | Définition d'un job par module, et d'un step par dataFlow                                                                                                         |
| [`FeignClientApiGen`](/generator/jpa/endpoints)                                                                  | `apiGeneration: client` && `clientApiGeneration: feignClient`  | Endpoints                                                                    | Interface contenant les annotations nécessaires à la construction par Feign d'une API cliente                                                                      |
| [`SpringApiClientGen`](/generator/jpa/endpoints)                                                                 | `apiGeneration: client` && `clientApiGeneration: restClient`   | Endpoints                                                                    | Interface contenant les annotations `@XXXExchange` pour la génération d'un client API via `HttpServiceProxyFactory`                                               |
| [`SpringRestTemplateGen`](/generator/jpa/endpoints)                                                              | `apiGeneration: client` && `clientApiGeneration: restTemplate` | Endpoints                                                                    | Classe abstraite définissant les méthodes permettant d'appeler une API externe à l'aide d'un `RestTemplate` Spring                                                 |
| [`SpringApiServerGen`](/generator/jpa/endpoints)                                                                 | `apiGeneration: server`                                        | Endpoints                                                                    | Interface définissant les méthodes annotées permettant de définir une API serveur. L'implémentation est à la charge du développeur                                |
| [`JpaMapperGenerator`](/generator/jpa/mappers)                                                                   | Toujours                                                       | Mappers                                                                      | Classe statique contenant des méthodes statiques, correspondant aux mappers définis dans le modèle                                                                |
| [`JpaResourceGen`](/generator/jpa/resources)                                                                     | `resourcesPath` défini                                         | Classes qui contiennent des labels ou des values qui ont des defaultProperty | Fichiers de resource `.properties` dans les différentes langues de l'application                                                                                  |
| [`JpaMetaModelGen`](/generator/jpa/metamodel)                                                                    | `metaModel: true` && `useJdbc: false`                          | Classes persistées non abstraites qui ne sont pas en `enum: true`            | Classes représentant le métamodèle des entités persistées. Une classe par entité avec le suffixe `_`                                                               |

Pour la configuration **transverse** (options globales qui ne sont pas rattachées à un générateur particulier), consultez la page [Configuration](/generator/jpa/configuration).

## Dépendances

### Modèle

Le modèle généré par TopModel dépend d'une api de persistence. Par défaut, c'est l'API de persistence `javax` qui est utilisée, mais le mode `jakarta` est aussi disponible.

La validation est gérée par le package `jakarta.validation-api`, dont les imports changent entre la version 2 et la version 3.

#### Javax (spring-boot < v3)

```xml
<!-- https://mvnrepository.com/artifact/javax.persistence/javax.persistence-api -->
<dependency>
    <groupId>javax.persistence</groupId>
    <artifactId>javax.persistence-api</artifactId>
</dependency>

<!-- https://mvnrepository.com/artifact/jakarta.validation/jakarta.validation-api -->
<dependency>
  <groupId>jakarta.validation</groupId>
  <artifactId>jakarta.validation-api</artifactId>
</dependency>
```

#### Jakarta (spring-boot > v3)

```xml
<!-- https://mvnrepository.com/artifact/jakarta.persistence/jakarta.persistence-api -->
<dependency>
    <groupId>jakarta.persistence</groupId>
    <artifactId>jakarta.persistence-api</artifactId>
    <version>3.1.0</version>
</dependency>

<!-- https://mvnrepository.com/artifact/jakarta.validation/jakarta.validation-api -->
<dependency>
  <groupId>jakarta.validation</groupId>
  <artifactId>jakarta.validation-api</artifactId>
</dependency>
```

### Endpoints

Actuellement, la seule génération d'endpoint cliente et serveur qui est gérée passe par les API de `Spring-web` :

```xml
<!-- https://mvnrepository.com/artifact/org.springframework/spring-web -->
<dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-web</artifactId>
</dependency>
```

Si l'option `openApiAnnotations` est activée, les annotations de cette librairie sont utilisées :

```xml
<!-- https://mvnrepository.com/artifact/io.swagger.core.v3/swagger-annotations-jakarta -->
<dependency>
    <groupId>io.swagger.core.v3</groupId>
    <artifactId>swagger-annotations-jakarta</artifactId>
</dependency>
```

### Version Java

Le code Java généré est compatible avec toutes les versions de Java postérieures à `Java 11`.

## Configuration Maven

Pour ajouter les sources du dossier `javagen` au build, vous pouvez utiliser la configuration suivante :

```xml
<!-- Ajout des sources générées -->
<plugin>
    <groupId>org.codehaus.mojo</groupId>
    <artifactId>build-helper-maven-plugin</artifactId>
    <executions>
        <execution>
            <id>add-source</id>
            <phase>generate-sources</phase>
            <goals>
                <goal>add-source</goal>
            </goals>
            <configuration>
                <sources>
                    <source>src/main/javagen</source>
                </sources>
            </configuration>
        </execution>
    </executions>
</plugin>
```

## Snippets

Des snippets prêts à l'emploi pour les **domaines** et les **décorateurs** couramment utilisés avec le générateur JPA sont disponibles sur la page [Snippets](/generator/jpa/snippets).
