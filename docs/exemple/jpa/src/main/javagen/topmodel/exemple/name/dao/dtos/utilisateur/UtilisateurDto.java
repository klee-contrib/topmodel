////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.dao.dtos.utilisateur;

import java.io.Serializable;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.constraints.NotNull;

import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

import topmodel.exemple.name.dao.entities.securite.TypeProfil;
import topmodel.exemple.name.dao.entities.utilisateur.TypeUtilisateur;

/**
 * Objet non persisté de communication avec le serveur.
 */
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode
@ToString
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class UtilisateurDto implements Serializable {
	/** Serial ID */
    private static final long serialVersionUID = 1L;

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getId() Utilisateur#getId()} 
     *
     * @return value of id.
     */
    @Getter
    @Setter
    private long id;

    /**
     * Email de l'utilisateur.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getEmail() Utilisateur#getEmail()} 
     *
     * @return value of email.
     */
    @Email
    @Getter
    @Setter
    private String email;

    /**
     * Type d'utilisateur en Many to one.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCode() Utilisateur#getTypeUtilisateurCode()} 
     *
     * @return value of typeUtilisateurCode.
     */
    @Getter
    @Setter
    private TypeUtilisateur.Values typeUtilisateurCode;

    /**
     * Type d'utilisateur en one to one.
     * Alias of {@link topmodel.exemple.name.dao.entities.utilisateur.Utilisateur#getTypeUtilisateurCodeOrigin() Utilisateur#getTypeUtilisateurCodeOrigin()} 
     *
     * @return value of typeUtilisateurCodeOrigin.
     */
    @Getter
    @Setter
    private TypeUtilisateur.Values typeUtilisateurCodeOrigin;

    /**
     * Id technique.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getId() Profil#getId()} 
     *
     * @return value of profilId.
     */
    @NotNull
    @Getter
    @Setter
    private long profilId;

    /**
     * Type de profil.
     * Alias of {@link topmodel.exemple.name.dao.entities.securite.Profil#getTypeProfilCode() Profil#getTypeProfilCode()} 
     *
     * @return value of profilTypeProfilCode.
     */
    @Getter
    @Setter
    private TypeProfil.Values profilTypeProfilCode;

    /**
     * UtilisateurParent.
     *
     * @return value of UtilisateurDto.
     */
    @Getter
    @Setter
    private UtilisateurDto utilisateurParent;
}
