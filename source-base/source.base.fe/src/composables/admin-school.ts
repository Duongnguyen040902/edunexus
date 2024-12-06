import { useSchoolAdminStore } from '@/stores/admin-school';
import { startLoading, endLoading } from '@/helpers/mixins';
import router from '@/router';
import { clearErrorKeys, mapErrorKeys } from './../helpers/state';
import { ErrorResponse } from '@/constants/api/responses';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { useClassStore } from '@/stores/class';
import { useClassEnrollmentStore } from '@/stores/class-enrollment';
import { usePupilStore } from '@/stores/pupil';
import { useSemesterStore } from '@/stores/semester';
import { useTeacherStore } from '@/stores/teacher';
import {
  AddNewClassRequestInterface,
  DeleteClassRequestInterface,
  ErrorResponseAddNewClass,
  updateClassRequestInterface,
} from '@/types/model/class';
import {
  RequestAssignPupilInterface,
  RequestAssignTeacherInterface,
  RequestDeleteTeacherInterface,
  RequestGetTeacherIdInterface,
  RequestGetTeacherSwapInterface,
  RequestSwapTeacherInterface,
  RequestUpdateAssignTeacherInterface,
} from '@/types/model/class-enrollment';
import { ListPupilAssignRequestInterface, RequestGetPupilAssignInterface } from '@/types/model/pupil';
import { RequestGetListClassesInterface } from '@/types/model/admin-school';
import {
  RequestListSemesterInterface,
  ResponseGetSemesterInterface,
} from '@/types/model/semester';
import { ListTeacherAssignResponseInterface, RequestGetTeacherAssignInterface } from '@/types/model/teacher';
import { reactive, ref } from 'vue';
import { ROUTER_PATHS } from '@/constants/api/router-paths';
export const useAdminSchoolComposable = () => {
  const schoolAdminStore = useSchoolAdminStore();
  const classStore = useClassStore();
  const teacherStore = useTeacherStore();
  const semesterStore = useSemesterStore();
  const classEnrollmentStore = useClassEnrollmentStore();
  const pupilStore = usePupilStore();
  const { getAllClass } = schoolAdminStore;
  const { addNewClass, deleteClass, dataClassDetail, getClassDetailForAdmin, updateClass } = classStore;
  const { getTeacherAssign } = teacherStore;
  const { fetchSemester } = semesterStore;
  const {
    removeTeacherFromClass,
    getTeacherIdFromClassEnrollment,
    getTeacherSwap,
    swapTeacher,
    getMemberInClass,
    requestGetMemberClass,
    dataMember,
    removeMemberFromClass,
    getMemberInClassInNextSemester,
    dataMemberInNextSemester,
    pupilsToGradute,
  } = classEnrollmentStore;
  const { getPupilAssign } = pupilStore;
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
    classForSchoolAdmin,
    requestGetClass,
    createSchoolAdmin,
  } = schoolAdminStore;
  const isShowModalEdit = ref(false);
  const requestGetTeacherAssign = reactive<RequestGetTeacherAssignInterface>({
    semesterId: 0,
  });
  const classData = ref([]);
  const teachers = ref([]);
  const pupils = ref([]);
  const schoolId = reactive<RequestListSemesterInterface>({
    schoolId: 0,
  });
  const classIdToDelete = ref<DeleteClassRequestInterface>();
  const selectedClassId = ref<number>();
  const selectedSemester = ref<number>();
  const selectedTeacherId = ref<number>();
  const selectedClassEnrollment = ref<number>();
  const requestRemoveTeacher = ref<RequestDeleteTeacherInterface>({
    classId: 0,
    teacherId: 0,
    semesterId: 0,
  });
  const showModalAdd = ref(false);
  const isUpdate = ref(false);
  const showModalDelete = ref(false);
  const isShowModalTeacherDelete = ref(false);
  const showAssignTeacherModal = ref(false);
  const showTeacherSwapModal = ref(false);
  const showAssignPupilModal = ref(false);
  const semesterData = ref<ResponseGetSemesterInterface[]>([]);
  const currentSemester = ref<number>();
  const semesterNextYear = ref<number>();
  const checkGraduate = ref(false);
  const isSemesterDisabled = ref(true);
  const selectedMemberId = ref<number>();
  const showModalDeleteMember = ref(false);
  const showUpLevel = ref(false);
  const refeshPage = ref(false);
  const currentPage = ref<number>(1);
  const checkUpClass = ref(false);
  const checkHasTeacher = ref(false);
  const nextSemester = ref<number>();
  const checkConditionCopy = ref(true);
  const showModalUpdate = ref(false);
  const showModalGradute= ref(false);
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const requestAddClass = reactive<AddNewClassRequestInterface>({
    name: '',
    block: 0,
  });
  const errorsAddNewClass = reactive<ErrorResponseAddNewClass>({
    Name: [],
  });
  const errorAddKeys: (keyof ErrorResponseAddNewClass)[] = ['Name'];

  const handleLoading = async (action: () => Promise<void>) => {
    startLoading();
    await action();
    endLoading();
  };

  const updateRequestAdminSchoolUpdate = (schoolDetail: any) => {
    requestAdminSchoolUpdate.id = schoolDetail.id;
    requestAdminSchoolUpdate.address = schoolDetail.address;
    requestAdminSchoolUpdate.phoneNumber = schoolDetail.phoneNumber;
    requestAdminSchoolUpdate.websiteLink = schoolDetail.websiteLink;
    requestAdminSchoolUpdate.schoolName = schoolDetail.schoolName;
    requestAdminSchoolUpdate.email = schoolDetail.email;
    requestAdminSchoolUpdate.standardCode = schoolDetail.standardCode;
    requestAdminSchoolUpdate.dateOfEstablishment = schoolDetail.dateOfEstablishment;
    requestAdminSchoolUpdate.accountStatus = schoolDetail.accountStatus;
  };

  const handleGetAllAcccountSchoolAdmin = async () => {
    await handleLoading(getAllAccountSchoolAdmin);
  };

  const handleGetSchoolDetail = async (id: number) => {
    await handleLoading(() =>
      getSchoolDetail(
        id,
        () => {},
        () => {},
      ),
    );
  };

  const handlePageChange = (page: number) => {
    requestAdminSchoolIndex.pageNumber = page;
    handleGetAllAcccountSchoolAdmin();
  };

  const handleSearchAccountSchool = async () => {
    await handleGetAllAcccountSchoolAdmin();
  };

  const handleRedirectToDetail = (id: number) => {
    router.push({ path: 'detail-account', query: { id: id } });
  };

  const handleOpenModalEdit = () => {
    isShowModalEdit.value = true;
    updateRequestAdminSchoolUpdate(schoolAdminDetail.value.school);
  };

  const handleCloseModalEdit = () => {
    isShowModalEdit.value = false;
    handleClearState();
    clearErrorKeys(errorAdminSchoolKeys, errorAdminSchool);
  };

  const handleClearState = () => {
    updateRequestAdminSchoolUpdate({
      id: 0,
      address: '',
      phoneNumber: '',
      websiteLink: '',
      schoolName: '',
      accountStatus: 0,
      email: '',
      standardCode: '',
      dateOfEstablishment: '',
    });
  };

  const handleConfirmEdit = async () => {
    await updateSchoolAdmin(
      requestAdminSchoolUpdate.id,
      async () => {
        isShowModalEdit.value = false;
        notifySuccess('Cập nhật thông tin trường thành công', 'Thành công');
        await handleGetSchoolDetail(requestAdminSchoolUpdate.id);
        handleClearState();
      },
      () => {
        notifyError('Cập nhật thông tin trường thất bại', 'Thất bại');
      },
    );
  };

  const isShowModalCreate = ref(false);
  const handleOpenModalCreate = () => {
    isShowModalCreate.value = true;
  };

  const handleCloseModalCreate = () => {
    handleClearStateCreate();
    clearErrorKeys(errorAdminSchoolKeys, errorAdminSchool);
  };

  const handleConfirmCreateSchoolAdmin = async () => {
    await createSchoolAdmin(
      async () => {
        isShowModalCreate.value = false;
        notifySuccess('Tạo tài khoản trường thành công', 'Thành công');
        await handleGetAllAcccountSchoolAdmin();
        handleClearStateCreate();
      },
      () => {
        notifyError('Tạo tài khoản trường thất bại', 'Thất bại');
      },
    );
  };

  const handleClearStateCreate = () => {
    isShowModalCreate.value = false;
    requestCreateSchoolAdmin.username = '';
    requestCreateSchoolAdmin.password = '';
    requestCreateSchoolAdmin.schoolName = '';
    requestCreateSchoolAdmin.address = '';
    requestCreateSchoolAdmin.phoneNumber = null;
    requestCreateSchoolAdmin.email = '';
    requestCreateSchoolAdmin.subscriptionPlanId = null;
    requestCreateSchoolAdmin.paymentMethod = '';
  };

  //-----------------------------------------------------------------------------------------------------

  const handleFetchAssignTeacherToClass = async (request: RequestAssignTeacherInterface) => {
    startLoading();
    if (Array.isArray(request)) {
      request = request[0];
    }
    try {
      await classEnrollmentStore.assignTeacherToClass(
        request,
        () => {
          notifySuccess('Phân công giáo viên cho lớp thành công');
          refeshPage.value = true;
        },
        errorResponse => {
          const errorMessage = errorResponse?.message || 'Phân công giáo viên cho lớp thất bại';
          notifyError(errorMessage);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi khi phân công giáo viên');
    } finally {
      endLoading();
    }
  };

  const hanldeUpdatessignTeacher = async (request: RequestUpdateAssignTeacherInterface) => {
    startLoading();
    try {
      await classEnrollmentStore.updateAssignTeacher(
        request,
        () => {
          notifySuccess('Chỉnh sửa giáo viên thành công');
          refeshPage.value = true;
        },
        errorResponse => {
          const errorMessage = errorResponse?.message || 'Chỉnh sửa giáo viên cho lớp thất bại';
          notifyError(errorMessage);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi khi chỉnh sửa phân công giáo viên');
    } finally {
      endLoading();
    }
  };

  const hanldePupilsToGradute = async (request: number[]) => {
    startLoading();
    try {
      await classEnrollmentStore.pupilsToGradute(
        request,
        () => {
          notifySuccess('Cập nhật thành công');
          refeshPage.value = true;
        },
        errorResponse => {
          const errorMessage = errorResponse?.message || 'Cập nhật thất bại';
          notifyError(errorMessage);
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi!');
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
          refeshPage.value = true;
        },
        () => {
          notifyError('Phân công học sinh cho lớp thất bại');
        },
      );
    } catch (error) {
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
        (err: ErrorResponse) => {
          pupils.value = [];
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi khi lấy danh sách học sinh');
    } finally {
      endLoading();
    }
  };

  const handleFetchClass = async () => {
    startLoading();
    try {
      await getAllClass();
    } catch (error) {
      notifyError('Xảy ra lỗi!');
    } finally {
      endLoading();
    }
  };

  const handleGetListMember = async (classId: number) => {
    requestGetMemberClass.classId = classId;
    selectedClassId.value = classId;
    requestGetMemberClass.semesterId = selectedSemester.value;
    await handleLoading(() => getMemberInClass());
    checkHasTeacher.value = !dataMember.value.data.data.some(item => item.teacherId != null);
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
          console.error('Xảy ra lỗi thông tin kỳ học:', err);
        },
      );
    } catch (error) {
      console.error('Xảy ra lỗi thông tin kỳ học:', error);
    } finally {
      endLoading();
    }
  };

  const handleFetchGetClassId = async (request: RequestGetTeacherIdInterface) => {
    startLoading();
    try {
      await getTeacherIdFromClassEnrollment(
        request,
        res => {
          selectedTeacherId.value = res;
        },
        () => {},
      );
    } catch (error) {
      notifyError('Xảy ra lỗi!');
    } finally {
      endLoading();
    }
  };

  const handleFetchAddNewClass = async (requestAddClass: AddNewClassRequestInterface) => {
    startLoading();
    try {
      await addNewClass(
        requestAddClass,
        () => {
          notifySuccess('Thêm lớp thành công');
          clearErrorKeys(errorAddKeys, errorsAddNewClass);
          showModalAdd.value = false;
          refeshPage.value = true;
        },
        error => {
          handleErrors(error);
          notifyError(error.message);
        },
      );
    } catch (error) {
      notifyError('Xảy ra lỗi khi thêm lớp học!');
    } finally {
      endLoading();
    }
  };

  const handleFetchUpdateClass = async (requestUpdateClass: updateClassRequestInterface) => {
    startLoading();
    try {
      await updateClass(
        requestUpdateClass,
        () => {
          notifySuccess('Cập nhật thông tin lớp thành công');
          clearErrorKeys(errorAddKeys, errorsAddNewClass);
          showModalAdd.value = false;
          showModalUpdate.value = false;
          refeshPage.value = true;
        },
        error => {
          handleErrors(error);
          notifyError(error.message);
        },
      );
    } catch (error) {
      notifyError('Xảy ra lỗi khi thêm lớp học!');
    } finally {
      endLoading();
    }
  };

  const handleFetchDeleteClass = async (requestDeleteClass: DeleteClassRequestInterface) => {
    startLoading();
    try {
      await deleteClass(
        requestDeleteClass.value,
        () => {
          notifySuccess('Xóa lớp học thành công');
          refeshPage.value = true;
        },
        () => {
          notifyError('Lớp học đã hoạt động không thể xóa!');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi trong khi xóa lớp');
    } finally {
      endLoading();
    }
  };

  const handleFetchRemoveTeacherFromClass = async (request: RequestDeleteTeacherInterface) => {
    startLoading();
    try {
      await removeTeacherFromClass(
        request,
        () => {
          notifySuccess('Xóa giáo viên thành công');
          refeshPage.value = true;
        },
        () => {
          notifyError('Xóa giáo viên thất bại');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi trong khi xóa giáo viên');
    } finally {
      endLoading();
    }
  };

  const handleRemoveMemberFromClass = async (request: number) => {
    startLoading();
    try {
      await removeMemberFromClass(
        request,
        () => {
          notifySuccess('Xóa thành viên trong lớp thành công');
          refeshPage.value = true;
        },
        () => {
          notifyError('Xóa thành viên trong lớp thất bại');
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi trong khi xóa thành viên trong lớp');
    } finally {
      endLoading();
    }
  };

  const handleFetchTeacherAssign = async (request: RequestGetTeacherAssignInterface) => {
    startLoading();
    try {
      await getTeacherAssign(
        request,
        (res: ListTeacherAssignResponseInterface) => {
          if (res) {
            teachers.value.push(...res);
          }
        },
        (err: ErrorResponse) => {
          teachers.value = [];
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi khi lấy danh sách giáo viên');
    } finally {
      endLoading();
    }
  };

  const handleFetchTeacherInClassAssign = async (request: RequestGetTeacherSwapInterface) => {
    teachers.value = [];
    startLoading();
    try {
      await getTeacherSwap(
        request,
        res => {
          if (res) {
            teachers.value.push(...res);
          }
        },
        (err: ErrorResponse) => {
          teachers.value = [];
        },
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi khi lấy danh sách giáo viên');
    } finally {
      endLoading();
    }
  };

  const handleSwapTeacherAssign = async (request: RequestSwapTeacherInterface) => {
    startLoading();
    try {
      await swapTeacher(
        request,
        res => {
          notifySuccess('Thay đổi phân công giáo viên thành công');
          refeshPage.value = true;
        },
        (err: ErrorResponse) => {},
      );
    } catch (error) {
      notifyError('Đã xảy ra lỗi khi đổi phân công giáo viên');
    } finally {
      endLoading();
    }
  };

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors;
    mapErrorKeys(errorAddKeys, errorsAddNewClass, errorsResponse);
  };

  const openModal = () => {
    showModalAdd.value = true;
  };
  const closeAddModal = () => {
    showModalAdd.value = false;
  };

  const openUpdateClassModal = async (id: number) => {    
    await handleGetClassDetail(id);
    showModalUpdate.value = true;
    showModalAdd.value = true;
  };

  const handleGetClassDetail = async (id: number) => {
    //dataClassDetail.value = [];
    await handleLoading(() => getClassDetailForAdmin(id, [], []));
  };
  const closeModalDelete = () => {
    showModalDelete.value = false;
  };
  const openModalDelete = () => {
    showModalDelete.value = true;
  };
  const openModalPupil = (classId: number) => {
    pupils.value = [];
    showAssignPupilModal.value = true;
    selectedClassId.value = classId;
    const request: RequestGetPupilAssignInterface = {
      semesterId: selectedSemester.value,
    };
    handleFetchPupilAssign(request);
  };
  const closeModalPupil = () => {
    showModalDelete.value = false;
  };
  const openAssignTeacherModal = () => {
    showAssignTeacherModal.value = true;
  };

  const openModalSwapTeacher = async (classEnrollmentId: number) => {
    showTeacherSwapModal.value = true;
    teachers.value = [];
    const request: RequestGetTeacherSwapInterface = {
      semesterId: selectedSemester.value,
      ceTeacherId1: classEnrollmentId,
    };
    selectedClassEnrollment.value = classEnrollmentId;
    await handleFetchTeacherInClassAssign(request);
  };

  const openIsUpdateAssignTeacher = () => {
    isUpdate.value = true;
  };

  const openRemoveTeacherModal = () => {
    isShowModalTeacherDelete.value = true;
  };

  const openModalDeleteClass = (id: number) => {
    classIdToDelete.value = id;
    showModalDelete.value = true;
  };

  const openModalRemoveTeacher = async (classId: number) => {
    const request: RequestDeleteTeacherInterface = {
      classId: classId,
      semesterId: selectedSemester.value,
    };
    classIdToDelete.value = classId;
    await handleFetchGetClassId(request);
    openRemoveTeacherModal();
  };

  const openModalAssignTeacher = async (classId: number) => {
    teachers.value = [];
    requestGetTeacherAssign.semesterId = selectedSemester.value;
    selectedClassId.value = classId;
    await handleFetchTeacherAssign(requestGetTeacherAssign);
    isUpdate.value = false;
    showAssignTeacherModal.value = true;
  };
  const openModalDeleteMemberInClass = (memberId: number) => {
    showModalDeleteMember.value = true;
    selectedMemberId.value = memberId;
  };

  const openModalUpdateAssignTeacher = (classEnrollmentId: number, classId: number) => {
    teachers.value = [];
    selectedClassEnrollment.value = classEnrollmentId;
    requestGetTeacherAssign.semesterId = selectedSemester.value;
    selectedClassId.value = classId;
    handleFetchTeacherAssign(requestGetTeacherAssign);
    isUpdate.value = true;
    showAssignTeacherModal.value = true;
  };

  const filterBySemester = async () => {
    requestGetClass.semesterId = selectedSemester.value;
  };

  const filterChangeSemester = async event => {
    requestGetMemberClass.semesterId = event.target.value;
    await handleGetListMember(selectedClassId.value);
  };

  const searchMember = async event => {
    requestGetMemberClass.keyword = event.target.value;
    await handleGetListMember(selectedClassId.value);
  };
  const showDetail = async (classId: number) => {
    startLoading();
    await router.push({
      path: ROUTER_PATHS.SCHOOL_ADMIN.CLASS_DETAIL,
      query: { classId: classId },
    });
    endLoading();
  };

  const getActiveSemester = async () => {
    const activeSemester = semesterData.value.find(semester => semester.isActive);
    if (activeSemester) {
      selectedSemester.value = activeSemester.id;
      await filterBySemester();
    }
  };

  const gotoClassManage = async () => {
    startLoading();
    await router.push({ path: ROUTER_PATHS.SCHOOL_ADMIN.CLASS_MANAGEMENT });
    endLoading();
  };

  const filteredClasses = reactive([]);

  const filterClassesByBlock = (allClasses, currentClass) => {
    if (!currentClass || !currentClass.block) return [];

    const currentBlock = Number(currentClass.block); // Chuyển block thành số
    return allClasses.filter(cls => Number(cls.block) === currentBlock + 1); // Chuyển cls.block thành số để so sánh
  };
  const getListClassToUpLevel = async () => {
    await getAllClass();
    await getClassDetailForAdmin(selectedClassId.value, [], []);
    if (classForSchoolAdmin.value) {
      filteredClasses.value = filterClassesByBlock(classForSchoolAdmin.value.data.data, dataClassDetail.value);
    }
  };

  const checkSemesterStatus = () => {
    if (selectedSemester.value === currentSemester.value || selectedSemester.value === nextSemester.value) {
      return true;
    }
    return false;
  };
  const openUpLevelModal = async () => {
    await getMemberInClassInNextSemester(nextSemester.value);
    await getListClassToUpLevel();
    filteredClasses.value;
    showUpLevel.value = true;
  };

  const openGraduteModal = async () => {

    showModalGradute.value = true;
  };
  const handleConfirmPupilsGradute = async (pupilIds: number[]) => {
    hanldePupilsToGradute(pupilIds);
    showModalGradute.value = false;
  };
  const checkFinish = () => {
    if (semesterNextYear.value != null || nextSemester.value == null) {
      return true;
    }
    return false;
  };

  const checkFinishBlock = async (classId: number) => {
    await handleGetClassDetail(classId);
    if (
      semesterNextYear.value != null &&
      nextSemester.value == null &&
      selectedSemester.value == currentSemester.value
    ) {
      if (dataClassDetail.value.block == 5) {
        checkGraduate.value = true;
      }
      checkUpClass.value = true;
    } else {
      checkUpClass.value = false;
      checkGraduate.value = false;
    }
  };

  const handleModalConfirm = async request => {
    if (showModalUpdate.value) {
      const reqUpdate: updateClassRequestInterface = {
        id: request.id,
        name: request.name,
        block: request.block,
      };

      await handleFetchUpdateClass(reqUpdate);
    } else {
      const reqAdd: AddNewClassRequestInterface = {
        name: request.name,
        block: request.block,
      };
      await handleFetchAddNewClass(reqAdd);
    }
  };

  const handleTeacherRemove = async (request: RequestDeleteTeacherInterface) => {
    requestRemoveTeacher.value = request;
    await handleFetchRemoveTeacherFromClass(requestRemoveTeacher);
  };

  const handleModalConfirmDelete = async (classId: number) => {
    await handleFetchDeleteClass(classIdToDelete);
  };

  const handleModalConfirmSwapTeacher = async (res: object) => {
    const request: RequestSwapTeacherInterface = {
      ceTeacherId1: selectedClassEnrollment.value,
      ceTeacherId2: res.classEnrollmentId,
    };
    await handleSwapTeacherAssign(request);
    showTeacherSwapModal.value = false;
  };

  const handleModalConfirmAssignTeacher = async (request: {
    classId: number;
    teacherId: number;
    semesterId: number;
  }) => {
    try {
      if (isUpdate.value) {
        const requestUpdate: RequestUpdateAssignTeacherInterface = {
          classEnrollmentId: selectedClassEnrollment.value,
          classId: request.classId,
          teacherId: request.teacherId,
          semesterId: request.semesterId,
        };
        await hanldeUpdatessignTeacher(requestUpdate);
      } else {
        await handleFetchAssignTeacherToClass(request);
      }
      await handleFetchClass(requestGetClass);
    } catch (error) {
      console.error('Error in assigning teacher:', error);
    }
  };

  const handleModalConfirmAssignPupils = async (request: any[]) => {
    try {
      await handleFetchAssignPupils(request);
      closeModalPupil();
      await handleFetchClass(requestGetClass);
    } catch (error) {
      console.error('Error in assigning teacher:', error);
    }
  };

  return {
    isShowModalCreate,
    errorAdminSchool,
    errorAdminSchoolKeys,
    isShowModalEdit,
    requestAdminSchoolUpdate,
    requestAdminSchoolIndex,
    requestCreateSchoolAdmin,
    dataSchoolAdmin,
    schoolAdminDetail,
    showUpLevel,
    showModalUpdate,
    dataClassDetail,
    checkHasTeacher,
    semesterNextYear,
    checkGraduate,
    checkUpClass,
    dataMemberInNextSemester,
    filteredClasses,
    checkFinishBlock,
    openUpdateClassModal,
    handleModalConfirmAssignTeacher,
    handleModalConfirmAssignPupils,
    handleModalConfirmSwapTeacher,
    handleModalConfirmDelete,
    handleTeacherRemove,
    handleModalConfirm,
    openUpLevelModal,
    checkSemesterStatus,
    checkFinish,
    searchMember,
    createSchoolAdmin,
    handleCloseModalCreate,
    handleOpenModalCreate,
    handleGetAllAcccountSchoolAdmin,
    handleGetSchoolDetail,
    handlePageChange,
    handleSearchAccountSchool,
    handleRedirectToDetail,
    handleOpenModalEdit,
    handleCloseModalEdit,
    handleConfirmEdit,
    handleClearState,
    handleConfirmCreateSchoolAdmin,
    openModalDeleteMemberInClass,
    showDetail,
    handleRemoveMemberFromClass,
    openGraduteModal,
    showModalGradute,
    checkConditionCopy,
    classForSchoolAdmin,
    currentSemester,
    nextSemester,
    isSemesterDisabled,
    selectedMemberId,
    refeshPage,
    showModalDeleteMember,
    showTeacherSwapModal,
    isUpdate,
    showAssignPupilModal,
    classIdToDelete,
    selectedClassId,
    selectedSemester,
    selectedTeacherId,
    requestRemoveTeacher,
    showModalAdd,
    isShowModalTeacherDelete,
    requestGetTeacherAssign,
    showModalDelete,
    requestAddClass,
    schoolId,
    teachers,
    requestGetClass,
    classData,
    showAssignTeacherModal,
    semesterData,
    pupils,
    errorsAddNewClass,
    apiUrl,
    selectedClassEnrollment,
    dataMember,
    currentPage,
    handleConfirmPupilsGradute,
    filterChangeSemester,
    handleGetListMember,
    openModalSwapTeacher,
    handleSwapTeacherAssign,
    closeModalPupil,
    gotoClassManage,
    handleFetchTeacherInClassAssign,
    hanldeUpdatessignTeacher,
    closeAddModal,
    closeModalDelete,
    openModalPupil,
    handleFetchAssignPupils,
    handleFetchPupilAssign,
    openModalUpdateAssignTeacher,
    openIsUpdateAssignTeacher,
    getActiveSemester,
    filterBySemester,
    openModalAssignTeacher,
    openModalRemoveTeacher,
    openModalDeleteClass,
    openRemoveTeacherModal,
    handleFetchRemoveTeacherFromClass,
    handleFetchAssignTeacherToClass,
    handleFetchSemester,
    handleFetchTeacherAssign,
    handleFetchClass,
    handleFetchDeleteClass,
    handleFetchAddNewClass,
    openModal,
    openModalDelete,
    openAssignTeacherModal,
  };
};
