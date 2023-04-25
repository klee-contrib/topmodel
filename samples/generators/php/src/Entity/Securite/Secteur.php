////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

<?php

namespace App\Entity\Securite;

use App\Repository\Securite\SecteurRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\GeneratedValue;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToOne;
use Doctrine\ORM\Mapping\Table;

#[Doctrine\ORM\Mapping\Entity(repositoryClass: SecteurRepository::class)]
#[Doctrine\ORM\Mapping\Table(name: 'SECTEUR')]
class Secteur
{
  #[Doctrine\ORM\Mapping\Id]
  #[ORM\GeneratedValue]
  #[Doctrine\ORM\Mapping\Column(name: 'SEC_ID')]
  private int $id;

  #[ManyToOne(targetEntity: Profil::class)]
  #[JoinColumn(name: 'PRO_ID', referencedColumnName: 'PRO_ID')]
  private Profil profil;


  public function getId() : int
  {
    return $this->id;
  }

  public function getProfil() : Profil|null
  {
    return $this->profil;
  }

  public function setId(int|null $id) : self
  {
    $this->id = $id;

    return $this;
  }

  public function setProfil(Profil|null $profil) : self
  {
    $this->profil = $profil;

    return $this;
  }
}
