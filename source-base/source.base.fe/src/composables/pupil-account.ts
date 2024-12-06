import { endLoading, startLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';
import router from '@/router';
import { usePupilAccountStore } from '@/stores/pupil-account';
import {
  ErrorResponsePupil,
  RequestCreatePupilInterface,
  RequestGetPupilDetailInterface,
  RequestListPupilInterface,
  ResponseListPupilInterface,
  ResponsePupilDetailInterface,
} from '@/types/model/pupil-account';
import { UploadFile } from 'element-plus';
import { nextTick, reactive, ref } from 'vue';

export const usePupilAccountComposable = () => {
  const pupilAccountStore = usePupilAccountStore();
  const { 
    getListPupilAccount, 
    createPupil, 
    getPupilDetail, 
    updatePupil, 
    deletePupils, 
    requestDeletePupil,
    importExcelPupilAccount,
    requestImportExcelPupil,
  } = pupilAccountStore;
  const params = reactive<RequestListPupilInterface>({ pageNumber: 1, pageSize: 10 });
  const isCreatePupil = ref(false);
  const isUpdatePupil = ref(false);
  const isShowCreate = ref(false);
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const formattedDateOfBirth = new Date().toISOString().split('T')[0];
  const isCheckedDelete = ref(false);
  const requestDataCreatePupil = reactive<RequestCreatePupilInterface>({
    firstName: '',
    lastName: '',
    gender: null,
    donorName: '',
    donorPhoneNumber: '',
    email: '',
    address: '',
    dateOfBirth: '',
    schoolId: null,
    accountStatus: 0,
    image: '',
  });

  const listPupilResponse = ref<ResponseListPupilInterface>({
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
        genderName: '',
        donorName: '',
        donorPhoneNumber: '',
        email:'',
        address: '',
        dateOfBirth: '',
        schoolId: 0,
        accountStatusName: '',
        username: '',
        image: '',
      },
    ],
  });

  const errorsPupil = reactive(<ErrorResponsePupil>{
    FirstName: [],
    LastName: [],
    Address: [],
    DonorName: [],
    DateOfBirth: [],
    DonorPhoneNumber: [],
    Email: [],
  });

  const errorPupilKeys: (keyof ErrorResponsePupil)[] = [
    'FirstName',
    'LastName',
    'Address',
    'DonorName',
    'DateOfBirth',
    'DonorPhoneNumber',
    'Email',
  ];

  const handleGetListPupil = async () => {
    await getListPupilAccount(
      params,
      res => {
        const resVal = res.data as ResponseListPupilInterface;
        console.log('Received from API:', resVal);
        listPupilResponse.value.pageNumber = resVal.pageNumber;
        listPupilResponse.value.pageSize = resVal.pageSize;
        listPupilResponse.value.firstPage = resVal.firstPage;
        listPupilResponse.value.lastPage = resVal.lastPage;
        listPupilResponse.value.totalPages = resVal.totalPages;
        listPupilResponse.value.totalRecords = resVal.totalRecords;
        listPupilResponse.value.nextPage = resVal.nextPage;
        listPupilResponse.value.previousPage = resVal.previousPage;
        listPupilResponse.value.data = resVal.data;
      },
      err => {
        console.log('Error:', err);
      },
    );
  };

  const handleCreatePupil = async (): Promise<boolean> => {
    return new Promise(async resolve => {
        await createPupil(
          requestDataCreatePupil,
          res => {
            notifySuccess('Học sinh đã được thêm thành công');
            handleGetListPupil();
            endLoading();
            resolve(true);
          },
          err => {
            handleErrors(err);
            notifyError(err.message);
            endLoading();
            resolve(false);
          },
        );
    });
  };

  const handleUpdatePupil = async (
    pupilId: number,
    requestCreatePupilInterface: RequestCreatePupilInterface,
  ): Promise<boolean> => {
    startLoading();
    return new Promise(async resolve => {
        await updatePupil(
          pupilId,
          requestCreatePupilInterface,
          res => {
            notifySuccess('Cập nhật thông tin học sinh thành công');
            handleGetListPupil();
            endLoading();
            resolve(true);
          },
          err => {
            handleErrors(err);
            notifyError(err.message);
            endLoading();
            resolve(false);
          },
        );       
    });
  };

  const handleFetchPupilDetail = async (pupilId: number) => {
    startLoading();
    console.log('pupilId', pupilId);
    try {
      await getPupilDetail(
        { pupilId },
        res => {
          clearState(false, res);
        },
        error => {
          console.error('Error response from server:', error);
        },
      );
    } catch (error) {
      notifyError('An error occurred while fetching pupil detail');
    } finally {
      endLoading();
    }
  };


  const handleDeletePupils = async () => {
    startLoading();
    await deletePupils(
      () => {
        notifySuccess('Xóa sinh viên thành công');
        handleGetListPupil();
        endLoading();
      },
      err => {
        notifyError(err.message);
        endLoading();
      },
    );
  };

  const handleImportExcelPupilAccount = async () => {
    startLoading();
    await importExcelPupilAccount(
      () => {
        notifySuccess('Thêm học sinh thành công');
        handleGetListPupil();
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
    const errorsResponse = err.errors as ErrorResponsePupil;
    mapErrorKeys(errorPupilKeys, errorsPupil, errorsResponse);
  };

  const clearState = (
    isCreateMode: boolean,
    pupilDetailData: ResponsePupilDetailInterface = {
      firstName: '',
      lastName: '',
      gender: null,
      donorName: '',
      donorPhoneNumber: '',
      email: '',
      address: '',
      dateOfBirth: '',
      accountStatus: 0,
      image: '',
    },
  ) => {
    Object.assign(
      requestDataCreatePupil,
      isCreateMode
        ? {
            firstName: '',
            lastName: '',
            gender: null,
            donorName: '',
            donorPhoneNumber: '',
            email: '',
            address: '',
            dateOfBirth: formattedDateOfBirth,
            image: '',
            accountStatus: 0,
          }
        : {
            Id: pupilDetailData.id,
            pupilId: pupilDetailData.id,
            firstName: pupilDetailData.firstName,
            lastName: pupilDetailData.lastName,
            gender: pupilDetailData.gender,
            donorName: pupilDetailData.donorName,
            dateOfBirth: pupilDetailData.dateOfBirth,
            donorPhoneNumber: pupilDetailData.donorPhoneNumber,
            email: pupilDetailData.email,
            address: pupilDetailData.address,
            accountStatus: pupilDetailData.accountStatus,
            image: pupilDetailData.image,
          },
    );
  };

  const updateRequestPupils = async (key: keyof RequestListPupilInterface, value: any) => {
    params[key] = value;
    await handleGetListPupil();
  };

  const openEditModal = () => {
    isShowCreate.value = true;
    isUpdatePupil.value = true;
  };

  const handleOpen = async () => {
    isShowCreate.value = true;
    isCreatePupil.value = true;
    isUpdatePupil.value = false;
  };

  const handleClose = async () => {
    resetPupilForm();
  };

  const handleSave = async () => {
    const success = await handleCreatePupil();
    if (success) {
      resetPupilForm();
    }
  };

  const handleEditPupil = async () => {
    const success = await handleUpdatePupil(requestDataCreatePupil.pupilId, requestDataCreatePupil);
    if (success) {
      resetPupilForm();
    }
  };
  
  const handlePageChange = (page: number) => {
    params.pageNumber = page;
    handleGetListPupil();
  };


  const handleShowPupilDetail = async (pupilId: RequestGetPupilDetailInterface) => {
    await handleFetchPupilDetail(pupilId);
    isShowCreate.value = true;
    isCreatePupil.value = false;
    isUpdatePupil.value = false;
  };

  const resetPupilForm = (isCreateMode: boolean = true) => {
    isCreatePupil.value = false;
    isShowCreate.value = false;
    clearState(isCreateMode);
    clearErrorKeys(errorPupilKeys, errorsPupil);
  };

  const handleShowModalDelete = () => {
    isCheckedDelete.value = true;
  };

  const handleCloseModalDelete = () => {
    isCheckedDelete.value = false;
    requestDeletePupil.ids = [];
  };

  const handleConfirmDelete = async () => {
    await handleDeletePupils();
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
    requestImportExcelPupil.file = null;
    isImportModalVisible.value = false;
  
  };
  
  const onFileChange = file => {
    requestImportExcelPupil.file = file.raw;
  };

  return {
    params,
    listPupilResponse,
    requestDataCreatePupil,
    isCreatePupil,
    isUpdatePupil,
    errorPupilKeys,
    errorsPupil,
    isShowCreate,
    apiUrl,
    requestDeletePupil,
    isCheckedDelete,
    isImportModalVisible,
    requestImportExcelPupil,
    openImportModal,
    formatDate,
    handleGetListPupil,
    handleCreatePupil,
    clearState,
    handleFetchPupilDetail,
    handleUpdatePupil,
    updateRequestPupils,
    openEditModal,
    handleOpen,
    handleClose,
    handleSave,
    handleShowPupilDetail,
    handleEditPupil,
    handleDeletePupils,   
    handleShowModalDelete,
    handleCloseModalDelete,
    handleConfirmDelete,
    handlePageChange,
    handleImportExcelPupilAccount,
    onFileChange,
    closeImportModal,
  };
};
