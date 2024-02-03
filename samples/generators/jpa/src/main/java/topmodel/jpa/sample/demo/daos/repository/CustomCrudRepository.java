package topmodel.jpa.sample.demo.daos.repository;

import org.springframework.data.repository.CrudRepository;

public interface CustomCrudRepository<T, ID> extends CrudRepository<T, ID> {
}
