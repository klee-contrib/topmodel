package topmodel.jpa.sample.demo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Import;

import topmodel.jpa.sample.demo.configuration.JpaConfiguration;
import topmodel.jpa.sample.demo.configuration.WebappConfiguration;

@SpringBootApplication
@Import({ //
		JpaConfiguration.class, //
		WebappConfiguration.class })
public class DemoApplication {

	public static void main(String[] args) {
		SpringApplication.run(DemoApplication.class, args);
	}
}
