package topmodel.jpa.sample.demo.configuration;

import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.data.jpa.repository.config.EnableJpaAuditing;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.orm.jpa.JpaTransactionManager;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.annotation.EnableTransactionManagement;

import jakarta.persistence.EntityManagerFactory;

@Configuration
@EnableJpaAuditing
@EntityScan(basePackages = { "topmodel.jpa.sample.demo.entities" })
//Gestion des transaction au niveau des composants avec l'annotation @Transactionnal (ou @TransctionnalWithRollback)
@EnableTransactionManagement
//Liste des package comportant des repositories qui acc�s aux sources de donn�es JPA (BDD).
@EnableJpaRepositories(basePackages = { "topmodel.jpa.sample.demo.daos" })
public class JpaConfiguration {
	/**
	 * Initialise le gestionnaire de transactions
	 * 
	 * @param emf gestionnaire d'entites
	 * @return gestionnaire de transactions
	 */
	@Bean("transactionManager")
	@Primary
	public PlatformTransactionManager transactionManager(EntityManagerFactory emf) {
		JpaTransactionManager transactionManager = new JpaTransactionManager();
		transactionManager.setEntityManagerFactory(emf);
		return transactionManager;
	}
}
