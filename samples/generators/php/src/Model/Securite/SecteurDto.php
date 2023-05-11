<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Model\Securite;


class SecteurDto
{
  private int $id;

  public function getId(): int
  {
    return $this->id;
  }

  public function setId(int|null $id): self
  {
    $this->id = $id;

    return $this;
  }
}
