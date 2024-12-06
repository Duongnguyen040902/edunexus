import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { mapErrorKeys } from '@/helpers/state';
import { useBusEnrollmentStore } from '@/stores/bus-enrollment';
import { CreateBusEnrollment } from '@/types/model/bus-enrollment';
import { reactive, ref, watch } from 'vue';
import router from '@/router';
import { useRoute, useRouter } from 'vue-router';
import { useSemesterStore } from '@/stores/semester';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { useBusStopStore } from '@/stores/bus-stop';
import { ROUTER_PATHS } from '@/constants/api/router-paths';

export const useBusEnrollmentComposable = () => {
  const busEnrollmentStore = useBusEnrollmentStore();
  const {
    dataBusEnrollment,
    dataPupils,
    requestBusEnrollmentIndex,
    dataPupilsInBusStop,
    getAllBusEnrollments,
    createBusEnrollment,
    updateBusEnrollment,
    dataBusEnrollmentDetail,
    dataBusSupervisor,
    getBusEnrollmentDetail,
    deleteBusEnrollment,
    getAllPupilWithoutBus,
    getAllBusSupervisorWithoutBus,
    getAllPupilInBusStop,
  } = busEnrollmentStore;
  const { getAllBusStop, dataBusStop, RequestBusStopIndex } = useBusStopStore();
  const semesterStore = useSemesterStore();
  const semesterData = ref<ResponseGetSemesterInterface[]>([]);
  const busEnrollmentSearchKey = ref('');
  const isAddBusEnrollmentModalVisible = ref(false);
  const isAssignBusSupervisor = ref(false);
  const isEditBusEnrollmentMode = ref(false);
  const isDeleteBusEnrollmentModalVisible = ref(false);
  const selectedBusEnrollment = ref<number>(0);
  const selectedBusId = ref<number>(0);
  const checkHasBusSupervisor = ref(false);
  const selectedSemester = ref<number>(1);
  const showUpSemesterBus = ref(false);
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const currentPage = ref(1);
  const busEnrollmentErrors = reactive({
    StudentName: [],
    StudentPhone: [],
    BusRoute: [],
    Status: [],
  });

  const resetPage = ref(false);
  const resetPageForRedirect = ref(false);
  const isShowPupilInBusStop = ref(false);
  const selectedBusEnrollmentStatus = ref('');
  const busEnrollmentErrorKeys = ['StudentName', 'StudentPhone', 'BusRoute', 'Status'];
  const route = useRoute();
  const router = useRouter();
  const currentView = ref<string>((route.query.view as string) || 'tableBus');
  const selectedId = ref<string | null>(route.query.id ? (route.query.id as string) : null);
  const busRouteId = ref<number>();
  const currentSemester = ref<number>();
  const semesterNextYear = ref<number>();
  const isSemesterDisabled = ref(true);
  const checkConditionCopy = ref(true);
  const nextSemester = ref<number>();
  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleBusEnrollmentErrors = async (err: any) => {
    endLoading();
    const errorsResponse = err.errors;
    await mapErrorKeys(busEnrollmentErrorKeys, busEnrollmentErrors, errorsResponse);
  };

  const handleGetPupilsAssignToBus = async (semesterId: number) => {
    resetModal();
    await handleLoading(() => getAllPupilWithoutBus(semesterId));
  };

  const handleGetBusSupervisorAssignToBus = async (semesterId: number) => {
    resetModal();
    await handleLoading(() => getAllBusSupervisorWithoutBus(semesterId));
  };

  const handleCreateBusEnrollment = async (request: CreateBusEnrollment[]) => {
    try {
      resetModal();
      await createBusEnrollment(
        request,
        () => {
          notifySuccess('Thêm thành viên thành công');
          resetModal();
          resetPage.value = true;
          isAddBusEnrollmentModalVisible.value = false;
        },
        err => {
          handleBusEnrollmentErrors(err), notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleUpdateBusEnrollment = async (id, request) => {
    try {
      resetModal();
      await updateBusEnrollment(
        id,
        request,
        () => {
          notifySuccess('Cập nhật thành công');
          resetModal();
          resetPage.value = true;
          isAddBusEnrollmentModalVisible.value = false;
        },
        err => {
          handleBusEnrollmentErrors(err), notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleDeleteBusEnrollment = async id => {
    try {
      await deleteBusEnrollment(
        id,
        () => {
          notifySuccess('Xóa thành công');
          isDeleteBusEnrollmentModalVisible.value = false;
          resetPage.value = true;
        },
        () => {
          notifyError('Xóa thất bại');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleConfirmBusEnrollment = async localBusEnrollment => {
    if (isEditBusEnrollmentMode.value) {
      await handleUpdateBusEnrollment(localBusEnrollment.id, localBusEnrollment);
    } else {
      await handleCreateBusEnrollment(localBusEnrollment);
    }
  };

  const checkSemesterStatus = () => {
    if (selectedSemester.value === currentSemester.value || selectedSemester.value === nextSemester.value) {
      return true; 
    }
    return false;
  };

  const checkFinish = () => {
    if (semesterNextYear.value != null || nextSemester.value == null) {
      return true; 
    }
    return false;
  };

  const checkAllowCopy = () => {
    if (selectedSemester.value === currentSemester.value) {
      return true; 
    }
    return false;
  };

  const handleConfirmDelete = async () => {
    await handleDeleteBusEnrollment(selectedBusEnrollment.value);
  };

  const handleGetAllBusEnrollments = async (busId: number) => {
    requestBusEnrollmentIndex.pageSize = 10;
    requestBusEnrollmentIndex.busId = busId;
    requestBusEnrollmentIndex.semesterId = selectedSemester.value;
    selectedBusId.value = busId;
    await handleLoading(() => getAllBusEnrollments());
    checkHasBusSupervisor.value = !dataBusEnrollment.value.data.data.some(item => item.busSupervisorId != null);
  };

  const handleFetchSemester = async () => {
    startLoading();
    try {
      await semesterStore.fetchSemester(
        res => {
          semesterData.value = res;
          const currentSemesterData = semesterData.value.find(x => x.isActive === true);
          selectedSemester.value = currentSemesterData?.id;
          currentSemester.value = currentSemesterData?.id;
          if (currentSemesterData) {
            const futureSemesters = semesterData.value.filter(
              x => new Date(x.endDate) > new Date(currentSemesterData.endDate),
            );

            const sortedFutureSemesters = futureSemesters.sort((a, b) => new Date(a.endDate) - new Date(b.endDate));
            if (sortedFutureSemesters.length > 0) {
              const nextSemesterData = sortedFutureSemesters[0];
              if (nextSemesterData.schoolYearId !== currentSemesterData.schoolYearId) {
                semesterNextYear.value = nextSemesterData.id;
              } else {
                nextSemester.value = nextSemesterData.id;
              }
            }
          }
        },
        err => {
          console.error('Error fetching semesters:', err);
        }
      );
    } catch (error) {
      console.error('Error fetching semesters:', error);
    } finally {
      endLoading();
    }
  };

  const handleGetBusEnrollmentDetail = async id => {
    await handleLoading(() => getBusEnrollmentDetail(id));
  };

  const OpenStudentList = async busStopId => {
    dataPupilsInBusStop.value = [];
    await handleLoading(() => getAllPupilInBusStop(selectedSemester.value,busStopId));
    isShowPupilInBusStop.value = true;
  };

  const openModalAddBusEnrollment = async () => {
    resetModal();
    dataPupils.value = [];
    RequestBusStopIndex.busRouteId = selectedId.value;
    await handleGetPupilsAssignToBus(selectedSemester.value);
    await getAllBusStop();
    isAddBusEnrollmentModalVisible.value = true;
  };

  const openModalAssignBusSupervisor = async () => {
    resetModal();
    dataBusSupervisor.value= [];
    RequestBusStopIndex.busRouteId = selectedId.value;
    await handleGetBusSupervisorAssignToBus(selectedSemester.value);
    await getAllBusStop();
    isAssignBusSupervisor.value = true;
  };

  const openUpSemesterBus = async () => {
    await resetModal();
    //await handleGetBusSupervisorAssignToBus(selectedSemester.value);
    showUpSemesterBus.value = true;
  };

  const openModalDeleteBusEnrollment = id => {
    isDeleteBusEnrollmentModalVisible.value = true;
    selectedBusEnrollment.value = id;
  };

  const resetModal = () => {
    busEnrollmentErrors.StudentName = [];
    busEnrollmentErrors.StudentPhone = [];
    busEnrollmentErrors.BusRoute = [];
    busEnrollmentErrors.Status = [];
  };

  const openModalEditBusEnrollment = async id => {
    resetModal();
    await handleGetBusEnrollmentDetail(id);
    isEditBusEnrollmentMode.value = true;
    isAddBusEnrollmentModalVisible.value = true;
  };

  const handlePageChange = async page => {
    requestBusEnrollmentIndex.pageNumber = page;
    await handleGetAllBusEnrollments(selectedBusId.value);
  };

  const handleRefreshBusEnrollment = async () => {
    requestBusEnrollmentIndex.pageNumber = 1;
    requestBusEnrollmentIndex.keyword = '';
    busEnrollmentSearchKey.value = '';
    selectedBusEnrollmentStatus.value = '';
    await handleGetAllBusEnrollments(selectedBusId.value);
  };

  const handleSearchBusEnrollment = async () => {
    requestBusEnrollmentIndex.keyword = busEnrollmentSearchKey.value;
    await handleGetAllBusEnrollments(selectedBusId.value);
  };

  const handleSemesterChange = async event => {
    selectedSemester.value = event.target.value;
    await handleGetAllBusEnrollments(selectedBusId.value);
  };

  const updateUrlWithIdAndView = async (id: string, view: string) => {
    await router.push({ path: ROUTER_PATHS.BUS_ROUTE.BUS_ROUTE_DETAIL, query: { id, view } });
  };

  const showTableBus = async () => {
    startLoading();
    currentView.value = 'tableBus';
    busRouteId.value = selectedId.value;
    await updateUrlWithIdAndView(selectedId.value, 'tableBus');
    endLoading();
  };

  const showDetail = async (busRouteId: number, busId: number | null) => {
    startLoading();
    currentView.value = 'detail';
    await router.push({
      path: ROUTER_PATHS.BUS_ROUTE.BUS_ROUTE_DETAIL,
      query: { id: busRouteId, bid: busId, view: 'detail' },
    });
    endLoading();
  };

  const handleRedirectToIndex = async () => {
    startLoading();
    currentView.value = 'index';
    await updateUrlWithIdAndView(selectedId.value, 'index');
    endLoading();
  };

  watch(
    () => route.query,
    newQuery => {
      if (selectedId.value !== newQuery.id || currentView.value !== newQuery.view) {
        selectedId.value = newQuery.id || selectedId.value;
        currentView.value = newQuery.view || currentView.value;
      }
    },
    { immediate: true },
  );
  return {
    currentView,
    selectedId,
    showTableBus,
    showDetail,
    handleRedirectToIndex,
    handleFetchSemester,
    handleSemesterChange,
    OpenStudentList,
    checkFinish,
    checkAllowCopy,
    semesterNextYear,
    checkConditionCopy,
    dataPupilsInBusStop,
    isSemesterDisabled,
    resetPageForRedirect,
    semesterData,
    dataBusEnrollment,
    isAddBusEnrollmentModalVisible,
    isEditBusEnrollmentMode,
    busEnrollmentErrorKeys,
    busEnrollmentErrors,
    busEnrollmentSearchKey,
    dataBusEnrollmentDetail,
    isDeleteBusEnrollmentModalVisible,
    resetPage,
    selectedBusEnrollmentStatus,
    currentPage,
    dataPupils,
    selectedBusId,
    selectedSemester,
    isAssignBusSupervisor,
    apiUrl,
    dataBusSupervisor,
    checkHasBusSupervisor,
    dataBusStop,
    currentSemester,
    nextSemester,
    isShowPupilInBusStop,
    showUpSemesterBus,
    openUpSemesterBus,
    checkSemesterStatus,
    openModalDeleteBusEnrollment,
    openModalAssignBusSupervisor,
    handleConfirmDelete,
    resetModal,
    handleUpdateBusEnrollment,
    openModalEditBusEnrollment,
    handleRefreshBusEnrollment,
    handleCreateBusEnrollment,
    handleGetBusEnrollmentDetail,
    handleConfirmBusEnrollment,
    openModalAddBusEnrollment,
    handleGetAllBusEnrollments,
    handleSearchBusEnrollment,
    handlePageChange,
  };
};
