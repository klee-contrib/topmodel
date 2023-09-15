<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Model\Securite\Profil;

use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use NotNull;
use Symfony\Component\Validator\Constraints\Length;

class ProfilRead
{
  private int $id;

  #[Symfony\Component\Validator\Constraints\NotNull]
  #[Length(max: 3)]
  private string $libelle;

  #[Length(max: 3)]
  private Collection|null $droits;

  #[Symfony\Component\Validator\Constraints\NotNull]
  private Date $dateCreation;

  private Date|null $dateModification;

  private Collection $utilisateurs;

  public function __construct()
  {
    $this->droits = new ArrayCollection();
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

  public function getUtilisateurs(): Collection|null
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

  public function setUtilisateurs(Collection|null $utilisateurs): self
  {
    $this->utilisateurs = $utilisateurs;

    return $this;
  }
}
