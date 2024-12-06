import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { mapErrorKeys } from '@/helpers/state';
import router from '@/router';
import { useBusStopStore } from '@/stores/bus-stop';
import { useSemesterStore } from '@/stores/semester';
import { CreateBusStop, ErrorResponseCreateBusStop } from '@/types/model/bus-stop';
import { RequestGetSemesterInterface } from '@/types/model/semester';
import { reactive, ref } from 'vue';

export const useBusStopComposable = () => {
  const busStopStore = useBusStopStore();
  const {
    dataBusStop,
    RequestBusStopIndex,
    getAllBusStop,
    createBusStop,
    updateBusStop,
    dataBusStopDetail,
    getBusStopDetail,
    deleteBusStop,
  } = busStopStore;

  const semesterStore = useSemesterStore();
  const { semester, fetchSemester } = semesterStore;

  const searchKey = ref('');
  const showModalAdd = ref(false);
  const isUpdateMode = ref(false);
  const showModalDelete = ref(false);
  const selectedBusStop = ref<number>(0);
  const selectedBusRouteId =ref<number>();
  const errorsBusStop = reactive({
    Name: [],
    PickUpTime: [],
    ReturnTime: [],
    Address: [],
    BusRouteId: [],
    Status: [],
  });

  const resetPage = ref(false);
  const selectedStatus = ref('');
  const errorBusStopKeys: (keyof ErrorResponseCreateBusStop)[] = [
    'Name',
    'PickUpTime',
    'ReturnTime',
    'Address',
    'BusRouteId',
    'Status',
  ];

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseCreateBusStop;
    mapErrorKeys(errorBusStopKeys, errorsBusStop, errorsResponse);
  };

  const handleCreateBusStop = async (request: CreateBusStop) => {
    request.busRouteId = selectedBusRouteId.value;
    try {
      resetModal();
      await createBusStop(
        request,
        () => {
          notifySuccess('Điểm dừng được thêm thành công');
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
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleUpdateBusStop = async (id: number, request: CreateBusStop) => {
    try {
      resetModal();
      await updateBusStop(
        id,
        request,
        () => {
          notifySuccess('Điểm dừng cập nhật thành công');
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
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleDeleteBusStop = async (id: number) => {
    try {
      await deleteBusStop(
        id,
        () => {
          notifySuccess('Điểm dừng xóa thành công');
          showModalDelete.value = false;
          resetPage.value = true;
        },
        err => {
          notifyError('Điểm dừng đã hoạt động, không thể xóa!');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleConfirm = async localBusStop => {
    if (isUpdateMode.value) {
      await handleUpdateBusStop(localBusStop.id, localBusStop);
    } else {
      await handleCreateBusStop(localBusStop);
    }
  };

  const handleConfirmDelete = async () => {
    await handleDeleteBusStop(selectedBusStop.value);
  };

  const handleFetchSemester = async () => {
    await fetchSemester();
  };

  const handleGetAllBusStops = async (busRouteId: number) => {
    RequestBusStopIndex.pageSize = 10;
    RequestBusStopIndex.busRouteId = busRouteId;
    selectedBusRouteId.value = busRouteId;
    await handleLoading(getAllBusStop);
  };

  const handleGetBusStopDetail = async (id: number) => {
    await handleLoading(() => getBusStopDetail(id));
  };

  const OpenModalAddBusStop = () => {
    resetModal();
    showModalAdd.value = true;
  };

  const OpenModalDeleteBusStop = (id: number) => {
    showModalDelete.value = true;
    selectedBusStop.value = id;
  };

  const resetModal = () => {
    errorsBusStop.Name = [];
    errorsBusStop.PickUpTime = [];
    errorsBusStop.ReturnTime = [];
    errorsBusStop.Address = [];
    errorsBusStop.BusRouteId = [];
    errorsBusStop.Status = [];
  };

  const OpenModalUpdateBusStop = async (id: number) => {
    resetModal();
    await handleGetBusStopDetail(id);
    isUpdateMode.value = true;
    showModalAdd.value = true;
  };

  const handlePageChange = (page: number) => {
    RequestBusStopIndex.pageNumber = page;
    handleGetAllBusStops(selectedBusRouteId.value);
  };

  const handleRefreshBusStop = () => {
    RequestBusStopIndex.pageNumber = 1;
    RequestBusStopIndex.keyword = '';
    searchKey.value = '';
    selectedStatus.value = '';
    handleGetAllBusStops(selectedBusRouteId.value);
  };

  const handleSearchBusStop = async () => {
    RequestBusStopIndex.keyword = searchKey.value;
    await handleGetAllBusStops(selectedBusRouteId.value );
  };

  const handleFilterByStatus = async (event: number) => {
    RequestBusStopIndex.status = event.target.value;
    await handleGetAllBusStops(selectedBusRouteId.value );
  };

  return {
    dataBusStop,
    semester,
    showModalAdd,
    isUpdateMode,
    errorBusStopKeys,
    errorsBusStop,
    searchKey,
    dataBusStopDetail,
    showModalDelete,
    resetPage,
    selectedStatus,
    handleFilterByStatus,
    OpenModalDeleteBusStop,
    handleConfirmDelete,
    resetModal,
    handleUpdateBusStop,
    OpenModalUpdateBusStop,
    handleRefreshBusStop,
    handleCreateBusStop,
    handleGetBusStopDetail,
    handleConfirm,
    handleFetchSemester,
    OpenModalAddBusStop,
    handleGetAllBusStops,
    handleSearchBusStop,
    handlePageChange,
  };
};
