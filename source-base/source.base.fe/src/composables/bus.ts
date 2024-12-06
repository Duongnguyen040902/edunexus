import { notifyError, notifySuccess } from '@/helpers/notify';
import { useBusStore } from '@/stores/bus';
import { useSemesterStore } from '@/stores/semester';
import { CreateBus, ResponseGetBus, ResponseGetBusDetail, ViewBusEnrollDetailDTO } from '@/types/model/bus';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { reactive, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ROUTER_PATHS } from '@/constants/api/router-paths.ts';
import { endLoading, startLoading } from '@/helpers/mixins';
import { mapErrorKeys } from '@/helpers/state';

export const useBusDetailComposable = () => {
  const busStore = useBusStore();
  const busDetail = ref<ResponseGetBusDetail | null>(null);
  const assignedBus = ref<ResponseGetBus>();
  const router = useRouter();
  const route = useRoute();
  const busId = ref<number>(parseInt(route.query.busId as string));
  const { currentSemester, fetchCurrentSemester } = useSemesterStore();
  const pupilBus = ref<ViewBusEnrollDetailDTO[]>([]);
  const { dataBus, RequestBusIndex, getAllBuses, createBus, updateBus, dataBusDetail, getBusForAdmin, deleteBus } =
    busStore;

  const busSearchKey = ref('');
  const isAddBusModalVisible = ref(false);
  const isEditBusMode = ref(false);
  const isDeleteBusModalVisible = ref(false);

  const selectedBus = ref<number>(0);
  const selectedBusRoute = ref<number>(parseInt(route.query.id as string));
  const currentPage = ref(1);
  const busErrors = reactive({
    Name: [],
    DriverName: [],
    DriverPhone: [],
    LicensePlate: [],
    SeatNumber: [],
    Status: [],
  });

  const resetPage = ref(false);
  const resetPageDetail = ref(false);
  const selectedBusStatus = ref('');
  const busErrorKeys = ['Name', 'DriverName', 'DriverPhone', 'LicensePlate', 'SeatNumber', 'Status'];

  watch(
    () => route.query.busId,
    async newBusId => {
      if (newBusId) {
        busId.value = parseInt(newBusId as string);
        await fetchBusDetail(busId.value);
      }
    },
  );

  const fetchBusDetail = async (busId: number) => {
    try {
      await busStore.getBusDetail(
        { busId },
        res => {
          busDetail.value = res.data as ResponseGetBus;
          console.log('Bus detail:', busDetail.value);
        },
        err => {
          console.error('Error fetching bus detail:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching bus detail:', error);
    }
  };

  const fetchAssignedBus = async () => {
    try {
      await busStore.getAssignedBus(
        res => {
          assignedBus.value = res.data as ResponseGetBus;
        },
        err => {
          console.error('Error fetching assigned bus:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching assigned bus:', error);
    }
  };

  const fetchEnrolledBus = async () => {
    try {
      await busStore.getEnrolledBus(
        res => {
          assignedBus.value = res.data as ResponseGetBus;
        },
        err => {
          console.error('Error fetching enrolled bus:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching enrolled bus:', error);
    }
  };

  const fetchPupilBus = async () => {
    try {
      await busStore.getBusEnrollDetail(
        res => {
          pupilBus.value = res.data as ViewBusEnrollDetailDTO[];
        },
        err => {
          pupilBus.value = [];
          console.error('Error fetching pupil bus:', err);
        },
      );
    } catch (error) {
      pupilBus.value = [];
      console.error('Error fetching pupil bus:', error);
    }
  };

  const fetchBusDetailOfPupil = async (busId: number, semesterId: number) => {
    try {
      await busStore.getBusDetailOfPupil(
        { busId, semesterId: semesterId },
        res => {
          busDetail.value = res.data as ResponseGetBusDetail;
        },
        err => {
          console.error('Error fetching bus detail:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching bus detail:', error);
    }
  };

  const goToBusDetail = async (busId: number) => {
    router.push({ path: ROUTER_PATHS.SUPERVISOR.BUS_DETAIL, query: { busId } });
  };

  const goToBusAttendance = async (busId: number) => {
    router.push({ path: ROUTER_PATHS.SUPERVISOR.BUS_ATTENDANCE, query: { busId } });
  };

  const gotoBusDetailOfPupil = async (busId: number) => {
    router.push({ path: ROUTER_PATHS.PUPIL.BUS_DETAIL, query: { busId } });
  };

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleBusErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors;
    mapErrorKeys(busErrorKeys, busErrors, errorsResponse);
  };

  const handleCreateBus = async (request: CreateBus) => {
    try {
      request.busRouteId = selectedBusRoute.value;
      resetModal();
      await createBus(
        request,
        () => {
          notifySuccess('Thêm xe bus thành công!');
          resetModal();
          resetPage.value = true;
          isAddBusModalVisible.value = false;
        },
        err => {
          handleBusErrors(err);
          notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleUpdateBus = async (id, request) => {
    request.busRouteId = selectedBusRoute.value;
    try {
      resetModal();
      await updateBus(
        id,
        request,
        () => {
          notifySuccess('Cập nhật thông tin xe bus thành công!');
          resetModal();
          resetPage.value = true;
          resetPageDetail.value=true;
          isEditBusMode.value =false;
          isAddBusModalVisible.value = false;
        },
        err => {
          handleBusErrors(err);
          notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleDeleteBus = async id => {
    try {
      await deleteBus(
        id,
        () => {
          notifySuccess('Xóa xe bus thành công');
          isDeleteBusModalVisible.value = false;
          resetPage.value = true;
        },
        () => {
          notifyError('Xe bus đã vào hoạt động không thể xóa!');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi');
    }
  };

  const handleConfirm = async localBus => {
    localBus.busRouteId = selectedBusRoute.value;
    if (isEditBusMode.value) {
      await handleUpdateBus(localBus.id, localBus);
    } else {
      await handleCreateBus(localBus);
    }
  };

  const handleConfirmDelete = async () => {
    await handleDeleteBus(selectedBus.value);
  };

  const handleGetAllBuses = async (busRouteId: number) => {
    RequestBusIndex.pageSize = 10;
    selectedBusRoute.value = busRouteId;
    await handleLoading(() => getAllBuses(busRouteId));
  };

  const handleGetBusDetail = async (id: number) => {
    try {
      await busStore.getBusForAdmin(
        id,
        res => {
          busDetail.value = res.data as ResponseGetBusDetail;
        },
        err => {
          console.error('Error fetching bus detail:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching bus detail:', error);
    }
  };

  const openModalAddBus = () => {
    resetModal();
    isAddBusModalVisible.value = true;
  };

  const openModalDeleteBus = id => {
    isDeleteBusModalVisible.value = true;
    selectedBus.value = id;
  };

  const resetModal = () => {
    busErrors.Name = [];
    busErrors.DriverName = [];
    busErrors.DriverPhone = [];
    busErrors.LicensePlate = [];
    busErrors.SeatNumber = [];
    busErrors.Status = [];
  };

  const openModalEditBus = async (id: number) => {
    resetModal();
    await handleGetBusDetail(id);
    isEditBusMode.value = true;
    isAddBusModalVisible.value = true;
  };

  const handlePageChange = async page => {
    RequestBusIndex.pageNumber = page;
    await handleGetAllBuses(selectedBusRoute.value);
  };

  const handleRefreshBus = async () => {
    RequestBusIndex.pageNumber = 1;
    RequestBusIndex.keyword = '';
    busSearchKey.value = '';
    selectedBusStatus.value = '';
    await handleGetAllBuses(selectedBusRoute.value);
  };

  const handleSearchBus = async (event) => {
    //RequestBusIndex.keyword = busSearchKey.value;
    RequestBusIndex.keyword = event.target.value;
    await handleGetAllBuses(selectedBusRoute.value);
  };

  const handleChangeStatus = async event => {
    RequestBusIndex.status = event.target.value;
    await handleGetAllBuses(selectedBusRoute.value);
  };


  return {
    busDetail,
    assignedBus,
    router,
    route,
    busId,
    currentSemester,
    fetchCurrentSemester,
    fetchBusDetail,
    fetchAssignedBus,
    fetchEnrolledBus,
    goToBusDetail,
    goToBusAttendance,
    gotoBusDetailOfPupil,
    fetchPupilBus,
    fetchBusDetailOfPupil,
    pupilBus,
    dataBus,
    isAddBusModalVisible,
    isEditBusMode,
    busErrorKeys,
    busErrors,
    busSearchKey,
    dataBusDetail,
    isDeleteBusModalVisible,
    resetPage,
    selectedBusStatus,
    currentPage,
    resetPageDetail,
    handleChangeStatus,
    openModalDeleteBus,
    handleConfirmDelete,
    resetModal,
    handleUpdateBus,
    openModalEditBus,
    handleRefreshBus,
    handleCreateBus,
    handleGetBusDetail,
    handleConfirm,
    openModalAddBus,
    handleGetAllBuses,
    handleSearchBus,
    handlePageChange,
  };
};
