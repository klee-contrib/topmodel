import { autorun, makeAutoObservable } from "mobx";
import path = require("path");
import {
    WebviewPanel,
    ExtensionContext,
    Uri,
    window,
    workspace,
    ViewColumn,
    TextEditor,
    TextDocumentChangeEvent,
    SymbolInformation,
    Position,
    Range,
} from "vscode";
import { Mermaid } from "./types";
import { t } from "./i18n";
import { LanguageClient } from "vscode-languageclient/node";

export class TopModelPreviewPanel {
    private readonly diagramMap: Record<string, Mermaid> = {};
    private readonly context: ExtensionContext;
    private readonly previewSrcUri: Uri;
    private matrix: { scale: number; x: number; y: number };

    public readonly panel: WebviewPanel;
    public currentFsPath: string = "";
    public currentScope: "file" | "module" | "model" = "file";

    constructor(
        context: ExtensionContext,
        private readonly client?: LanguageClient,
    ) {
        makeAutoObservable(this);
        autorun(() => this.refresh());
        this.context = context;
        this.panel = window.createWebviewPanel(
            "preview", // Identifies the type of the webview. Used internally
            "Top Model Preview", // Title of the panel displayed to the user
            { viewColumn: ViewColumn.Beside, preserveFocus: true }, // Editor column to show the new webview panel in.
            { enableScripts: true }, // Webview options. More on these later.
        );

        this.matrix = {
            x: -1,
            y: -1,
            scale: 1,
        };
        this.previewSrcUri = this.panel.webview.asWebviewUri(
            Uri.file(path.join(this.context.extensionPath, "out", "topmodel-preview.js")),
        );

        this.initSubscriptions();

        if (window.activeTextEditor) {
            this.currentFsPath = window.activeTextEditor.document.uri.fsPath;
        }
    }

    private initSubscriptions() {
        this.context.subscriptions.push(
            window.onDidChangeActiveTextEditor(async (textEditor?: TextEditor) => {
                if (textEditor?.document.uri.fsPath.endsWith(".tmd")) {
                    this.currentFsPath = textEditor.document.uri.fsPath;
                    this.currentScope = "file";
                    this.matrix.x = -1;
                    this.matrix.y = -1;
                    this.matrix.scale = 1;
                }
            }),
            workspace.onDidChangeTextDocument(async (textDocumentChangeEvent?: TextDocumentChangeEvent) => {
                if (textDocumentChangeEvent?.document.uri.fsPath.endsWith(".tmd")) {
                    this.currentFsPath = textDocumentChangeEvent.document.uri.fsPath;
                    this.currentScope = "file";
                }
            }),
            this.panel.webview.onDidReceiveMessage((message) => {
                this.handleMessage(message);
            }),
            workspace.onDidSaveTextDocument((e) => {
                if (e.uri.fsPath === this.currentFsPath) {
                    this.refresh();
                }
            }),
        );
    }

    async handleMessage(message: any) {
        if (message.type === "update:matrix") {
            this.matrix = message.matrix;
        }
        if (message.type === "update:scope") {
            this.currentScope = message.scope;
        }
        if (message.type === "click:class") {
            const className = message.className;
            const symbolInformations: SymbolInformation[] =
                (await this.client?.sendRequest("workspace/symbol", {
                    query: className,
                })) ?? [];
            const symbol = symbolInformations.find((s) => s.name === className);
            const position = new Position(symbol!.location.range.start.line, 0);
            const uri = Uri.parse(symbol!.location.uri as any);
            const textEditor =
                window.visibleTextEditors.find((t) => t.document.uri.fsPath === uri.fsPath) ??
                (window.activeTextEditor?.document.uri.fsPath.endsWith(".tmd") ? window.activeTextEditor : undefined) ??
                window.visibleTextEditors.find((t) => t.document.uri.fsPath.endsWith(".tmd")) ??
                window.visibleTextEditors[0];
            await window.showTextDocument(uri, {
                preserveFocus: false,
                viewColumn: textEditor.viewColumn,
            });
            textEditor?.revealRange(new Range(position, position));
        }
    }

    async refresh() {
        if (this.client) {
            const data = await this.client.sendRequest("mermaid", {
                uri: this.currentFsPath,
                scope: this.currentScope,
            });
            this.diagramMap[this.currentFsPath] = data as Mermaid;
            this.panel.webview.html = this.webviewContent;
        }
    }

