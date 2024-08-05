import static org.assertj.core.api.Assertions.assertThat;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.api.client.securite.utilisateur.UtilisateurClient;

class UtilisateurClientTest {
    @Test
    void isInterfaceWithMethods() {
        assertThat(UtilisateurClient.class)
                .isInterface()
                .hasMethods("addUtilisateur", "deleteUtilisateur", "getUtilisateur", "searchUtilisateur",
                        "updateUtilisateur");
    }

    @Test
    void getIdHasAnnotations() {
        try {
            assertThat(UtilisateurClient.class.getMethod("getUtilisateur", Long.class)).satisfies(method -> {
                assertThat(method).hasOnlyFields("id");
            });
        } catch (NoSuchMethodException | SecurityException e) {
        }
    }
}
