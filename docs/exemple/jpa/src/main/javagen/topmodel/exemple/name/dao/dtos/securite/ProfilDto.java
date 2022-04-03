////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.securite;

import java.io.Serializable;
import java.util.List;

import javax.annotation.Generated;

import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.entities.securite.TypeProfil;

/**
 * Objet métier non persisté représentant Profil.
 */
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ProfilDto implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
     *
     * @return value of id.
     */
    @Getter
    @Setter
    private long id;

    /**
     * Type de profil.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
     *
     * @return value of typeProfilCode.
     */
    @Getter
    @Setter
    private TypeProfil.Values typeProfilCode;

    /**
     * Liste paginée des utilisateurs de ce profil.
     *
     * @return value of UtilisateurDto.
     */
    @Getter
    @Setter
    private List<UtilisateurDto> utilisateurs;
}
