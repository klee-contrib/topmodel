import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.assertThatThrownBy;

import java.util.Arrays;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilWrite;
import topmodel.jpa.sample.demo.entities.securite.profil.Droit;
import topmodel.jpa.sample.demo.entities.securite.profil.Profil;
import topmodel.jpa.sample.demo.entities.securite.profil.ProfilDroit;
import topmodel.jpa.sample.demo.entities.securite.profil.SecuriteProfilMappers;

public class SecuriteProfilMappersTest {

    @Test
    public void testCreateProfilRead() {
        // GIVEN
        // Create a Profil object with some data
        var profil = new Profil();
        profil.setId(1);
        profil.setLibelle("Test Profil");
        var profilDroit = new ProfilDroit();
        profilDroit.setProfil(profil);
        profilDroit.setDroit(Droit.CREATE);
        profil.setProfilDroits(Arrays.asList(profilDroit));

        // WHEN
        // Call the createProfilRead method with the Profil object and a null target
        ProfilRead profilRead = SecuriteProfilMappers.createProfilRead(profil);

        // THEN
        // Verify that the ProfilRead object has the same data as the Profil object
        assertThat(profilRead.getId()).isEqualTo(profil.getId());
        assertThat(profilRead.getLibelle()).isEqualTo(profil.getLibelle());
    }

    @Test
    public void testToProfilFromProfil() {
        // GIVEN
        // Create a source Profil object with some data
        Profil source = new Profil();
        source.setLibelle("Source Profil");

        // WHEN
        // Call the toProfil method with the source Profil object and a null target
        Profil target = SecuriteProfilMappers.toProfil(source);

        // THEN
        // Verify that the target Profil object has the same data as the source Profil
        // object
        assertThat(target.getLibelle()).isEqualTo(source.getLibelle());
    }

    @Test
    public void testToProfilFromProfilWrite() {
        // GIVEN
        // Create a source ProfilWrite object with some data
        ProfilWrite source = new ProfilWrite();
        source.setLibelle("Source Profil");

        // WHEN
        // Call the toProfil method with the source ProfilWrite object and a null target
        Profil target = SecuriteProfilMappers.toProfil(source);

        // THEN
        // Verify that the target Profil object has the same data as the source
        // ProfilWrite object
        assertThat(target.getLibelle()).isEqualTo(source.getLibelle());
    }

    @Test
    public void testToProfilFromProfilWithExistingTarget() {
        // GIVEN
        // Create a source Profil object with some data
        Profil source = new Profil();
        source.setLibelle("Source Profil");

        // Create a target Profil object with some data
        Profil target = new Profil();
        target.setLibelle("Target Profil");

        // WHEN
        // Call the toProfil method with the source Profil object and the target Profil
        // object
        Profil result = SecuriteProfilMappers.toProfil(source, target);

        // THEN
        // Verify that the result Profil object has the same data as the source Profil
        // object
        assertThat(result.getLibelle()).isEqualTo(source.getLibelle());
    }

    @Test
    public void testToProfilFromProfilWriteWithExistingTarget() {
        // GIVEN
        // Create a source ProfilWrite object with some data
        ProfilWrite source = new ProfilWrite();
        source.setLibelle("Source Profil");

        // Create a target Profil object with some data
        Profil target = new Profil();
        target.setLibelle("Target Profil");

        // WHEN
        // Call the toProfil method with the source ProfilWrite object and the target
        // Profil object
        Profil result = SecuriteProfilMappers.toProfil(source, target);

        // THEN
        // Verify that the result Profil object has the same data as the source
        // ProfilWrite object
        assertThat(result.getLibelle()).isEqualTo(source.getLibelle());
    }

    @Test
    public void testToProfilFromNullSource() {
        // GIVEN
        // Create a target Profil object
        Profil target = new Profil();

        // WHEN & THEN
        // Verify that calling the toProfil method with a null source ProfilWrite object
        // throws an IllegalArgumentException
        assertThatThrownBy(() -> {
            SecuriteProfilMappers.toProfil((ProfilWrite) null, target);
        }).isInstanceOf(IllegalArgumentException.class);
    }

    @Test
    public void testCreateProfilReadFromNullProfil() {
        // WHEN & THEN
        // Verify that calling the createProfilRead method with a null source Profil
        // object throws an IllegalArgumentException
        assertThatThrownBy(() -> {
            SecuriteProfilMappers.createProfilRead(null);
        }).isInstanceOf(IllegalArgumentException.class);
    }
}
