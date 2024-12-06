import RepositoryFactory from '@/repositories/repository';
import { InvoiceManagerRepository } from '@/repositories/repository-invoice.ts';

import { defineStore } from 'pinia';
import { reactive } from 'vue';
import {
  ErrorResponseInvoice,
  ErrorResponsePayment,
  Invoice,
  InvoiceData,
  RequestCreateInvoiceManager,
  RequestCreatePayment,
  RequestDeleteInvoiceManager,
  RequestIndexInvoice,
} from '@/types/model/invoice.ts';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state.ts';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses.ts';
import { InvoiceStatus, PaymentStatuses } from '@/constants/enums/statuses.ts';

export const useInvoiceManagerStore = defineStore('invoiceManager', () => {
  const invoiceManagerFactory = RepositoryFactory.create('invoiceManager') as InvoiceManagerRepository;
  const isUpdate = reactive({ value: false });
  const isComplete = reactive({ value: false });
  const requestInvoiceManagerIndex = reactive<RequestIndexInvoice>({
    pageNumber: 1,
    pageSize: 10,
    status: null,
    keyword: '',
  });
  const requestInvoiceManagerCreate = reactive<RequestCreateInvoiceManager>({
    subscriptionPlanId: 0,
    schoolId: 0,
    issueDate: new Date().toISOString().slice(0, 10),
    dueDate: new Date().toISOString().slice(0, 10),
    status: 0,
    paymentMethod: '',
  });
  const requestDeleteInvoiceManager = reactive<RequestDeleteInvoiceManager>({
    ids: [],
  });
  const responseInvoiceManagerIndex = reactive<{ value: Invoice }>({
    value: {
      pageNumber: 1,
      pageSize: 10,
      firstPage: 1,
      lastPage: 1,
      totalPages: 0,
      totalRecords: 0,
      nextPage: null,
      previousPage: null,
      data: [],
    },
  });
  const responseInvoiceData = reactive<{ value: InvoiceData }>({
    value: {
      id: 0,
      issueDate: '',
      dueDate: '',
      status: 0,
      schoolId: 0,
      statusName: '',
      subscriptionPlanId: 0,
      payments: [],
    },
  });
  const errorInvoice = reactive(<ErrorResponseInvoice>{
    IssueDate: [],
    DueDate: [],
    SubscriptionPlanId: [],
    SchoolId: [],
    Status: [],
    PaymentMethod: [],
  });
  const requestCreatePayment = reactive<RequestCreatePayment>({
    invoiceId: 0,
    amount: 0,
    paymentDate: '',
    paymentMethod: '',
    status: 0,
  });

  const errorPaymentKeys: (keyof ErrorResponsePayment)[] = [
    'Amount',
    'PaymentDate',
    'PaymentMethod',
    'InvoiceId',
    'Status',
    'PaymentMethod',
  ];

  const errorPayment = reactive<ErrorResponsePayment>({
    Amount: [],
    PaymentDate: [],
    PaymentMethod: [],
    InvoiceId: [],
    Status: [],
  });

  const errorInvoiceKeys: (keyof ErrorResponseInvoice)[] = [
    'IssueDate',
    'DueDate',
    'SubscriptionPlanId',
    'SchoolId',
    'Status',
    'PaymentMethod',
  ];
  const getAllInvoice = async () => {
    isUpdate.value = false;
    requestInvoiceManagerCreate.subscriptionPlanId = 0;
    requestInvoiceManagerCreate.schoolId = 0;
    requestInvoiceManagerCreate.issueDate = '';
    requestInvoiceManagerCreate.dueDate = '';
    requestInvoiceManagerCreate.status = 0;
    requestInvoiceManagerCreate.paymentMethod = '';
    await invoiceManagerFactory.getAllInvoice(
      requestInvoiceManagerIndex,
      res => {
        responseInvoiceManagerIndex.value = res.data as Invoice;
      },
      err => {
        console.log(err);
      },
    );
  };

  const detailInvoice = async (id: number) => {
    isUpdate.value = false;
    isComplete.value = false;
    await invoiceManagerFactory.getInvoiceManagerDetail(
      id,
      res => {
        isUpdate.value = true;
        responseInvoiceData.value = res.data.data as InvoiceData;
        requestInvoiceManagerCreate.subscriptionPlanId = responseInvoiceData.value.subscriptionPlanId;
        requestInvoiceManagerCreate.schoolId = responseInvoiceData.value.schoolId;
        requestInvoiceManagerCreate.issueDate = responseInvoiceData.value.issueDate;
        requestInvoiceManagerCreate.dueDate = responseInvoiceData.value.dueDate;
        requestInvoiceManagerCreate.status = responseInvoiceData.value.status;
        requestInvoiceManagerCreate.paymentMethod = responseInvoiceData?.value?.payments[0]?.paymentMethod;
        if (Number(responseInvoiceData.value.status) === InvoiceStatus.PAID) {
          isComplete.value = true;
        }
      },
      err => {
        console.log(err);
      },
    );
  };

  const createInvoice = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await invoiceManagerFactory.createInvoiceManager(
      requestInvoiceManagerCreate,
      res => {
        requestInvoiceManagerCreate.subscriptionPlanId = 0;
        requestInvoiceManagerCreate.schoolId = 0;
        requestInvoiceManagerCreate.issueDate = '';
        requestInvoiceManagerCreate.dueDate = '';
        requestInvoiceManagerCreate.status = 0;
        requestInvoiceManagerCreate.paymentMethod = '';
        clearErrorKeys(errorInvoiceKeys, errorInvoice);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseInvoice;
        mapErrorKeys(errorInvoiceKeys, errorInvoice, errorsResponse);
        return error(err);
      },
    );
  };

  const updateInvoice = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await invoiceManagerFactory.updateInvoiceManager(
      id,
      requestInvoiceManagerCreate,
      res => {
        clearErrorKeys(errorInvoiceKeys, errorInvoice);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseInvoice;
        mapErrorKeys(errorInvoiceKeys, errorInvoice, errorsResponse);
        return error(err);
      },
    );
  };

  const deleteInvoices = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await invoiceManagerFactory.deleteInvoiceManager(
      requestDeleteInvoiceManager,
      res => {
        requestDeleteInvoiceManager.ids = [];
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const createPayment = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    if (isUpdate.value) {
      requestCreatePayment.invoiceId = responseInvoiceData.value.id;
      requestCreatePayment.amount = responseInvoiceData.value.totalAmount;
      requestCreatePayment.paymentDate = new Date().toISOString().slice(0, 10);
      requestCreatePayment.status = PaymentStatuses.SUCCESS;
    }
    await invoiceManagerFactory.createPayment(
      requestCreatePayment,
      res => {
        clearErrorKeys(errorPaymentKeys, errorPayment);
        success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponsePayment;
        mapErrorKeys(errorPaymentKeys, errorPayment, errorsResponse);
        error(err);
      },
    );
  };

  return {
    isComplete,
    isUpdate,
    errorInvoice,
    errorInvoiceKeys,
    requestInvoiceManagerIndex,
    requestInvoiceManagerCreate,
    requestDeleteInvoiceManager,
    responseInvoiceManagerIndex,
    responseInvoiceData,
    requestCreatePayment,
    errorPayment,
    errorPaymentKeys,
    getAllInvoice,
    detailInvoice,
    createInvoice,
    updateInvoice,
    deleteInvoices,
    createPayment,
  };
});
