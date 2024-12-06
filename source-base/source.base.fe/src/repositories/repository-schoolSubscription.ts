import { API_URL } from '@/constants/api/endpoint';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { BaseRepository } from '@/repositories/base-repository';
import { RequestAllSchoolSubscriptionInterface, RequestGeneratePayMentUrlInterface } from '@/types/model/school-subscription';


export class SchoolSubscriptionRepository extends BaseRepository {
    public async getCurrentSubscription(
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void
    ) {
        return await this.get(`${API_URL.SCHOOL_SUBSCRIPTION.CURRENT}`, {}, success, error, false, false);
    }

    public async createInvoice(
        subscriptionPlanId: number,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void
    ) {
        return await this.post(`${API_URL.SCHOOL_SUBSCRIPTION.CREATE_INVOICE}/${subscriptionPlanId}`, {}, success, error, false);
    }

    public async generatePaymentUrl(
        params: RequestGeneratePayMentUrlInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void
    ) {
        return await this.post(`${API_URL.SCHOOL_SUBSCRIPTION.GENERATE_PAYMENT}/${params.invoiceId}`, params, success, error, false);
    }

    public async paymentCallBack(
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void
    ) {
        return await this.get(`${API_URL.SCHOOL_SUBSCRIPTION.GENERATE_PAYMENT}`, {}, success, error, false);
    }

    public async getAllSchoolSubscription(
        params: RequestAllSchoolSubscriptionInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void
    ) {
        return await this.get(`${API_URL.SCHOOL_SUBSCRIPTION.ALL}`, params, success, error, false, true);
    }
}