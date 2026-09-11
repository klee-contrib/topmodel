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
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.jdbc.core.BeanPropertyRowMapper;

import jakarta.annotation.Generated;
import jakarta.persistence.EntityManagerFactory;

import restaurant.jpa_feign.dtos.restaurant.MenuRead;
import restaurant.jpa_feign.entities.restaurant.Menu;
import restaurant.jpa_feign.entities.restaurant.RestaurantMappers;

@Configuration
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ExportMenusFlow {

	protected ExportMenusFlow() {
		// protected constructor to hide implicite public one
	}

	@Bean("ExportMenusFlow")
	public static Flow exportMenusFlow(
				@Qualifier("ExportMenusStep") Step exportMenusStep) {
		return new FlowBuilder<Flow>("ExportMenusFlow") //
			.start(exportMenusStep) //
			.build();
	}

	@Bean("ExportMenusStep")
	public static Step exportMenusStep(
			JobRepository jobRepository, //
			@Qualifier("ExportMenusReader") ItemReader<Menu> reader, //
			@Qualifier("ExportMenusWriter") ItemWriter<MenuRead> writer //
	) {
		return new StepBuilder("ExportMenusStep", jobRepository) //
			.<Menu, MenuRead>chunk(100000) //
			.reader(reader) //
			.processor((Menu item) -> RestaurantMappers.createMenuRead(item)) //
			.faultTolerant() //
			.skipPolicy(new AlwaysSkipItemSkipPolicy()) //
			.writer(writer) //
			.build();
	}

	@Bean("ExportMenusReader")
	public static ItemReader<Menu> exportMenusReader( //
			@Qualifier("Primary") DataSource datasource) {
		return new JdbcCursorItemReaderBuilder<Menu>() //
				.name("ExportMenusReader") //
				.rowMapper(new BeanPropertyRowMapper<>(Menu.class)) //
				.sql("select * from public.menu") //
				.fetchSize(100000) //
				.dataSource(datasource) //
				.build();
	}

	@Bean("ExportMenusWriter")
	public static ItemWriter<MenuRead> exportMenusWriter(EntityManagerFactory entityManagerFactory) {
		return new JpaItemWriterBuilder<MenuRead>().entityManagerFactory(entityManagerFactory).build();
	}
}
