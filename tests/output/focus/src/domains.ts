import z from "zod";
import { domain } from "@focus4/form-toolbox";
export const DO_BOOLEEN = domain(z.boolean());
export const DO_CODE = domain(z.string().max(3));
export const DO_ENTIER = domain(z.number());
export const DO_CODE_LISTE = domain(z.array(z.string().max(10)));
export const DO_DATE = domain(z.string());
export const DO_DATE_HEURE = domain(z.string());
export const DO_EMAIL = domain(z.email());
export const DO_ID = domain(z.number().positive());
export const DO_ID_2 = domain(z.number().positive());
export const DO_SEQ_ID = domain(z.number().positive());
export const DO_LIBELLE = domain(z.string());

export const DO_LISTE = domain(z.array(z.any()));

export const DO_QUANTITE = domain(z.number());

export const DO_PRIX = domain(z.number());

export const DO_TELEPHONE = domain(z.string());
