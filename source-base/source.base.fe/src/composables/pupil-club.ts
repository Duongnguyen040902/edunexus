import { useClubStore } from '@/stores/pupil-club';
import { startLoading, endLoading } from '@/helpers/mixins';
import { ref } from 'vue';
import { notifyError, notifySuccess } from '@/helpers/notify.ts';

export const useClubComposable = () => {
  const clubStore = useClubStore();
  const {
    requestGetClubIndex,
    requestCreateAndUpdateClubEnrollment,
    dataClubIndex,
    dataClubEnrollment,
    getClubsBySemesterActive,
    pupilCreateClubEnrollment,
    updateClubEnrollment,
    getClubEnrollmentByPupilId,
    nextSemester,
    getNextSemester,
    getSemesters,
    semesters,
  } = clubStore;

  const isShowModalDetail = ref(false);

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleGetNextSemester = async () => {
    await handleLoading(getNextSemester);
  }
  const handleGetClubsBySemesterActive = async () => {
    await handleLoading(getClubsBySemesterActive);
  };

  const handleCreateClubEnrollment = async (id: number) => {
    requestCreateAndUpdateClubEnrollment.clubId = id;
    await pupilCreateClubEnrollment(
      () => {
        notifySuccess('Đăng kí câu lạc bộ thành công');
        handleCloseModalConfirm();
        handleGetClubsBySemesterActive();
        handleGetClubEnrollmentByPupilId(nextSemester.id);
      },
      () => {
        notifyError('Đăng kí câu lạc bộ thất bại');
      },
    );
  };

  const handleUpdateClubEnrollment = async (id: number, statusId: number) => {
    requestCreateAndUpdateClubEnrollment.clubId = id;
    requestCreateAndUpdateClubEnrollment.status = statusId;
    await updateClubEnrollment(
      () => {
        notifySuccess('Gửi yêu cầu thành công');
        handleCloseModalConfirm();
        handleGetClubsBySemesterActive();
        handleGetClubEnrollmentByPupilId(nextSemester.id);
      },
      () => {
        notifyError('Gửi yêu cầu thất bại');
      },
    );
  };

  const handleGetClubEnrollmentByPupilId = async (semesterId: number) => {
    await handleLoading(() => getClubEnrollmentByPupilId(semesterId,
      () => {
      },
      () => {
        dataClubEnrollment.value.data = [];
      },
    ));
  };

  const handleGetSemester = async () => {
    await handleLoading(getSemesters);
  }

  const handlePageChange = (page: number) => {
    requestGetClubIndex.pageNumber = page;
    handleGetClubsBySemesterActive();
  };

  const handleOpenModalDetail = () => {
    isShowModalDetail.value = true;
  };

  const handleCloseModalDetail = () => {
    isShowModalDetail.value = false;
  };

  const isShowModalConfirm = ref(false);
  const isRegisterClub = ref(false);
  const isUnRegisterClub = ref(false);
  const handleOpenModalConfirm =() => {
    isShowModalConfirm.value = true;
  }
  const handleCloseModalConfirm =() => {
    isShowModalConfirm.value = false;
    isRegisterClub.value = false;
    isUnRegisterClub.value = false;
  };

  return {
    semesters,
    handleOpenModalConfirm,
    handleGetSemester,
    requestGetClubIndex,
    requestCreateAndUpdateClubEnrollment,
    dataClubIndex,
    dataClubEnrollment,
    isShowModalDetail,
    nextSemester,
    isShowModalConfirm,
    isRegisterClub,
    isUnRegisterClub,
    handleCloseModalConfirm,
    handleGetNextSemester,
    handleGetClubsBySemesterActive,
    handleCreateClubEnrollment,
    handleUpdateClubEnrollment,
    handleGetClubEnrollmentByPupilId,
    handlePageChange,
    handleOpenModalDetail,
    handleCloseModalDetail,
  };
};