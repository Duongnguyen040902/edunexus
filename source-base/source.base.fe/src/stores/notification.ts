import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { NotificationRepository } from '@/repositories/repository-notification';
import { reactive } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import {
  ResponseGetListNotificationsInterface,
  ResponseGetNotificationDetailInterface,
  ResponseGetNotificationCategoryInterface,
  RequestCreateNotificationInterface,
  RequestDeleteNotificationInterface,
  RequestGetNotificationDetailInterface,
  RequestGetListNotificationsInterface,
} from '@/types/model/notification';

export const useNotificationStore = defineStore('notification', () => {
  const notificationFactory = RepositoryFactory.create('notification') as NotificationRepository;
  const notifications = reactive<{ value: ResponseGetListNotificationsInterface | null }>({ value: null });
  const notificationDetail = reactive<{ value: ResponseGetNotificationDetailInterface | null }>({ value: null });
  const notificationCategories = reactive<{ value: ResponseGetNotificationCategoryInterface[] | null }>({
    value: null,
  });
  const getAllNotifications = async (
    params: RequestGetListNotificationsInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await notificationFactory.getAllNotifications(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data,
          message: res.message,
          succeeded: res.succeeded,
      };
      return success(successResponse);

      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getNotification = async (
    params: RequestGetNotificationDetailInterface,
    success: (res: ResponseGetNotificationDetailInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await notificationFactory.getNotification(
      { id: params },
      res => {
        notificationDetail.value = res.data as ResponseGetNotificationDetailInterface;
        return success(res.data as ResponseGetNotificationDetailInterface);
      },
      err => {
        error(err);
      },
    );
  };

  const getNotificationCategories = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await notificationFactory.getNotificationCategories(
      res => {
        notificationCategories.value = res.data as ResponseGetNotificationCategoryInterface[];
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const createNotification = async (
    params: RequestCreateNotificationInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await notificationFactory.createNotification(
      params,
      res => {
        return success(res);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const updateNotification = async (
    params: ResponseGetNotificationDetailInterface,
    success: (res: SuccessResponse) => boolean,
    error: (err: ErrorResponse) => boolean,
  ) => {
    await notificationFactory.updateNotification(
      params,
      res => {
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const deleteNotification = async (
    params: RequestDeleteNotificationInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await notificationFactory.deleteNotification(
      params,
      res => {
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    notifications,
    notificationDetail,
    notificationCategories,
    getNotificationCategories,
    getAllNotifications,
    getNotification,
    createNotification,
    updateNotification,
    deleteNotification,
  };
});
