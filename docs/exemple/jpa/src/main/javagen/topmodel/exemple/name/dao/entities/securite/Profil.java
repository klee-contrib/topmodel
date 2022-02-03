////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.securite;

import java.io.Serializable;
import java.util.Collections;
import java.util.Set;

import javax.annotation.Generated;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;

import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.utils.IFieldEnum;

/**
 * Profil des utilisateurs.
 */
@SuperBuilder
@Setter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "PROFIL")
public class Profil implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private long id;
    private Set<TypeProfil> typeProfilList;

    /**
     * Id technique.
     *
     * @return value of id.
     */
    @Id
    @SequenceGenerator(name = "SEQ_PROFIL", sequenceName = "SEQ_PROFIL",  initialValue = 1000, allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PROFIL")
    @Column(name = "ID", nullable = false)
    public long getId() {
         return this.id;
    }

    /**
     * Type de profil.
     *
     * @return value of typeProfilList.
     */
    @OneToMany(cascade=CascadeType.ALL, orphanRemoval = true)
    public Set<TypeProfil> getTypeProfilList() {
        if(typeProfilList == null) this.typeProfilList = Collections.emptySet();
        return this.typeProfilList;
    }

    public enum Fields implements IFieldEnum<Profil> {
         ID, //
         TYPE_PROFIL_LIST
    }
}
