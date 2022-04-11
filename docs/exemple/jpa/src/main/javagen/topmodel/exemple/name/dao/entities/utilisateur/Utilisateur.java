////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.utilisateur;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.Collections;
import java.util.DateTime;
import java.util.List;

import javax.annotation.Generated;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.EntityListeners;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.JoinTable;
import javax.persistence.ManyToMany;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;
import javax.persistence.Transient;
import javax.validation.constraints.Email;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import oorg.springframework.data.jpa.domain.support.AuditingEntityListener;

import topmodel.exemple.name.dao.entities.securite.Profil;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Utilisateur de l'application.
 */
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(of = { "id" })
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    /**
     * Id technique.
     *
     * @return value of id.
     */
    @Id
    @SequenceGenerator(name = "SEQ_UTILISATEUR", sequenceName = "SEQ_UTILISATEUR",  initialValue = 1000, allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_UTILISATEUR")
    @Column(name = "UTI_ID", nullable = false)
    @Getter
    @Setter
    private long id;

    /**
     * Liste des profils.
     *
     * @return value of profils.
     */
    @ManyToMany(fetch = FetchType.LAZY)
    @JoinTable(name = "PROFIL_UTILISATEUR", joinColumns = @JoinColumn(name = "UTI_ID"), inverseJoinColumns = @JoinColumn(name = "PRO_ID"))
    @Builder.Default
    @Getter
    @Setter
    private List<Profil> profils = Collections.emptyList();

    /**
     * Date de création de l'utilisateur.
     *
     * @return value of dateCreation.
     */
    @Column(name = "UTI_DATE_CREATION", nullable = true)
    @CreatedDate
    @Getter
    @Setter
    private DateTime dateCreation;

    /**
     * Date de modification de l'utilisateur.
     *
     * @return value of dateModification.
     */
    @Column(name = "UTI_DATE_MODIFICATION", nullable = true)
    @LastModifiedDate
    @Getter
    @Setter
    private DateTime dateModification;

    /**
     * Email de l'utilisateur.
     *
     * @return value of email.
     */
    @Column(name = "UTI_EMAIL", nullable = true)
    @Email
    @Getter
    @Setter
    private String email;

    /**
     * Type d'utilisateur en Many to one.
     *
     * @return value of typeUtilisateur.
     */
    @ManyToOne(fetch = FetchType.LAZY, optional = true, targetEntity = TypeUtilisateur.class)
    @JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
    @Getter(AccessLevel.PROTECTED)
    @Setter
    private TypeUtilisateur typeUtilisateur;

    /**
     * Type d'utilisateur en one to one.
     *
     * @return value of typeUtilisateurOrigin.
     */
    @OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = true)
    @JoinColumn(name = "ORIGIN_TUT_CODE", referencedColumnName = "TUT_CODE", unique = true)
    @Getter(AccessLevel.PROTECTED)
    @Setter
    private TypeUtilisateur typeUtilisateurOrigin;

    /**
     * Type d'utilisateur en Many to one.
     * Setter enum
     */
    public void setTypeUtilisateurCode(TypeUtilisateur.Values typeUtilisateurCode) {
        if(typeUtilisateurCode != null)
            this.typeUtilisateur = TypeUtilisateur.builder().code(typeUtilisateurCode).build();
    }

    /**
     * Type d'utilisateur en Many to one.
     * Getter enum
     */
    @Transient
    public TypeUtilisateur.Values getTypeUtilisateurCode() {
        return this.typeUtilisateur != null ? this.typeUtilisateur.getCode() : null;
    }

    /**
     * Type d'utilisateur en one to one.
     * Setter enum
     */
    public void setTypeUtilisateurCodeOrigin(TypeUtilisateur.Values typeUtilisateurCodeOrigin) {
        if(typeUtilisateurCodeOrigin != null)
            this.typeUtilisateurOrigin = TypeUtilisateur.builder().code(typeUtilisateurCodeOrigin).build();
    }

    /**
     * Type d'utilisateur en one to one.
     * Getter enum
     */
    @Transient
    public TypeUtilisateur.Values getTypeUtilisateurCodeOrigin() {
        return this.typeUtilisateurOrigin != null ? this.typeUtilisateurOrigin.getCode() : null;
    }

    /**
     * Enumération des champs de la classe {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur Utilisateur}.
     */
    public enum Fields implements IFieldEnum<Utilisateur> {
        ID, //
        PROFILS, //
        DATE_CREATION, //
        DATE_MODIFICATION, //
        EMAIL, //
        TYPE_UTILISATEUR, //
        TYPE_UTILISATEUR_ORIGIN
    }
}
