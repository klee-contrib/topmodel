<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Model\Securite;

use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Symfony\Component\Validator\Constraints\Length;

class ProfilDto
{
  private int $id;

  #[Length(max: 3)]
  private string|null $typeProfilCode;

  #[Length(max: 3)]
  private Collection|null $droits;

  private Collection $utilisateurs;

  private Collection $secteurs;

  public function __construct()
  {
    $this->droits = new ArrayCollection();
    $this->utilisateurs = new ArrayCollection();
    $this->secteurs = new ArrayCollection();
  }

  public function getId(): int
  {
    return $this->id;
  }

  public function getTypeProfilCode(): string|null
  {
    return $this->typeProfilCode;
  }

  public function getDroits(): Collection|null
  {
    return $this->droits;
  }

  public function getUtilisateurs(): Collection|null
  {
    return $this->utilisateurs;
  }

  public function getSecteurs(): Collection|null
  {
    return $this->secteurs;
  }

  public function setId(int|null $id): self
  {
    $this->id = $id;

    return $this;
  }

  public function setTypeProfilCode(string|null $typeProfilCode): self
  {
    $this->typeProfilCode = $typeProfilCode;

    return $this;
  }

  public function setDroits(Collection|null $droits): self
  {
    $this->droits = $droits;

    return $this;
  }

  public function setUtilisateurs(Collection|null $utilisateurs): self
  {
    $this->utilisateurs = $utilisateurs;

    return $this;
  }

  public function setSecteurs(Collection|null $secteurs): self
  {
    $this->secteurs = $secteurs;

    return $this;
  }
}
