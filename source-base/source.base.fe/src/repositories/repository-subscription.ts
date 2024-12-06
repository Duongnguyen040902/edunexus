import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestSubscriptionDetailInterface, RequestSubscriptionInterface } from '@/types/model/school-subscription';
import { RequestCreateSubscriptionInterface } from '@/types/model/subscription.ts';

export class SubscriptionRepository extends BaseRepository {
  public async getAllSubscription(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.SERVICE_PACKAGE.INDEX, {}, success, error);
  }

  public async createServicePackage(
    data: RequestCreateSubscriptionInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.SERVICE_PACKAGE.CREATE, data, success, error);
  }

  public async updateServicePackage(
    id: number,
    params: RequestCreateSubscriptionInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.SERVICE_PACKAGE.UPDATE(id), params, success, error);
  }

  public async detailServicePackage(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.SERVICE_PACKAGE.DETAIL(id), {}, success, error);
  }

  public async getSubscription(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.SUBSCRIPTION.ALL}`, {}, success, error, false, false);
  }

  public async getSubscriptionDetail(
    subscriptionId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.SUBSCRIPTION.DETAIL}/${subscriptionId}`, {}, success, error, false, false);
  }
}
