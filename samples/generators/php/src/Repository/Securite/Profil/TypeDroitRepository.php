<?php
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace App\Repository\Securite\Profil;

use App\Entity\Securite\Profil\TypeDroit;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Symfony\Bridge\Doctrine\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<TypeDroit>
 */
class TypeDroitRepository extends ServiceEntityRepository
{
  public function __construct(ManagerRegistry $registry)
  {
    parent::__construct($registry, TypeDroit::class);
  }
}
