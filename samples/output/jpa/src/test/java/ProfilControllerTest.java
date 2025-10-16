import static org.assertj.core.api.Assertions.assertThat;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.api.server.securite.profil.ProfilController;

class ProfilControllerTest {
    @Test
    void isInterfaceWithMethods() {
        assertThat(ProfilController.class)
                .isInterface()
                .hasMethods("addProfil", "getProfil", "getProfils", "updateProfil");
    }
}
