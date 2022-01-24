////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.utilisateur;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.constraints.NotNull;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.name.dao.references.securite.TypeProfilCode;
import topmodel.exemple.name.dao.references.utilisateur.TypeUtilisateurCode;

/**
 * Objet non persisté de communication avec le serveur.
 */
@Builder
@Setter
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private long id;
    private String email;
    private TypeUtilisateurCode typeUtilisateurCode;
    private TypeUtilisateurCode typeUtilisateurCodeOrigin;
    private long profilId;
    private TypeProfilCode profilTypeProfilCode;
    private UtilisateurDto utilisateurParent;

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
     *
     * @return value of id.
     */
    public long getId() {
         return this.id;
    }

    /**
     * Email de l'utilisateur.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getEmail() Utilisateur#getEmail()} 
     *
     * @return value of email.
     */
    @Email
    public String getEmail() {
         return this.email;
    }

    /**
     * Type d'utilisateur en Many to one.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
     *
     * @return value of typeUtilisateurCode.
     */
    public TypeUtilisateurCode getTypeUtilisateurCode() {
         return this.typeUtilisateurCode;
    }

    /**
     * Type d'utilisateur en one to one.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCodeOrigin() Utilisateur#getTypeUtilisateurCodeOrigin()} 
     *
     * @return value of typeUtilisateurCodeOrigin.
     */
    public TypeUtilisateurCode getTypeUtilisateurCodeOrigin() {
         return this.typeUtilisateurCodeOrigin;
    }

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
     *
     * @return value of profilId.
     */
    @NotNull
    public long getProfilId() {
         return this.profilId;
    }

    /**
     * Type de profil.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
     *
     * @return value of profilTypeProfilCode.
     */
    public TypeProfilCode getProfilTypeProfilCode() {
         return this.profilTypeProfilCode;
    }

    /**
     * UtilisateurParent.
     *
     * @return value of UtilisateurDto.
     */
    public UtilisateurDto getUtilisateurParent() {
        return this.utilisateurParent;
    }
}
