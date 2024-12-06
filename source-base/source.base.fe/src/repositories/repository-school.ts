import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { ChangePasswordDTO } from '@/types/model/change-password.ts';

export class SchoolRepository extends BaseRepository {
  public async getInfoSchool(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.SCHOOL.GET_INFO, [], success, error);
  }

  public async updateSchool(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.put(API_URL.SCHOOL.UPDATE, data, success, error,true);
  }

  public async getDashboard(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.SCHOOL.DASHBOARD, [], success, error);
  }

  public async ChangePassword(data: ChangePasswordDTO, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.put(API_URL.SCHOOL.CHANGE_PASSWORD, data, success, error);
  }
}