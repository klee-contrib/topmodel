package topmodel.jpa.sample.demo.configuration;

import java.time.Duration;

import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;
import org.springframework.web.client.RestTemplate;

@Configuration
@ComponentScan(basePackages = { //
		"topmodel.jpa.sample.demo.services", //
})
@PropertySource({ "classpath:application.yml" })
public class WebappConfiguration {
	@Bean
	public RestTemplate restTemplate() {
		RestTemplateBuilder restTemplateBuilder = new RestTemplateBuilder();
		return restTemplateBuilder //
				.setConnectTimeout(Duration.ofSeconds(5)) //
				.setReadTimeout(Duration.ofSeconds(120)) //
				.build();//
	}
}
