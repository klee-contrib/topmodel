<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Securite\Profil;

use App\Repository\Securite\Profil\TypeDroitRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\Table;

#[Entity(repositoryClass: TypeDroitRepository::class)]
#[Table(name: 'TYPE_DROIT')]
class TypeDroit
{
  #[Id]
  #[Column(name: 'TDR_CODE', length: 10)]
  private string $code;

  #[Column(name: 'TDR_LIBELLE', length: 100)]
  private string $libelle;

  public function getCode(): string
  {
    return $this->code;
  }

  public function getLibelle(): string
  {
    return $this->libelle;
  }

  public function setCode(string|null $code): self
  {
    $this->code = $code;

    return $this;
  }

  public function setLibelle(string|null $libelle): self
  {
    $this->libelle = $libelle;

    return $this;
  }
}
