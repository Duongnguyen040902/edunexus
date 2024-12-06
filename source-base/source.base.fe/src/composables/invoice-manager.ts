import { useInvoiceManagerStore } from '@/stores/invoice-manager.ts';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify.ts';
import { ref } from 'vue';
import router from '@/router';
import { ROUTER_PATHS } from '@/constants/api/router-paths.ts';

export const useInvoiceManagerComposable = () => {
  const invoiceManagerStore = useInvoiceManagerStore();
  const {
    isComplete,
    isUpdate,
    errorInvoice,
    requestInvoiceManagerIndex,
    requestInvoiceManagerCreate,
    requestDeleteInvoiceManager,
    responseInvoiceManagerIndex,
    responseInvoiceData,
    requestCreatePayment,
    errorPayment,
    createPayment,
    getAllInvoice,
    createInvoice,
    deleteInvoices,
    detailInvoice,
    updateInvoice,
  } = invoiceManagerStore;
  const isEmitValue = ref(false);
  const selectAll = ref(false);
  const selectedInvoices = ref<number[]>([]);
  const handleGetAllInvoiceManager = async () => {
    startLoading();
    await getAllInvoice();
    endLoading();
  };

  const handleCreateInvoice = async () => {
    startLoading();
    await createInvoice(
      () => {
        notifySuccess('Tạo hóa đơn thành công');
        router.push({ path: ROUTER_PATHS.ADMIN.INVOICE });
        endLoading();
      },
      () => {
        notifyError('Tạo hóa đơn thất bại');
        endLoading();
      },
    );
  };

  const handleDeleteInvoices = async () => {
    startLoading();
    await deleteInvoices(
      () => {
        notifySuccess('Xóa hóa đơn thành công');
        isEmitValue.value = false;
        selectAll.value = false;
        selectedInvoices.value = [];
        handleGetAllInvoiceManager();
        endLoading();
      },
      err => {
        isEmitValue.value = false;
        selectAll.value = false;
        selectedInvoices.value = [];
        notifyError(err.message);
        endLoading();
      },
    );
  };

  const handleDetailInvoice = async (id: number) => {
    startLoading();
    await detailInvoice(id);
    endLoading();
  };

  const handleUpdateInvoice = async (id: number) => {
    startLoading();
    await updateInvoice(
      id,
      () => {
        notifySuccess('Cập nhật hóa đơn thành công');
        endLoading();
      },
      err => {
        const { code, message } = err || {};
        if (code === 409) {
          notifyError(message);
        } else {
          notifyError('Cập nhật hóa đơn thất bại');
        }
        endLoading();
      },
    );
  };

  const isCheckedDelete = ref(false);
  const handlePageChange = (page: number) => {
    requestInvoiceManagerIndex.pageNumber = page;
    isEmitValue.value = false;
    selectAll.value = false;
    handleGetAllInvoiceManager();
  };

  const handleShowModalDelete = () => {
    isCheckedDelete.value = true;
  };

  const handleCloseModalDelete = () => {
    isCheckedDelete.value = false;
    requestDeleteInvoiceManager.ids = [];
    selectedInvoices.value = [];
  };

  const handleConfirmDelete = async () => {
    await handleDeleteInvoices();
    isCheckedDelete.value = false;
  };

  const handleSubmitCreateOrUpdateInvoice = async () => {
    if (isUpdate.value) {
      await handleUpdateInvoice(responseInvoiceData.value.id);
    } else {
      await handleCreateInvoice();
    }
  };

  const isShowModalCreatePayment = ref(false);
  const handleShowModalCreatePayment = () => {
    isShowModalCreatePayment.value = true;
  };

  const handleCloseModalCreatePayment = () => {
    isShowModalCreatePayment.value = false;
  };
  const handleCreatePayment = async () => {
    startLoading();
    await createPayment(
      () => {
        notifySuccess('Tạo thanh toán thành công');
        router.push({ path: ROUTER_PATHS.ADMIN.INVOICE });
        endLoading();
        isShowModalCreatePayment.value = false;
      },
      () => {
        notifyError('Tạo thanh toán thất bại');
        endLoading();
      },
    );
  };

  return {
    selectedInvoices,
    selectAll,
    isEmitValue,
    isShowModalCreatePayment,
    isComplete,
    isUpdate,
    isCheckedDelete,
    errorInvoice,
    requestInvoiceManagerIndex,
    requestInvoiceManagerCreate,
    requestDeleteInvoiceManager,
    responseInvoiceManagerIndex,
    responseInvoiceData,
    requestCreatePayment,
    errorPayment,
    createInvoice,
    deleteInvoices,
    detailInvoice,
    handleGetAllInvoiceManager,
    handleCreateInvoice,
    handleDeleteInvoices,
    handleDetailInvoice,
    handleUpdateInvoice,
    handlePageChange,
    handleShowModalDelete,
    handleCloseModalDelete,
    handleConfirmDelete,
    handleSubmitCreateOrUpdateInvoice,
    handleCreatePayment,
    handleShowModalCreatePayment,
    handleCloseModalCreatePayment,
  };
};
