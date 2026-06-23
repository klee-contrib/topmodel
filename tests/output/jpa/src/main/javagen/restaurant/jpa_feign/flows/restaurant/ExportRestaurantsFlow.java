////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package restaurant.jpa_feign.flows.restaurant;

import javax.sql.DataSource;

import org.springframework.batch.core.job.builder.FlowBuilder;
import org.springframework.batch.core.job.flow.Flow;
import org.springframework.batch.core.repository.JobRepository;
import org.springframework.batch.core.step.builder.StepBuilder;
import org.springframework.batch.core.step.skip.AlwaysSkipItemSkipPolicy;
import org.springframework.batch.core.step.Step;
import org.springframework.batch.infrastructure.item.database.builder.JdbcCursorItemReaderBuilder;
import org.springframework.batch.infrastructure.item.database.builder.JpaItemWriterBuilder;
import org.springframework.batch.infrastructure.item.ItemReader;
import org.springframework.batch.infrastructure.item.ItemWriter;
import org.springframework.batch.infrastructure.item.support.CompositeItemProcessor;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.jdbc.core.BeanPropertyRowMapper;
import org.springframework.transaction.PlatformTransactionManager;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityManagerFactory;

import restaurant.jpa_feign.dtos.restaurant.RestaurantRead;
import restaurant.jpa_feign.entities.restaurant.Restaurant;

@Configuration
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ExportRestaurantsFlow {

	protected ExportRestaurantsFlow() {
		// protected constructor to hide implicite public one
	}

	@Bean("ExportRestaurantsFlow")
	public static Flow exportRestaurantsFlow(
				@Qualifier("ExportRestaurantsStep") Step exportRestaurantsStep) {
		return new FlowBuilder<Flow>("ExportRestaurantsFlow") //
			.start(exportRestaurantsStep) //
			.build();
	}

	@Bean("ExportRestaurantsStep")
	public static Step exportRestaurantsStep(
			JobRepository jobRepository, //
			PlatformTransactionManager transactionManager, //
			@Qualifier("ExportRestaurantsReader") ItemReader<Restaurant> reader, //
			ExportRestaurantsPartialFlow exportRestaurantsPartialFlow,
			@Qualifier("ExportRestaurantsWriter") ItemWriter<RestaurantRead> writer //
	) {
		return new StepBuilder("ExportRestaurantsStep", jobRepository) //
			.<Restaurant, RestaurantRead>chunk(100000, transactionManager) //
			.reader(reader) //
			.processor(new CompositeItemProcessor<>(exportRestaurantsPartialFlow.afterSource(), exportRestaurantsPartialFlow.map(), exportRestaurantsPartialFlow.beforeTarget())) //
			.faultTolerant() //
			.skipPolicy(new AlwaysSkipItemSkipPolicy()) //
			.writer(writer) //
			.build();
	}

	@Bean("ExportRestaurantsReader")
	public static ItemReader<Restaurant> exportRestaurantsReader( //
			@Qualifier("Primary") DataSource datasource) {
		return new JdbcCursorItemReaderBuilder<Restaurant>() //
				.name("ExportRestaurantsReader") //
				.rowMapper(new BeanPropertyRowMapper<>(Restaurant.class)) //
				.sql("select * from public.RESTAURANT") //
				.fetchSize(100000) //
				.dataSource(datasource) //
				.build();
	}

	@Bean("ExportRestaurantsWriter")
	public static ItemWriter<RestaurantRead> exportRestaurantsWriter(EntityManagerFactory entityManagerFactory) {
		return new JpaItemWriterBuilder<RestaurantRead>().entityManagerFactory(entityManagerFactory).build();
	}
}
