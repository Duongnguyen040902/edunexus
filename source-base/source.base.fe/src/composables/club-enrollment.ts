import { ROUTER_PATHS } from '@/constants/api/router-paths';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { mapErrorKeys } from '@/helpers/state';
import router from '@/router';
import { useClubEnrollmentStore } from '@/stores/club-enrollment';
import { useSemesterStore } from '@/stores/semester';
import { CreateClubEnrollment, UpdateClubEnrollment } from '@/types/model/club-enrollment';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { reactive, ref, watch } from 'vue';

export const useClubEnrollmentComposable = () => {
  const clubEnrollmentStore = useClubEnrollmentStore();
  const {
    dataClubEnrollment,
    dataTeachersNotInClub,
    dataPupilsNotInClub,
    requestClubEnrollmentIndex,
    dataPupilRegisterClub,
    dataCopyClub,
    getBySemester,
    getTeachersNotInClub,
    getPupilsNotInClub,
    createClubEnrollment,
    updateClubEnrollment,
    removeClubEnrollment,
    getPupilsRegisterClub,
    getMemberToCopyClub,
  } = clubEnrollmentStore;

  const clubEnrollmentErrors = reactive({
    ClubId: [],
    SemesterId: [],
    Members: [],
  });
  const semesterStore = useSemesterStore();
  const semesterData = ref<ResponseGetSemesterInterface[]>([]);
  const clubEnrollmentErrorKeys = ['ClubId', 'SemesterId', 'Members'];
  const isAddClubEnrollmentModalVisible = ref(false);
  const isAssignTeacherModalVisible = ref(false);
  const isEditClubEnrollmentMode = ref(false);
  const isDeleteClubEnrollmentModalVisible = ref(false);
  const selectedClubEnrollmentId = ref<number | null>(null);
  const selectedSemester = ref<number>(1);
  const currentSemester = ref<number>();
  const nextSemester = ref<number>();
  const semesterNextYear = ref<number>();
  const selectedClubId = ref<number>(1);
  const isSemesterDisabled = ref(false);
  const showConfirmPupilToClub = ref(false);
  const checkHasTeacher = ref(false);
  const checkConditionCopy  = ref(true);
  const showUpSemester= ref(false);
  const clubEnrollmentSearchKey = ref('');
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const currentPage = ref(1);
  const resetPageClubEnrollment = ref(false);

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleClubEnrollmentErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors;
    mapErrorKeys(clubEnrollmentErrorKeys, clubEnrollmentErrors, errorsResponse);
    notifyError('Đã xảy ra lỗi khi xử lý yêu cầu.');
  };

  const handleGetClubEnrollments = async (id: number) => {
    requestClubEnrollmentIndex.pageNumber = currentPage.value;
    requestClubEnrollmentIndex.semesterId = selectedSemester.value;
    requestClubEnrollmentIndex.clubId = id;
    selectedClubId.value = id;
    await handleLoading(() => getBySemester());
    checkHasTeacher.value = !dataClubEnrollment.value.data.data.some(item => item.teacherId != null);
  };

  const handleGetTeachersNotInClub = async (clubId: number) => {
    await handleLoading(() => getTeachersNotInClub(clubId, selectedSemester.value));
  };

  const handleGetPupilsNotInClub = async (clubId: number) => {
    await handleLoading(() => getPupilsNotInClub(clubId, selectedSemester.value));
  };

  const handleGetPupilsRegisterClub = async (clubId: number) => {
    await handleLoading(() => getPupilsRegisterClub(clubId, selectedSemester.value));
  };

  const handleCreateClubEnrollment = async (request: CreateClubEnrollment[]) => {
    try {
      await createClubEnrollment(
        request,
        () => {
          notifySuccess('Thêm thành công!');
          isAddClubEnrollmentModalVisible.value = false;
          handleGetClubEnrollments(selectedClubId.value);
          resetPageClubEnrollment.value = true;
        },
        handleClubEnrollmentErrors,
      );
    } catch {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleUpdateClubEnrollment = async (request: UpdateClubEnrollment[]) => {
    try {
      await updateClubEnrollment(
        request,
        () => {
          notifySuccess('Cập nhật thành công!');
          isAddClubEnrollmentModalVisible.value = false;
          handleGetClubEnrollments(selectedClubId.value);
          resetPageClubEnrollment.value = true;
        },
        handleClubEnrollmentErrors,
      );
    } catch {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleDeleteClubEnrollment = async () => {
    try {
      await removeClubEnrollment(
        selectedClubEnrollmentId.value,
        () => {
          notifySuccess('Xóa thành công!');
          isDeleteClubEnrollmentModalVisible.value = false;
          handleGetClubEnrollments(selectedClubId.value);
          resetPageClubEnrollment.value = true;
        },
        () => notifyError('Xóa thất bại!'),
      );
    } catch {
      notifyError('Đã xảy ra lỗi!');
    }
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

        }
      );
    } catch (error) {
      console.log('Error fetching semesters:', error);
    } finally {
      endLoading();
    }
  };

  const openAddClubEnrollmentModal =  async () => {
    resetModal();
    await handleGetPupilsNotInClub(selectedClubId.value);
    isAddClubEnrollmentModalVisible.value = true;
  };

  const openAddAssignTeacherModal = async () => {
    resetModal();
    await handleGetTeachersNotInClub(selectedClubId.value);
    isAssignTeacherModalVisible.value = true;
  };

  const openConfirmRegisterPupilModal = async () => {
    resetModal(); 
    await handleGetPupilsRegisterClub(selectedClubId.value);
    showConfirmPupilToClub.value = true;
  };

  const openUpSemesterModal = async () => {
    resetModal(); 
    await getMemberToCopyClub(nextSemester.value ? nextSemester.value : semesterNextYear.value);
    showUpSemester.value = true;
  };

  const openEditClubEnrollmentModal = async (id: number) => {
    resetModal();
    selectedClubEnrollmentId.value = id;
    isEditClubEnrollmentMode.value = true;
    isAddClubEnrollmentModalVisible.value = true;
  };

  const openDeleteClubEnrollmentModal = (id: number) => {
    selectedClubEnrollmentId.value = id;
    isDeleteClubEnrollmentModalVisible.value = true;
  };

  const resetModal = () => {
    clubEnrollmentErrors.ClubId = [];
    clubEnrollmentErrors.SemesterId = [];
    clubEnrollmentErrors.Members = [];
  };

  const handleSearchClubEnrollment = async () => {
    requestClubEnrollmentIndex.keyword = clubEnrollmentSearchKey.value;
    await handleGetClubEnrollments(selectedClubId.value);
  };

  const handleSemesterChange = async event => {
    selectedSemester.value = event.target.value;
    await handleGetClubEnrollments(selectedClubId.value);
  };

  const handlePageChange = async (page: number) => {
    currentPage.value = page;
    await handleGetClubEnrollments(selectedClubId.value);
  };

  const handleRedirectToManageClub = async () => {
    startLoading();
    await router.push({ path: ROUTER_PATHS.SCHOOL_ADMIN.CLUB_MANAGEMENT });
    endLoading();
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

  watch(
    () => selectedSemester.value,
    () => {
      currentPage.value = 1;
      handleGetClubEnrollments(selectedClubId.value);
    },
  );

  return {
    dataClubEnrollment,
    dataTeachersNotInClub,
    dataPupilsNotInClub,
    isAddClubEnrollmentModalVisible,
    isEditClubEnrollmentMode,
    isDeleteClubEnrollmentModalVisible,
    apiUrl,
    semesterData,
    currentPage,
    resetPageClubEnrollment,
    clubEnrollmentSearchKey,
    selectedSemester,
    selectedClubId,
    isAssignTeacherModalVisible,
    currentSemester,
    nextSemester,
    isSemesterDisabled,
    showConfirmPupilToClub,
    dataPupilRegisterClub,
    showUpSemester,
    checkConditionCopy,
    checkHasTeacher,
    semesterNextYear,
    dataCopyClub,
    openUpSemesterModal,
    checkFinish,
    openConfirmRegisterPupilModal,
    checkSemesterStatus,
    openAddAssignTeacherModal,
    handleFetchSemester,
    handleSemesterChange,
    handleRedirectToManageClub,
    handleGetClubEnrollments,
    handleGetTeachersNotInClub,
    handleGetPupilsNotInClub,
    handleCreateClubEnrollment,
    handleUpdateClubEnrollment,
    handleDeleteClubEnrollment,
    openAddClubEnrollmentModal,
    openEditClubEnrollmentModal,
    openDeleteClubEnrollmentModal,
    handleSearchClubEnrollment,
    handlePageChange,
    resetModal,
    clubEnrollmentErrors,
  };
};
