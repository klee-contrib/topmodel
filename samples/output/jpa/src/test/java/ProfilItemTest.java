import static org.assertj.core.api.Assertions.assertThat;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilItem;

class ProfilItemTest {
    @Test
    void isInterfaceWithMethods() {
        assertThat(ProfilItem.class)
                .isInterface()
                .hasMethods("getId", "getLibelle", "getNombreUtilisateurs", "hydrate");
    }
}
