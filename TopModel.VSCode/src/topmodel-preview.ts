const ZOOM_SPEED = 0.4;
function zoomClick(inOut: boolean) {
    const event = new CustomEvent("zoomClick", { detail: { inOut } });
    document.dispatchEvent(event);
    return false;
}

(function () {
    let offsetX: number;
    let offsetY: number;
    let drag: boolean;
    //@ts-ignore
    const vscode = acquireVsCodeApi();

    function startDrag(e: DragEvent) {
        // calculate event X, Y coordinates
        offsetX = e.clientX;
        offsetY = e.clientY;

        drag = true;

        // move div element
        document.onmousemove = dragDiv;
        return false;
    }
    function dragDiv(e: MouseEvent) {
        if (!drag) {
            return;
        }
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
        const inOut = wheelEvent.deltaY < 0;
        zoomInOut(inOut);
        zoomTranslate(inOut, wheelEvent.clientX, wheelEvent.clientY);
        updateScaleAndPosition();
    }
    function handleZoomClick(clickZoom: CustomEvent) {
        drag = false;
        zoomInOut(clickZoom.detail.inOut);
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
        if (!drag) {
            const lowerClassName = classId.split("-")[1];
            vscode.postMessage({
                type: "click:class",
                className: lowerClassName.charAt(0).toUpperCase() + lowerClassName.slice(1),
            });
        }
    }
    function initPosition() {
        //@ts-ignore
        if (matrix.y === -1) {
            const svg = document.getElementsByTagName("svg")[0].viewBox.baseVal;
            //@ts-ignore
            matrix.y = -svg.height / 2 + 100;
        }
    }
    function initNavigation() {
        document.querySelectorAll('[id*="classid"]').forEach((element: Element) => {
            (element as HTMLElement).onclick = (event: MouseEvent) => {
                onClassClick((element as HTMLElement).id);
            };
            element.classList.add("clickable");
        });
    }
    window.onload = function () {
        const draggable = document.getElementById("draggable")!;

        draggable.addEventListener("wheel", handleZoom);
        document.onmouseup = stopDrag;

        //@ts-ignore
        draggable.onmousedown = startDrag;
        //@ts-ignore
        document.addEventListener("zoomClick", handleZoomClick);
        setTimeout(() => {
            initNavigation();
            initPosition();
            updateScaleAndPosition();
        }, 100);
    };
})();
