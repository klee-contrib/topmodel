////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.exemple.name.api.controller.utilisateur;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto;

@RestController
@RequestMapping("utilisateur")
public interface IutilisateurController {


    /**
     * Charge le détail d'un utilisateur.
     * @param utilisateurId Id technique
     * @returns Le détail de l'utilisateur
     */
    @GetMapping("api//{utiId}")
    default UtilisateurDto getUtilisateurMapping(@RequestParam("utilisateurId") long utilisateurId) {
        return this.getUtilisateur(utilisateurId);
    }

    /**
     * Charge le détail d'un utilisateur.
     * @param utilisateurId Id technique
     * @returns Le détail de l'utilisateur
     */
    UtilisateurDto getUtilisateur(long utilisateurId);
}
