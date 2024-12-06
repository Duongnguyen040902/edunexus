import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import {
  RequestCreateNotificationInterface,
  RequestDeleteNotificationInterface,
  RequestGetListNotificationsInterface,
  RequestGetNotificationDetailInterface,
  ResponseGetNotificationDetailInterface,
} from '@/types/model/notification';

export class NotificationRepository extends BaseRepository {
  public async getAllNotifications(
    data: RequestGetListNotificationsInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get( API_URL.NOTIFICATION.LIST, data, success, error);
  } 
  public async getNotification(
    data: RequestGetNotificationDetailInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.NOTIFICATION.DETAIL, data, success, error);
  }
  public async getNotificationCategories(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.NOTIFICATION.CATEGORIES, {}, success, error);
  }
  public async createNotification(
    data: RequestCreateNotificationInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.NOTIFICATION.CREATE, data, success, error, true);
  }
  public async updateNotification(
    data: ResponseGetNotificationDetailInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.NOTIFICATION.UPDATE, data, success, error, true);
  }
  public async deleteNotification(
    data: RequestDeleteNotificationInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.NOTIFICATION.DELETE}?id=${data.id}`, data, success, error);
  }
}