    get webviewContent() {
        if (!(this.diagramMap[this.currentFsPath] && this.client)) {
            return "";
        }
        return `<!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport">
            <style>
                html, body {
                    height: 100%;
                    margin: 0;
                    padding: 0;
                    overflow: hidden;
                }
                body {
                    display: flex;
                    flex-direction: column;
                }
                .content {
                    flex: 1;
                    display: flex;
                    flex-direction: column;
                    min-height: 0;
                }
                .dragme {
                    position: relative;
                    cursor: move;
                    max-width: fit-content;
                    max-height: fit-content;
                    overflow: hidden;
                }
                .dragme svg *, .dragme foreignObject * {
                    cursor: inherit;
                }
                .cadre {
                    position: relative;
                    flex: 1;
                    min-height: 0;
                    width: 100%;
                    overflow: hidden;
                    cursor: move;
                }
#draggable.zooming {
                    transition: transform 0.2s ease-out;
                }
                #draggable .mermaid g.fileReference.node rect {
                    opacity: 0.5;
                }
                h1 {
                    margin: 1rem;
                    font-size: 1.5rem;
                }
                .clickable{
                    cursor: pointer;
                }
                .clickable:hover {
                    background-color: rgba(2, 75, 153, 0.4);
                }
                button {
                    margin: 0.5rem;
                    padding: 0.5rem 1rem;
                    border: none;
                    border-radius: 4px;
                    background-color: #333f85;
                    color: #fff;
                    cursor: pointer;
                }
                button:hover {
                    background-color: rgb(2, 75, 153);
                }
                .code-wrapper {
                    flex: 1;
                    min-height: 0;
                    flex-direction: column;
                    overflow: hidden;
                }
                code {
                    display: block;
                    flex: 1;
                    min-height: 0;
                    margin: 1rem;
                    padding: 1rem;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    font-family: monospace;
                    white-space: pre-wrap;
                    position: relative;
                    overflow: auto;
                }
                .copy-button {
                    width: 5rem;
                    height: 3rem;
                    display: block;
                    position: relative;
                    top: 5rem;
                    left: 85%;
                    z-index: 1;
                    margin: 0;
                    padding: 2px 8px;

                    background-color: rgba(60,60,60,0.8);
                    transition: opacity 0.15s ease;
                }

                .menu {
                    display: flex;
                    justify-content: flex-start;
                    align-items: center;
                }
            </style>
        <script>const matrix = {x: ${this.matrix.x}, y: ${this.matrix.y}, scale: ${this.matrix.scale}}</script>
        <script src="${this.previewSrcUri}"></script>
        <title>TopModel</title>
    </head>
    <body>
        <h1>
            <span class="clickable" onclick="scope('model')">${this.appTitle}</span>
            ${
                this.currentScope === "model"
                    ? ""
                    : `/ <span class="clickable" onclick="scope('module')">${this.moduleTitle}</span>`
            }
            ${
                this.currentScope === "file"
                    ? `/ <span class="clickable" onclick="scope('file')">${this.fileTitle}</span>`
                    : ""
            }
        </h1>
        <div class="content">
            <nav class="menu">
                <button onclick="displayCodeClick()">${t("displayHideCode")}</button>
                <button style="display: block;" class="uml-element" onclick="zoomClick(false)">-</button>
                <button style="display: block;" class="uml-element" onclick="zoomClick(true)">+</button>
            </nav>
            <div class="cadre uml-element">
<div id="draggable" class="dragme" style="display: block;">
                    ${this.mermaidContent}
                </div>
            </div>
            <div class="code-element code-wrapper" style="display: none;">
            <button style="display: none;" class="copy-button code-element" onclick="copyCode(currentDiagram)">
                Copier
            </button>
            <code id="sourceCode" style="display: none; overflow: auto;" class="code-element">
                    ${this.diagramMap[this.currentFsPath].diagram.replaceAll("\n", "<br/>")}
                </code>
            </div>
        </div>
    </body>
    <script>
        const currentDiagram = \`${this.diagramMap[this.currentFsPath].diagram}\`;
    </script>
    </html>`;
    }

    get appTitle() {
        return `[${this.diagramMap[this.currentFsPath].app}]`;
    }

    get moduleTitle() {
        return `${this.diagramMap[this.currentFsPath].module}`;
    }

    get fileTitle() {
        return `${this.diagramMap[this.currentFsPath].fileName}`;
    }

    get mermaidContent() {
        if (
            this.diagramMap[this.currentFsPath]?.diagram &&
            this.diagramMap[this.currentFsPath].diagram !== "classDiagram\n\n"
        ) {
            return `<pre class="mermaid">
            %%{init: {'theme': 'base', 'hideEmptyMembersBox': true, 'themeVariables': { 'darkMode': true,  'primaryColor': '#333f85', 'lineColor': '#2d9cdb'}}}%%
                ${this.diagramMap[this.currentFsPath].diagram}
            </pre>`;
        } else {
            return `<h1>${t("noPersistenceClassInThisFile")}</h1>`;
        }
    }
}
