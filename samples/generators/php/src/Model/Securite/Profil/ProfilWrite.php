<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Model\Securite\Profil;

use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use NotNull;
use Symfony\Component\Validator\Constraints\Length;

class ProfilWrite
{
  #[Symfony\Component\Validator\Constraints\NotNull]
  #[Length(max: 3)]
  private string $libelle;

  #[Length(max: 3)]
  private Collection|null $droits;

  public function __construct()
  {
    $this->droits = new ArrayCollection();
  }

  public function getLibelle(): string
  {
    return $this->libelle;
  }

  public function getDroits(): Collection|null
  {
    return $this->droits;
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
}
