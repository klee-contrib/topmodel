import { themes as prismThemes } from "prism-react-renderer";
import type { Config } from "@docusaurus/types";
import type * as Preset from "@docusaurus/preset-classic";

const config: Config = {
  title: "TopModel",
  tagline: "Modélisez en toute simplicité",
  favicon: "img/IconDark.svg",

  future: {
    v4: true,
  },

  url: "https://klee-contrib.github.io",
  baseUrl: "",

  organizationName: "klee-contrib",
  projectName: "topmodel",

  onBrokenLinks: "warn",
  onBrokenAnchors: "warn",

  plugins: [
    [
      "@cmfcmf/docusaurus-search-local",
      {
        indexBlog: false,
        language: ["fr", "en"],
      },
    ],
  ],
  i18n: {
    defaultLocale: "fr",
    locales: ["fr", "en"],
    localeConfigs: {
      fr: {
        label: "Français",
        htmlLang: "fr",
      },
      en: {
        label: "English",
        htmlLang: "en",
      },
    },
  },

  markdown: {
    mermaid: true,
    format: "detect",
    hooks: {
      onBrokenMarkdownImages: "warn",
    },
  },
  themes: ["@docusaurus/theme-mermaid"],

  presets: [
    [
      "classic",
      {
        docs: {
          sidebarPath: "./sidebars.ts",
          routeBasePath: "/",
          editUrl:
            "https://github.com/klee-contrib/topmodel/edit/develop/docs-v2/docs/",
        },
        blog: false,
        theme: {
          customCss: "./src/css/custom.css",
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    colorMode: {
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: "TopModel",
      logo: {
        alt: "TopModel",
        src: "img/logo-light.svg",
        srcDark: "img/logo-Dark.svg",
      },
      items: [
        {
          type: "docSidebar",
          sidebarId: "docsSidebar",
          position: "left",
          label: "Documentation",
        },
        // {
        //   type: "localeDropdown",
        //   position: "right",
        // },
        {
          type: "html",
          value:
            '<a href="https://github.com/klee-contrib/topmodel" target="_blank" rel="noopener noreferrer" class="navbar__item navbar__link header-github-link" aria-label="GitHub repository"></a>',
          position: "right",
        },
      ],
    },
    footer: {
      style: "dark",
      links: [
        {
          title: "Documentation",
          items: [
            {
              label: "Présentation",
              to: "/",
            },
            {
              label: "Tutoriel",
              to: "/getting-started/getting_started",
            },
          ],
        },
        {
          title: "Community",
          items: [
            {
              label: "Github discussions",
              href: "https://github.com/klee-contrib/topmodel/discussions",
            },
          ],
        },
        {
          title: "Dépôt",
          items: [
            {
              label: "GitHub",
              href: "https://github.com/klee-contrib/topmodel",
            },
            {
              label: "Changelog",
              href: "https://github.com/klee-contrib/topmodel/blob/develop/CHANGELOG.md",
            },
          ],
        },
      ],
    },
    prism: {
      theme: prismThemes.oneLight,
      darkTheme: prismThemes.oneDark,
      additionalLanguages: ["bash", "csharp", "java", "sql", "typescript"],
    },
  } satisfies Preset.ThemeConfig,
};
export default config;
