////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.entities.utilisateur;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.Collections;
import java.util.DateTime;
import java.util.Set;

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
import javax.validation.constraints.Email;

import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import oorg.springframework.data.jpa.domain.support.AuditingEntityListener;

import topmodel.exemple.name.dao.entities.securite.Profil;
import topmodel.exemple.utils.IFieldEnum;

/**
 * Utilisateur de l'application.
 */
@SuperBuilder
@Setter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private long id;
    private Set<Profil> profilList;
    private DateTime dateCreation;
    private DateTime dateModification;
    private String email;
    private TypeUtilisateur typeUtilisateur;
    private TypeUtilisateur typeUtilisateurOrigin;

    /**
     * Id technique.
     *
     * @return value of id.
     */
    @Id
    @SequenceGenerator(name = "SEQ_UTILISATEUR", sequenceName = "SEQ_UTILISATEUR",  initialValue = 1000, allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_UTILISATEUR")
    @Column(name = "ID", nullable = false)
    public long getId() {
         return this.id;
    }

    /**
     * Liste des profils.
     *
     * @return value of profilList.
     */
    @ManyToMany(fetch = FetchType.LAZY)
    @JoinTable(name = "UTILISATEUR_PROFIL", joinColumns = @JoinColumn(name = "ID"), inverseJoinColumns = @JoinColumn(name = "ID"))
    public Set<Profil> getProfilList() {
        if(profilList == null) this.profilList = Collections.emptySet();
        return this.profilList;
    }

    /**
     * Date de création de l'utilisateur.
     *
     * @return value of dateCreation.
     */
    @Column(name = "DATE_CREATION", nullable = true)
    @CreatedDate
    public DateTime getDateCreation() {
         return this.dateCreation;
    }

    /**
     * Date de modification de l'utilisateur.
     *
     * @return value of dateModification.
     */
    @Column(name = "DATE_MODIFICATION", nullable = true)
    @LastModifiedDate
    public DateTime getDateModification() {
         return this.dateModification;
    }

    /**
     * Email de l'utilisateur.
     *
     * @return value of email.
     */
    @Column(name = "EMAIL", nullable = true)
    @Email
    public String getEmail() {
         return this.email;
    }

    /**
     * Type d'utilisateur en Many to one.
     *
     * @return value of typeUtilisateur.
     */
    @ManyToOne(fetch = FetchType.LAZY, optional = true)
    @JoinColumn(name = "TUT_CODE", referencedColumnName = "TUT_CODE")
    public TypeUtilisateur getTypeUtilisateur() {
        return this.typeUtilisateur;
    }

    /**
     * Type d'utilisateur en one to one.
     *
     * @return value of typeUtilisateurOrigin.
     */
    @OneToOne(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = true)
    @JoinColumn(name = "ORIGIN_TUT_CODE", referencedColumnName = "TUT_CODE", unique = true)
    public TypeUtilisateur getTypeUtilisateurOrigin() {
        return this.typeUtilisateurOrigin;
    }

    public enum Fields implements IFieldEnum<Utilisateur> {
         ID, //
         PROFIL_LIST, //
         DATE_CREATION, //
         DATE_MODIFICATION, //
         EMAIL, //
         TYPE_UTILISATEUR, //
         TYPE_UTILISATEUR_ORIGIN
    }
}
