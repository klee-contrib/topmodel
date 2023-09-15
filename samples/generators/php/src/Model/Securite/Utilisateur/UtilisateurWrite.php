<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Model\Securite\Utilisateur;

use NotNull;
use Symfony\Component\Validator\Constraints\Length;

class UtilisateurWrite
{
  #[Symfony\Component\Validator\Constraints\NotNull]
  #[Length(max: 3)]
  private string $nom;

  #[Symfony\Component\Validator\Constraints\NotNull]
  #[Length(max: 3)]
  private string $prenom;

  #[Symfony\Component\Validator\Constraints\NotNull]
  #[Length(max: 50)]
  private string $email;

  private Date|null $dateNaissance;

  #[Symfony\Component\Validator\Constraints\NotNull]
  private bool $actif = true;

  #[Symfony\Component\Validator\Constraints\NotNull]
  private int $profilId;

  #[Symfony\Component\Validator\Constraints\NotNull]
  #[Length(max: 3)]
  private string $typeUtilisateurCode = TypeUtilisateur.Gestionnaire;

  public function getNom(): string
  {
    return $this->nom;
  }

  public function getPrenom(): string
  {
    return $this->prenom;
  }

  public function getEmail(): string
  {
    return $this->email;
  }

  public function getDateNaissance(): Date|null
  {
    return $this->dateNaissance;
  }

  public function getActif(): bool
  {
    return $this->actif;
  }

  public function getProfilId(): int
  {
    return $this->profilId;
  }

  public function getTypeUtilisateurCode(): string
  {
    return $this->typeUtilisateurCode;
  }

  public function setNom(string|null $nom): self
  {
    $this->nom = $nom;

    return $this;
  }

  public function setPrenom(string|null $prenom): self
  {
    $this->prenom = $prenom;

    return $this;
  }

  public function setEmail(string|null $email): self
  {
    $this->email = $email;

    return $this;
  }

  public function setDateNaissance(Date|null $dateNaissance): self
  {
    $this->dateNaissance = $dateNaissance;

    return $this;
  }

  public function setActif(bool|null $actif): self
  {
    $this->actif = $actif;

    return $this;
  }

  public function setProfilId(int|null $profilId): self
  {
    $this->profilId = $profilId;

    return $this;
  }

  public function setTypeUtilisateurCode(string|null $typeUtilisateurCode): self
  {
    $this->typeUtilisateurCode = $typeUtilisateurCode;

    return $this;
  }
}
