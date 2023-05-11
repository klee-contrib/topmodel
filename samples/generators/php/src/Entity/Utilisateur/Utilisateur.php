<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Utilisateur;

use App\Entity\Securite\Profil;
use App\Repository\Utilisateur\UtilisateurRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\GeneratedValue;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToOne;
use Doctrine\ORM\Mapping\OneToMany;
use Doctrine\ORM\Mapping\OneToOne;
use Doctrine\ORM\Mapping\SequenceGenerator;
use Doctrine\ORM\Mapping\Table;
use Doctrine\ORM\Mapping\UniqueConstraint;

#[Entity(repositoryClass: UtilisateurRepository::class)]
#[Table(name: 'UTILISATEUR')]
#[UniqueConstraint(name: "UTI_EMAIL_UTI_ID_PARENT_UNIQ", columns: ["UTI_EMAIL, UTI_ID_PARENT"])]
class Utilisateur
{
  #[Id]
  #[GeneratedValue(strategy: "SEQUENCE")]
  #[SequenceGenerator(sequenceName: "SEQ_UTILISATEUR")]
  #[Column(name: 'UTI_ID')]
  private int $id;

  #[Column(name: 'UTI_AGE', length: 20, scale: 9, nullable: true)]
  private int|null $age = 6l;

  #[ManyToOne(targetEntity: Profil::class)]
  #[JoinColumn(name: 'PRO_ID', referencedColumnName: 'PRO_ID')]
  private Profil $profil;

  #[Column(name: 'UTI_EMAIL', length: 50, nullable: true)]
  private string|null $email;

  #[Column(name: 'UTI_NOM', length: 3, nullable: true)]
  private string|null $nom = "Jabx";

  #[Column(name: 'UTI_ACTIF', nullable: true)]
  private bool|null $actif;

  #[ManyToOne(targetEntity: TypeUtilisateur::class)]
  #[JoinColumn(name: 'TUT_CODE', referencedColumnName: 'TUT_CODE')]
  private TypeUtilisateur $typeUtilisateur;

  #[OneToOne(targetEntity: Utilisateur::class)]
  #[JoinColumn(name: 'UTI_ID_PARENT', referencedColumnName: 'UTI_ID')]
  private Utilisateur $utilisateurParent;

  /**
   * @var Collection<Utilisateur>
   */
  #[OneToMany(mappedBy: 'utilisateurEnfant', targetEntity: Utilisateur::class)]
  private Collection $utilisateursEnfant;

  #[Column(name: 'UTI_DATE_CREATION', nullable: true)]
  private Date|null $dateCreation;

  #[Column(name: 'UTI_DATE_MODIFICATION', nullable: true)]
  private Date|null $dateModification;

  #[ManyToOne(targetEntity: Utilisateur::class)]
  #[JoinColumn(name: 'UTI_ID_ENFANT', referencedColumnName: 'UTI_ID')]
  private Utilisateur $utilisateurEnfant;

  public function __construct()
  {
    $this->utilisateursEnfant = new ArrayCollection();
  }

  public function getId(): int
  {
    return $this->id;
  }

  public function getAge(): int|null
  {
    return $this->age;
  }

  public function getProfil(): Profil|null
  {
    return $this->profil;
  }

  public function getEmail(): string|null
  {
    return $this->email;
  }

  public function getNom(): string|null
  {
    return $this->nom;
  }

  public function getActif(): bool|null
  {
    return $this->actif;
  }

  public function getTypeUtilisateur(): TypeUtilisateur|null
  {
    return $this->typeUtilisateur;
  }

  public function getUtilisateurParent(): Utilisateur|null
  {
    return $this->utilisateurParent;
  }

  /**
   * @return Collection<Utilisateur>|null
   */
  public function getUtilisateursEnfant(): Collection|null
  {
    return $this->utilisateursEnfant;
  }

  public function getDateCreation(): Date|null
  {
    return $this->dateCreation;
  }

  public function getDateModification(): Date|null
  {
    return $this->dateModification;
  }

  public function getUtilisateurEnfant(): Utilisateur|null
  {
    return $this->utilisateurEnfant;
  }

  public function setId(int|null $id): self
  {
    $this->id = $id;

    return $this;
  }

  public function setAge(int|null $age): self
  {
    $this->age = $age;

    return $this;
  }

  public function setProfil(Profil|null $profil): self
  {
    $this->profil = $profil;

    return $this;
  }

  public function setEmail(string|null $email): self
  {
    $this->email = $email;

    return $this;
  }

  public function setNom(string|null $nom): self
  {
    $this->nom = $nom;

    return $this;
  }

  public function setActif(bool|null $actif): self
  {
    $this->actif = $actif;

    return $this;
  }

  public function setTypeUtilisateur(TypeUtilisateur|null $typeUtilisateur): self
  {
    $this->typeUtilisateur = $typeUtilisateur;

    return $this;
  }

  public function setUtilisateurParent(Utilisateur|null $utilisateurParent): self
  {
    $this->utilisateurParent = $utilisateurParent;

    return $this;
  }

  /**
   * @param Collection<Utilisateur>|null $utilisateursEnfant
   */
  public function setUtilisateursEnfant(Collection|null $utilisateursEnfant): self
  {
    $this->utilisateursEnfant = $utilisateursEnfant;

    return $this;
  }

  public function setDateCreation(Date|null $dateCreation): self
  {
    $this->dateCreation = $dateCreation;

    return $this;
  }

  public function setDateModification(Date|null $dateModification): self
  {
    $this->dateModification = $dateModification;

    return $this;
  }

  public function setUtilisateurEnfant(Utilisateur|null $utilisateurEnfant): self
  {
    $this->utilisateurEnfant = $utilisateurEnfant;

    return $this;
  }
}
