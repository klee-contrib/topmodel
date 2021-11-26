////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.securite;

import java.io.Serializable;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

/**
 * Objet métier non persisté représentant Profil.
 */
@Builder
@Setter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
public class ProfilDto implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private long id;

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
     *
     * @return value of id.
     */
    public long getId() {
         return this.id;
    }
}
