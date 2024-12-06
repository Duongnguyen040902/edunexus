import { ROUTER_PATHS } from '@/constants/api/router-paths';
import { endLoading, startLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import router from '@/router';
import { useInvoiceStore } from '@/stores/invoice';
import { RequestListInvoiceInterface, ResponseInvoiceDetailInterface, ResponseListInvoiceInterface } from '@/types/model/invoice';
import { reactive, ref } from "vue";

export const useInvoiceComposable = () => {
    const invoiceStore = useInvoiceStore();
    const {
        getListInvoice,
        getInvoiceDetail,
        invoiceDetailResponse,
        listInvoiceResponse,
        requestInvoiceList,
        deleteInvoices,
        requestDeleteInvoice,
    } = invoiceStore;

    const invoiceId = ref<number>(0);


    const handleGetListInvoice = async () => {
        startLoading();
        await getListInvoice();
        endLoading();
    };

    const handleFetchInvoiceDetail = async (id: number) => {
        startLoading();
        await getInvoiceDetail(id);
        endLoading();
    };

    const handleDateFilter = async ({
        startDate,
        endDate
    }: { startDate: string | null; endDate: string | null }) => {
        if (startDate) {
            await updateRequestInvoices('startDate', startDate);
        }
        if (endDate) {
            await updateRequestInvoices('endDate', endDate);
        } else {
            await updateRequestInvoices('startDate', null);
            await updateRequestInvoices('endDate', null);
        }
    };

    const updateRequestInvoices = async (key: keyof RequestListInvoiceInterface, value: any) => {
        requestInvoiceList[key] = value;
        await handleGetListInvoice();
    };

    const handlePageChange = (page: number) => {
        requestInvoiceList.pageNumber = page;
        handleGetListInvoice();
    };

    const handleDeleteInvoices = async () => {
        startLoading();
        await deleteInvoices(
            () => {
                notifySuccess('Xóa hóa đơn thành công');
                handleGetListInvoice();
                endLoading();
            },
            err => {
                notifyError(err.message);
                endLoading();
            },
        );
    };

    const isCheckedDelete = ref(false);
    const handleShowModalDelete = () => {
        isCheckedDelete.value = true;
    };

    const handleCloseModalDelete = () => {
        isCheckedDelete.value = false;
        requestDeleteInvoice.ids = [];
    };

    const handleConfirmDelete = async () => {
        await handleDeleteInvoices();
        isCheckedDelete.value = false;
    };

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');

        return `${day}-${month}-${year} ${hours}:${minutes}:${seconds}`;
    };

    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
        }).format(amount);
    };

    const handleOpenInvoiceDetail = async (invoiceId: number) => {
        handleFetchInvoiceDetail(invoiceId);
        router.push({ path: ROUTER_PATHS.SCHOOL_ADMIN.INVOICE_DETAIL.replace(':id', invoiceId.toString()) });
    };

    const calculateShowingEntries = () => {
        const start = (requestInvoiceList.pageNumber - 1) * requestInvoiceList.pageSize + 1;
        const end = Math.min(requestInvoiceList.pageNumber * requestInvoiceList.pageSize, listInvoiceResponse.value.totalRecords);
        return {
            start,
            end,
            total: listInvoiceResponse.value.totalRecords
        };
    };
    
    return {
        handleGetListInvoice,
        updateRequestInvoices,
        formatDate,
        formatCurrency,
        calculateShowingEntries,
        handleFetchInvoiceDetail,
        handleOpenInvoiceDetail,
        handleDeleteInvoices,
        handlePageChange,
        handleShowModalDelete,
        handleCloseModalDelete,
        handleConfirmDelete,
        handleDateFilter,
        invoiceId,
        invoiceDetailResponse,
        requestInvoiceList,
        listInvoiceResponse,
        requestDeleteInvoice,
        isCheckedDelete,
    };


}