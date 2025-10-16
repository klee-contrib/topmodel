<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Securite\Profil;

use App\Entity\Securite\Utilisateur\Utilisateur;
use App\Repository\Securite\Profil\ProfilRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\GeneratedValue;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\OneToMany;
use Doctrine\ORM\Mapping\SequenceGenerator;
use Doctrine\ORM\Mapping\Table;

#[Entity(repositoryClass: ProfilRepository::class)]
#[Table(name: 'PROFIL')]
class Profil
{
  #[Id]
  #[GeneratedValue(strategy: "SEQUENCE")]
  #[SequenceGenerator(sequenceName: "SEQ_PROFIL")]
  #[Column(name: 'PRO_ID')]
  private int $id;

  #[Column(name: 'PRO_LIBELLE', length: 100)]
  private string $libelle;

  /**
   * @var Collection<ProfilDroit>
   */
  #[OneToMany(mappedBy: 'profil', targetEntity: ProfilDroit::class)]
  private Collection $profilDroits;

  /**
   * @var Collection<Utilisateur>
   */
  #[OneToMany(mappedBy: 'profil', targetEntity: Utilisateur::class)]
  private Collection $utilisateurs;

  #[Column(name: 'PRO_DATE_CREATION')]
  private Date $dateCreation = now;

  #[Column(name: 'PRO_DATE_MODIFICATION', nullable: true)]
  private Date|null $dateModification = now;

  public function __construct()
  {
    $this->profilDroits = new ArrayCollection();
    $this->utilisateurs = new ArrayCollection();
  }

  public function getId(): int
  {
    return $this->id;
  }

  public function getLibelle(): string
  {
    return $this->libelle;
  }

  /**
   * @return Collection<ProfilDroit>
   */
  public function getProfilDroits(): Collection
  {
    return $this->profilDroits;
  }

  /**
   * @return Collection<Utilisateur>
   */
  public function getUtilisateurs(): Collection
  {
    return $this->utilisateurs;
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

  public function setLibelle(string|null $libelle): self
  {
    $this->libelle = $libelle;

    return $this;
  }

  /**
   * @param Collection<ProfilDroit> $profilDroits
   */
  public function setProfilDroits(Collection|null $profilDroits): self
  {
    $this->profilDroits = $profilDroits;

    return $this;
  }

  /**
   * @param Collection<Utilisateur> $utilisateurs
   */
  public function setUtilisateurs(Collection|null $utilisateurs): self
  {
    $this->utilisateurs = $utilisateurs;

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
