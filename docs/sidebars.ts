import type { SidebarsConfig } from "@docusaurus/plugin-content-docs";

const sidebars: SidebarsConfig = {
  docsSidebar: [
    {
      type: "category",
      label: "Tutoriel",
      link: { type: "doc", id: "getting-started/getting_started" },
      items: [
        "getting-started/intro",
        "getting-started/classe_persistee",
        "getting-started/liste_ref",
        "getting-started/association",
        "getting-started/dto",
        "getting-started/endpoint",
        "getting-started/generation",
        "getting-started/vscode",
      ],
    },
    {
      type: "category",
      label: "Modélisation",
      link: { type: "doc", id: "model" },
      items: [
        "model/domains",
        "model/properties",
        "model/classes",
        "model/endpoints",
        "model/mappers",
        "model/annotations",
        "model/decorators",
        "model/dataFlows",
        "model/i18n",
        "model/templating",
      ],
    },
    "configuration",
    {
      type: "category",
      label: "Génération",
      link: { type: "doc", id: "generator" },
      items: [
        { type: "doc", id: "generator/csharp", label: "C#" },
        {
          type: "category",
          label: "JPA",
          link: { type: "doc", id: "generator/jpa" },
          items: [
            "generator/jpa/configuration",
            "generator/jpa/classes",
            "generator/jpa/daos",
            "generator/jpa/mappers",
            "generator/jpa/endpoints",
            "generator/jpa/dataflows",
            "generator/jpa/resources",
            "generator/jpa/metamodel",
            "generator/jpa/snippets",
          ],
        },
        {
          type: "category",
          label: "JavaScript",
          link: { type: "doc", id: "generator/js" },
          items: [
            { type: "doc", id: "generator/js/angular", label: "Angular" },
          ],
        },
        {
          type: "doc",
          id: "generator/translation",
          label: "Translation",
        },
        { type: "doc", id: "generator/sql", label: "SQL" },
      ],
    },
    {
      type: "category",
      label: "Migration d'un modèle externe",
      link: { type: "doc", id: "tmdgen" },
      items: ["tmdgen/openapi", "tmdgen/database"],
    },
    "cli",
  ],
};

export default sidebars;
