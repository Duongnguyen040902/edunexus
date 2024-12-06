import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestUpdateDataTeacherInterface } from '@/types/model/teacher';

export class TeacherProfileRepository extends BaseRepository {
  public async getTeacherProfile(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.TEACHER.PROFILE}`, {}, success, error);
  }

  public async updateTeacher(
    teacherData: RequestUpdateDataTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.TEACHER.PROFILE}`, teacherData, success, error, true);
  }
}