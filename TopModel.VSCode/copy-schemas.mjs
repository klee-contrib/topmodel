import { copyFileSync, mkdirSync } from "node:fs";
import { dirname, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const here = dirname(fileURLToPath(import.meta.url));
const coreDir = resolve(here, "../TopModel.Core");
const targetDir = resolve(here, "./schemas");

const schemas = ["schema.config.json", "schema.tmdgen.config.json", "schema.json"];

mkdirSync(targetDir, { recursive: true });

for (const schema of schemas) {
    copyFileSync(resolve(coreDir, schema), resolve(targetDir, schema));
}

console.info(`Les schémas ont été copiés dans ${targetDir}`);
