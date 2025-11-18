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
import { Application } from "./application";
import { Mermaid } from "./types";

export class TopModelPreviewPanel {
    private readonly diagramMap: Record<string, Mermaid> = {};
    private readonly context: ExtensionContext;
    private readonly mermaidSrcUri: Uri;
    private readonly previewSrcUri: Uri;
    private matrix: { scale: number; x: number; y: number };

    public readonly panel: WebviewPanel;
    public currentFsPath: string = "";
    public currentScope: "file" | "module" | "model" = "file";

    constructor(context: ExtensionContext, private readonly applications: Application[]) {
        makeAutoObservable(this);
        autorun(() => this.refresh());
        this.context = context;
        this.panel = window.createWebviewPanel(
            "preview", // Identifies the type of the webview. Used internally
            "Top Model Preview", // Title of the panel displayed to the user
            { viewColumn: ViewColumn.Beside, preserveFocus: true }, // Editor column to show the new webview panel in.
            { enableScripts: true } // Webview options. More on these later.
        );

        this.matrix = {
            x: -1,
            y: -1,
            scale: 1,
        };
        this.mermaidSrcUri = this.panel.webview.asWebviewUri(
            Uri.file(path.join(this.context.extensionPath, "out", "mermaid.js"))
        );
        this.previewSrcUri = this.panel.webview.asWebviewUri(
            Uri.file(path.join(this.context.extensionPath, "out", "topmodel-preview.js"))
        );

        this.initSubscriptions();

        if (window.activeTextEditor) {
            this.currentFsPath = window.activeTextEditor.document.uri.fsPath;
        }
    }

    private initSubscriptions() {
        this.context.subscriptions.push(
            window.onDidChangeActiveTextEditor(async (textEditor?: TextEditor) => {
                if (textEditor && textEditor.document.uri.fsPath.endsWith(".tmd")) {
                    this.currentFsPath = textEditor.document.uri.fsPath;
                    this.currentScope = "file";
                    this.matrix.x = -1;
                    this.matrix.y = -1;
                    this.matrix.scale = 1;
                }
            }),
            workspace.onDidChangeTextDocument(async (textDocumentChangeEvent?: TextDocumentChangeEvent) => {
                if (textDocumentChangeEvent && textDocumentChangeEvent.document.uri.fsPath.endsWith(".tmd")) {
                    this.currentFsPath = textDocumentChangeEvent.document.uri.fsPath;
                    this.currentScope = "file";
                }
            }),
            this.panel.webview.onDidReceiveMessage((message) => {
                this.handleMessage(message);
            })
        );
    }

    get currentApplication(): Application {
        if (this.currentFsPath) {
            return (
                this.applications.find((c) => {
                    if (this.currentFsPath.indexOf(c.modelRoot || "") >= 0) {
                        return c;
                    }
                }) ?? this.applications[0]
            );
        }
        return this.applications[0];
    }

    async handleMessage(message: any) {
        if (message.type === "update:matrix") {
            this.matrix = message.matrix;
        }
        if (message.type === "update:scope") {
            this.currentScope = message.scope as any;
        }
        if (message.type === "click:class") {
            const className = message.className;
            const symbolInformations: SymbolInformation[] =
                (await this.currentApplication?.client?.sendRequest("workspace/symbol", {
                    query: className,
                })) ?? [];
            const symbol = symbolInformations.filter((s) => s.name === className)[0];
            const position = new Position(symbol.location.range.start.line, 0);
            const uri = Uri.parse(symbol.location.uri as any);
            const textEditor =
                window.visibleTextEditors.filter((t) => t.document.uri.fsPath === uri.fsPath)[0] ??
                (window.activeTextEditor?.document.uri.fsPath.endsWith(".tmd") ? window.activeTextEditor : undefined) ??
                window.visibleTextEditors.filter((t) => t.document.uri.fsPath.endsWith(".tmd"))[0] ??
                window.visibleTextEditors[0];
            await window.showTextDocument(uri, {
                preserveFocus: false,
                viewColumn: textEditor.viewColumn,
            });
            textEditor?.revealRange(new Range(position, position));
        }
    }

    async refresh() {
        if (this.currentApplication?.client) {
            const data = await this.currentApplication.client.sendRequest("mermaid", {
                uri: this.currentFsPath,
                scope: this.currentScope,
            });
            this.diagramMap[this.currentFsPath] = data as Mermaid;
            this.panel.webview.html = this.webviewContent;
        }
    }

    get webviewContent() {
        if (!(this.diagramMap[this.currentFsPath] && this.currentApplication)) {
            return "";
        }
        return `<!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport">
            <style>
                body {
                    margin: 0;
                    padding: 0;
                    overflow: auto;
                }
                .dragme {
                    position: relative;
                    cursor: move;
                    max-width: fit-content;
                    max-height: fit-content;
                    overflow: hidden;
                }
                .cadre {
                    height: 100%;
                    width: 100%;
                    overflow: hidden;
                    margin-bottom: 1rem;
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
                code {
                    display: block;
                    margin: 1rem;
                    padding: 1rem;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    font-family: monospace;
                    white-space: pre-wrap;
                    position: relative;
                }
                .copy-button {
                    position: absolute;
                    top: 10px;
                    right: 10px;
                    background-color: transparent;
                    cursor: pointer;
                }
            </style>
        <script>const matrix = {x: ${this.matrix.x}, y: ${this.matrix.y}, scale: ${this.matrix.scale}}</script>
        <script src="${this.previewSrcUri}"></script>
        <script src="${this.mermaidSrcUri}"></script>
        <title>TopModel</title>
    </head>
    <body>
        <h1>
            <span class="clickable" onclick="scope('model')">${this.appTitle}</span>
            ${
                this.currentScope !== "model"
                    ? `/ <span class="clickable" onclick="scope('module')">${this.moduleTitle}</span>`
                    : ""
            }
            ${
                this.currentScope === "file"
                    ? `/ <span class="clickable" onclick="scope('file')">${this.fileTitle}</span>`
                    : ""
            }
            </h1>
        <div>
            <button onclick="zoomClick(false)">-</button>
            <button onclick="zoomClick(true)">+</button>
            </div>
            <div class="cadre">
            <div id="draggable" class="dragme">
            ${this.mermaidContent}
            </div>
            </div>
            <button onclick="displayCodeClick()">Afficher/masquer le code</button>
            <code id="sourceCode" style="display: none; overflow: auto;">
            <button class="copy-button" onclick="copyCode(currentDiagram)">Copier</button>
            ${this.diagramMap[this.currentFsPath].diagram.replaceAll("\n", "<br/>")}
        </code>
    </body>
    <script>
        const currentDiagram = \`${this.diagramMap[this.currentFsPath].diagram}\`;
    </script>
    </html>`;
    }

    get appTitle() {
        return "[" + this.currentApplication.config.app + "]";
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
            %%{init: {'securityLevel': 'loose', 'theme': 'base', 'themeVariables': { 'darkMode': true,  'primaryColor': '#333f85', 'lineColor': '#2d9cdb'}}}%%
                ${this.diagramMap[this.currentFsPath].diagram}
            </pre>`;
        } else {
            return `<h1> Pas de classe persistée dans ce fichier</h1>`;
        }
    }
}
