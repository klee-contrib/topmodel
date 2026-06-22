import { domain } from 'ngx-focus-entities';
import z from 'zod';

export const DO_BOOLEEN = domain({
  schema: z.boolean(),
});
export const DO_CODE = domain({
  schema: z.string().max(3),
});
export const DO_ENTIER = domain({
  schema: z.number(),
});
export const DO_CODE_LISTE = domain({
  schema: z.array(z.string().max(10)),
});
export const DO_DATE = domain({
  schema: z.string(),
});
export const DO_DATE_HEURE = domain({
  schema: z.string(),
});
export const DO_EMAIL = domain({
  schema: z.email(),
});
export const DO_ID = domain({
  schema: z.number().positive(),
});
export const DO_SEQ_ID = domain({
  schema: z.number().positive(),
});
export const DO_LIBELLE = domain({
  schema: z.string(),
});

export const DO_LISTE = domain({
  schema: z.array(z.any()),
});

export const DO_QUANTITE = domain({
  schema: z.number(),
});

export const DO_PRIX = domain({
  schema: z.number(),
});

export const DO_TELEPHONE = domain({
  schema: z.string(),
});
