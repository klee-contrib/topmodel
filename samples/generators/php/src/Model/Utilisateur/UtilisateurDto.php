<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Model\Utilisateur;

use Symfony\Component\Validator\Constraints\Length;

class UtilisateurDto
{
  private int $id;

  #[Length(max: 20)]
  private int $age = 6l;

  private int $profilId;

  #[Length(max: 50)]
  private string $email;

  #[Length(max: 3)]
  private string $nom = "Jabx";

  private bool $actif;

  #[Length(max: 3)]
  private string $typeUtilisateurCode = TypeUtilisateur.ADM;

  private int $utilisateursEnfant;

  private Date $dateCreation;

  private Date $dateModification;

  private UtilisateurDto $utilisateurParent;

  public function getId() : int
  {
    return $this->id;
  }

  public function getAge() : int|null
  {
    return $this->age;
  }

  public function getProfilId() : int|null
  {
    return $this->profilId;
  }

  public function getEmail() : string|null
  {
    return $this->email;
  }

  public function getNom() : string|null
  {
    return $this->nom;
  }

  public function getActif() : bool|null
  {
    return $this->actif;
  }

  public function getTypeUtilisateurCode() : string|null
  {
    return $this->typeUtilisateurCode;
  }

  public function getUtilisateursEnfant() : int|null
  {
    return $this->utilisateursEnfant;
  }

  public function getDateCreation() : Date|null
  {
    return $this->dateCreation;
  }

  public function getDateModification() : Date|null
  {
    return $this->dateModification;
  }

  public function getUtilisateurParent() : UtilisateurDto|null
  {
    return $this->utilisateurParent;
  }

  public function setId(int|null $id) : self
  {
    $this->id = $id;

    return $this;
  }

  public function setAge(int|null $age) : self
  {
    $this->age = $age;

    return $this;
  }

  public function setProfilId(int|null $profilId) : self
  {
    $this->profilId = $profilId;

    return $this;
  }

  public function setEmail(string|null $email) : self
  {
    $this->email = $email;

    return $this;
  }

  public function setNom(string|null $nom) : self
  {
    $this->nom = $nom;

    return $this;
  }

  public function setActif(bool|null $actif) : self
  {
    $this->actif = $actif;

    return $this;
  }

  public function setTypeUtilisateurCode(string|null $typeUtilisateurCode) : self
  {
    $this->typeUtilisateurCode = $typeUtilisateurCode;

    return $this;
  }

  public function setUtilisateursEnfant(int|null $utilisateursEnfant) : self
  {
    $this->utilisateursEnfant = $utilisateursEnfant;

    return $this;
  }

  public function setDateCreation(Date|null $dateCreation) : self
  {
    $this->dateCreation = $dateCreation;

    return $this;
  }

  public function setDateModification(Date|null $dateModification) : self
  {
    $this->dateModification = $dateModification;

    return $this;
  }

  public function setUtilisateurParent(UtilisateurDto|null $utilisateurParent) : self
  {
    $this->utilisateurParent = $utilisateurParent;

    return $this;
  }
}
