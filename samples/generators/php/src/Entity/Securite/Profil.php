////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

<?php

namespace App\Entity\Securite;

use App\Repository\Securite\ProfilRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\ORM\Mapping\Column;
use Doctrine\ORM\Mapping\Entity;
use Doctrine\ORM\Mapping\GeneratedValue;
use Doctrine\ORM\Mapping\Id;
use Doctrine\ORM\Mapping\InverseJoinColumn;
use Doctrine\ORM\Mapping\JoinColumn;
use Doctrine\ORM\Mapping\ManyToMany;
use Doctrine\ORM\Mapping\ManyToOne;
use Doctrine\ORM\Mapping\OneToMany;
use Doctrine\ORM\Mapping\SequenceGenerator;
use Doctrine\ORM\Mapping\Table;

#[Doctrine\ORM\Mapping\Entity(repositoryClass: ProfilRepository::class)]
#[Doctrine\ORM\Mapping\Table(name: 'PROFIL')]
class Profil
{
  #[Doctrine\ORM\Mapping\Id]
  #[Doctrine\ORM\Mapping\GeneratedValue(strategy: "SEQUENCE")]
  #[Doctrine\ORM\Mapping\SequenceGenerator(sequenceName = "SEQ_PROFIL")]
  #[Doctrine\ORM\Mapping\Column(name: 'PRO_ID')]
  private int $id;

  #[ManyToOne(targetEntity: TypeProfil::class)]
  #[JoinColumn(name: 'TPR_CODE', referencedColumnName: 'TPR_CODE')]
  private TypeProfil typeProfil;

  /**
   * @var Collection<Droit>
   */
  #[JoinColumn(name: 'PRO_ID', referencedColumnName: 'PRO_ID')]
  #[InverseJoinColumn(name: 'DRO_CODE', referencedColumnName: 'DRO_CODE')]
  #[ManyToMany(targetEntity: Droit::class)]
  private Collection droits;

  /**
   * @var Collection<Secteur>
   */
  #[Doctrine\ORM\Mapping\OneToMany(mappedBy: 'profil', targetEntity: Secteur::class)]
  private Collection secteurs;

  public function __construct()
  {
    $this->droits = new ArrayCollection();
    $this->secteurs = new ArrayCollection();
  }

  public function getId() : int
  {
    return $this->id;
  }

  public function getTypeProfil() : TypeProfil|null
  {
    return $this->typeProfil;
  }

  /**
   * @return Collection<Droit>|null
   */
  public function getDroits() : Collection|null
  {
    return $this->droits;
  }

  /**
   * @return Collection<Secteur>|null
   */
  public function getSecteurs() : Collection|null
  {
    return $this->secteurs;
  }

  public function setId(int|null $id) : self
  {
    $this->id = $id;

    return $this;
  }

  public function setTypeProfil(TypeProfil|null $typeProfil) : self
  {
    $this->typeProfil = $typeProfil;

    return $this;
  }

  /**
   * @param Collection<Droit>|null $droits
   */
  public function setDroits(Collection|null $droits) : self
  {
    $this->droits = $droits;

    return $this;
  }

  /**
   * @param Collection<Secteur>|null $secteurs
   */
  public function setSecteurs(Collection|null $secteurs) : self
  {
    $this->secteurs = $secteurs;

    return $this;
  }
}
