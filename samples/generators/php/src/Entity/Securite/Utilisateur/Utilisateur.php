<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Securite\Utilisateur;

use App\Entity\Securite\Profil\Profil;
use App\Repository\Securite\Utilisateur\UtilisateurRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\GeneratedValue;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToOne;
use Doctrine\ORM\Mapping\SequenceGenerator;
use Doctrine\ORM\Mapping\Table;
use Doctrine\ORM\Mapping\UniqueConstraint;

#[Entity(repositoryClass: UtilisateurRepository::class)]
#[Table(name: 'UTILISATEUR')]
#[UniqueConstraint(name: "UTI_EMAIL_UNIQ", columns: ["UTI_EMAIL"])]
class Utilisateur
{
  #[Id]
  #[GeneratedValue(strategy: "SEQUENCE")]
  #[SequenceGenerator(sequenceName: "SEQ_UTILISATEUR")]
  #[Column(name: 'UTI_ID')]
  private int $id;

  #[Column(name: 'UTI_NOM', length: 100)]
  private string $nom;

  #[Column(name: 'UTI_PRENOM', length: 100)]
  private string $prenom;

  #[Column(name: 'UTI_EMAIL', length: 50)]
  private string $email;

  #[Column(name: 'UTI_DATE_NAISSANCE', nullable: true)]
  private Date|null $dateNaissance;

  #[Column(name: 'UTI_ACTIF')]
  private bool $actif = true;

  #[ManyToOne(targetEntity: Profil::class)]
  #[JoinColumn(name: 'PRO_ID', referencedColumnName: 'PRO_ID')]
  private Profil $profil;

  #[ManyToOne(targetEntity: TypeUtilisateur::class)]
  #[JoinColumn(name: 'TUT_CODE', referencedColumnName: 'TUT_CODE')]
  private TypeUtilisateur $typeUtilisateur;

  #[Column(name: 'UTI_DATE_CREATION')]
  private Date $dateCreation = now;

  #[Column(name: 'UTI_DATE_MODIFICATION', nullable: true)]
  private Date|null $dateModification = now;

  public function getId(): int
  {
    return $this->id;
  }

  public function getNom(): string
  {
    return $this->nom;
  }

  public function getPrenom(): string
  {
    return $this->prenom;
  }

  public function getEmail(): string
  {
    return $this->email;
  }

  public function getDateNaissance(): Date|null
  {
    return $this->dateNaissance;
  }

  public function getActif(): bool
  {
    return $this->actif;
  }

  public function getProfil(): Profil
  {
    return $this->profil;
  }

  public function getTypeUtilisateur(): TypeUtilisateur
  {
    return $this->typeUtilisateur;
  }

  public function getDateCreation(): Date
  {
    return $this->dateCreation;
  }

  public function getDateModification(): Date|null
  {
    return $this->dateModification;
  }

  public function setId(int|null $id): self
  {
    $this->id = $id;

    return $this;
  }

  public function setNom(string|null $nom): self
  {
    $this->nom = $nom;

    return $this;
  }

  public function setPrenom(string|null $prenom): self
  {
    $this->prenom = $prenom;

    return $this;
  }

  public function setEmail(string|null $email): self
  {
    $this->email = $email;

    return $this;
  }

  public function setDateNaissance(Date|null $dateNaissance): self
  {
    $this->dateNaissance = $dateNaissance;

    return $this;
  }

  public function setActif(bool|null $actif): self
  {
    $this->actif = $actif;

    return $this;
  }

  public function setProfil(Profil|null $profil): self
  {
    $this->profil = $profil;

    return $this;
  }

  public function setTypeUtilisateur(TypeUtilisateur|null $typeUtilisateur): self
  {
    $this->typeUtilisateur = $typeUtilisateur;

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
}
