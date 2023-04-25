////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

<?php

namespace App\Repository\Securite;

use App\Entity\Securite\Profil;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;

/**
 * @extends ServiceEntityRepository<Profil>.
 */
class ProfilRepository extends ServiceEntityRepository
{
	public function __construct(ManagerRegistry $registry)
	{
		parent::__construct($registry, Profil::class);
	}
}
