import { notifyError, notifySuccess } from '@/helpers/notify';
import { useClubStore } from '@/stores/club';
import { useSemesterStore } from '@/stores/semester';
import {
  RequestCreateClubInterface,
  ResponseGetClubDetailInterface,
  ResponseGetClubInterface,
} from '@/types/model/club';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { reactive, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ROUTER_PATHS } from '@/constants/api/router-paths.ts';
import { endLoading, startLoading } from '@/helpers/mixins';
import { mapErrorKeys } from '@/helpers/state';

export const useClubDetailComposable = () => {
  const clubStore = useClubStore();
  const clubDetail = ref<ResponseGetClubDetailInterface | null>(null);
  const assignedClub = reactive<ResponseGetClubInterface[]>([]);
  const router = useRouter();
  const route = useRoute();
  const clubId = ref<number>(parseInt(route.query.clubId as string));
  const { currentSemester, fetchCurrentSemester } = useSemesterStore();

  watch(
    () => route.query.clubId,
    async newClubId => {
      if (newClubId) {
        clubId.value = parseInt(newClubId as string);
        await fetchClubDetail(clubId.value);
      }
    },
  );

  const fetchClubDetail = async (clubId: number) => {
    try {
      await clubStore.getClubDetail(
        { clubId },
        res => {
          clubDetail.value = res.data;
        },
        err => {
          console.error('Error fetching club detail:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching club detail:', error);
    }
  };

  const fetchAssignedClub = async () => {
    try {
      await clubStore.getAssignedClub(
        res => {
          assignedClub.splice(0, assignedClub.length, ...res.data);
        },
        err => {
          console.error('Error fetching assigned club:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching assigned club:', error);
    }
  };

  const fetchEnrolledClub = async () => {
    try {
      await clubStore.getEnrolledClub(
        res => {
          assignedClub.splice(0, assignedClub.length, ...res.data);
        },
        err => {
          console.error('Error fetching enrolled club:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching enrolled club:', error);
    }
  };

  const goToClubDetail = async (clubId: number) => {
    router.push({ path: ROUTER_PATHS.TEACHER.CLUB_DETAIL, query: { clubId } });
  };

  const goToClubDetailOfPupil = async (clubId: number) => {
    router.push({ path: ROUTER_PATHS.PUPIL.CLUB_DETAIL, query: { clubId } });
  };

  const {
    dataClub,
    dataClubDetail,
    requestGetClub,
    getAllClubsForAdmin,
    createClub,
    updateClub,
    getClubDetailForAdmin,
    deleteClub,
  } = clubStore;
  const clubSearchKey = ref('');
  const isAddClubModalVisible = ref(false);
  const isEditClubMode = ref(false);
  const isDeleteClubModalVisible = ref(false);
  const selectedClub = ref<number>(0);
  const currentPage = ref(1);

  const clubErrors = reactive({
    Name: [],
    Description: [],
    Status: [],
  });

  const resetPage = ref(false);
  const selectedClubStatus = ref('');
  const clubErrorKeys = ['Name', 'Description', 'Status'];

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    try {
      await action();
    } finally {
      endLoading();
    }
  };

  const handleClubErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors;
    mapErrorKeys(clubErrorKeys, clubErrors, errorsResponse);
  };

  const handleCreateClub = async (request: RequestCreateClubInterface) => {
    try {
      await createClub(
        request,
        res => {
          notifySuccess('Thêm câu lạc bộ thành công!');
          resetModal();
          resetPage.value = true;
          isAddClubModalVisible.value = false;
        },
        err => {
          handleClubErrors(err);
          notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi hệ thống!');
    }
  };

  const handleUpdateClub = async (id: number, request: RequestCreateClubInterface) => {
    try {
      await updateClub(
        id,
        request,
        () => {
          notifySuccess('Cập nhật câu lạc bộ thành công!');
          resetModal();
          resetPage.value = true;
          isAddClubModalVisible.value = false;
          isEditClubMode.value = false;
        },
        err => {
          handleClubErrors(err);
          notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi hệ thống!');
    }
  };

  const handleDeleteClub = async (id: number) => {
    try {
      await deleteClub(
        id,
        () => {
          notifySuccess('Xóa câu lạc bộ thành công');
          isDeleteClubModalVisible.value = false;
          resetPage.value = true;
        },
        () => {
          notifyError('Câu lạc bộ đã hoạt động không thể xóa!');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi hệ thống!');
    }
  };

  const handleConfirm = async (clubData: RequestCreateClubInterface) => {
    if (isEditClubMode.value) {
      await handleUpdateClub(selectedClub.value, clubData);
    } else {
      await handleCreateClub(clubData);
    }
  };

  const handleConfirmDelete = async () => {
    await handleDeleteClub(selectedClub.value);
  };

  const handleGetAllClubs = async () => {
    requestGetClub.pageSize = 10;
    await handleLoading(() => getAllClubsForAdmin());
  };

  const handleGetClubDetail = async (id: number) => {
    await handleLoading(() => getClubDetailForAdmin(id));
  };

  const openModalAddClub = () => {
    resetModal();
    isAddClubModalVisible.value = true;
  };

  const openModalDeleteClub = (id: number) => {
    selectedClub.value = id;
    isDeleteClubModalVisible.value = true;
  };

  const resetModal = () => {
    clubErrors.Name = [];
    clubErrors.Description = [];
    clubErrors.Status = [];
  };

  const openModalEditClub = async (id: number) => {
    resetModal();
    await handleGetClubDetail(id);
    selectedClub.value = id;
    isEditClubMode.value = true;
    isAddClubModalVisible.value = true;
  };

  const handlePageChange = async (page: number) => {
    requestGetClub.pageNumber = page;
    await handleGetAllClubs();
  };

  const handleRefreshClub = async () => {
    requestGetClub.pageNumber = 1;
    requestGetClub.keyword = '';
    clubSearchKey.value = '';
    selectedClubStatus.value = '';
    await handleGetAllClubs();
  };

  const handleSearchClub = async () => {
    requestGetClub.keyword = clubSearchKey.value;
    await handleGetAllClubs();
  };

  const handleChangeStatus = async (event: Event) => {
    const target = event.target as HTMLSelectElement;
    requestGetClub.status = target.value ? parseInt(target.value) : null;
    await handleGetAllClubs();
  };

  const handleRedirectToDetail = async (clubId: number) => {
    startLoading();
    await router.push({ path: ROUTER_PATHS.SCHOOL_ADMIN.CLUB_DETAIL, query: { clubId } });
    endLoading();
  };

  return {
    goToClubDetailOfPupil,
    clubDetail,
    assignedClub,
    router,
    route,
    clubId,
    currentSemester,
    fetchCurrentSemester,
    fetchClubDetail,
    fetchAssignedClub,
    fetchEnrolledClub,
    goToClubDetail,
    dataClub,
    isAddClubModalVisible,
    isEditClubMode,
    clubErrors,
    clubSearchKey,
    dataClubDetail,
    isDeleteClubModalVisible,
    resetPage,
    selectedClubStatus,
    currentPage,
    handleChangeStatus,
    handleRedirectToDetail,
    openModalDeleteClub,
    handleConfirmDelete,
    resetModal,
    handleUpdateClub,
    openModalEditClub,
    handleRefreshClub,
    handleCreateClub,
    handleGetClubDetail,
    handleConfirm,
    openModalAddClub,
    handleGetAllClubs,
    handleSearchClub,
    handlePageChange,
  };
};
