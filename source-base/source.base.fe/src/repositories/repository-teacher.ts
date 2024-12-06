import { API_URL } from '@/constants/api/endpoint';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { BaseRepository } from './base-repository';

export class TeacherAssignRepository extends BaseRepository {

  public async getListTeacherAssign(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.TEACHER.GET_ASSIGN, data, success, error,true);
  }
}