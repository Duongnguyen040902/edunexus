import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { AddNewClassRequestInterface, RequestGetClassDetailInterface } from '@/types/model/class';
import { DeleteClassRequestInterface } from '@/types/model/class';

export class ClassRepository extends BaseRepository {
  public async getClassDetail(
    params: RequestGetClassDetailInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.CLASS.DETAIL}/${params.classId}/${params.semesterId}`, params, success, error);
  }
  public async getAssignedClass(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS.ASSIGNED_CLASS}`, {}, success, error);
  }

  public async addNewClass(data: AddNewClassRequestInterface, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.CLASS.ADD, data, success, error);
  }
  public async deleteClass(
    data: DeleteClassRequestInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.CLASS.DELETE}/${data}`, data, success, error);
  }

  public async getEnrolmentClass(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS.GET_ENROLLED_CLASS}`, {}, success, error);
  }

  public  async getPupilClasses( success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS.GET_PUPIL_CLASS}`, {}, success, error);
  }

  public async getPupilClassDetail(
    params: RequestGetClassDetailInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.CLASS.GET_PUPIL_CLASS_DETAIL}/${params.classId}/${params.semesterId}`, params, success, error);
  }

  public async getClassDetailForAdmin(
    params: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.CLASS.DETAIL_FOR_SCHOOL_ADMIN}/${params}`, [], success, error);
  }

  public async updateNewClass(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.put(API_URL.CLASS.UPDATE, data, success, error);
  }
}
