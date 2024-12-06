import RepositoryFactory from '@/repositories/repository';
import { defineStore } from 'pinia';
import { InvoiceManagerRepository } from '@/repositories/repository-invoice';
import { RequestDeleteInvoiceManager, RequestListInvoiceInterface, ResponseInvoiceDetailInterface, ResponseListInvoiceInterface } from '@/types/model/invoice';
import { reactive, ref } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';


export const useInvoiceStore = defineStore('invoice', () => {
    const InvoiceFactory = RepositoryFactory.create('invoiceManager') as InvoiceManagerRepository;

    const requestDeleteInvoice = reactive<RequestDeleteInvoiceManager>({
        ids: [],
    });

    const requestInvoiceList = reactive<RequestListInvoiceInterface>({
        pageNumber: 1,
        pageSize: 10,
        invoiceStatus: null,
        startDate: null,
        endDate: null,
        school: null,
    });

    const listInvoiceResponse = reactive<{ value: ResponseListInvoiceInterface }>({
        value: {
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
                    subscriptionPlanId: 0,
                    subscriptionPlanName: '',
                    price: 0,
                    invoiceStatusName: '',
                    status: 0,
                    issueDate: '',
                    dueDate: '',
                }
            ]
        }
    });


    const invoiceDetailResponse = reactive<{ value: ResponseInvoiceDetailInterface }>({
        value: {
            id: 0,
            subscriptionPlanName: '',
            totalAmount: 0,
            durationDays: 0,
            statusName: '',
            status: 0,
            issueDate: '',
            dueDate: '',
            startDate: '',
            endDate: '',
            payments: [
                {
                    id: 0,
                    amount: 0,
                    paymentDate: '',
                    paymentMethod: '',
                    status: 0,
                }
            ],
        },
    });

    const getListInvoice = async () => {
        await InvoiceFactory.getAllInvoices(
            requestInvoiceList,
            res => {
                listInvoiceResponse.value = res.data as ResponseListInvoiceInterface;
            },
            err => {
                console.log(err);
            },
        );
    };


    const getInvoiceDetail = async (id: number) => {
        await InvoiceFactory.getInvoiceDetail(
            id,
            res => {
                invoiceDetailResponse.value = res.data.data as ResponseInvoiceDetailInterface
            },
            err => {
                console.log(err);
            },
        );
    };

    const deleteInvoices = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
        await InvoiceFactory.deleteListInvoice(
            requestDeleteInvoice,
            res => {
                requestDeleteInvoice.ids = [];
                success(res);
            },
            err => {
                requestDeleteInvoice.ids = [];
                console.log(err);
                error(err);
            },
        );
    };
    return {
        getListInvoice,
        getInvoiceDetail,
        deleteInvoices,
        invoiceDetailResponse,
        listInvoiceResponse,
        requestInvoiceList,    
        requestDeleteInvoice,  
    }
});

