# JS Generator

## Configuration

### Modes de génération des fichiers de ressource

#### JS

La génération des listes de références se met en mode **JavaScript** lorsque le paramètre `resourceMode` est défini à `js`.

#### JSON

La génération des listes de références se met en mode **JSON** lorsque le paramètre `resourceMode` est défini à `json`.

#### SCHEMA

La génération des listes de références se met en mode **SCHEMA** lorsque le paramètre `resourceMode` est défini à `schema`.

Il permet de générer un schéma de validation JSON. Celui-ci permet de créer plus facilement un fichier de traduction. Il donne la possibilité d'avoir de l'auto-complétion, et donne la description du champ à traduire

### Modes de génération des listes de références

#### Définition (défaut)

La génération des listes de références se met en mode **Définition** lorsque le paramètre `referenceMode` est défini à `definition`.

Ce mode permet de générer, pour chaque liste de référence, une définition de ses propriétés (laquelle représente la clé primaire, laquelle représente le texte à afficher).

Exemple :

```ts
export type TypeProfilCode = "ADM" | "GES";
export interface TypeProfil {
  code: TypeProfilCode;
  libelle: string;
}
export const typeProfil = {
  type: {} as TypeProfil,
  valueKey: "code",
  labelKey: "libelle",
} as const;
```

#### Valeurs

La génération des listes de références se met en mode **Valeurs** lorsque le paramètre `referenceMode` est défini à `values`.

Il permet de générer l'ensemble des valeurs de chaque liste de référence telles que définies dans la propriété `values`. Ce mode est préconisé pour les listes de références statiques uniquement.

Exemple :

```ts
export type TypeProfilCode = "ADM" | "GES";

export interface TypeProfil {
  code: TypeProfilCode;
  libelle: string;
}

export const typeProfilList: TypeProfil[] = [
  {
    code: "ADM",
    libelle: "Administrateur",
  },
  {
    code: "GES",
    libelle: "Gestionnaire",
  },
];
```
