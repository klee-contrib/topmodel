import { merge, isObject, toPairs } from "lodash";

export async function fetch<D>(
    method:
        | "GET"
        | "POST"
        | "PUT"
        | "DELETE"
        | "PATCH"
        | "OPTIONS"
        | "HEAD"
        | "TRACE"
        | "CONNECT",
    url: string,
    { body, query }: { body?: {}; query?: {} } = {},
    options: RequestInit = {}
): Promise<D> {
    url = url.replace("./", "/tenet-webapp/api/");
    const queryString = buildQueryString(query);
    url += queryString ? `${url.includes("?") ? "&" : "?"}${queryString}` : "";
    options = merge(
        { method, credentials: "include" },
        body instanceof FormData
            ? { body }
            : body
                ? {
                    headers: {
                        "Content-Type": isObject(body)
                            ? "application/json"
                            : "text/plain",
                    },
                    body: JSON.stringify(body),
                }
                : {},
        options
    );

    // On lance la requête.
    const response = await window.fetch(url, options);
    if (response.status >= 200 && response.status < 300) {
        // On détermine le type de retour en fonction du Content-Type dans le header.
        const contentType = response.headers.get("Content-Type");
        if (contentType?.includes("application/json")) {
            return response.json();
        } else if (contentType?.includes("text/plain")) {
            return response.text() as any;
        } else if (response.status === 204) {
            return null as any; // Cas réponse vide.
        } else {
            return response as any;
        }
    } else {
        // Retour en erreur
        // On détermine le type de retour en fonction du Content-Type dans le header.
        const contentType = response.headers.get("Content-Type");
        if (contentType?.includes("application/json")) {
            // Pour une erreur JSON, on la parse pour trouver et enregistrer les erreurs "attendues".
            throw response.json();
        } else {
            // Sinon, on renvoie le body de la réponse sous format texte (faute de mieux).
            console.error(`${response.status} error when calling ${url}`);
            throw response.text();
        }
    }
}

/** Construit le query string associé à l'objet donné. */
function buildQueryString(obj: any, prefix = ""): string {
    let queryString = "";
    if (isObject(obj)) {
        queryString = toPairs(obj).reduce(
            (acc, [key, value]) =>
                acc +
                (acc && acc !== "" && !acc.endsWith("&") && value !== undefined
                    ? "&"
                    : "") +
                buildQueryString(
                    value,
                    prefix !== ""
                        ? Array.isArray(obj)
                            ? prefix
                            : `${prefix}.${key}`
                        : key
                ),
            ""
        );
    } else if (prefix && prefix !== "" && obj !== undefined) {
        queryString = `${prefix}=${encodeURIComponent(obj)}`;
    }
    return queryString;
}