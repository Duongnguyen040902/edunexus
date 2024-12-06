import { reactive, ref } from 'vue';
import { useTimeSlotStore } from '@/stores/timeslot';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { startLoading, endLoading } from '@/helpers/mixins';
import { mapErrorKeys } from '@/helpers/state';
import { ErrorResponse } from '@/constants/api/responses';
import router from '@/router';
import { ErrorResponseCreateTimeSlot, RequestCreateTimeSlotInterface } from '@/types/model/timeslot';

export const useTimeSlotComposable = () => {
  const timeSlotStore = useTimeSlotStore();
  const {
    requestGetAllTimeSlots,
    getAllTimeSlots,
    createTimeSlot,
    updateTimeSlot,
    deleteTimeSlot,
    timeSlotData,
    timeSlotDetail,
    getTimeSlotDetail,
  } = timeSlotStore;

  const searchKey = ref('');
  const showModalAdd = ref(false);
  const isUpdateMode = ref(false);
  const showModalDelete = ref(false);
  const currentPage = ref<number>(1);
  const selectedTimeSlotId = ref<number>(0);
  const errorsTimeSlot = reactive<ErrorResponseCreateTimeSlot>({
    Name: [],
    StartTime: [],
    EndTime: [],
    IsActive: [],
  });

  const resetPage = ref(false);
  const selectedStatus = ref('');
  const errorTimeSlotKeys: (keyof ErrorResponseCreateTimeSlot)[] = ['Name', 'StartTime', 'EndTime', 'IsActive'];

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleErrors = (err: ErrorResponse) => {
    endLoading();
    mapErrorKeys(errorTimeSlotKeys, errorsTimeSlot, err.errors);
  };

  const handleCreateTimeSlot = async (request: RequestCreateTimeSlotInterface) => {
    try {
      resetModal();
      await createTimeSlot(
        request,
        () => {
          notifySuccess('Tạo tiết học thành công!');
          resetModal();
          resetPage.value = true;
          showModalAdd.value = false;
        },
        err => {
          handleErrors(err);
          notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Xảy ra lỗi hệ thống!');
    }
  };

  const handleUpdateTimeSlot = async (id: number, request: RequestCreateTimeSlotInterface) => {
    try {
      resetModal();
      await updateTimeSlot(
        id,
        request,
        () => {
          notifySuccess('Chỉnh sửa tiết học thành công!');
          resetModal();
          resetPage.value = true;
          showModalAdd.value = false;
          isUpdateMode.value = false;
        },
        err => {
          handleErrors(err);
          notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi hệ thống!');
    }
  };

  const handleDeleteTimeSlot = async (id: number) => {
    try {
      await deleteTimeSlot(
        id,
        () => {
          notifySuccess('Xóa tiết học thành công');
          showModalDelete.value = false;
          resetPage.value = true;
        },
        err => {
          notifyError('Tiết học đã hoạt động, không thể xóa!');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi hệ thống!');
    }
  };

  const handleConfirm = async (timeSlot: RequestCreateTimeSlotInterface) => {
    if (isUpdateMode.value) {
      await handleUpdateTimeSlot(timeSlot.id as number, timeSlot);
    } else {
      await handleCreateTimeSlot(timeSlot);
    }
  };

  const handleConfirmDelete = async () => {
    await handleDeleteTimeSlot(selectedTimeSlotId.value);
  };

  const handleGetAllTimeSlots = async () => {
    requestGetAllTimeSlots.keyword = searchKey.value;
    requestGetAllTimeSlots.pageNumber = 1;
    await handleLoading(getAllTimeSlots);
  };

  const handleGetTimeSlotDetail = async (id: number) => {
    await handleLoading(() => getTimeSlotDetail(id));
  };

  const OpenModalAddTimeSlot = () => {
    resetModal();
    showModalAdd.value = true;
  };

  const OpenModalDeleteTimeSlot = (id: number) => {
    showModalDelete.value = true;
    selectedTimeSlotId.value = id;
  };

  const OpenModalUpdateTimeSlot = async (id: number) => {
    resetModal();
    await handleGetTimeSlotDetail(id);
    isUpdateMode.value = true;
    showModalAdd.value = true;
  };

  const resetModal = () => {
    errorsTimeSlot.Name = [];
    errorsTimeSlot.StartTime = [];
    errorsTimeSlot.EndTime = [];
    errorsTimeSlot.IsActive = [];
  };

  const handleSearchTimeSlot = async () => {
    await handleGetAllTimeSlots();
  };

  const handleFilterByStatus = async (status: Event) => {
    selectedStatus.value = status.target.value;
    requestGetAllTimeSlots.isActive = status.target.value;
    await handleGetAllTimeSlots();
  };

  const handlePageChange = async (page: number) => {
    requestGetAllTimeSlots.pageNumber = page;
    await handleGetAllTimeSlots();
  };

  const handleRefreshTimeSlot = async () => {
    requestGetAllTimeSlots.pageNumber = 1;
    requestGetAllTimeSlots.keyword = '';
    searchKey.value = '';
    selectedStatus.value = null;
    await handleGetAllTimeSlots();
  };

  const handleRedirectToDetail = async (id: number) => {
    await router.push({ path: '/time-slot-detail', query: { id } });
  };

  return {
    timeSlotData,
    timeSlotDetail,
    searchKey,
    selectedStatus,
    resetPage,
    showModalAdd,
    showModalDelete,
    isUpdateMode,
    currentPage,
    errorsTimeSlot,
    handleSearchTimeSlot,
    handleFilterByStatus,
    handlePageChange,
    handleRefreshTimeSlot,
    handleRedirectToDetail,
    handleCreateTimeSlot,
    handleUpdateTimeSlot,
    handleDeleteTimeSlot,
    handleConfirm,
    handleConfirmDelete,
    handleGetAllTimeSlots,
    handleGetTimeSlotDetail,
    OpenModalAddTimeSlot,
    OpenModalDeleteTimeSlot,
    OpenModalUpdateTimeSlot,
  };
};
