import { useSubjectComposable } from '@/composables/subject';
import { useTeacherStore } from '@/stores/teacher-account';
import { nextTick, onMounted, reactive, ref, watchEffect } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifySuccess, notifyError } from '@/helpers/notify';
import {
  RequestGetListTeachersInterface,
  RequestCreateTeacherInterface,
  RequestGetTeacherDetailInterface,
  ResponseGetTeacherDetailInterface,
  ResponseGetListTeachersInterface,
  ErrorResponseCreateTeacher,
} from '@/types/model/teacher-account';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';
import { UploadFile } from 'ant-design-vue';

export const useTeachersComposable = () => {
  const teacherStore = useTeacherStore();
  const {
    getAllTeachers,
    createTeacher,
    getTeacherDetail,
    updateTeacher,
    deleteTeachers,
    importExcelTeacherAccount,
    requestDeleteTeacher,
    requestImportExcelTeacher,
  } = teacherStore;

  const subjectComposable = useSubjectComposable();
  const { handleGetAllSubject, responseSubject } = subjectComposable;
  const requestTeachers = reactive<RequestGetListTeachersInterface>({ pageNumber: 1, pageSize: 10 });
  const isShowModal = ref(false);
  const totalRecords = ref(0);
  const currentPage = ref(1);
  const isCreateTeacher = ref(false);
  const isUpdateTeacher = ref(false);
  const isShowCreate = ref(false);
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const formattedDateOfBirth = new Date().toISOString().split('T')[0];
  const isCheckedDelete = ref(false);
  const listTeacherResponse = ref<ResponseGetListTeachersInterface>({
    pageNumber: 0,
    pageSize: 0,
    firstPage: '',
    lastPage: '',
    totalPages: 0,
    totalRecords: 0,
    nextPage: '',
    previousPage: '',
    data: [
      {
        id: 0,
        firstName: '',
        lastName: '',
        username: '',
        genderName: '',
        dateOfBirth: '',
        phoneNumber: '',
        email: '',
        address: '',
        accountStatusName: '',
        subjects: [],
        image: '',
      },
    ],
  });

  const requestDataCreateTeacher = reactive<RequestCreateTeacherInterface>({
    firstName: '',
    lastName: '',
    gender: null,
    dateOfBirth: formattedDateOfBirth,
    phoneNumber: '',
    email: '',
    address: '',
    subjectIds: [],
    accountStatus: null,
    image: '',
  });

  const errorsCreateTeacher = reactive(<ErrorResponseCreateTeacher>{
    FirstName: [],
    LastName: [],
    Address: [],
    SubjectIds: [],
    DateOfBirth: [],
    PhoneNumber: [],
    Email: [],
  });

  const errorCreateKeys: (keyof ErrorResponseCreateTeacher)[] = [
    'FirstName',
    'LastName',
    'Address',
    'SubjectIds',
    'DateOfBirth',
    'PhoneNumber',
    'Email',
  ];

  const listSubjects = ref([]);

  onMounted(() => {
    handleGetAllSubject();
  });

  watchEffect(() => {
    listSubjects.value = responseSubject.data;
  });

  const handleFetchTeachers = async () => {
    startLoading();
    await getAllTeachers(
      requestTeachers,
      res => {
        const resVal = res.data as ResponseGetListTeachersInterface;
        listTeacherResponse.value.pageNumber = resVal.pageNumber;
        listTeacherResponse.value.pageSize = resVal.pageSize;
        listTeacherResponse.value.firstPage = resVal.firstPage;
        listTeacherResponse.value.lastPage = resVal.lastPage;
        listTeacherResponse.value.totalPages = resVal.totalPages;
        listTeacherResponse.value.totalRecords = resVal.totalRecords;
        listTeacherResponse.value.nextPage = resVal.nextPage;
        listTeacherResponse.value.previousPage = resVal.previousPage;
        listTeacherResponse.value.data = resVal.data;
        endLoading();
      },
      err => {
        console.log('Error:', err);
      },
    );
  };

  const handlePageChange = (page: number) => {
    requestTeachers.pageNumber = page;
    handleFetchTeachers();
  };

  const handleShowModal = () => {
    isShowModal.value = true;
  };

  const handleCloseModal = () => {
    isShowModal.value = false;
  };

  const handleCreateTeacher = async (): Promise<boolean> => {
    startLoading();
    return new Promise(async resolve => {
      await createTeacher(
        requestDataCreateTeacher,
        res => {
          notifySuccess('Giáo viên đã được thêm thành công');
          handleFetchTeachers();
          endLoading();
          resolve(true);
        },
        err => {
          handleErrors(err);
          notifyError(err.message);
          resolve(false);
        },
      );
    });
  };

  const handleUpdateTeacher = async (
    teacherId: number,
    requestCreateTeacherInterface: RequestCreateTeacherInterface,
  ): Promise<boolean> => {
    startLoading();
    return new Promise(async resolve => {
      await updateTeacher(
        teacherId,
        requestCreateTeacherInterface,
        res => {
          notifySuccess('Cập nhật thông tin giáo viên thành công');
          handleFetchTeachers();
          resolve(true);
        },
        err => {
          handleErrors(err);
          notifyError(err.message);
          resolve(false);
        },
      );
      endLoading();
    });
  };

  const handleFetchTeacherDetail = async (teacherId: RequestGetTeacherDetailInterface) => {
    startLoading();
    try {
      await getTeacherDetail(
        teacherId,
        res => {
          clearState(false, res);
        },
        () => {
        },
      );
    } catch (error) {
      notifyError('An error occurred while fetching teacher detail');
    } finally {
      endLoading();
    }
  };

  const handleDeleteTeachers = async () => {
    startLoading();
    await deleteTeachers(
      () => {
        notifySuccess('Xóa giáo viên thành công');
        handleFetchTeachers();
        endLoading();
      },
      err => {
        notifyError(err.message);
        endLoading();
      },
    );
  };

  const handleImportExcelTeacherAccount = async () => {
    startLoading();
    await importExcelTeacherAccount(
      () => {
        notifySuccess('Thêm giáo viên thành công');
        handleFetchTeachers();
        closeImportModal();
        endLoading();
      },
      err => {
        notifyError(err.message);
        closeImportModal();
        endLoading();
      },
    );
  };

  const handleErrors = (err: any) => {
    endLoading();
    notifyError('Create failed');
    const errorsResponse = err.errors as ErrorResponseCreateTeacher;
    mapErrorKeys(errorCreateKeys, errorsCreateTeacher, errorsResponse);
  };

  const clearState = (
    isCreateMode: boolean,
    teacherDetailData: ResponseGetTeacherDetailInterface = {
      id: null,
      firstName: '',
      lastName: '',
      gender: null,
      dateOfBirth: '',
      phoneNumber: '',
      email: '',
      address: '',
      subjectIds: [],
      subjects: [],
      accountStatus: 0,
      image: '',
    },
  ) => {
    Object.assign(
      requestDataCreateTeacher,
      isCreateMode
        ? {
          teacherId: null,
          firstName: '',
          lastName: '',
          gender: null,
          dateOfBirth: formattedDateOfBirth,
          phoneNumber: '',
          email: '',
          address: '',
          subjectIds: [],
          image: '',
          accountStatus: null,
        }
        : {
          teacherId: teacherDetailData.id,
          id: teacherDetailData.id,
          firstName: teacherDetailData.firstName,
          lastName: teacherDetailData.lastName,
          gender: teacherDetailData.gender,
          dateOfBirth: teacherDetailData.dateOfBirth,
          phoneNumber: teacherDetailData.phoneNumber,
          email: teacherDetailData.email,
          address: teacherDetailData.address,
          subjectIds: teacherDetailData.subjectIds,
          subjects: teacherDetailData.subjects,
          accountStatus: teacherDetailData.accountStatus,
          image: teacherDetailData.image,
        },
    );
  };

  const updateRequestTeachers = async (key: keyof RequestGetListTeachersInterface, value: any) => {
    requestTeachers[key] = value;
    await handleFetchTeachers();
  };


  const handleClose = async () => {
    resetTeacherForm();

  };

  const handleSave = async () => {
    const success = await handleCreateTeacher();
    if (success) {
      resetTeacherForm();
    }
  };

  const handleEditTeacher = async () => {
    const success = await handleUpdateTeacher(requestDataCreateTeacher.teacherId, requestDataCreateTeacher);
    if (success) {
      resetTeacherForm();
    }
  };

  const handleOpen = async () => {
    isShowCreate.value = true;
    isCreateTeacher.value = true;
    isUpdateTeacher.value = false;
  };

  const openEditModal = () => {
    isShowCreate.value = true;
    isUpdateTeacher.value = true;
  };

  const handleShowTeacherDetail = async (teacherId: RequestGetTeacherDetailInterface) => {
    await handleFetchTeacherDetail(teacherId);
    isShowCreate.value = true;
    isCreateTeacher.value = false;
    isUpdateTeacher.value = false;
  };

  const resetTeacherForm = (isCreateMode: boolean = true) => {
    isCreateTeacher.value = false;
    isShowCreate.value = false;
    clearState(isCreateMode);
    clearErrorKeys(errorCreateKeys, errorsCreateTeacher);
  };

  const handleShowModalDelete = () => {
    isCheckedDelete.value = true;
  };

  const handleCloseModalDelete = () => {
    isCheckedDelete.value = false;
    requestDeleteTeacher.ids = [];
  };

  const handleConfirmDelete = async () => {
    await handleDeleteTeachers();
    isCheckedDelete.value = false;
  };
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  };


  
  
  const isImportModalVisible = ref(false); 

  const openImportModal = () => {
    isImportModalVisible.value = true;
  };      

  const closeImportModal = () => {
    isImportModalVisible.value = false;
  
  };
  
  const onFileChange = file => {
    requestImportExcelTeacher.file = file.raw;
  };

  return {
    requestTeachers,
    totalRecords,
    currentPage,
    isShowModal,
    requestDataCreateTeacher,
    isCreateTeacher,
    isUpdateTeacher,
    listTeacherResponse,
    errorsCreateTeacher,
    errorCreateKeys,
    listSubjects,
    apiUrl,
    isShowCreate,
    requestDeleteTeacher,
    isCheckedDelete,
    requestImportExcelTeacher,
    isImportModalVisible,
    onFileChange,
    closeImportModal,
    openImportModal,
    handleFetchTeachers,
    handleShowModal,
    handleCloseModal,
    handlePageChange,
    handleCreateTeacher,
    handleFetchTeacherDetail,
    clearState,
    handleUpdateTeacher,
    updateRequestTeachers,
    handleSave,
    handleOpen,
    handleClose,
    openEditModal,
    handleShowTeacherDetail,
    handleEditTeacher,
    formatDate,
    handleDeleteTeachers,
    handleShowModalDelete,
    handleCloseModalDelete,
    handleConfirmDelete,
    handleImportExcelTeacherAccount,
  };
};
