package topmodel.jpa.sample.demo.api.client.securite.utilisateur;

import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

@Service
public class UtilisateurApiClient extends AbstractUtilisateurApiClient {

	protected UtilisateurApiClient(RestTemplate restTemplate) {
		super(restTemplate, "http://localhost:8080");
	}

}
