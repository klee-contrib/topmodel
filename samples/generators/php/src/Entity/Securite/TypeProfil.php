////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

<?php

namespace App\Entity\Securite;

use App\Repository\Securite\TypeProfilRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\Table;

#[Doctrine\ORM\Mapping\Entity(repositoryClass: TypeProfilRepository::class)]
#[Doctrine\ORM\Mapping\Table(name: 'TYPE_PROFIL')]
class TypeProfil
{
  #[Doctrine\ORM\Mapping\Id]
  #[Doctrine\ORM\Mapping\Column(name: 'TPR_CODE')]
  private string $code;

  #[Doctrine\ORM\Mapping\Column(name: 'TPR_LIBELLE')]
  private string $libelle;


  public function getCode() : string
  {
    return $this->code;
  }

  public function getLibelle() : string
  {
    return $this->libelle;
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
}
