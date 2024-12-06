import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import {
  RequestAdminSchoolIndex,
  RequestAdminSchoolUpdate,
  RequestCreateSchoolAdmin,
  RequestGetListClassesInterface,
} from '@/types/model/admin-school';

export class AdminSchoolRepository extends BaseRepository {
  public async getAllAccountSchoolAdmin(
    params: RequestAdminSchoolIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.SCHOOL_ADMIN.INDEX, params, success, error, false, true);
  }

  public async getSchoolDetail(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.SCHOOL_ADMIN.DETAIL(id), {}, success, error);
  }

  public async updateSchoolAdmin(
    id: number,
    params: RequestAdminSchoolUpdate,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.SCHOOL_ADMIN.EDIT(id), params, success, error);
  }

  public async createSchoolAdmin(
    params: RequestCreateSchoolAdmin,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.SCHOOL_ADMIN.CREATE, params, success, error);
  }
  public async getListClass(data: RequestGetListClassesInterface, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.CLASS.GET_CLASS, data, success, error,false,true);
  }
}
