import { computed, reactive, ref } from 'vue';
import { endLoading, startLoading } from "@/helpers/mixins";
import { notifyError, notifySuccess } from "@/helpers/notify";
import { useSchoolSubscriptionStore } from "@/stores/school-subscription";
import { useSchoolYearStore } from "@/stores/school-year";
import { RequestAllSchoolSubscriptionInterface, RequestCreateInvoiceInterface, RequestCurrentSchoolSubscriptionInterface, RequestGeneratePayMentUrlInterface, RequestSubscriptionDetailInterface, RequestSubscriptionInterface, ResponseAllSchoolSubscriptionInterface, ResponseCurrentSchoolSubscriptionInterface, ResponseInvoiceInterface, ResponseSubscriptionDetailInterface, ResponseSubscriptionInterface } from "@/types/model/school-subscription";
import { useSubscriptionStore } from '@/stores/subcription';
import router from '@/router';
import { ROUTER_PATHS } from '@/constants/api/router-paths';


export const useSchoolSubscriptionComposable = () => {
    const schoolSubscriptionStore = useSchoolSubscriptionStore();
    const { 
        getCurrentSubscription, 
        createInvoice, 
        generatePaymentUrl, 
        getListSchoolSubscription, 
        responseInvoice,
        requestAllSchoolSubscription,
        listSchoolSubscriptionResponse,
        responseCurrentSchoolSubscription,
        responseInvoiceInterface,
        getInvoiceBySubscription,
    } = schoolSubscriptionStore;
    const schoolYearStore = useSchoolYearStore();
    const { 
        getSchoolYearIndex,
        dataSchoolYear,
    } = schoolYearStore;

    const subscriptionStore = useSubscriptionStore();
    const { getSubscriptionByDurationDay, responseSubscription } = subscriptionStore;
    const isShowModal = ref(false);
    const paymentUrl = ref('');



    const handleGetCurrentSubscription = async () => {
        startLoading();
        await getCurrentSubscription();
        endLoading();
    };
    
    const handleGetAllSchoolSubscription = async () => {
        startLoading();
        await getListSchoolSubscription();
        endLoading();
    };

    const handleGetSubscriptionByDuration = async () => {
        startLoading();
        await getSubscriptionByDurationDay();
        endLoading();
    };


    const handleGetInvoiceBySubscription = async (subscriptionId: number) => {
        
        startLoading();
        await getInvoiceBySubscription(subscriptionId);
        console.log("subscriptionId", responseInvoiceInterface.value)
        endLoading();
    };
    

    const handleCreateInvoice = async (subscriptionPlanId: number) => {      
        startLoading();
        await createInvoice(subscriptionPlanId);
        console.log("subscriptionPlanId", responseInvoiceInterface.value)
        endLoading();
    };

    const handleGetSchoolYear = async () => {
        startLoading();
        await getSchoolYearIndex();
        endLoading();
    };

    const handleGeneratePaymentUrl = async (invoiceId: number) => {
        startLoading();
        try {
            const requestPayload: RequestGeneratePayMentUrlInterface = { invoiceId };
            await generatePaymentUrl(
                requestPayload,
                res => {
                    paymentUrl.value = res as string;
                    console.log("paymentUrl", res);
                },
                error => {
                    console.error("Error response from server:", error);
                },
            );
        } catch (error) {
            notifyError('An error occurred while fetching subscription data');
        } finally {
            endLoading();
        }
    };


    const handlePayment = async (invoiceId: number) => {
        try {
            await handleGeneratePaymentUrl(invoiceId);
            if (paymentUrl.value) {
                window.location.href = paymentUrl.value;
            } else {
                notifyError("Không có URL để thanh toán.");
            }
        } catch (error) {
            notifyError("Đã xảy ra lỗi khi thực hiện thanh toán.");
        }
    };


    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
    };


    const handleUpgrade = async (subscriptionPlanId: number) => {
        await handleCreateInvoice(subscriptionPlanId);
        isShowModal.value = false;
        router.push({ path: ROUTER_PATHS.SCHOOL_ADMIN.ORDER_CHECKOUT, query: { id: subscriptionPlanId } });
    };


    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
        }).format(amount);
    };

    const updateRequestSchoolSubscription = async (key: keyof RequestAllSchoolSubscriptionInterface, value: any) => {
        requestAllSchoolSubscription[key] = value;   
        await handleGetAllSchoolSubscription();
    };

    const calculateShowingEntries = () => {
        const start = (requestAllSchoolSubscription.pageNumber - 1) * requestAllSchoolSubscription.pageSize + 1;
        const end = Math.min(requestAllSchoolSubscription.pageNumber * requestAllSchoolSubscription.pageSize, listSchoolSubscriptionResponse.value.totalRecords);
        return {
            start,
            end,
            total: listSchoolSubscriptionResponse.value.totalRecords
        };
    };

    const daysUsed = computed(() => {
        const startDate = new Date(responseCurrentSchoolSubscription.value.startDate);
        const today = new Date();
        return Math.ceil((today.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24));
    });

    const progressPercentage = computed(() => {
        const startDate = new Date(responseCurrentSchoolSubscription.value.startDate);
        const endDate = new Date(responseCurrentSchoolSubscription.value.endDate);

        const totalDays = Math.ceil((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24));

        return Math.min((daysUsed.value / totalDays) * 100, 100);
    });

    const totalDays = computed(() => {
        const startDate = new Date(responseCurrentSchoolSubscription.value.startDate);
        const endDate = new Date(responseCurrentSchoolSubscription.value.endDate);
    
        return Math.ceil((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24));
    });
    
    const handleOpen = async () => {
        await handleGetSubscriptionByDuration();
        isShowModal.value = true;
    };

    const handleClose = async () => {
        isShowModal.value = false;
    };
    return {
        responseInvoiceInterface,
        responseCurrentSchoolSubscription,
        isShowModal,
        responseSubscription,
        responseInvoice,
        paymentUrl,
        listSchoolSubscriptionResponse,
        daysUsed,
        progressPercentage,
        totalDays,
        dataSchoolYear,
        requestAllSchoolSubscription,
        handleGetCurrentSubscription,
        formatDate,
        formatCurrency,
        handleOpen,
        handleClose,
        handleGetSubscriptionByDuration,
        handleCreateInvoice,
        handleGeneratePaymentUrl,
        handlePayment,
        handleUpgrade,
        handleGetInvoiceBySubscription,
        handleGetAllSchoolSubscription,
        updateRequestSchoolSubscription,
        calculateShowingEntries,
        handleGetSchoolYear, 
    };
}