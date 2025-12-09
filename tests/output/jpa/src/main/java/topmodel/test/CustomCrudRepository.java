package topmodel.test;

import org.springframework.data.repository.CrudRepository;

public interface CustomCrudRepository<T, ID> extends CrudRepository<T, ID> {
}
