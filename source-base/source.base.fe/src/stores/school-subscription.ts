import { ErrorResponse, SuccessResponse } from "@/constants/api/responses";
import RepositoryFactory from "@/repositories/repository";
import { InvoiceManagerRepository } from "@/repositories/repository-invoice";
import { SchoolRepository } from "@/repositories/repository-school";
import { SchoolSubscriptionRepository } from "@/repositories/repository-schoolSubscription";
// import { SchoolYearResponseInterface } from "@/types/model/school";
import { RequestAllSchoolSubscriptionInterface, RequestGeneratePayMentUrlInterface, ResponseAllSchoolSubscriptionInterface, ResponseCurrentSchoolSubscriptionInterface, ResponseInvoiceInterface, ResponseSubscriptionDetailInterface, ResponseSubscriptionInterface } from "@/types/model/school-subscription";
import { defineStore } from "pinia";
import { reactive, ref } from "vue";

export const useSchoolSubscriptionStore = defineStore('schoolSubscription', () => {
    const SchoolSubscriptionFactory = RepositoryFactory.create('schoolSubscription') as SchoolSubscriptionRepository;
    const InvoiceFactory = RepositoryFactory.create('invoiceManager') as InvoiceManagerRepository;
    const SchoolFactory = RepositoryFactory.create('school') as SchoolRepository;
    const responseInvoice = ref<ResponseInvoiceInterface | null>(null);
    const requestAllSchoolSubscription = reactive<RequestAllSchoolSubscriptionInterface>
        ({
            pageNumber: 1,
            pageSize: 10,
            status: null,
            year: null,
        });

    const listSchoolSubscriptionResponse = reactive<{ value: ResponseAllSchoolSubscriptionInterface }>({
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
                    description: '',
                    price: 0,
                    durationDays: 0,
                    startDate: '',
                    endDate: '',
                    status: 0,
                    statusName: '',
                }
            ]
        }
    });

    const responseCurrentSchoolSubscription = reactive<{ value: ResponseCurrentSchoolSubscriptionInterface }>({
        value: {
            id: 0,
            subscriptionPlanName: '',
            SubscriptionPlanId: 0,
            description: '',
            price: 0,
            durationDays: 0,
            startDate: '',
            endDate: '',
            features: [
                {
                    id: 0,
                    featureName: '',
                    description: '',
                }
            ]
        }
    });
    const responseInvoiceInterface = reactive<{ value: ResponseInvoiceInterface }>({
        value: {
            id: 0,
            schoolId: 0,
            subscriptionPlanName: '',
            totalAmount: 0,
            status: 0,
            durationDays: 0,
            startDate: '',
            endDate: '',
        }
    });

    const responseSubscriptionDetail = reactive<{ value: ResponseSubscriptionDetailInterface }>({
        value: {
            id: 0,
            name: '',
            description: '',
            price: 0,
            durationDays: 0,
            startDate: '',
            endDate: '',
        }
    });

    // const listSchoolYearResponse = reactive<{ value: SchoolYearResponseInterface }>({
    //     value: {
    //         pageNumber: 0,
    //         pageSize: 0,
    //         firstPage: '',
    //         lastPage: '',
    //         totalPages: 0,
    //         totalRecords: 0,
    //         nextPage: '',
    //         previousPage: '',
    //         data: [
    //             {
    //                 id: 0,
    //                 name: '',
    //                 startDate: '',
    //                 endDate: '',
    //                 isActive: false,
    //                 schoolId: 0,
    //             }
    //         ]
    //     }
    // });
    const getCurrentSubscription = async () => {
        await SchoolSubscriptionFactory.getCurrentSubscription(
            res => {
                responseCurrentSchoolSubscription.value = res.data as ResponseCurrentSchoolSubscriptionInterface;
            },
            err => {
                console.log(err);
            },
        );
    };

    const createInvoice = async (subscriptionPlanId: number) => {
        await SchoolSubscriptionFactory.createInvoice(
            subscriptionPlanId,
            res => {
                responseInvoiceInterface.value = res.data as ResponseInvoiceInterface
            },
            err => {
                console.log(err);
            },
        );
    };

    const getInvoiceBySubscription = async (subscriptionPlanId: number) => {
        await InvoiceFactory.getInvoiceBySubscription(
            subscriptionPlanId,
            res => {
                responseInvoiceInterface.value = res.data as ResponseInvoiceInterface
            },
            err => {
                console.log(err);
            },
        );
    };



    const generatePaymentUrl = async (
        params: RequestGeneratePayMentUrlInterface,
        success: (res: string) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        console.log('params', params);
        await SchoolSubscriptionFactory.generatePaymentUrl(
            params,
            res => {
                console.log('res-store', res);
                res.data as string;
                return success(res.data as string);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    };
    const paymentCallBack = async (
        success: (res: string) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await SchoolSubscriptionFactory.paymentCallBack(
            res => {
                res.data as string;
                return success(res.data as string);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    };

    const getListSchoolSubscription = async () => {
        await SchoolSubscriptionFactory.getAllSchoolSubscription(
            requestAllSchoolSubscription,
            res => {
                listSchoolSubscriptionResponse.value = res.data as ResponseAllSchoolSubscriptionInterface;
            },
            err => {
                console.log(err);
            },
        );
    };


    return {
        responseInvoice,
        responseInvoiceInterface,
        responseSubscriptionDetail,
        requestAllSchoolSubscription,
        listSchoolSubscriptionResponse,
        responseCurrentSchoolSubscription,
        generatePaymentUrl,
        getCurrentSubscription,
        createInvoice,
        paymentCallBack,
        getListSchoolSubscription,
        getInvoiceBySubscription,
    }

});
