<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Entity\Common;

use App\Repository\Common\TraductionRepository;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\Table;

#[Entity(repositoryClass: TraductionRepository::class)]
#[Table(name: 'TRADUCTION')]
class Traduction
{
  #[Id]
  #[Column(name: 'TRD_RESOURCE_KEY', length: 100)]
  private string $resourceKey;

  #[Column(name: 'TRD_LABEL', length: 100)]
  private string $label;

  public function getResourceKey(): string
  {
    return $this->resourceKey;
  }

  public function getLabel(): string
  {
    return $this->label;
  }

  public function setResourceKey(string|null $resourceKey): self
  {
    $this->resourceKey = $resourceKey;

    return $this;
  }

  public function setLabel(string|null $label): self
  {
    $this->label = $label;

    return $this;
  }
}
