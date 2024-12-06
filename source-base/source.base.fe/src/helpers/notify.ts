import { ElNotification } from 'element-plus';

/**
 * Displays a success notification.
 * @param message - The message to display in the notification.
 * @param title - The title of the notification (optional, default is 'Success').
 */
export function notifySuccess(message: string, title: string = 'Thành công'): void {
  ElNotification({
    title,
    message,
    type: 'success',
    duration: 3000, 
  });
}

/**
 * Displays an error notification.
 * @param message - The message to display in the notification.
 * @param title - The title of the notification (optional, default is 'Error').
 */
export function notifyError(message: string, title: string = 'Thất bại'): void {
  ElNotification({
    title,
    message,
    type: 'error',
    duration: 3000,
  });
}
