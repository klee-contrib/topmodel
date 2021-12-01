////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.utilisateur;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.validation.constraints.Email;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

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
@Generated("TopModel : https://github.com/JabX/topmodel")
public class UtilisateurDto implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    private long id;
    private String email;
    private TypeUtilisateurCode typeUtilisateurCode;
    private TypeUtilisateurCode typeUtilisateurCodeOneToOneType;

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
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCodeOneToOneType() Utilisateur#getTypeUtilisateurCodeOneToOneType()} 
     *
     * @return value of typeUtilisateurCodeOneToOneType.
     */
    public TypeUtilisateurCode getTypeUtilisateurCodeOneToOneType() {
         return this.typeUtilisateurCodeOneToOneType;
    }
}
