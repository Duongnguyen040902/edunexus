// source-base/source.base.fe/src/composables/pupil-application.ts
import { endLoading, startLoading } from '@/helpers/mixins';
import { useSemesterStore } from './../stores/semester';
import { useClassApplicationStore } from '@/stores/class-application';
import {
  ErrorCreateAndUpdateClassApplication,
  RequestGetCategory,
  ResponseGetClassApplication,
} from '@/types/model/class-application';
import { reactive, ref } from 'vue';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';

export const usePupilApplicationComposable = () => {
  const classApplicationStore = useClassApplicationStore();
  const { currentSemester, fetchCurrentSemester } = useSemesterStore();

  const classApplication = ref<ResponseGetClassApplication[]>([]);
  const selectedApplicationId = ref<number | null>(null);
  const classApplicationSelected = reactive<ResponseGetClassApplication>({
    id: 0,
    pupilId: 0,
    firstName: '',
    lastName: '',
    donorName: '',
    semesterId: 0,
    title: '',
    description: '',
    applicationCategoryId: 0,
    categoryName: '',
    response: '',
    status: 0,
    statusName: '',
    createDate: new Date(),
  });

  const listCategory = reactive<RequestGetCategory[]>([]);
  const isShowModal = ref<boolean>(false);
  const isUpdate = ref<boolean>(false);
  const isShowDeleteModal = ref<boolean>(false);
  const errorCreateAndUpdate = reactive<ErrorCreateAndUpdateClassApplication>({
    PupilId: [],
    SemesterId: [],
    Title: [],
    Description: [],
    ApplicationCategoryId: [],
  });

  const localClassApplication = ref<ResponseGetClassApplication>({
    title: '',
    description: '',
    applicationCategoryId: 1,
    semesterId: 0,
  });

  const errorKeys: (keyof ErrorCreateAndUpdateClassApplication)[] = [
    'PupilId',
    'SemesterId',
    'Title',
    'Description',
    'ApplicationCategoryId',
  ];

  const initializeLocalClassApplication = (applicationDetail: ResponseGetClassApplication | null) => {
    if (applicationDetail) {
      localClassApplication.value = { ...applicationDetail };
    } else {
      localClassApplication.value = {
        title: '',
        description: '',
        applicationCategoryId: null,
        semesterId: 0,
      };
    }
  };

  const resetLocalClassApplication = () => {
    localClassApplication.value = {
      title: '',
      description: '',
      applicationCategoryId: null,
      semesterId: 0,
    };
  };

  const getPupilApplication = async (semesterId: number) => {
    startLoading();
    try {
      await classApplicationStore.getPupilApplication(
        { semesterId: semesterId },
        res => {
          classApplication.value = (res.data as ResponseGetClassApplication[]) ?? [];
        },
        err => {
          classApplication.value = [];
        },
      );
    } catch (error) {
      console.error('Error fetching pupil application:', error);
    } finally {
      endLoading();
    }
  };

  const handleSelectClassApplication = (classApplicationId: number) => {
    const application = classApplication.value.find(app => app.id === classApplicationId);
    if (application) {
      Object.assign(classApplicationSelected, {
        id: application.id,
        pupilId: application.pupilId,
        firstName: application.firstName,
        lastName: application.lastName,
        donorName: application.donorName,
        semesterId: application.semesterId,
        title: application.title,
        description: application.description,
        applicationCategoryId: application.applicationCategoryId,
        categoryName: application.categoryName,
        response: application.response,
        status: application.status,
        statusName: application.statusName,
        createDate: application.createDate,
      });
      selectedApplicationId.value = classApplicationId;
      isShowModal.value = false;
    } else {
      console.error('Không tìm thấy đơn');
    }
  };

  const createApplication = async (emit: any, data: ResponseGetClassApplication) => {
    startLoading();
    try {
      await classApplicationStore.createClassApplication(
        data,
        async res => {
          notifySuccess('Tạo đơn thành công');
          isShowModal.value = false;
          emit('update-showModal', false);
          await getPupilApplication(currentSemester.id);
          await CreateOrUpdateSuccess(emit);
          clearErrorKeys(errorKeys, errorCreateAndUpdate);
          resetLocalClassApplication();
        },
        err => {
          console.error('Lỗi khi tạo đơn:', err);
          handleErrors(err, emit);
          if (err.code === 404) {
            notifyError(err.message);
          }
        }
      );
    } catch (error) {
      console.error('Lỗi khi tạo đơn:', error);
      return false;
    } finally {
      endLoading();
    }
  };

  const updateApplication = async (emit: any, data: ResponseGetClassApplication) => {
    startLoading();
    try {
      await classApplicationStore.updateClassApplication(
        data,
        async res => {
          notifySuccess('Cập nhật đơn thành công');
          isShowModal.value = false;
          clearErrorKeys(errorKeys, errorCreateAndUpdate);
          await getPupilApplication(currentSemester.id);
          await CreateOrUpdateSuccess(emit);
          emit('update-showModal', false);
          emit('update-isUpdate', false);
          await emit('success');
          resetLocalClassApplication();
        },
        err => {
          console.error('Lỗi khi cập nhật đơn:', err);
          handleErrors(err, emit);
        },
      );
    } catch (error) {
      console.error('Lỗi khi cập nhật đơn:', error);
    } finally {
      endLoading();
    }
  };

  const fetchListCategory = async () => {
    startLoading();
    try {
      await classApplicationStore.getCategory(
        res => {
          listCategory.splice(0, listCategory.length, ...res.data);
        },
        err => {
          console.error('Lỗi khi lấy loại đơn:', err);
        },
      );
    } catch (error) {
      console.error('Lỗi khi lấy loại đơn:', error);
    } finally {
      endLoading();
    }
  };

  const handleErrors = (err: any) => {
    const errors = err.errors as ErrorCreateAndUpdateClassApplication;
    mapErrorKeys(errorKeys, errorCreateAndUpdate, errors);
  };

  const CreateOrUpdateSuccess = async (emit: any) => {
    await fetchCurrentSemester();
    await getPupilApplication(currentSemester.id);
    await emit('update-list', classApplication.value);
  };

  const handleShowModal = async (isShow: boolean, isUpdateMode: boolean) => {
    await fetchListCategory();
    isShowModal.value = isShow;
    if (isUpdateMode) {
      isUpdate.value = isUpdateMode;
    }
  };

  const updateListApplication = (newval: any) => {
    classApplication.value = newval;
  };

  const handleClose = (emit: any) => {
    clearErrorKeys(errorKeys, errorCreateAndUpdate);
    emit('update-showModal', false);
    emit('update-isUpdate', false);
    isUpdate.value = false;
  };

  const handleConfirm = async (data: ResponseGetClassApplication, emit: any, isUpdateMode: boolean) => {
    if (isUpdateMode) {
      await updateApplication(emit, data);
    } else {
      await createApplication(emit, data);
    }
  };

  const handleDelete = async (id: number) => {
    startLoading();
    try {
      await classApplicationStore.deleteClassApplication(
        { id: id },
        async res => {
          notifySuccess('Xóa đơn thành công');
          await getPupilApplication(currentSemester.id);
        },
        err => {
          console.error('Lỗi khi xóa đơn:', err);
        },
      );
    } catch (error) {
      console.error('Lỗi khi xóa đơn:', error);
    } finally {
      endLoading();
    }
  };

  const id = ref<number>(0);

  const handleDeleteModal = (idVal: number) => {
    id.value = idVal;
    isShowDeleteModal.value = true;
  };

  const handleDeleteConfirm = (id: number) => {
    handleDelete(id);
    isShowDeleteModal.value = false;
  };

  return {
    localClassApplication,
    initializeLocalClassApplication,
    resetLocalClassApplication,
    id,
    handleDeleteModal,
    handleDeleteConfirm,
    handleDelete,
    isShowDeleteModal,
    handleClose,
    handleConfirm,
    listCategory,
    handleShowModal,
    isUpdate,
    updateListApplication,
    CreateOrUpdateSuccess,
    currentSemester,
    classApplication,
    selectedApplicationId,
    classApplicationSelected,
    isShowModal,
    getPupilApplication,
    fetchCurrentSemester,
    handleSelectClassApplication,
    errorCreateAndUpdate,
    createApplication,
    updateApplication,
  };
};
