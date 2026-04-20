import type { ReactNode } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Layout from "@theme/Layout";
import CodeBlock from "@theme/CodeBlock";
import Tabs from "@theme/Tabs";
import TabItem from "@theme/TabItem";

import styles from "./index.module.css";

type Feature = {
  icon: string;
  title: string;
  description: ReactNode;
};

const FEATURES: Feature[] = [
  {
    icon: "📝",
    title: "Modélisation simple",
    description:
      "Un modèle de données décrit en YAML, facilement lisible, éditable, comparable et compatible Git.",
  },
  {
    icon: "⚙️",
    title: "Génération multi-langages",
    description:
      "Générez du code idiomatique pour C#, Java/JPA, JavaScript/TypeScript et SQL à partir d'un modèle unique.",
  },
  {
    icon: "🧩",
    title: "Non intrusif",
    description:
      "Pas de runtime, pas de framework imposé. Le code généré est standard et « débranchable » à tout moment.",
  },
  {
    icon: "🔌",
    title: "Extension VSCode",
    description:
      "Autocomplétion, validation temps réel, navigation et prévisualisation UML directement dans l'éditeur.",
  },
  {
    icon: "🏷️",
    title: "Système de tags",
    description:
      "Un seul modèle, plusieurs applications : filtrez finement ce qui est généré pour chaque cible.",
  },
  {
    icon: "🧬",
    title: "Extensible",
    description:
      "Créez vos propres générateurs personnalisés pour répondre aux besoins spécifiques de votre projet.",
  },
];

const STACKS: { label: string; language: string }[] = [
  { label: "C#", language: "csharp" },
  { label: "Java / JPA", language: "java" },
  { label: "TypeScript", language: "ts" },
  { label: "SQL", language: "sql" },
];

const MODEL_EXAMPLE = `---
module: Utilisateur
tags: [Back, Front]
---
class:
  name: Utilisateur
  trigram: UTI
  comment: Un utilisateur de l'application
  properties:
    - name: Id
      domain: DO_ID
      required: true
      primaryKey: true
    - name: Email
      domain: DO_EMAIL
      required: true
    - name: Nom
      domain: DO_LIBELLE
      required: true
`;

const GENERATED_CSHARP = `[Table("utilisateur")]
public partial record Utilisateur
{
    [Column("id")]
    [Domain(Domains.Id)]
    [Key]
    public long? Id { get; set; }

    [Column("email")]
    [Required]
    [Domain(Domains.Email)]
    [StringLength(50)]
    public string Email { get; set; }

    [Column("nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(15)]
    public string Nom { get; set; }
}`;

const GENERATED_JAVA = `@Entity
@Table(name = "UTILISATEUR")
@Generated("TopModel")
public class Utilisateur {

    @Id
    @Column(name = "ID", nullable = false)
    private long id;

    @Column(name = "EMAIL", length = 50, nullable = false)
    private String email;

    @Column(name = "NOM", length = 15, nullable = false)
    private String nom;

    public long getId() { return this.id; }
    public void setId(long id) { this.id = id; }

    public String getEmail() { return this.email; }
    public void setEmail(String email) { this.email = email; }

    public String getNom() { return this.nom; }
    public void setNom(String nom) { this.nom = nom; }
}`;

function Hero(): ReactNode {
  const { siteConfig } = useDocusaurusContext();

  return (
    <header className={styles.hero}>
      <div className={styles.heroInner}>
        <h1 className={styles.heroTitle}>{siteConfig.title}</h1>
        <h2 className={styles.heroTagline}>{siteConfig.tagline}</h2>
        <p className={styles.heroDescription}>
          Décrivez votre modèle de données une seule fois, en YAML, et générez
          un code idiomatique et standard pour l'ensemble de votre stack.
        </p>
        <div className={styles.heroActions}>
          <Link
            className={clsx("button button--primary button--lg", styles.cta)}
            to="/getting-started/getting_started"
          >
            Démarrer le tutoriel
          </Link>
          <Link
            className={clsx("button button--secondary button--lg", styles.cta)}
            to="/model"
          >
            Lire la documentation
          </Link>
        </div>
      </div>
    </header>
  );
}

