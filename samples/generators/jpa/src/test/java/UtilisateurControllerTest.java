import static org.assertj.core.api.Assertions.assertThat;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.api.server.securite.utilisateur.UtilisateurController;

class UtilisateurControllerTest {
    @Test
    void isInterfaceWithMethods() {
        assertThat(UtilisateurController.class)
                .isInterface()
                .hasMethods("addUtilisateur", "deleteUtilisateur", "getUtilisateur", "searchUtilisateur",
                        "updateUtilisateur");
    }
}
