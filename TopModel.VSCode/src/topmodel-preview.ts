import mermaid from "mermaid";
mermaid.initialize({ securityLevel: "loose" });

const ZOOM_SPEED = 0.4;
function zoomClick(inOut: boolean) {
    const event = new CustomEvent("zoomClick", { detail: { inOut } });
    document.dispatchEvent(event);
    return false;
}

function scope(scope: "file" | "module" | "model") {
    const event = new CustomEvent("update:scope", { detail: { scope } });
    document.dispatchEvent(event);
    return false;
}

function displayCodeClick() {
    const codeElements = document.getElementsByClassName("code-element") as any;
    for (const element of codeElements) {
        const showAs = element.classList.contains("code-wrapper") ? "flex" : "block";
        element.style.display = element.style.display === "none" ? showAs : "none";
    }
    const umlElements = document.getElementsByClassName("uml-element") as any;
    for (const element of umlElements) {
        element.style.display = element.style.display === "none" ? "block" : "none";
    }
}

function copyCode(diagram: string) {
    const textArea = document.createElement("textarea");
    textArea.value = `${diagram}`;
    document.body.appendChild(textArea);
    textArea.select();
    document.execCommand("copy");
    document.body.removeChild(textArea);
    showFeedback("Code copié dans le presse-papiers");
}
// Expose functions used in inline HTML onclick handlers (required with IIFE bundle format)
(globalThis as any).zoomClick = zoomClick;
(globalThis as any).scope = scope;
(globalThis as any).displayCodeClick = displayCodeClick;
(globalThis as any).copyCode = copyCode;

function showFeedback(message: string) {
    const feedbackElement = document.createElement("div");
    feedbackElement.textContent = message;
    feedbackElement.style.position = "fixed";
    feedbackElement.style.top = "10px";
    feedbackElement.style.right = "10px";
    feedbackElement.style.backgroundColor = "green";
    feedbackElement.style.color = "white";
    feedbackElement.style.padding = "10px";
    feedbackElement.style.borderRadius = "5px";
    document.body.appendChild(feedbackElement);

    setTimeout(() => {
        document.body.removeChild(feedbackElement);
    }, 3000);
}
(function () {
    let offsetX: number;
    let offsetY: number;
    let drag: boolean;
    let hasDragged: boolean;
    let draggable!: HTMLElement;
    //@ts-ignore
    const vscode = acquireVsCodeApi();

    function startDrag(e: DragEvent) {
        draggable.classList.remove("zooming");
        offsetX = e.clientX;
        offsetY = e.clientY;
        drag = true;
        hasDragged = false;
        document.onmousemove = dragDiv;
        return false;
    }
    function dragDiv(e: MouseEvent) {
        if (!drag) {
            return;
        }
        hasDragged = true;
        // move div element
        //@ts-ignore
        matrix.x += e.clientX - offsetX;
        //@ts-ignore
        matrix.y += e.clientY - offsetY;
        offsetX = e.clientX;
        offsetY = e.clientY;
        updateScaleAndPosition();
        return false;
    }
    function stopDrag(event: MouseEvent) {
        drag = false;
        event.stopPropagation();
    }
    function zoomInOut(inOut: boolean) {
        const zoomScale = 1 + (inOut ? 1 : -1) * ZOOM_SPEED;
        //@ts-ignore
        matrix.scale *= zoomScale;
    }
    function handleZoom(wheelEvent: WheelEvent) {
        wheelEvent.preventDefault();
        const inOut = wheelEvent.deltaY < 0;
        zoomInOut(inOut);
        zoomTranslate(inOut, wheelEvent.clientX, wheelEvent.clientY);
        updateScaleAndPosition();
    }
    function handleZoomClick(clickZoom: CustomEvent) {
        drag = false;
        draggable.classList.add("zooming");
        const cadre = document.querySelector(".cadre") as HTMLElement;
        const rect = cadre.getBoundingClientRect();
        const centerX = rect.left + rect.width / 2;
        const centerY = rect.top + rect.height / 2;
        zoomInOut(clickZoom.detail.inOut);
        zoomTranslate(clickZoom.detail.inOut, centerX, centerY);
        updateScaleAndPosition();
    }
    function updateScaleAndPosition() {
        const targ = document.getElementById("draggable");
        //@ts-ignore
        targ.style.transform = `matrix(${matrix.scale}, 0, 0, ${matrix.scale}, ${matrix.x}, ${matrix.y})`;
        vscode.postMessage({
            type: "update:matrix",
            //@ts-ignore
            matrix,
        });
    }

    function zoomTranslate(inOut: boolean, centerX: number, centerY: number) {
        const graph = document.getElementsByTagName("svg")[0];
        const x = graph.clientWidth / 2;
        const y = graph.clientHeight / 2;
        const zoomScale = 1 + (inOut ? 1 : -1) * ZOOM_SPEED;
        //@ts-ignore
        matrix.x -= (centerX - matrix.x - x) * zoomScale - (centerX - matrix.x - x);
        //@ts-ignore
        matrix.y -= (centerY - matrix.y - y) * zoomScale - (centerY - matrix.y - y);
    }
    function onClassClick(classId: string) {
        if (!hasDragged) {
            // mermaid v11 format: mermaid-{timestamp}-classId-{ClassName}-{index}
            const className = classId.split("-classId-")[1].replace(/-\d+$/, "");
            vscode.postMessage({
                type: "click:class",
                className,
            });
        }
    }
    function initPosition() {
        //@ts-ignore
        if (matrix.y === -1) {
            //@ts-ignore
            matrix.y = 0;
        }
        //@ts-ignore
        if (matrix.x === -1) {
            //@ts-ignore
            matrix.x = 0;
        }
    }
    function initNavigation() {
        document.querySelectorAll('[id*="-classId-"]').forEach((element: Element) => {
            (element as HTMLElement).onclick = (_event: MouseEvent) => {
                onClassClick((element as HTMLElement).id);
            };
            element.classList.add("clickable");
        });
    }
    window.onload = async function () {
        draggable = document.getElementById("draggable")!;
        const cadre = document.querySelector(".cadre") as HTMLElement;

        cadre.addEventListener("wheel", handleZoom);
        draggable.addEventListener("transitionend", () => draggable.classList.remove("zooming"));
        document.onmouseup = stopDrag;

        //@ts-ignore
        cadre.onmousedown = startDrag;
        //@ts-ignore
        document.addEventListener("zoomClick", handleZoomClick);
        document.addEventListener("update:scope", (event) => {
            vscode.postMessage({
                type: "update:scope",
                //@ts-ignore
                scope: event.detail.scope,
            });
        });
        await mermaid.run();
        initNavigation();
        initPosition();
        updateScaleAndPosition();
    };
})();
