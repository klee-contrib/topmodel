<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Repository\Securite\Profil;

use App\Entity\Securite\Profil\ProfilDroit;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Symfony\Bridge\Doctrine\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<ProfilDroit>
 */
class ProfilDroitRepository extends ServiceEntityRepository
{
  public function __construct(ManagerRegistry $registry)
  {
    parent::__construct($registry, ProfilDroit::class);
  }
}
