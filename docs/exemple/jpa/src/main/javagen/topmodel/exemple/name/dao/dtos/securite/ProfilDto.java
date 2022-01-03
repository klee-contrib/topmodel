////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.securite;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import javax.annotation.Generated;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.references.securite.TypeProfilCode;

/**
 * Objet métier non persisté représentant Profil.
 */
@Builder
@Setter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class ProfilDto implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private long id;
    private TypeProfilCode typeProfilCode;
    private List<UtilisateurDto> utilisateurs;

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
     *
     * @return value of id.
     */
    public long getId() {
         return this.id;
    }

    /**
     * Type de profil.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
     *
     * @return value of typeProfilCode.
     */
    public TypeProfilCode getTypeProfilCode() {
         return this.typeProfilCode;
    }

    /**
     * Liste paginée des utilisateurs de ce profil.
     *
     * @return value of UtilisateurDto.
     */
    public List<UtilisateurDto> getUtilisateurs() {
        if(utilisateurs == null) this.utilisateurs = new ArrayList<>();
        return this.utilisateurs;
    }
}
