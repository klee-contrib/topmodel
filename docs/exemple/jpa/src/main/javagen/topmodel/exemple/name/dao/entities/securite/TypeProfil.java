////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.securite;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Enumerated;
import javax.persistence.EnumType;
import javax.persistence.Id;
import javax.persistence.Table;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.name.dao.references.securite.TypeProfilCode;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Type d'utilisateur.
 */
@Builder
@Setter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_PROFIL")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Immutable
public class TypeProfil implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private TypeProfilCode code;
    private String libelle;

    /**
     * Code du type d'utilisateur.
     *
     * @return value of code.
     */
    @Id
    @Column(name = "CODE", nullable = false, length = 3)
    @Enumerated(EnumType.STRING)
    public TypeProfilCode getCode() {
         return this.code;
    }

    /**
     * Libellé du type d'utilisateur.
     *
     * @return value of libelle.
     */
    @Column(name = "LIBELLE", nullable = false, length = 3)
    public String getLibelle() {
         return this.libelle;
    }

    public enum Fields implements IFieldEnum<TypeProfil> {
         CODE, //
         LIBELLE
    }
}
