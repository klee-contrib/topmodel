<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Securite\Profil;

use App\Repository\Securite\Profil\ProfilDroitRepository;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToOne;
use Doctrine\ORM\Mapping\Table;

#[Entity(repositoryClass: ProfilDroitRepository::class)]
#[Table(name: 'PROFIL_DROIT')]
class ProfilDroit
{
  #[ManyToOne(targetEntity: Profil::class)]
  #[JoinColumn(name: 'PRO_ID', referencedColumnName: 'PRO_ID')]
  private Profil $profil;

  #[ManyToOne(targetEntity: Droit::class)]
  #[JoinColumn(name: 'DRO_CODE', referencedColumnName: 'DRO_CODE')]
  private Droit $droit;

  public function getProfil(): Profil
  {
    return $this->profil;
  }

  public function getDroit(): Droit
  {
    return $this->droit;
  }

  public function setProfil(Profil|null $profil): self
  {
    $this->profil = $profil;

    return $this;
  }

  public function setDroit(Droit|null $droit): self
  {
    $this->droit = $droit;

    return $this;
  }
}
