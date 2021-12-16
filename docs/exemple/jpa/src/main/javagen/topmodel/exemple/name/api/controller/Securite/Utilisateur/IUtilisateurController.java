////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.api.controller.securite.utilisateur;

import java.util.ArrayList;
import java.util.List;

import javax.annotation.Generated;
import javax.validation.constraints.Email;
import javax.validation.Valid;

import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;
import topmodel.exemple.name.dao.references.securite.TypeProfilCode;
import topmodel.exemple.name.dao.references.utilisateur.TypeUtilisateurCode;

@RestController
@Generated("TopModel : https://github.com/JabX/topmodel")
@RequestMapping("securite.utilisateur")
public interface IUtilisateurController {


    /**
     * Charge le détail d'un utilisateur.
     * @param utilisateurId Id technique
     * @return Le détail de l'utilisateur
     */
    @GetMapping("utilisateur/{utiId}")
    default UtilisateurDto getUtilisateurMapping(@RequestParam("utilisateurId") long utilisateurId) {
        return this.getUtilisateur(utilisateurId);
    }

    /**
     * Charge le détail d'un utilisateur.
     * @param utilisateurId Id technique
     * @return Le détail de l'utilisateur
     */
    UtilisateurDto getUtilisateur(long utilisateurId);

    /**
     * Charge une liste d'utilisateurs par leur type.
     * @param typeUtilisateurCode Type d'utilisateur en Many to one
     * @return Liste des utilisateurs
     */
    @GetMapping("utilisateur/list")
    default List<UtilisateurDto> getUtilisateurListMapping(@RequestParam("typeUtilisateurCode") TypeUtilisateurCode typeUtilisateurCode) {
        return this.getUtilisateurList(typeUtilisateurCode);
    }

    /**
     * Charge une liste d'utilisateurs par leur type.
     * @param typeUtilisateurCode Type d'utilisateur en Many to one
     * @return Liste des utilisateurs
     */
    List<UtilisateurDto> getUtilisateurList(TypeUtilisateurCode typeUtilisateurCode);

    /**
     * Sauvegarde un utilisateur.
     * @param utilisateur Utilisateur à sauvegarder
     * @return Utilisateur sauvegardé
     */
    @PostMapping("utilisateur/save")
    default UtilisateurDto saveUtilisateurMapping(@RequestBody @Valid UtilisateurDto utilisateur) {
        return this.saveUtilisateur(utilisateur);
    }

    /**
     * Sauvegarde un utilisateur.
     * @param utilisateur Utilisateur à sauvegarder
     * @return Utilisateur sauvegardé
     */
    UtilisateurDto saveUtilisateur(UtilisateurDto utilisateur);

    /**
     * Sauvegarde une liste d'utilisateurs.
     * @param utilisateur Utilisateur à sauvegarder
     * @return Utilisateur sauvegardé
     */
    @PostMapping("utilisateur/saveAll")
    default List<UtilisateurDto> saveAllUtilisateurMapping(@RequestBody @Valid List<UtilisateurDto> utilisateur) {
        return this.saveAllUtilisateur(utilisateur);
    }

    /**
     * Sauvegarde une liste d'utilisateurs.
     * @param utilisateur Utilisateur à sauvegarder
     * @return Utilisateur sauvegardé
     */
    List<UtilisateurDto> saveAllUtilisateur(List<UtilisateurDto> utilisateur);

    /**
     * Recherche des utilisateurs.
     * @param utilisateurId Id technique
     * @param email Email de l'utilisateur
     * @param typeUtilisateurCode Type d'utilisateur en Many to one
     * @param typeUtilisateurCodeOneToOneType Type d'utilisateur en one to one
     * @param profilProfilId Id technique
     * @param profilTypeProfilCode Type de profil
     * @return Utilisateurs matchant les critères
     */
    @PostMapping("utilisateur/search")
    default Page<UtilisateurDto> searchMapping(@RequestParam("utilisateurId") long utilisateurId, @RequestParam("email") String email, @RequestParam("typeUtilisateurCode") TypeUtilisateurCode typeUtilisateurCode, @RequestParam("typeUtilisateurCodeOneToOneType") TypeUtilisateurCode typeUtilisateurCodeOneToOneType, @RequestParam("profilProfilId") long profilProfilId, @RequestParam("profilTypeProfilCode") TypeProfilCode profilTypeProfilCode) {
        return this.search(utilisateurId, email, typeUtilisateurCode, typeUtilisateurCodeOneToOneType, profilProfilId, profilTypeProfilCode);
    }

    /**
     * Recherche des utilisateurs.
     * @param utilisateurId Id technique
     * @param email Email de l'utilisateur
     * @param typeUtilisateurCode Type d'utilisateur en Many to one
     * @param typeUtilisateurCodeOneToOneType Type d'utilisateur en one to one
     * @param profilProfilId Id technique
     * @param profilTypeProfilCode Type de profil
     * @return Utilisateurs matchant les critères
     */
    Page<UtilisateurDto> search(long utilisateurId, String email, TypeUtilisateurCode typeUtilisateurCode, TypeUtilisateurCode typeUtilisateurCodeOneToOneType, long profilProfilId, TypeProfilCode profilTypeProfilCode);
}
