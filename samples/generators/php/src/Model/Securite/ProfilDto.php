////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

<?php

namespace App\Model\Securite;


class ProfilDto
{
  private int $id;

  private string $typeProfilCode;

  private string $droits;

  private List<UtilisateurDto> $utilisateurs;

  private List<SecteurDto> $secteurs;


  public function getId() : int
  {
    return $this->id;
  }

  public function getTypeProfilCode() : string|null
  {
    return $this->typeProfilCode;
  }

  public function getDroits() : string|null
  {
    return $this->droits;
  }

  public function getUtilisateurs() : List<UtilisateurDto>|null
  {
    return $this->utilisateurs;
  }

  public function getSecteurs() : List<SecteurDto>|null
  {
    return $this->secteurs;
  }

  public function setId(int|null $id) : self
  {
    $this->id = $id;

    return $this;
  }

  public function setTypeProfilCode(string|null $typeProfilCode) : self
  {
    $this->typeProfilCode = $typeProfilCode;

    return $this;
  }

  public function setDroits(string|null $droits) : self
  {
    $this->droits = $droits;

    return $this;
  }

  public function setUtilisateurs(List<UtilisateurDto>|null $utilisateurs) : self
  {
    $this->utilisateurs = $utilisateurs;

    return $this;
  }

  public function setSecteurs(List<SecteurDto>|null $secteurs) : self
  {
    $this->secteurs = $secteurs;

    return $this;
  }
}
