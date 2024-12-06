import { endLoading, startLoading } from '@/helpers/mixins';
import { notifySuccess } from '@/helpers/notify';
import { useClassApplicationStore } from '@/stores/class-application';
import { useSemesterStore } from '@/stores/semester';
import {
  ErrorResponseClassApplication,
  RequestGetCategory,
  RequestResponseClassApplication,
  ResponseGetClassApplication,
} from '@/types/model/class-application';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { reactive, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';

export const useClassApplicationComposable = () => {
  const { currentSemester, fetchCurrentSemester } = useSemesterStore();
  const classApplicationStore = useClassApplicationStore();
  const router = useRouter();
  const route = useRoute();
  const classId = ref<number>(parseInt(route.query.classId as string) || 1);
  const classApplication = ref<ResponseGetClassApplication[]>([]);
  const classApplicationCategory = reactive<RequestGetCategory[]>([]);
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
  const selectedCategoryId = ref<number | null>(null);
  const isShowModal = ref<boolean>(false);
  const responsePayload = reactive<RequestResponseClassApplication>({
    id: 0,
    response: '',
    status: 0,
  });

  const errorsResponse = reactive<ErrorResponseClassApplication>({
    Id: [],
    Response: [],
    Status: [],
  });
  const errorsResponseKey: (keyof ErrorResponseClassApplication)[] = ['Id', 'Response', 'Status'];

  watch(
    () => route.query.classId,
    async newClassId => {
      if (newClassId) {
        classId.value = parseInt(newClassId as string);
        await fetchCurrentSemester();
        await fetchClassApplicationList(classId.value, currentSemester.id, 1);
      }
    },
  );

  const responseApplication = async (emit: any, response: RequestResponseClassApplication) => {
    startLoading();
    try {
      await classApplicationStore.responseClassApplication(
        response,
        async res => {
          notifySuccess('Phản hồi đơn thành công');
          isShowModal.value = false;
          clearErrorKeys(errorsResponseKey, errorsResponse);
          emit('update-showModal', false);
          await emit('success');
        },
        err => {
          console.error('Lỗi khi phản hồi đơn:', err);
          handleErrors(err);
        },
      );
    } catch (error) {
      console.error('Lỗi khi phản hồi đơn:', error);
    } finally {
      endLoading();
    }
  };

  const handleErrors = (err: any) => {
    const errors = err.errors as ErrorResponseClassApplication;
    mapErrorKeys(errorsResponseKey, errorsResponse, errors);
  };

  const updateSuccess = async (emit: any, category: number) => {
    await fetchCurrentSemester();
    await fetchClassApplicationList(classId.value, currentSemester.id, category);
    await emit('update-list', classApplication.value);
  };

  const updateListApplication = (newval: any) => {
    classApplication.value = newval;
  };

  const fetchClassApplicationList = async (classId: number, semesterId: number, categoryId: number) => {
    startLoading();
    try {
      await classApplicationStore.getClassApplicationList(
        { classId, semesterId, categoryId },
        res => {
          classApplication.value = res.data ?? [];
        },
        err => {
          classApplication.value = [];
        },
      );
    } catch (error) {
      console.error('Lỗi khi lấy danh sách đơn:', error);
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
    } else {
      console.error('Không tìm thấy đơn');
    }
  };

  const fetchClassApplicationCategory = async () => {
    startLoading();
    try {
      await classApplicationStore.getCategory(
        res => {
          classApplicationCategory.splice(0, classApplicationCategory.length, ...res.data);
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

  const handleSelectCategory = async (categoryId: number) => {
    if (selectedCategoryId.value === categoryId) {
      selectedCategoryId.value = 0;
    } else {
      selectedCategoryId.value = categoryId;
    }
    await fetchClassApplicationList(classId.value, currentSemester.id, selectedCategoryId.value);
  };

  return {
    classApplicationCategory,
    isShowModal,
    selectedCategoryId,
    handleSelectCategory,
    classId,
    classApplication,
    currentSemester,
    fetchClassApplicationList,
    fetchCurrentSemester,
    classApplicationSelected,
    selectedApplicationId,
    handleSelectClassApplication,
    fetchClassApplicationCategory,
    responseApplication,
    updateListApplication,
    responsePayload,
    updateSuccess,
    errorsResponse,
    errorsResponseKey,
  };
};
