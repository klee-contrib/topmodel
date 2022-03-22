////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.securite;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;
import javax.persistence.Transient;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Profil des utilisateurs.
 */
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(of = { "id" })
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "PROFIL")
public class Profil implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    /**
     * Id technique.
     *
     * @return value of id.
     */
    @Id
    @SequenceGenerator(name = "SEQ_PROFIL", sequenceName = "SEQ_PROFIL",  initialValue = 1000, allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_PROFIL")
    @Column(name = "PRO_ID", nullable = false)
    @Getter
    @Setter
    private long id;
    /**
     * Type de profil.
     *
     * @return value of typeProfil.
     */
    @ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeProfil.class)
    @JoinColumn(name = "CODE", referencedColumnName = "CODE")
    @Getter(AccessLevel.PROTECTED)
    @Setter
    private TypeProfil typeProfil;

    /**
     * Type de profil.
     * Setter enum
     */
    public void setTypeProfilCode(TypeProfil.Values typeProfilCode) {
        if(typeProfilCode != null)
            this.typeProfil = TypeProfil.builder().code(typeProfilCode).build();
    }

    /**
     * Type de profil.
     * Getter enum
     */
    @Transient
    public TypeProfil.Values getTypeProfilCode() {
        return this.typeProfil != null ? this.typeProfil.getCode() : null;
    }

    public enum Fields implements IFieldEnum<Profil> {
         ID, //
         TYPE_PROFIL
    }
}
