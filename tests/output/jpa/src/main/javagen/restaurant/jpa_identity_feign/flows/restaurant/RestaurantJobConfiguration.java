////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_identity_feign.flows.restaurant;

import org.springframework.batch.core.Job;
import org.springframework.batch.core.job.builder.JobBuilder;
import org.springframework.batch.core.job.flow.Flow;
import org.springframework.batch.core.launch.support.RunIdIncrementer;
import org.springframework.batch.core.repository.JobRepository;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Import;
import org.springframework.core.task.TaskExecutor;

import jakarta.annotation.Generated;

@Configuration
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Import({ExportMenusFlow.class})
public class RestaurantJobConfiguration {
	@Bean("RestaurantJob")
	public Job restaurantJob( //
				JobRepository jobRepository, //
				TaskExecutor taskExecutor, //
			@Qualifier("ExportMenusFlow") Flow exportMenusFlow
	) {
		return new JobBuilder("RestaurantJob", jobRepository) //
				.incrementer(new RunIdIncrementer()) //
				.start(exportMenusFlow)
				.end() //
				.build();
	}
}
