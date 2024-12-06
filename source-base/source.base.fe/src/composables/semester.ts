import { ref } from 'vue';
import { useSemesterStore } from '@/stores/semester';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { clearErrorKeys } from '@/helpers/state';
import { ResponseSemesterDetail } from '@/types/model/semester';
import { ROUTER_PATHS } from '@/constants/api/router-paths';
import router from '@/router';
export const useSemesterComposable = () => {
  const semesterStore = useSemesterStore();
  const {
    errorSemester,
    errorSemesterKeys,
    dataSemester,
    requestSemesterUpdate,
    semesterDetail,
    deleteSemester,
    getSemesterBySchoolYear,
    getSemesterDetail,
    createSemester,
    updateSemester,
    schoolYearId,
  } = semesterStore;

  const isShowModalEdit = ref(false);
  const isCreateSemester = ref(false);
  const isShowModalDelete = ref(false);

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const formatDate = (date: string | Date) => {
    const d = new Date(date);
    const month = `${d.getMonth() + 1}`.padStart(2, '0');
    const day = `${d.getDate()}`.padStart(2, '0');
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
  };

  const updateRequestSemesterUpdate = (semesterDetail: any) => {
    requestSemesterUpdate.id = semesterDetail.id;
    requestSemesterUpdate.semesterName = semesterDetail.semesterName;
    requestSemesterUpdate.semesterCode = semesterDetail.semesterCode;
    requestSemesterUpdate.startDate = formatDate(semesterDetail.startDate);
    requestSemesterUpdate.endDate = formatDate(semesterDetail.endDate);
    requestSemesterUpdate.isActive = semesterDetail.isActive;
    requestSemesterUpdate.schoolYearId = semesterDetail.schoolYearId;
  };

  const handleGetSemesterIndex = async () => {
    await handleLoading(getSemesterBySchoolYear);
  };

  const handleGetSemesterDetail = async (id: number) => {
    await handleLoading(() => getSemesterDetail(id, () => {}, () => {}));
  };

  const handleClearState = () => {
    requestSemesterUpdate.id = 0;
    requestSemesterUpdate.semesterName = '';
    requestSemesterUpdate.semesterCode = '';
    requestSemesterUpdate.startDate = formatDate(new Date());
    requestSemesterUpdate.endDate = formatDate(new Date());
    requestSemesterUpdate.isActive = false;
    requestSemesterUpdate.schoolYearId = schoolYearId;
  };

  const handleOpenModalEdit = (semester: ResponseSemesterDetail) => {
    updateRequestSemesterUpdate(semester);
    isShowModalEdit.value = true;
    isCreateSemester.value = false;
  };

  const handleOpenModalCreate = () => {
    handleClearState();
    isShowModalEdit.value = true;
    isCreateSemester.value = true;
  };

  const handleCloseModalEdit = () => {
    isShowModalEdit.value = false;
    isCreateSemester.value = false;
    handleClearState();
    clearErrorKeys(errorSemesterKeys, errorSemester);
  };

  const handleConfirmEdit = async () => {
    await updateSemester(
      async () => {
        notifySuccess('Cập nhật thông tin học kỳ thành công');
        await handleGetSemesterIndex();
        handleClearState();
        handleCloseModalEdit();
      },
      (err: any) => {
        notifyError('Cập nhật thông tin học kỳ thất bại');
        // Handle error response and update errorSemester
        if (err.errors) {
          Object.keys(err.errors).forEach(key => {
            errorSemester[key] = err.errors[key];
          });
        }
      },
    );
  };

  const handleConfirmCreate = async () => {
    await createSemester(
      async () => {
        notifySuccess('Tạo thông tin học kỳ thành công');
        await handleGetSemesterIndex();
        handleClearState();
        handleCloseModalEdit();
      },
      (err: any) => {
        notifyError('Tạo thông tin học kỳ thất bại');
        // Handle error response and update errorSemester
        if (err.errors) {
          Object.keys(err.errors).forEach(key => {
            errorSemester[key] = err.errors[key];
          });
        }
      }
    );
  };

  const handleOpenModalDelete = () => {
    isShowModalDelete.value = true;
  };

  const handleCloseModalDelete = () => {
    isShowModalDelete.value = false;
  };

  const handleConfirmDelete = async (id: number) => {
    await deleteSemester(id,
      async () => {
        isShowModalDelete.value = false;
        notifySuccess('Xóa thông tin học kỳ thành công');
        await handleGetSemesterIndex();
      },
      (err) => {
        notifyError('Xóa thông tin học kỳ thất bại');
      },
    );
  };

  const gotoSchoolYear = () => {
    router.push({
      path: ROUTER_PATHS.SCHOOL_ADMIN.SCHOOLYEAR_MANAGER
  });
  }
  return {
    gotoSchoolYear,
    errorSemester,
    isCreateSemester,
    getSemesterBySchoolYear,
    errorSemesterKeys,
    dataSemester,
    isShowModalDelete,
    handleOpenModalCreate,
    handleConfirmCreate,
    handleOpenModalDelete,
    handleCloseModalDelete,
    handleConfirmDelete,
    formatDate,
    requestSemesterUpdate,
    semesterDetail,
    deleteSemester,
    getSemesterDetail,
    createSemester,
    updateSemester,
    handleGetSemesterIndex,
    handleGetSemesterDetail,
    isShowModalEdit,
    handleOpenModalEdit,
    handleCloseModalEdit,
    handleConfirmEdit,
  };
};