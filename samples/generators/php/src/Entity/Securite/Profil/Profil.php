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
use Doctrine\ORM\Mapping\InverseJoinColumn;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToMany;
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
   * @var Collection<Droit>
   */
  #[JoinColumn(name: 'PRO_ID', referencedColumnName: 'PRO_ID')]
  #[InverseJoinColumn(name: 'DRO_CODE', referencedColumnName: 'DRO_CODE')]
  #[ManyToMany(targetEntity: Droit::class)]
  private Collection $droits;

  #[Column(name: 'PRO_DATE_CREATION')]
  private Date $dateCreation = now;

  #[Column(name: 'PRO_DATE_MODIFICATION', nullable: true)]
  private Date|null $dateModification = now;

  /**
   * @var Collection<Utilisateur>
   */
  #[OneToMany(mappedBy: 'profil', targetEntity: Utilisateur::class)]
  private Utilisateur $utilisateurs;

  public function __construct()
  {
    $this->droits = new ArrayCollection();
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
   * @return Collection<Droit>|null
   */
  public function getDroits(): Collection|null
  {
    return $this->droits;
  }

  public function getDateCreation(): Date
  {
    return $this->dateCreation;
  }

  public function getDateModification(): Date|null
  {
    return $this->dateModification;
  }

  /**
   * @return Collection<Utilisateur>|null
   */
  public function getUtilisateurs(): Utilisateur|null
  {
    return $this->utilisateurs;
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
   * @param Collection<Droit>|null $droits
   */
  public function setDroits(Collection|null $droits): self
  {
    $this->droits = $droits;

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

  /**
   * @param Collection<Utilisateur>|null $utilisateurs
   */
  public function setUtilisateurs(Utilisateur|null $utilisateurs): self
  {
    $this->utilisateurs = $utilisateurs;

    return $this;
  }
}
