# JS Generator

## Configuration

### Modes de génération des fichiers de ressource

#### JS

La génération des listes de références se met en mode **JavaScript** lorsque le paramètre `resourceMode` est défini à `js`.

#### JSON

La génération des listes de références se met en mode **JSON** lorsque le paramètre `resourceMode` est défini à `json`.

### Modes de génération des listes de références

#### Définition (défaut)

La génération des listes de références se met en mode **Définition** lorsque le paramètre `referenceMode` est défini à `definition`.

Ce mode permet de générer, pour chaque liste de référence, une définition de ses propriétés (laquelle représente la clé primaire, laquelle représente le texte à afficher).

Exemple :

```typescript
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

```typescript
export type TypeProfilCode = "ADM" | "GES";

export interface TypeProfil {
  code: TypeProfilCode;
  libelle: string;
}

export const typeProfilList: TypeProfil[] = [
  {
    code: "ADM",
    libelle: "typeProfil.values.ADM",
  },
  {
    code: "GES",
    libelle: "typeProfil.values.GES",
  },
];
```

Si le paramètre `translateReferences` est passé à `false`, les clés de traductions ci-dessus seront remplacées par les libellés correspondant.
