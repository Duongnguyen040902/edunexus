import { ROUTER_PATHS } from '@/constants/api/router-paths';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { mapErrorKeys } from '@/helpers/state';
import router from '@/router';
import { useBusRouteStore } from '@/stores/bus-route';
import { useSemesterStore } from '@/stores/semester';
import { CreateBusRoute, ErrorResponseCreateBusRoute, RequestBusRouteIndex } from '@/types/model/bus-route';
import { RequestGetSemesterInterface } from '@/types/model/semester';
import { reactive, ref } from 'vue';
export const useBusRouteComposable = () => {
  const busRouteStore = useBusRouteStore();
  const {
    dataBusRoute,
    RequestBusRouteIndex,
    getAllBusRoute,
    createBusRoute,
    updateBusRoute,
    dataBusRouteDetail,
    getBusRouteDetail,
    deleteBusRoute,
  } = busRouteStore;
  const semesterStore = useSemesterStore();
  const { semester, fetchSemester, fetchCurrentSemester } = semesterStore;
  const searchKey = ref('');
  const showModalAdd = ref(false);
  const isUpdateMode = ref(false);
  const showModalDelete = ref(false);
  const seletedBusRoute = ref<number>(0);
  const errorsBusRoute = reactive(<ErrorResponseCreateBusRoute>{
    Name: [],
    Description: [],
    Status: [],
  });
  const numberBusRouteActive = ref([]);
  const totalActiveRoutes = ref(0);
  const totalInactiveRoutes = ref(0);
  const resetPage = ref(false);
  const selectedStatus = ref('');
  const errorBusRouteKeys: (keyof ErrorResponseCreateBusRoute)[] = ['Name', 'Description', 'Status'];

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleErrors = async (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseCreateBusRoute;
    await mapErrorKeys(errorBusRouteKeys, errorsBusRoute, errorsResponse);
  };

  const handleCreateBusRoute = async (request: CreateBusRoute) => {
    try {
      await createBusRoute(
        request,
        () => {
          notifySuccess('Tuyến xe đã được thêm thành công');
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
      notifyError('Có lỗi xảy ra khi thêm tuyến xe');
    }
  };

  const handleUpdateBusRoute = async (id: number, request: CreateBusRoute) => {
    try {
      await updateBusRoute(
        id,
        request,
        () => {
          notifySuccess('Tuyến xe đã được cập nhật thành công');
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
      notifyError('Có lỗi xảy ra khi thêm tuyến xe');
    }
  };

  const handleDeleteBusRoute = async (id: number) => {
    try {
      await deleteBusRoute(
        id,
        () => {
          notifySuccess('Tuyến xe đã được xóa thành công');
          showModalDelete.value = false;
          resetPage.value = true;
        },
        err => {
          notifyError('Tuyến xe đã vào hoạt động không thể xóa!');
        },
      );
    } catch (error) {
      notifyError('Có lỗi xảy ra khi xóa tuyến xe');
    }
  };

  const handleConfirm = async localBusRoute => {
    if (isUpdateMode.value) {
      await  handleUpdateBusRoute(localBusRoute.id, localBusRoute);
    } else {
      await handleCreateBusRoute(localBusRoute);
    }
  };

  const handleConfirmDelete = async () => {
    await handleDeleteBusRoute(seletedBusRoute.value);
  };

  const handleFetchSemester = async () => {
    await fetchSemester();
  };

  const handleGetAllBusRoute = async () => {
    RequestBusRouteIndex.pageSize = 5;
    await handleLoading(getAllBusRoute);
  };

  const getOverviewRouteForView = async () => {
    RequestBusRouteIndex.pageNumber = null;
    RequestBusRouteIndex.keyword = null;
    RequestBusRouteIndex.pageSize = null;
    RequestBusRouteIndex.status = null;
    await handleLoading(getAllBusRoute);
    numberBusRouteActive.value = dataBusRoute.value.data.data;
    totalActiveRoutes.value = numberBusRouteActive.value.filter(route => route.status === 1).length;
    totalInactiveRoutes.value = numberBusRouteActive.value.filter(route => route.status === 0).length;
  };

  const handleGetBusRouteDetail = async (id: number) => {
    await handleLoading(() => getBusRouteDetail(id));
  };

  const OpenModalAddBusRoute = () => {
    resetModal();
    showModalAdd.value = true;
  };

  const OpenModalDeleteBusRoute = (id: number) => {
    showModalDelete.value = true;
    seletedBusRoute.value = id;
  };

  const resetModal = () => {
    errorsBusRoute.Name = [];
    errorsBusRoute.Description = [];
    errorsBusRoute.Status = [];
  };

  const OpenModalUpdateBusRoute = async (id: number) => {
    resetModal();
    await handleGetBusRouteDetail(id);
    isUpdateMode.value = true;
    showModalAdd.value = true;
  };

  const handlePageChange = async (page: number) => {
    RequestBusRouteIndex.pageNumber = page;
    await handleGetAllBusRoute();
  };

  const handleRefreshBusRoute = async () => {
    RequestBusRouteIndex.pageNumber = 1;
    RequestBusRouteIndex.keyword = '';
    searchKey.value = '';
    selectedStatus.value = '';
    RequestBusRouteIndex.status = selectedStatus.value;
    await handleGetAllBusRoute();
  };

  const handleSearchBusRoute = async () => {
    RequestBusRouteIndex.keyword = searchKey.value;
    await handleGetAllBusRoute();
  };

  const handleFilterByStatus = async (event: number) => {
    RequestBusRouteIndex.status = event.target.value;
    await handleGetAllBusRoute();
  };

  const handleRedirectToDetail = async (id: number) => {
    startLoading();
    await router.push({ path: ROUTER_PATHS.BUS_ROUTE.BUS_ROUTE_DETAIL, query: { id: id, view: 'tableBus' } });
    endLoading();
  };

  const handleBackPage = async () => {
    startLoading();
    await router.push({ path: ROUTER_PATHS.BUS_ROUTE.INDEX });
    endLoading();
  };

  return {
    dataBusRoute,
    semester,
    showModalAdd,
    isUpdateMode,
    errorBusRouteKeys,
    errorsBusRoute,
    searchKey,
    dataBusRouteDetail,
    totalActiveRoutes,
    totalInactiveRoutes,
    showModalDelete,
    resetPage,
    selectedStatus,
    handleRedirectToDetail,
    handleBackPage,
    handleFilterByStatus,
    OpenModalDeleteBusRoute,
    handleConfirmDelete,
    getOverviewRouteForView,
    resetModal,
    handleUpdateBusRoute,
    OpenModalUpdateBusRoute,
    handleRefreshBusRoute,
    handleCreateBusRoute,
    handleGetBusRouteDetail,
    handleConfirm,
    handleFetchSemester,
    fetchSemester,
    OpenModalAddBusRoute,
    fetchCurrentSemester,
    handleGetAllBusRoute,
    handleSearchBusRoute,
    handlePageChange,
  };
};
