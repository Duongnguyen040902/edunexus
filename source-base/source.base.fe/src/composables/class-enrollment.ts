import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { mapErrorKeys } from '@/helpers/state';
import { useClassEnrollmentStore } from '@/stores/class-enrollment';
import { CreateClassEnrollment, RequestAssignPupilInterface, RequestUpdateAssignTeacherInterface } from '@/types/model/class-enrollment';
import { reactive, ref, watch } from 'vue';
import router from '@/router';
import { useRoute, useRouter } from 'vue-router';
import { useSemesterStore } from '@/stores/semester';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { useClassRoomStore } from '@/stores/class-room';
import { ROUTER_PATHS } from '@/constants/api/router-paths';
import { useSchoolAdminStore } from '@/stores/admin-school';
import { RequestGetPupilAssignInterface } from '@/types/model/pupil';

export const useClassEnrollmentComposable = () => {
  const classEnrollmentStore = useClassEnrollmentStore();
  const schoolAdminStore = useSchoolAdminStore();
  const {
    classIdFromClassEnrollment,
    requestGetMemberClass,
    swapTeacher,
    dataMember,
    getTeacherSwap,
    updateAssignTeacher,
    assignPupilToClass,
    getMemberInClass,
    assignTeacherToClass,
    removeTeacherFromClass,
    getTeacherIdFromClassEnrollment,
  } = classEnrollmentStore;
  const {
    errorAdminSchoolKeys,
    errorAdminSchool,
    requestAdminSchoolIndex,
    dataSchoolAdmin,
    schoolAdminDetail,
    requestAdminSchoolUpdate,
    requestCreateSchoolAdmin,
    getAllAccountSchoolAdmin,
    getSchoolDetail,
    updateSchoolAdmin,
    createSchoolAdmin,
  } = schoolAdminStore;
  const { getAllClassRoom, dataClassRoom, RequestClassRoomIndex } = useClassRoomStore();
  const semesterStore = useSemesterStore();
  const semesterData = ref<ResponseGetSemesterInterface[]>([]);
  const classEnrollmentSearchKey = ref('');
  const isAddClassEnrollmentModalVisible = ref(false);
  const isAssignClassRoom = ref(false);
  const isEditClassEnrollmentMode = ref(false);
  const isDeleteClassEnrollmentModalVisible = ref(false);
  const selectedClassEnrollment = ref<number>(0);
  const selectedClassId = ref<number>(0);
  const checkHasClassRoom = ref(false);
  const selectedSemester = ref<number>(1);
  const showUpSemesterClass = ref(false);
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const currentPage = ref(1);
  const classEnrollmentErrors = reactive({
    StudentName: [],
    StudentPhone: [],
    ClassRoom: [],
    Status: [],
  });

  const resetPage = ref(false);
  const resetPageForRedirect = ref(false);
  const isShowPupilInClassRoom = ref(false);
  const selectedClassEnrollmentStatus = ref('');
  const classEnrollmentErrorKeys = ['StudentName', 'StudentPhone', 'ClassRoom', 'Status'];
  const route = useRoute();
  const selectedId = ref<string | null>(route.query.id ? (route.query.id as string) : null);
  const currentSemester = ref<number>();
  const semesterNextYear = ref<number>();
  const isSemesterDisabled = ref(true);
  const nextSemester = ref<number>();
  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const handleClassEnrollmentErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors;
    mapErrorKeys(classEnrollmentErrorKeys, classEnrollmentErrors, errorsResponse);
  };



  const hanldeUpdatessignTeacher = async (request: RequestUpdateAssignTeacherInterface) => {
    startLoading();
    try {
      await classEnrollmentStore.updateAssignTeacher(
        request,
        () => {
          notifySuccess('Chỉnh sửa giáo viên thành công');
        },
        errorResponse => {
          const errorMessage = errorResponse?.message || 'Chỉnh sửa giáo viên cho lớp thất bại';
          notifyError(errorMessage);
        },
      );
    } catch (error) {
      console.error('Error assigning teacher to class:', error);
      notifyError('Đã xảy ra lỗi khi chỉnh sửa phân công giáo viên');
    } finally {
      endLoading();
    }
  };

  const handleFetchAssignPupils = async (request: RequestAssignPupilInterface) => {
    startLoading();
    try {
      await classEnrollmentStore.assignPupilToClass(
        request,
        () => {
          notifySuccess('Phân công học sinh cho lớp thành công');
        },
        () => {
          notifyError('Phân công học sinh cho lớp thất bại');
        },
      );
    } catch (error) {
      console.error('Error assigning teacher to class:', error);
      notifyError('Đã xảy ra lỗi khi phân công học sinh');
    } finally {
      endLoading();
    }
  };
  const handleFetchPupilAssign = async (request: RequestGetPupilAssignInterface) => {
    startLoading();
    try {
      await getPupilAssign(
        request,
        (res: ListPupilAssignRequestInterface) => {
          if (res) {
            pupils.value.push(...res);
          }
        },
        (err: ErrorResponse) => {},
      );
    } catch (error) {
      console.error('Error:', error);
      notifyError('Đã xảy ra lỗi khi lấy danh sách học sinh');
    } finally {
      endLoading();
    }
  };

  const handleCreateClassEnrollment = async (request: CreateClassEnrollment[]) => {
    try {
      resetModal();
      await createClassEnrollment(
        request,
        () => {
          notifySuccess('Thêm thành công');
          resetModal();
          resetPage.value = true;
          isAddClassEnrollmentModalVisible.value = false;
        },
        err => {
          handleClassEnrollmentErrors(err), notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleUpdateClassEnrollment = async (id, request) => {
    try {
      resetModal();
      await updateClassEnrollment(
        id,
        request,
        () => {
          notifySuccess('Cập nhật thành công');
          resetModal();
          resetPage.value = true;
          isAddClassEnrollmentModalVisible.value = false;
        },
        err => {
          handleClassEnrollmentErrors(err), notifyError(err.message);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
    }
  };

  const handleDeleteClassEnrollment = async id => {
    try {
      await deleteClassEnrollment(
        id,
        () => {
          notifySuccess('Xóa thành công');
          isDeleteClassEnrollmentModalVisible.value = false;
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

  const handleConfirmClassEnrollment = localClassEnrollment => {
    if (isEditClassEnrollmentMode.value) {
      handleUpdateClassEnrollment(localClassEnrollment.id, localClassEnrollment);
    } else {
      handleCreateClassEnrollment(localClassEnrollment);
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

  const handleConfirmDelete = async () => {
    await handleDeleteClassEnrollment(selectedClassEnrollment.value);
  };

  const handleGetAllClassEnrollments = async (classId: number) => {
    requestClassEnrollmentIndex.pageSize = 10;
    requestClassEnrollmentIndex.classId = classId;
    requestClassEnrollmentIndex.semesterId = selectedSemester.value;
    selectedClassId.value = classId;
    await handleLoading(() => getAllClassEnrollments());
    checkHasClassRoom.value = !dataClassEnrollment.value.data.data.some(item => item.classRoomId != null);
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

  const handleGetClassEnrollmentDetail = async id => {
    await handleLoading(() => getClassEnrollmentDetail(id));
  };

  const OpenStudentList = async classRoomId => {
    dataPupilsInClassRoom.value = [];
    await handleLoading(() => getAllPupilInClassRoom(selectedSemester.value,classRoomId));
    isShowPupilInClassRoom.value = true;
  };

  const openModalAddClassEnrollment = async () => {
    resetModal();
    dataPupils.value = [];
    RequestClassRoomIndex.classId = selectedId.value;
    await handleGetPupilsAssignToClass(selectedSemester.value);
    await getAllClassRoom();
    isAddClassEnrollmentModalVisible.value = true;
  };

  const openModalAssignClassRoom = async () => {
    resetModal();
    await handleGetClassRoomAssignToClass(selectedSemester.value);
    isAssignClassRoom.value = true;
  };

  const openUpSemesterClass = async () => {
    resetModal();
    showUpSemesterClass.value = true;
  };

  const openModalDeleteClassEnrollment = id => {
    isDeleteClassEnrollmentModalVisible.value = true;
    selectedClassEnrollment.value = id;
  };

  const resetModal = () => {
    classEnrollmentErrors.StudentName = [];
    classEnrollmentErrors.StudentPhone = [];
    classEnrollmentErrors.ClassRoom = [];
    classEnrollmentErrors.Status = [];
  };

  const openModalEditClassEnrollment = async id => {
    resetModal();
    await handleGetClassEnrollmentDetail(id);
    isEditClassEnrollmentMode.value = true;
    isAddClassEnrollmentModalVisible.value = true;
  };

  const handlePageChange = page => {
    requestClassEnrollmentIndex.pageNumber = page;
    handleGetAllClassEnrollments(selectedClassId.value);
  };

  return {
    dataClassEnrollment,
    requestClassEnrollmentIndex,
    selectedClassId,
    isAddClassEnrollmentModalVisible,
    isDeleteClassEnrollmentModalVisible,
    selectedClassEnrollment,
    classEnrollmentErrors,
    currentPage,
    resetPage,
    resetPageForRedirect,
    showUpSemesterClass,
    classEnrollmentSearchKey,
    handlePageChange,
    handleConfirmClassEnrollment,
    checkSemesterStatus,
    checkFinish,
    handleConfirmDelete,
    isShowPupilInClassRoom,
    OpenStudentList,
    handleFetchSemester,
    handleCreateClassEnrollment,
    handleGetAllClassEnrollments,
    openModalAddClassEnrollment,
    openModalDeleteClassEnrollment,
    openModalEditClassEnrollment,
    handleClassEnrollmentErrors,
    openUpSemesterClass,
    isSemesterDisabled,
    handleUpdateClassEnrollment,
    openModalAssignClassRoom,
    handleGetClassEnrollmentDetail,
    handleGetPupilsAssignToClass,
    handleGetClassRoomAssignToClass,
    handleDeleteClassEnrollment,
  };
};
