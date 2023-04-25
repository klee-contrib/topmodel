////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

<?php

namespace App\Entity\Securite;

use App\Repository\Securite\DroitRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\Table;

#[Doctrine\ORM\Mapping\Entity(repositoryClass: DroitRepository::class)]
#[Doctrine\ORM\Mapping\Table(name: 'DROIT')]
class Droit
{
  #[Doctrine\ORM\Mapping\Id]
  #[Doctrine\ORM\Mapping\Column(name: 'DRO_CODE')]
  private string $code;

  #[Doctrine\ORM\Mapping\Column(name: 'DRO_LIBELLE')]
  private string $libelle;

  #[ORM\ManyToOne(inversedBy: 'droits')]
  private TypeProfil typeProfil;


  public function getCode() : string
  {
    return $this->code;
  }

  public function getLibelle() : string
  {
    return $this->libelle;
  }

  public function getTypeProfil() : TypeProfil|null
  {
    return $this->typeProfil;
  }

  public function setCode(string|null $code) : self
  {
    $this->code = $code;

    return $this;
  }

  public function setLibelle(string|null $libelle) : self
  {
    $this->libelle = $libelle;

    return $this;
  }

  public function setTypeProfil(TypeProfil|null $typeProfil) : self
  {
    $this->typeProfil = $typeProfil;

    return $this;
  }
}
