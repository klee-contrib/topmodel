<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Securite;

use App\Repository\Securite\TypeProfilRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\Table;

#[Entity(repositoryClass: TypeProfilRepository::class)]
#[Table(name: 'TYPE_PROFIL')]
class TypeProfil
{
  #[Id]
  #[Column(name: 'TPR_CODE', length: 3)]
  private string $code;

  #[Column(name: 'TPR_LIBELLE', length: 3)]
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
