import { ipc } from "./ipc";

document.addEventListener("mousedown", async (event) => {
  if (event.buttons !== 1) {
    return;
  }

  if (!(event.target instanceof Element)) {
    return;
  }

  if (!event.target.hasAttribute("lantern-drag-region")) {
    return;
  }

  event.preventDefault();
  event.stopImmediatePropagation();

  if (event.detail === 2) {
    const isMaximized = await ipc.invoke<boolean>("lantern.window.isMaximized");
    if (isMaximized) {
      await ipc.invoke("lantern.window.restore");
    } else {
      await ipc.invoke("lantern.window.maximize");
    }
  } else {
    await ipc.invoke("lantern.window.startDragMove");
  }
});
