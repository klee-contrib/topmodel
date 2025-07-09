import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.assertThatThrownBy;

import java.util.Arrays;

import org.junit.jupiter.api.Test;

import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilRead;
import topmodel.jpa.sample.demo.dtos.securite.profil.ProfilWrite;
import topmodel.jpa.sample.demo.entities.securite.profil.Droit;
import topmodel.jpa.sample.demo.entities.securite.profil.Profil;
import topmodel.jpa.sample.demo.entities.securite.profil.SecuriteProfilMappers;
import topmodel.jpa.sample.demo.enums.securite.profil.DroitCode;

public class SecuriteProfilMappersTest {

    @Test
    public void testCreateProfilRead() {
        // GIVEN
        // Create a Profil object with some data
        Profil profil = new Profil();
        profil.setId(1);
        profil.setLibelle("Test Profil");
        profil.setDroits(Arrays.asList(Droit.DELETE, Droit.CREATE));

        // WHEN
        // Call the createProfilRead method with the Profil object and a null target
        ProfilRead profilRead = SecuriteProfilMappers.createProfilRead(profil);

        // THEN
        // Verify that the ProfilRead object has the same data as the Profil object
        assertThat(profilRead.getId()).isEqualTo(profil.getId());
        assertThat(profilRead.getLibelle()).isEqualTo(profil.getLibelle());
        assertThat(profilRead.getDroits()).hasSize(profil.getDroits().size());
        assertThat(profilRead.getDroits()).containsAll(Arrays.asList(DroitCode.DELETE, DroitCode.CREATE));
    }

    @Test
    public void testToProfilFromProfil() {
        // GIVEN
        // Create a source Profil object with some data
        Profil source = new Profil();
        source.setLibelle("Source Profil");
        source.setDroits(Arrays.asList(Droit.CREATE, Droit.DELETE));

        // WHEN
        // Call the toProfil method with the source Profil object and a null target
        Profil target = SecuriteProfilMappers.toProfil(source);

        // THEN
        // Verify that the target Profil object has the same data as the source Profil
        // object
        assertThat(target.getLibelle()).isEqualTo(source.getLibelle());
        assertThat(target.getDroits()).isEqualTo(source.getDroits());
    }

    @Test
    public void testToProfilFromProfilWrite() {
        // GIVEN
        // Create a source ProfilWrite object with some data
        ProfilWrite source = new ProfilWrite();
        source.setLibelle("Source Profil");
        source.setDroits(Arrays.asList(DroitCode.CREATE, DroitCode.DELETE));

        // WHEN
        // Call the toProfil method with the source ProfilWrite object and a null target
        Profil target = SecuriteProfilMappers.toProfil(source);

        // THEN
        // Verify that the target Profil object has the same data as the source
        // ProfilWrite object
        assertThat(target.getLibelle()).isEqualTo(source.getLibelle());
        assertThat(target.getDroits()).hasSize(source.getDroits().size());
        assertThat(target.getDroits().stream().map(Droit::getCode)).containsAll(source.getDroits());
    }

    @Test
    public void testToProfilFromProfilWithExistingTarget() {
        // GIVEN
        // Create a source Profil object with some data
        Profil source = new Profil();
        source.setLibelle("Source Profil");
        source.setDroits(Arrays.asList(Droit.CREATE, Droit.DELETE));

        // Create a target Profil object with some data
        Profil target = new Profil();
        target.setLibelle("Target Profil");
        target.setDroits(Arrays.asList(Droit.CREATE, Droit.DELETE));

        // WHEN
        // Call the toProfil method with the source Profil object and the target Profil
        // object
        Profil result = SecuriteProfilMappers.toProfil(source, target);

        // THEN
        // Verify that the result Profil object has the same data as the source Profil
        // object
        assertThat(result.getLibelle()).isEqualTo(source.getLibelle());
        assertThat(result.getDroits()).isEqualTo(source.getDroits());
    }

    @Test
    public void testToProfilFromProfilWriteWithExistingTarget() {
        // GIVEN
        // Create a source ProfilWrite object with some data
        ProfilWrite source = new ProfilWrite();
        source.setLibelle("Source Profil");
        source.setDroits(Arrays.asList(DroitCode.CREATE, DroitCode.DELETE));

        // Create a target Profil object with some data
        Profil target = new Profil();
        target.setLibelle("Target Profil");
        target.setDroits(Arrays.asList(Droit.CREATE, Droit.DELETE));

        // WHEN
        // Call the toProfil method with the source ProfilWrite object and the target
        // Profil object
        Profil result = SecuriteProfilMappers.toProfil(source, target);

        // THEN
        // Verify that the result Profil object has the same data as the source
        // ProfilWrite object
        assertThat(result.getLibelle()).isEqualTo(source.getLibelle());
        assertThat(result.getDroits()).hasSize(source.getDroits().size());
        assertThat(result.getDroits().stream().map(Droit::getCode)).containsAll(source.getDroits());
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
