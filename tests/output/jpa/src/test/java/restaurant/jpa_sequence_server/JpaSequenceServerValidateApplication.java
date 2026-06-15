package restaurant.jpa_sequence_server;

import org.springframework.boot.SpringBootConfiguration;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.hibernate.autoconfigure.HibernateJpaAutoConfiguration;
import org.springframework.boot.jdbc.autoconfigure.DataSourceAutoConfiguration;
import org.springframework.boot.persistence.autoconfigure.EntityScan;

@SpringBootConfiguration
@EntityScan(basePackages = "restaurant.jpa_sequence_server.entities")
@ImportAutoConfiguration({ DataSourceAutoConfiguration.class, HibernateJpaAutoConfiguration.class })
public class JpaSequenceServerValidateApplication {
}
