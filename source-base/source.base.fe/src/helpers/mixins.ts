import { ElLoading } from 'element-plus';
import type { LoadingInstance } from 'element-plus/lib/components/loading/src/loading';

let loadingInstance: LoadingInstance | null = null;

/**
 * Starts the loading overlay.
 * @param target - The target element to attach the loading overlay to.
 * @param text - The text to display in the loading overlay.
 */
export function startLoading(target: HTMLElement = document.body, text: string = 'Đang tải...'): void {
  if (!loadingInstance) {
    loadingInstance = ElLoading.service({
      target,
      text,
      background: 'rgba(255, 255, 255, 0.8)',
    });
  }
}

/**
 * Ends the loading overlay.
 */
export function endLoading(): void {
  if (loadingInstance) {
    loadingInstance.close();
    loadingInstance = null;
  }
}
