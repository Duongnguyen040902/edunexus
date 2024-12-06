import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestGetDashboardDataInterface } from '@/types/model/dashboard.ts';

export class DashboardRepository extends BaseRepository {
  public async GetRevenue(
    params: RequestGetDashboardDataInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.DASHBOARD.INDEX, params, success, error);
  }

  public async GetTotalSchoolAsync(
    params: RequestGetDashboardDataInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.DASHBOARD.TOTAL_SCHOOL, params, success, error);
  }

  public async GetTotalSchoolSubscriptionAsync(
    params: RequestGetDashboardDataInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.DASHBOARD.TOTAL_SCHOOL_SUBSCRIPTION, params, success, error);
  }
}
