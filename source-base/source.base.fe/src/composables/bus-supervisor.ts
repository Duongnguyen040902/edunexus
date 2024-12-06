import { useBusSupervisorStore } from '@/stores/bus-supervisor';
import { ref, reactive, computed, nextTick } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifySuccess, notifyError } from '@/helpers/notify';
import {
  UpdateProfileBusSupervisor,
  ErrorResponseUpdateBusSupervisor,
  RequestListBusSupervisorInterface,
  RequestCreateBusSupervisorInterface,
  ResponseListBusSupervisorInterface,
  ErrorResponseBusSupervisor,
  ResponseBusSupervisorDetailInterface,
} from '@/types/model/bus-supervisor';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';

export const useBusSupervisorComposable = () => {
  const busSupervisorStore = useBusSupervisorStore();
  const isEditMode = ref(false);
  const { 
    updateBusSupervisor, 
    getBusSupervisorProfile, 
    requestImportExcelBusSupervisor,
    requestDeleteBusSupervisor,
    getListBusSupervisorAccount,
    createBusSupervisor,
    getBusSupervisorDetail,
    updateBusSupervisorAccount,
    deleteBusSupervisors,
    importExcelBusSupervisorAccount,
    } = busSupervisorStore;
  const requestDataUpdateBusSupervisor = ref<UpdateProfileBusSupervisor>({
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    image: new Blob(),
    phoneNumber: '',
    address: '',
    gender: true,
  });
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const params = reactive<RequestListBusSupervisorInterface>({ pageNumber: 1, pageSize: 10 });
  const isCreateBusSupervisor = ref(false);
  const isUpdateBusSupervisor = ref(false);
  const isShowCreate = ref(false);
  const formattedDateOfBirth = new Date().toISOString().split('T')[0];
  const isCheckedDelete = ref(false);

  const requestDataCreateBusSupervisor = reactive<RequestCreateBusSupervisorInterface>({
    busSupervisorId: 0,
    firstName: '',
    lastName: '',
    gender: null,
    phoneNumber: '',
    email: '',
    address: '',
    schoolId: null,
    accountStatus: 0,
    image: '',
  });

  const listBusSupervisorResponse = ref<ResponseListBusSupervisorInterface>({
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
        gender: false,
        genderName: '',
        phoneNumber: '',
        email:'',
        address: '',
        schoolId: 0,
        accountStatusName: '',
        username: '',
        image: '',
      },
    ],
  });

  const errorsBusSupervisor = reactive(<ErrorResponseBusSupervisor>{
    FirstName: [],
    LastName: [],
    Address: [],
    PhoneNumber: [],
    Email: [],
  });

  const errorBusSupervisorKeys: (keyof ErrorResponseBusSupervisor)[] = [
    'FirstName',
    'LastName',
    'Address',
    'PhoneNumber',
    'Email',
  ];

  const handleFetchBusSupervisor = async () => {
    startLoading();
    await getBusSupervisorProfile(
      (res: UpdateProfileBusSupervisor) => {
          requestDataUpdateBusSupervisor.value = res;
      },
      (err: any) => {
        notifyError(err.message);
      },
    );
    endLoading();
  };

  const handleUpdateBusSupervisor = async (data: UpdateProfileBusSupervisor) => {
    startLoading();
    await updateBusSupervisor(
      data,
      (res: any) => {
        notifySuccess('Cập nhật thành công');
        clearErrorKeys(errorUpdateKeys, errorsUpdateBusSupervisor);
        handleFetchBusSupervisor();
        isEditMode.value = false;
      },
      (err: any) => { 
        handleErrors(err);
        notifyError('Cập nhật thất bại');
      },
    );
    endLoading();
  };

  const enableEditMode = () => {
    isEditMode.value = true;
  };

  const disableEditMode = () => {
    handleFetchBusSupervisor();
    clearErrorKeys(errorUpdateKeys, errorsUpdateBusSupervisor);
    isEditMode.value = false;
  };

  const errorsUpdateBusSupervisor = reactive<ErrorResponseUpdateBusSupervisor>({
    FirstName: [],
    LastName: [],
    PhoneNumber: [],
    Address: [],
    Image: [],
  });

  const errorUpdateKeys: (keyof ErrorResponseUpdateBusSupervisor)[] = [
    'FirstName',
    'LastName',
    'PhoneNumber',
    'Address',
    'Image',
  ];

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseUpdateBusSupervisor;
    mapErrorKeys(errorUpdateKeys, errorsUpdateBusSupervisor, errorsResponse);
  };

  const imageFile = ref<{ name: string; url: string } | null>(null);
  const imageUrl = computed(() => {
    return imageFile.value
      ? imageFile.value.url
      : `${apiUrl}${requestDataUpdateBusSupervisor.value.image}`;
  });

  const handleFileChange = (event: Event) => {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
        requestDataUpdateBusSupervisor.value.image = file;
        imageFile.value = { name: file.name, url: URL.createObjectURL(file) };
    }
};

  const handleGetListBusSupervisor = async () => {
    await getListBusSupervisorAccount(
      params,
      res => {
        const resVal = res.data as ResponseListBusSupervisorInterface;
        console.log('Received from API:', resVal);
        listBusSupervisorResponse.value.pageNumber = resVal.pageNumber;
        listBusSupervisorResponse.value.pageSize = resVal.pageSize;
        listBusSupervisorResponse.value.firstPage = resVal.firstPage;
        listBusSupervisorResponse.value.lastPage = resVal.lastPage;
        listBusSupervisorResponse.value.totalPages = resVal.totalPages;
        listBusSupervisorResponse.value.totalRecords = resVal.totalRecords;
        listBusSupervisorResponse.value.nextPage = resVal.nextPage;
        listBusSupervisorResponse.value.previousPage = resVal.previousPage;
        listBusSupervisorResponse.value.data = resVal.data;
      },
      err => {
        console.log('Error:', err);
      },
    );
  };
  const handleCreateBusSupervisor = async (): Promise<boolean> => {
    return new Promise(async resolve => {
        await createBusSupervisor(
          requestDataCreateBusSupervisor,
          res => {
            notifySuccess('Người phụ trách bus đã được thêm thành công');
            handleGetListBusSupervisor();
            endLoading();
            resolve(true);
          },
          err => {
            handleCreateErrors(err);
            notifyError(err.message);
            endLoading();
            resolve(false);
          },
        );
    });
  };

  const handleUpdateBusSupervisorAccount = async (
    busSupervisorId: number,
    requestCreateBusSupervisorInterface: RequestCreateBusSupervisorInterface,
  ): Promise<boolean> => {
    startLoading();
    return new Promise(async resolve => {
        await updateBusSupervisorAccount(
          busSupervisorId,
          requestCreateBusSupervisorInterface,
          res => {
            notifySuccess('Cập nhật thông tin người phụ trách xe thành công');
            handleGetListBusSupervisor();
            endLoading();
            resolve(true);
          },
          err => {
            handleCreateErrors(err);
            notifyError(err.message);
            endLoading();
            resolve(false);
          },
        );       
    });
  };
  const handleFetchBusSupervisorDetail = async (busSupervisorId: number) => {
    startLoading();
    console.log('pupilId', busSupervisorId);
    try {
      await getBusSupervisorDetail(
        { busSupervisorId },
        res => {
          clearState(false, res);
        },
        error => {
          console.error('Error response from server:', error);
        },
      );
    } catch (error) {
      notifyError('An error occurred while fetching bus supervisor detail');
    } finally {
      endLoading();
    }
  };


  const handleDeleteBusSupervisors = async () => {
    startLoading();
    await deleteBusSupervisors(
      () => {
        notifySuccess('Xóa người phụ trách xe thành công');
        handleGetListBusSupervisor();
        endLoading();
      },
      err => {
        notifyError(err.message);
        endLoading();
      },
    );
  };

  const handleImportExcelBusSupervisorAccount = async () => {
    startLoading();
    await importExcelBusSupervisorAccount(
      () => {
        notifySuccess('Thêm người phụ trách xe thành công');
        handleGetListBusSupervisor();
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

  const handleCreateErrors = (err: any) => {
    endLoading();
    notifyError('Create failed');
    const errorsResponse = err.errors as ErrorResponseBusSupervisor;
    mapErrorKeys(errorBusSupervisorKeys, errorsBusSupervisor, errorsResponse);
  };

  const clearState = (
    isCreateMode: boolean,
    busSupervisorDetailData: ResponseBusSupervisorDetailInterface = {
      firstName: '',
      lastName: '',
      gender: null,
      phoneNumber: '',
      email: '',
      address: '',
      accountStatus: 0,
      image: '',
    },
  ) => {
    Object.assign(
      requestDataCreateBusSupervisor,
      isCreateMode
        ? {
            firstName: '',
            lastName: '',
            gender: null,
            phoneNumber: '',
            email: '',
            address: '',
            image: '',
            accountStatus: 0,
          }
        : {
            Id: busSupervisorDetailData.id,
            busSupervisorId: busSupervisorDetailData.id,
            firstName: busSupervisorDetailData.firstName,
            lastName: busSupervisorDetailData.lastName,
            gender: busSupervisorDetailData.gender,
            phoneNumber: busSupervisorDetailData.phoneNumber,
            email: busSupervisorDetailData.email,
            address: busSupervisorDetailData.address,
            accountStatus: busSupervisorDetailData.accountStatus,
            image: busSupervisorDetailData.image,
          },
    );
  };
  const updateRequestBusSupervisors = async (key: keyof RequestListBusSupervisorInterface, value: any) => {
    params[key] = value;
    await handleGetListBusSupervisor();
  };

  const openEditModal = () => {
    isShowCreate.value = true;
    isUpdateBusSupervisor.value = true;
  };

  const handleOpen = async () => {
    isShowCreate.value = true;
    isCreateBusSupervisor.value = true;
    isUpdateBusSupervisor.value = false;
  };

  const handleClose = async () => {
    resetBusSupervisorForm();
  };

  const handleSave = async () => {
    const success = await handleCreateBusSupervisor();
    if (success) {
      resetBusSupervisorForm();
    }
  };
  
  const handleEditBusSupervisor = async () => {
    const success = await handleUpdateBusSupervisorAccount(requestDataCreateBusSupervisor.busSupervisorId, requestDataCreateBusSupervisor);
    if (success) {
      resetBusSupervisorForm();
    }
  };
  const handlePageChange = (page: number) => {
    params.pageNumber = page;
    handleGetListBusSupervisor();
  };

  const handleShowBusSupervisorDetail = async (busSupervisorId: number) => {
    await handleFetchBusSupervisorDetail(busSupervisorId);
    isShowCreate.value = true;
    isCreateBusSupervisor.value = false;
    isUpdateBusSupervisor.value = false;
  };

  const resetBusSupervisorForm = (isCreateMode: boolean = true) => {
    isCreateBusSupervisor.value = false;
    isShowCreate.value = false;
    clearState(isCreateMode);
    clearErrorKeys(errorBusSupervisorKeys, errorsBusSupervisor);
  };

  const handleShowModalDelete = () => {
    isCheckedDelete.value = true;
  };

  const handleCloseModalDelete = () => {
    isCheckedDelete.value = false;
    requestDeleteBusSupervisor.ids = [];
  };

  const handleConfirmDelete = async () => {
    await handleDeleteBusSupervisors();
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
    requestImportExcelBusSupervisor.file = file.raw;
  };

  return {
    handleFetchBusSupervisor,
    imageUrl,
    enableEditMode,
    handleFileChange,
    disableEditMode,
    apiUrl,
    isEditMode,
    errorsUpdateBusSupervisor,
    requestDataUpdateBusSupervisor,
    requestDeleteBusSupervisor,
    listBusSupervisorResponse,
    params,
    isShowCreate,
    isCreateBusSupervisor,
    isUpdateBusSupervisor,
    requestDataCreateBusSupervisor,
    errorsBusSupervisor,
    isCheckedDelete,
    isImportModalVisible,
    handleCloseModalDelete,
    handleConfirmDelete,
    handleUpdateBusSupervisor,
    handleGetListBusSupervisor,
    onFileChange,
    openImportModal,
    closeImportModal,
    formatDate,
    handleShowModalDelete,
    handlePageChange,
    updateRequestBusSupervisors,
    handleOpen,
    handleClose,
    handleEditBusSupervisor,
    handleShowBusSupervisorDetail,
    handleSave,
    openEditModal,
    handleImportExcelBusSupervisorAccount,
  };
};