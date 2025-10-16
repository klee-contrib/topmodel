import static org.assertj.core.api.Assertions.assertThat;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.api.client.securite.profil.ProfilClient;

class ProfilClientTest {
    @Test
    void isInterfaceWithMethods() {
        assertThat(ProfilClient.class)
                .isInterface()
                .hasMethods("addProfil", "getProfil", "getProfils", "updateProfil");
    }
}
