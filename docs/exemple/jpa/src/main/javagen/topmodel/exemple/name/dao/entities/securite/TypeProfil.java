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
import javax.persistence.Transient;

import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.annotations.Immutable;

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
 * Type d'utilisateur.
 */
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(of = { "code" })
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_PROFIL")
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Immutable
public class TypeProfil implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    /**
     * Code du type d'utilisateur.
     *
     * @return value of code.
     */
    @Id
    @Column(name = "CODE", nullable = false, updatable = false, length = 3)
    @Enumerated(EnumType.STRING)
    @Getter
    @Setter
    private TypeProfil.Values code;

    /**
     * Libellé du type d'utilisateur.
     *
     * @return value of libelle.
     */
    @Column(name = "LIBELLE", nullable = false, updatable = false, length = 3)
    @Getter
    @Setter
    private String libelle;

    /**
     * Enumération des champs de la classe {@link topmodel.exemple.name.dao.entities.securite.TypeProfil TypeProfil}.
     */
    public enum Fields implements IFieldEnum<TypeProfil> {
        CODE, //
        LIBELLE
    }

    @AllArgsConstructor
    @Getter
    public enum Values {
        ADM("Administrateur"), //
        GES("Gestionnaire"); 

        /**
         * Libellé du type d'utilisateur.
         */
        private final String libelle;
    }
}
