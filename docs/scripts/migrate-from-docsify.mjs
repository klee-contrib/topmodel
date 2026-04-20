/**
 * One-shot migration helpers: fix internal links and convert docsify-tabs to MDX.
 */
import fs from 'node:fs';
import path from 'node:path';
import {fileURLToPath} from 'node:url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const DOCS_ROOT = path.join(__dirname, '..', 'docs');

const TAB_FILES = new Set([
  'getting-started/02_classe_persistee.md',
  'getting-started/03_liste_ref.md',
  'getting-started/04_association.md',
  'getting-started/05_dto.md',
  'getting-started/06_endpoint.md',
]);

function walkDir(dir, files = []) {
  for (const name of fs.readdirSync(dir)) {
    const p = path.join(dir, name);
    const st = fs.statSync(p);
    if (st.isDirectory()) walkDir(p, files);
    else if (name.endsWith('.md') || name.endsWith('.mdx')) files.push(p);
  }
  return files;
}

function fixLinks(content, relFromDocs) {
  let c = content.replace(/ <!-- \{docsify-ignore-all\} -->/g, '');
  c = c.replace(/ <!-- \{docsify-ignore\} -->/g, '');

  // Docsify ?id= anchors -> Docusaurus #anchors (only used in internal doc links here)
  c = c.replace(/\?id=/g, '#');

  // Absolute paths from docsify site root -> Docusaurus /docs/...
  const absPrefixes = [
    ['/model/', '/docs/model/'],
    ['/getting-started/', '/docs/getting-started/'],
    ['/generator/', '/docs/generator/'],
    ['/tmdgen/', '/docs/tmdgen/'],
  ];
  for (const [from, to] of absPrefixes) {
    c = c.split(`](${from}`).join(`](${to}`);
  }

  c = c.replace(/\]\(\/model\.md\)/g, '](/docs/model)');
  c = c.replace(/\]\(\/generator\.md\)/g, '](/docs/generator)');
  c = c.replace(/\]\(\/tmdgen\.md\)/g, '](/docs/tmdgen)');
  c = c.replace(/\]\(\/configuration\.md\)/g, '](/docs/configuration)');
  c = c.replace(/\]\(\/cli\.md\)/g, '](/docs/cli)');

  // Typo in original doc
  c = c.replace(/\/model\/dataFlows\.mddata\)/g, '/docs/model/dataFlows)');

  // Strip .md from internal doc links (relative and /docs/)
  c = c.replace(/\]\((\.\.?\/[^)#\s]+)\.md(#?[^)]*)\)/g, ']($1$2)');
  c = c.replace(/\]\(\/(docs\/[^)#\s]+)\.md(#?[^)]*)\)/g, '](/$1$2)');

  return c;
}

function convertTabsToMdx(inner) {
  const headerRe = /^(#{2,6}) \*\*(.+?)\*\*\s*$/gm;
  const matches = [...inner.matchAll(headerRe)];
  if (matches.length === 0) return null;

  const parts = [];
  for (let k = 0; k < matches.length; k++) {
    const start = matches[k].index + matches[k][0].length;
    const end = matches[k + 1]?.index ?? inner.length;
    const label = matches[k][2].trim();
    const body = inner.slice(start, end).trim();
    const value =
      label
        .toLowerCase()
        .normalize('NFD')
        .replace(/\p{M}/gu, '')
        .replace(/[^a-z0-9]+/g, '-')
        .replace(/^-|-$/g, '') || `tab-${k}`;
    const safeLabel = label.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
    parts.push(
      `<TabItem value="${value}" label="${safeLabel}">\n\n${body}\n\n</TabItem>`,
    );
  }
  return `<Tabs>\n${parts.join('\n')}\n</Tabs>`;
}

function convertFileTabs(content) {
  const start = '<!-- tabs:start -->';
  const end = '<!-- tabs:end -->';
  let out = content;
  let converted = false;
  while (out.includes(start)) {
    const i = out.indexOf(start);
    const j = out.indexOf(end, i);
    if (j === -1) break;
    const inner = out.slice(i + start.length, j).trim();
    const mdxBlock = convertTabsToMdx(inner);
    if (!mdxBlock) break;
    out = out.slice(0, i) + mdxBlock + out.slice(j + end.length);
    converted = true;
  }
  if (!converted) return content;
  const imports = `import Tabs from '@theme/Tabs';\nimport TabItem from '@theme/TabItem';\n\n`;
  return imports + out;
}

function main() {
  const files = walkDir(DOCS_ROOT);
  for (const file of files) {
    const rel = path.relative(DOCS_ROOT, file).replace(/\\/g, '/');
    let content = fs.readFileSync(file, 'utf8');
    content = fixLinks(content, rel);
    if (TAB_FILES.has(rel)) {
      content = convertFileTabs(content);
      const newPath = file.replace(/\.md$/, '.mdx');
      fs.writeFileSync(newPath, content, 'utf8');
      if (newPath !== file) fs.unlinkSync(file);
    } else {
      fs.writeFileSync(file, content, 'utf8');
    }
  }
}

main();
