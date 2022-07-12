import path = require("path");
import { WebviewPanel, ExtensionContext, Uri, window, workspace, commands, ViewColumn, TextEditor, TextDocumentChangeEvent, SymbolInformation, Position } from "vscode";
import { LanguageClient } from "vscode-languageclient/node";
import { COMMANDS } from "./extension";
import { Mermaid, TopModelConfig } from "./types";

let currentPanel: TopModelPreviewPanel | null;
let currentClient: { client: LanguageClient, modelRoot: string, config: TopModelConfig };
const CLIENTS: { client: LanguageClient, modelRoot: string, config: TopModelConfig }[] = [];

export function registerPreview(context: ExtensionContext) {
    context.subscriptions.push(
        commands.registerCommand(COMMANDS.preview, () => {
            if (!currentPanel) {
                currentPanel = new TopModelPreviewPanel(context);
            } else {
                currentPanel.panel.reveal();
            }
        })
    );
}

export function addClient(client: LanguageClient, modelRoot: string, config: TopModelConfig) {
    currentClient = { client, modelRoot, config };
    CLIENTS.push({ client, modelRoot, config });
}

function updateCurrentClient(currentFsPath: string) {
    CLIENTS.forEach(c => {
        if (currentFsPath.indexOf(c.modelRoot) >= 0) {
            currentClient = c;
        }
    });
}

class TopModelPreviewPanel {
    private readonly diagramMap: Record<string, Mermaid> = {};
    public readonly panel: WebviewPanel;
    private readonly context: ExtensionContext;

    private mermaidSrcUri: Uri;
    private previewSrcUri: Uri;
    private currentFsPath: string = "";
    private matrix: { scale: number, x: number, y: number };

    constructor(context: ExtensionContext) {
        this.context = context;
        this.panel = window.createWebviewPanel(
            'preview', // Identifies the type of the webview. Used internally
            'Top Model Preview', // Title of the panel displayed to the user
            { viewColumn: ViewColumn.Beside, preserveFocus: true }, // Editor column to show the new webview panel in.
            { enableScripts: true } // Webview options. More on these later.
        );
        this.matrix = {
            x: -1,
            y: -1,
            scale: 1
        };
        this.mermaidSrcUri = this.panel.webview.asWebviewUri(Uri.file(
            path.join(this.context.extensionPath, "out", "mermaid.js")));
        this.previewSrcUri = this.panel.webview.asWebviewUri(Uri.file(
            path.join(this.context.extensionPath, "out", "topmodel-preview.js")));
        context.subscriptions.push(window.onDidChangeActiveTextEditor(async (textEditor?: TextEditor) => {
            if (textEditor) {
                this.currentFsPath = textEditor.document.uri.fsPath;
                this.matrix.x = -1;
                this.matrix.y = -1;
                this.matrix.scale = 1;
                this.refresh();
            }
        }));
        context.subscriptions.push(workspace.onDidChangeTextDocument(async (textDocumentChangeEvent?: TextDocumentChangeEvent) => {
            if (textDocumentChangeEvent) {
                this.currentFsPath = textDocumentChangeEvent.document.uri.fsPath;
                this.refresh();
            }
        }));
        if (window.activeTextEditor) {
            const textEditor = window.activeTextEditor;
            this.currentFsPath = textEditor.document.uri.fsPath;
            this.refresh();
        }
        context.subscriptions.push(this.panel.webview.onDidReceiveMessage((message) => {
            this.handleMessage(message);
        }));
        this.panel.onDidDispose(
            () => currentPanel = null,
            undefined,
            context.subscriptions
        );
    }
    handleMessage(message: any) {
        if (message.type === "update:matrix") {
            this.matrix = message.matrix;
        }
        if (message.type === "click:class") {
            const className = message.className;
            currentClient.client.sendRequest("workspace/symbol", { query: className }).then((value) => {
                const symbol = (value as SymbolInformation[]).filter(s => s.name === className)[0];
                const uri = Uri.file((symbol.location.uri as any).replace('file:///', ''));
                commands.executeCommand("editor.action.goToLocations", uri, new Position(symbol.location.range.start.line, 0), []);
            });
        }
    }
    async refresh() {
        updateCurrentClient(this.currentFsPath);
        await this.refreshDiagram();
        this.refreshContent();
    }
    async refreshDiagram() {
        const data = await currentClient.client.sendRequest("mermaid", { uri: this.currentFsPath });
        this.diagramMap[this.currentFsPath] = (data as Mermaid);
    }
    refreshContent() {
        this.panel.webview.html = this.getWebviewContent();
    }
    getWebviewContent() {
        return `<!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport">
            <style>
                body {
                    overflow: hidden;
                }
                .dragme {
                    position:relative;
                    cursor: move;
                    max-width: fit-content;
                    max-height: fit-content;
                    overflow: hidden;
                    z-index=-1
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
            </style>
        <script>const matrix = {x: ${this.matrix.x}, y: ${this.matrix.y}, scale: ${this.matrix.scale}}</script>
        <script src="${this.previewSrcUri}"></script>
        <script src="${this.mermaidSrcUri}"></script>
        <title>TopModel</title>  
    </head>   
    <body>
        <h1>${CLIENTS.length > 1 ? "[" + currentClient.config.app + "] : " : ''}${this.diagramMap[this.currentFsPath].module}</h1>
        <div>
            <button onclick="zoomClick(false)">-</button>
            <button onclick="zoomClick(true)">+</button>
        </div>
        <div class="cadre">
            <div id="draggable" class="dragme">
                ${this.getMermaidContent()}
            </div>
        </div>
    </body>
    </html>`;
    }

    getMermaidContent() {
        if (this.diagramMap[this.currentFsPath].diagram !== 'classDiagram\n\n') {
            return `<div class="mermaid"> 
            %%{init: {'securityLevel': 'loose', 'theme': 'base', 'themeVariables': { 'darkMode': true,  'primaryColor': '#333f85', 'lineColor': '#2d9cdb'}}}%%
                ${this.diagramMap[this.currentFsPath].diagram}
            </div>`;
        } else {
            return `<h1> Pas de classe persistée dans ce fichier</h1>`;
        }
    }
}