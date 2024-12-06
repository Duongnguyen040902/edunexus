import { useNotificationStore } from '@/stores/notification';
import { reactive, ref } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifySuccess, notifyError } from '@/helpers/notify';
import {
  ResponseGetListNotificationsInterface,
  ResponseGetNotificationDetailInterface,
  RequestCreateNotificationInterface,
  RequestGetNotificationDetailInterface,
  RequestGetListNotificationsInterface,
  ResponseGetNotificationCategoryInterface,
  ErrorResponseNotificationInterface,
} from '@/types/model/notification';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';
import { UploadFile } from 'ant-design-vue';
import { useRoute } from 'vue-router';

export const useNotificationComposable = () => {
  const notificationStore = useNotificationStore();
  const {
    getAllNotifications,
    getNotification,
    createNotification,
    updateNotification,
    deleteNotification,
    getNotificationCategories,
  } = notificationStore;

  const errorsNotification = reactive(<ErrorResponseNotificationInterface>{
    Title: [],
    CategoryId: [],
    Descriptions: [],
    FileImage: [],
  });
  const errorsNotificationKeys: (keyof ErrorResponseNotificationInterface)[] = [
    'Title',
    'CategoryId',
    'Descriptions',
    'FileImage',
  ];
  const route = useRoute();
  const classId = ref<number>(parseInt(route.query.classId as string));
  const requestNotifications = reactive<RequestGetListNotificationsInterface>({
    classId: classId.value,
  });
  const notificationsData = reactive<ResponseGetListNotificationsInterface[]>([]);
  const requestNotificationDetail = reactive<RequestGetNotificationDetailInterface>({ id: 1 });
  const isShowModal = ref(false);
  const isShowDeleteModal = ref(false);
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const notificationDetail = reactive<ResponseGetNotificationDetailInterface>({
    id: 0,
    classId: classId.value,
    title: '',
    descriptions: '',
    categoryId: 0,
    fileImage: [],
    notificationImages: [
      {
        id: 0,
        url: '',
        notificationId: 0,
      },
    ],
    createdDate: new Date(),
    updateDate: new Date(),
  });
  const notificationCategories = ref<ResponseGetNotificationCategoryInterface>({
    id: 0,
    name: '',
  });
  const isCreateTeacher = ref(false);
  const isUpdateTeacher = ref(false);
  const formattedDateOfBirth = new Date().toISOString().split('T')[0];
  const requestDataCreateNotification = reactive<RequestCreateNotificationInterface>({
    classId: classId.value,
    title: '',
    descriptions: '',
    categoryId: 1,
    fileImage: [],
  });
  const selectedFiles = ref<File[]>([]);

  const handleFileRemove = (file: any) => {
    const index = selectedFiles.value.findIndex(f => f.name === file.name);
    if (index !== -1) {
      selectedFiles.value.splice(index, 1);
    }
  };

  const handleFileChange = (file: { raw: File }) => {
    selectedFiles.value.push(file.raw);
  };

  const handleCreateNotification = async (): Promise<boolean> => {
    startLoading();
    const formData = new FormData();
    formData.append('classId', requestDataCreateNotification.classId.toString());
    formData.append('title', requestDataCreateNotification.title);
    formData.append('descriptions', requestDataCreateNotification.descriptions);
    formData.append('categoryId', requestDataCreateNotification.categoryId.toString());
    requestDataCreateNotification.fileImage = selectedFiles.value;
    (requestDataCreateNotification.fileImage ?? []).forEach((file, index) => {
      formData.append('fileImage', file);
    });
    return new Promise(async resolve => {
      startLoading();
      await createNotification(
        formData,
        (res: any) => {
          notifySuccess('Thông báo đã được thêm thành công');
          handleFetchNotifications();
          resolve(true);
          endLoading();
        },
        (err: any) => {
          handleErrors(err);
          notifyError('Thông báo thêm thất bại');
          resolve(false);
          endLoading();
        },
      );
      endLoading();
    });
    
  };

  const handleFetchNotifications = async () => {
    startLoading();
    await getAllNotifications(
      requestNotifications,
      res => {
        if (res) {
          notificationsData.splice(0, notificationsData.length, ...res.data);
        }
      },
      (err: any) => {
        notificationsData.splice(0, notificationsData.length);
      },
    );
    endLoading();
  };

  const handleFetchNotificationDetail = async (id: RequestGetNotificationDetailInterface) => {
    startLoading();
    cancelEditing();
    await getNotification(
      id,
      (res: ResponseGetNotificationDetailInterface) => {
        if (res) {
          notificationDetail.value = res;
        }
      },
      (err: any) => {
        notifyError(err.message);
      },
    );
    endLoading();
  };

  const handleFetchNotificationCategories = async () => {
    startLoading();
    await getNotificationCategories(
      (res: any) => {
        if (res) {
          notificationCategories.value = res.data;
          requestDataCreateNotification.categoryId = res.data[0].id;
        }
      },
      (err: any) => {
        notifyError(err.message);
      },
    );
    endLoading();
  };

  const handleUpdateNotification = async (notificationData: ResponseGetNotificationDetailInterface) => {
    await updateNotification(
      notificationData,
      (res: any) => {
        notifySuccess('Cập nhật thông báo thành công');
        handleFetchNotifications();
        const id = notificationData.get('id');
        handleFetchNotificationDetail(id);
        isEditing.value = false;
      },
      (err: any) => {
        handleErrors(err);
        notifyError('Cập nhật thông báo thất bại');
      },
    );
  };

  const handleDeleteNotification = async (id: number) => {
    await deleteNotification(
      { id },
      (res: any) => {
        notifySuccess('Thông báo đã được xóa thành công');
        isShowDeleteModal.value = false;
        handleFetchNotifications();
      },
      (err: any) => {
        notifyError('Thông báo đã được xóa thất bại');
      },
    );
  };

  const handleShowModal = () => {
    clearState();
    clearErrorKeys(errorsNotificationKeys, errorsNotification);
    isShowModal.value = true;
  };
  const uploadRef = ref<UploadFile | null>(null);
  const handleCloseModal = async (file: UploadFile) => {
    clearErrorKeys(errorsNotificationKeys, errorsNotification);
    if (file) {
      uploadRef.value = file;
      uploadRef.value.clearFiles();
    }
    isShowModal.value = false;

    clearState();
  };

  const clearState = () => {
    requestDataCreateNotification.title = '';
    requestDataCreateNotification.descriptions = '';
    requestDataCreateNotification.categoryId = 1;
    requestDataCreateNotification.fileImage = [];
    selectedFiles.value = [];
  };

  const isEditing = ref(false);
  const cancelEditing = () => {
    clearErrorKeys(errorsNotificationKeys, errorsNotification);
    isEditing.value = false;
  };

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseNotificationInterface;
    mapErrorKeys(errorsNotificationKeys, errorsNotification, errorsResponse);

    // Handle FileImage errors separately
    if (errorsResponse) {
      errorsNotification.FileImage = [];
      for (const key in errorsResponse) {
        if (errorsResponse.hasOwnProperty(key) && key.startsWith('FileImage')) {
          const fileIndex = key.match(/\d+/)?.[0];
          if (fileIndex) {
            if (!errorsNotification.FileImage[fileIndex]) {
              errorsNotification.FileImage[fileIndex] = [];
            }
            errorsNotification.FileImage[fileIndex].push(...errorsResponse[key]);
          }
        }
      }
    }
  };
  const showDeleteModal = () => {
    isShowDeleteModal.value = true;
  };

  const closeDeleteModal = (value: boolean) => {
    isShowDeleteModal.value = value;
  };

  const updateRequestNotifications = async (key: keyof RequestGetListNotificationsInterface, value: any) => {
    requestNotifications[key] = value;
    await handleFetchNotifications();
  };

  const handleAddNotification = async (file: UploadFile) => {
    try {
      const success = await handleCreateNotification();
      if (success) {
        clearErrorKeys(errorsNotificationKeys, errorsNotification);
        if (file) {
          uploadRef.value = file;
          uploadRef.value.clearFiles();
        }
        isShowModal.value = false;
        clearState();
      }
    } catch (error) {
      console.error('Error creating notification:', error);
    }
  };
  const resetList = (updatedNotifications: ResponseGetListNotificationsInterface[]) => {
    notificationsData.splice(0, notificationsData.length, ...updatedNotifications);
  };

  return {
    clearErrorKeys,
    classId,
    resetList,
    updateRequestNotifications,
    closeDeleteModal,
    handleAddNotification,
    showDeleteModal,
    notificationsData,
    isEditing,
    cancelEditing,
    uploadRef,
    notificationCategories,
    handleFileRemove,
    requestNotificationDetail,
    isShowModal,
    isShowDeleteModal,
    errorsNotificationKeys,
    errorsNotification,
    handleCloseModal,
    clearState,
    handleCreateNotification,
    handleFetchNotifications,
    handleFetchNotificationDetail,
    handleUpdateNotification,
    handleDeleteNotification,
    handleShowModal,
    handleFetchNotificationCategories,
    handleFileChange,
    apiUrl,
    notificationDetail,
    requestDataCreateNotification,
    formattedDateOfBirth,
    isCreateTeacher,
    isUpdateTeacher,
  };
};