function Features(): ReactNode {
  return (
    <section className={styles.section}>
      <div className={clsx("container", styles.sectionInner)}>
        <h2 className={styles.sectionTitle}>Pourquoi TopModel&nbsp;?</h2>
        <p className={styles.sectionSubtitle}>
          Un outil de modélisation pensé pour la simplicité, la collaboration et
          la pérennité.
        </p>
        <div className={styles.featuresGrid}>
          {FEATURES.map((feature) => (
            <article key={feature.title} className={styles.featureCard}>
              <div className={styles.featureIcon} aria-hidden>
                {feature.icon}
              </div>
              <h3 className={styles.featureTitle}>{feature.title}</h3>
              <p className={styles.featureDescription}>{feature.description}</p>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}

function Showcase(): ReactNode {
  return (
    <section className={clsx(styles.section, styles.sectionAlt)}>
      <div className={clsx("container", styles.sectionInner)}>
        <h2 className={styles.sectionTitle}>
          Du modèle au code, en une seule commande
        </h2>
        <p className={styles.sectionSubtitle}>
          Décrivez votre classe en YAML, lancez <code>modgen</code>, et obtenez
          du code propre, prêt à l'emploi.
        </p>
        <div className={styles.showcaseGrid}>
          <div className={styles.showcaseColumn}>
            <div className={styles.showcaseLabel}>
              <span className={styles.showcaseStep} aria-hidden="true">
                1
              </span>
              <span>Je modélise en YAML</span>
            </div>
            <CodeBlock language="yaml" title="utilisateur.tmd">
              {MODEL_EXAMPLE}
            </CodeBlock>
          </div>
          <div className={styles.showcaseArrow} aria-hidden>
            →
          </div>
          <div className={styles.showcaseColumn}>
            <div className={styles.showcaseLabel}>
              <span className={styles.showcaseStep} aria-hidden="true">
                2
              </span>
              <span>J'obtiens du code généré</span>
            </div>
            <div className={styles.showcaseTabs}>
              <Tabs groupId="showcase-lang">
                <TabItem value="java" label="Java / JPA">
                  <CodeBlock language="java" title="Utilisateur.java">
                    {GENERATED_JAVA}
                  </CodeBlock>
                </TabItem>
                <TabItem value="csharp" label="C#" default>
                  <CodeBlock language="csharp" title="Utilisateur.cs">
                    {GENERATED_CSHARP}
                  </CodeBlock>
                </TabItem>
              </Tabs>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function Stacks(): ReactNode {
  return (
    <section className={styles.section}>
      <div className={clsx("container", styles.sectionInner)}>
        <h2 className={styles.sectionTitle}>Un modèle, toute votre stack</h2>
        <p className={styles.sectionSubtitle}>
          TopModel fournit des générateurs pour les langages et frameworks les
          plus courants.
        </p>
        <div className={styles.stacksGrid}>
          {STACKS.map((stack) => (
            <div key={stack.label} className={styles.stackChip}>
              {stack.label}
            </div>
          ))}
        </div>
        <div className={styles.stacksLinks}>
          <Link to="/generator">Voir tous les générateurs →</Link>
        </div>
      </div>
    </section>
  );
}

function FinalCta(): ReactNode {
  return (
    <section
      className={clsx(styles.section, styles.sectionCta)}
      aria-labelledby="final-cta-title"
    >
      <div className={clsx("container", styles.sectionInner)}>
        <h2 id="final-cta-title" className={styles.sectionTitle}>
          Prêt à modéliser&nbsp;?
        </h2>
        <p className={styles.ctaText}>
          Parcourez le tutoriel pas-à-pas ou plongez directement dans la
          référence complète.
        </p>
        <div className={styles.heroActions}>
          <Link
            className={clsx("button button--primary button--lg", styles.cta)}
            to="/getting-started/getting_started"
          >
            Commencer maintenant
          </Link>
          <Link
            className={clsx("button button--outline button--lg", styles.cta)}
            to="https://github.com/klee-contrib/topmodel"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Voir TopModel sur GitHub (ouvre un nouvel onglet)"
          >
            Voir sur GitHub
          </Link>
        </div>
        <pre className={styles.installCommand}>
          <code>dotnet tool install --global TopModel.Generator</code>
        </pre>
      </div>
    </section>
  );
}

export default function Home(): ReactNode {
  const { siteConfig } = useDocusaurusContext();
  return (
    <Layout
      title={`${siteConfig.title} — ${siteConfig.tagline}`}
      description="TopModel : modélisez votre modèle de données en YAML et générez du code pour C#, Java, TypeScript et SQL."
    >
      <Hero />
      <main>
        <Features />
        <Showcase />
        <Stacks />
        <FinalCta />
      </main>
    </Layout>
  );
}
