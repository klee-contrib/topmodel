<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Securite\Profil;

use App\Repository\Securite\Profil\DroitRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToOne;
use Doctrine\ORM\Mapping\Table;

#[Entity(repositoryClass: DroitRepository::class)]
#[Table(name: 'DROIT')]
class Droit
{
  #[Id]
  #[Column(name: 'DRO_CODE', length: 10)]
  private string $code;

  #[Column(name: 'DRO_LIBELLE', length: 100)]
  private string $libelle;

  #[ManyToOne(targetEntity: TypeDroit::class)]
  #[JoinColumn(name: 'TDR_CODE', referencedColumnName: 'TDR_CODE')]
  private TypeDroit $typeDroit;

  public function getCode(): string
  {
    return $this->code;
  }

  public function getLibelle(): string
  {
    return $this->libelle;
  }

  public function getTypeDroit(): TypeDroit
  {
    return $this->typeDroit;
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

  public function setTypeDroit(TypeDroit|null $typeDroit): self
  {
    $this->typeDroit = $typeDroit;

    return $this;
  }
}
